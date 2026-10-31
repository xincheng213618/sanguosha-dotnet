#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHellSoulCardsRuntime
{
    internal const string HellSoulResolutionSkill = "hell:return-soul-resolution";
    internal static string HellSoulGrant(long id) => RuntimeSkills.RuntimeRules_Core_HellSoulCards.HellSoulGrant(id);
    internal string HellSoulGrantSource(long id) => $"turn:{(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)}:{(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)}:card-use:hell-return-soul:{(id)}";
    internal HellWanghunGrantLostEvent[] HellSoulOutstandingLosses(int seat)
    {
        var restored = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HellSoulGrantRestoredEvent>().Select(e => (e.LossFrameId, e.TargetSeat, e.GrantId)).ToHashSet();
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HellWanghunGrantLostEvent>().Where(e => e.TargetSeat == seat && !restored.Contains((e.FrameId, e.TargetSeat, e.Grant.GrantId))).OrderBy(e => e.FrameId).ThenBy(e => e.Grant.GrantId, StringComparer.Ordinal).ToArray();
    }

    internal IReadOnlyList<int> HellReturnSoulTargets(int actorSeat) => Array.AsReadOnly(Enumerable.Range(0, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreTableOrderOperations.TableStepSeat(actorSeat, offset)).Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && HellSoulOutstandingLosses(seat).Length != 0).ToArray());
    internal void AddHellSoulActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards)
    {
        if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.ReturnSoul, CardActionType.Use))
            return;
        foreach (var card in cards.Where(card => card.Kind == CardKind.ReturnSoul))
        {
            var targets = HellReturnSoulUseTargets(actor, card);
            if (targets.Count != 0)
                actions.Add(new(LegalActionKind.ReturnSoul, card.Id, null, "使用【回魂】，令因【忘魂】失去技能且尚未恢复的存活角色恢复这些技能", TargetSeats: targets));
        }
    }

    internal IReadOnlyList<int> HellReturnSoulUseTargets(CharacterState actor, Card card) => _coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, HellReturnSoulTargets(actor.Seat).Where(seat => !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, seat, CardKind.ReturnSoul) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.ReturnSoul, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, seat, CardKind.ReturnSoul) && !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.ExcludeGlobalTarget, CardKind.ReturnSoul) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).ToArray());
    internal void ResolveHellReturnSoul(CharacterState actor, Card card)
    {
        if (!_coreNativeLegalActionsOperations.BuildLegalActions(actor, selectedPhysicalCardId: card.Id).Any(a => a.Kind == LegalActionKind.ReturnSoul && a.CardId == card.Id))
            throw new InvalidOperationException("Return soul lost its actual playable entity or outstanding Wanghun target.");
        var targets = HellReturnSoulUseTargets(actor, card);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, targets);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, targets, LegalActionKind.ReturnSoul);
    }

    internal void ResolveHellReturnSoulEffect(NullificationWindowFrame pending)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId) ?? throw new InvalidOperationException("Return soul lost its actual native card owner.");
        var seat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("Return soul lost its exact native target.");
        if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.ReturnSoul || use.HellSoul is not null || use.Action is not { } action || use.TargetIndex < 0 || use.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != seat)
            throw new InvalidOperationException("Return soul changed its issued native effect or target cursor.");
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || HellSoulOutstandingLosses(seat).Length == 0)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CardKind.ReturnSoul);
            return;
        }

        var program = _contentRegistryOperations._contentRegistry.GetSkill(HellSoulResolutionSkill).Program!;
        var grant = HellSoulGrant(use.Id);
        _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].SkillGrants.Grant(new(grant, HellSoulResolutionSkill, grant, HellSoulGrantSource(use.Id), TurnExpiry: new(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)));
        var childId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { HellSoul = new(childId, action.ActionId, use.TargetIndex, seat) });
        var child = new ProgramSkillFrame(childId, use.SourceSeat, HellSoulResolutionSkill, "card-effect", program.GameplayHash, 0, [], Array.AsReadOnly(new[] { seat }))
        {
            SkillInstanceId = grant,
            HellSoulReturn = new(use.Id, action.ActionId, use.TargetIndex, seat)
        };
        _coreRuntimeOperations.PushRuntimeFrame(child);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(child.Id, child.OwnerSeat, child.SkillId, child.ActivationId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(child.Id);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 40053, SkillProgramEffectOp.RestoreHellWanghunSkills)]
    internal bool AllowsHellSoulVoluntaryActivation(ProgramActivationLegalityContext context) => context.SkillId != HellSoulResolutionSkill;
    internal bool IsExactHellSoulProgram(ProgramSkillFrame frame)
    {
        if (frame.HellSoulReturn is not { } returned || frame.SkillId != HellSoulResolutionSkill || frame.ActivationId != "card-effect" || frame.TriggerId is not null || frame.SelectedCardIds.Count != 0 || !frame.SelectedTargetSeats.SequenceEqual([returned.TargetSeat]) || frame.SkillInstanceId != HellSoulGrant(returned.CardUseFrameId) || frame.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!.GameplayHash || _coreCardUseLifecycleOperations.LifecycleCardUse(returned.CardUseFrameId)is not { Step: ResolutionFrameStep.ResolvingEffect, HellSoul: { } current, Action: { } action } use || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.ReturnSoul || current.ProgramFrameId != frame.Id || current.ActionId != action.ActionId || returned.ActionId != action.ActionId || current.TargetIndex != use.TargetIndex || returned.TargetIndex != use.TargetIndex || current.TargetSeat != returned.TargetSeat || use.TargetIndex < 0 || use.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != returned.TargetSeat || use.SourceSeat != frame.OwnerSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == use.Id && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.Grants.Any(g => g.GrantId == frame.SkillInstanceId && g.SkillId == frame.SkillId && g.SkillInstanceId == frame.SkillInstanceId && g.SourceId == HellSoulGrantSource(use.Id) && g.TurnExpiry == new SkillGrantTurnExpiry(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
    }

    internal SkillProgramStepOutcome BeginHellSoulRestoration(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsExactHellSoulProgram(frame) || frame.InstructionIndex != 1 || frame.HellSoulRestore is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HellSoulRestoreIssuedEvent>().Any(e => e.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("Return soul restoration requires its exact once-issued native effect child.");
        var returned = frame.HellSoulReturn!;
        var target = _coreMatchParticipantsPlayersAccess._players[returned.TargetSeat];
        var losses = HellSoulOutstandingLosses(target.Seat);
        var decisions = losses.Select(loss =>
        {
            var existing = target.SkillGrants.Grants.SingleOrDefault(g => g.GrantId == loss.Grant.GrantId);
            var outcome = _coreAlcoholSlashSkillReplacementsOperations.WasGrantPermanentlyRevokedAfterLoss(target.Seat, loss.Grant, loss) ? "permanently-lost" : !target.IsAlive ? "owner-dead" : target.General.Id != loss.GeneralId || target.SecondaryGeneral?.Id != loss.SecondaryGeneralId ? "template-changed" : loss.Grant.TurnExpiry is { } turn && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == turn.TurnNumber && e.ActorSeat == turn.TurnOwnerSeat) ? "grant-expired" : loss.Grant.PhaseExpiry is not null || _coreNextActualTurnStartSkillGrantsOperations.HasNextActualTurnStartSkillGrantExpired(loss.Grant) ? "grant-expired" : existing is null ? "restored" : existing == loss.Grant ? "already-present" : "source-changed";
            return (Loss: loss, Outcome: outcome);
        }).ToArray();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
        var count = decisions.Count(d => d.Outcome == "restored");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HellSoulRestore = new(frame.InstructionIndex, target.Seat, count) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellSoulRestoreIssuedEvent(frame.Id, returned.CardUseFrameId, target.Seat, count));
        foreach (var decision in decisions)
        {
            if (decision.Outcome == "restored")
                target.SkillGrants.Grant(decision.Loss.Grant);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellSoulGrantRestoredEvent(frame.Id, returned.CardUseFrameId, decision.Loss.FrameId, target.Seat, decision.Loss.Grant.GrantId, decision.Outcome));
        }

        ContinueHellSoulRestoration(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40053)]
    internal bool ResumeHellSoulRestoration(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HellSoulRestore: not null } frame || frame.Id != id)
            return false;
        ContinueHellSoulRestoration(frame);
        return true;
    }

    internal void ContinueHellSoulRestoration(ProgramSkillFrame frame)
    {
        if (!IsExactHellSoulProgram(frame) || frame.HellSoulRestore is not { } receipt || receipt.InstructionIndex != frame.InstructionIndex || receipt.TargetSeat != frame.HellSoulReturn!.TargetSeat || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HellSoulRestoreIssuedEvent>().Count(e => e.ProgramFrameId == frame.Id && e.CardUseFrameId == frame.HellSoulReturn.CardUseFrameId && e.TargetSeat == receipt.TargetSeat && e.RestoredGrants == receipt.RestoredGrants) != 1)
            throw new InvalidOperationException("Return soul lost its exact already restored skill receipt.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
            return;
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), true);
    }

    internal bool TryCompleteHellSoulProgram(ProgramSkillFrame frame, bool completed)
    {
        if (frame.HellSoulReturn is not { } returned)
            return false;
        if (!IsExactHellSoulProgram(frame) || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id)
            throw new InvalidOperationException("Return soul cannot return to a different native card or target.");
        var restored = frame.HellSoulRestore?.RestoredGrants ?? 0;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillResolvedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId, completed));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramSkill);
        _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.RemoveGrant(frame.SkillInstanceId);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(returned.CardUseFrameId)!;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { HellSoul = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellSoulTargetResolvedEvent(use.Id, returned.TargetSeat, restored, completed));
        _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: true), CardKind.ReturnSoul);
        return true;
    }

    internal bool HellSoulFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => child is ProgramSkillFrame { HellSoulReturn: { } returned } frame && returned.CardUseFrameId == parent.Id && IsExactHellSoulProgram(frame);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40053)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40053)]
    internal bool CanContinueHellSoulProgram(ProgramSkillFrame frame) => frame.HellSoulReturn is not null && IsExactHellSoulProgram(frame);
    internal void DestroyHellReturnSoulIfDiscarded(Card card, CardLocation to)
    {
        if (card.Kind != CardKind.ReturnSoul || to != CardLocation.DiscardPile || _cardZonesOperations._cardZones.GetLocation(card.Id) != to)
            return;
        var entered = _coreCardMovementJournalCardMovementsAccess._cardMovements.Last(m => m.CardId == card.Id);
        if (entered.To != to)
            throw new InvalidOperationException("Return soul destruction lost its actual discard entry.");
        _corePhysicalMovementFacadesOperations.MoveCard(card, to, CardLocation.OutsideGame, new("activity.hell.return-soul-destroyed"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellReturnSoulDestroyedEvent(card.Id, entered.Sequence, _coreCardMovementJournalCardMovementsAccess._cardMovements.Last(m => m.CardId == card.Id).Sequence));
    }

    internal bool IsHellDestroyedSoulMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.Action is { } action && action.PhysicalCards.Any(material => material.CardId == cardId && material.CardKind == CardKind.ReturnSoul) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HellReturnSoulDestroyedEvent>().Any(e => e.CardId == cardId && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == cardId && m.Sequence == e.DiscardSequence && m.To == CardLocation.DiscardPile) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == cardId && m.Sequence == e.DestroySequence && m.From == CardLocation.DiscardPile && m.To == CardLocation.OutsideGame && m.Reason.Value == "activity.hell.return-soul-destroyed"));
}
