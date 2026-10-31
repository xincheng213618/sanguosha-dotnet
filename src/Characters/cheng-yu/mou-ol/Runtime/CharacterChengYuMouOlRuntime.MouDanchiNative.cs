#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengYuMouOlRuntime
{
    internal bool CanMouDanchiSlash(int actor, int target) => _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && actor != target && _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, effectiveColor: null, effectiveRank: 0, physicalCardIds: [], countsTowardLimit: false) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null);
    internal bool IssueMouDanchiReward(ProgramSkillFrame supplied, CardKind kind, int target)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MouDanchi.Valid(f) || f.MouDanchi is not { } r || f.TriggerId != "source-next-use" || kind is not (CardKind.DrawTwo or CardKind.Slash) || (kind == CardKind.DrawTwo ? r.Stage != MouDanchiStage.DrawIssuing : r.Stage != MouDanchiStage.SlashChoice || !r.Different))
            throw new InvalidOperationException("Danchi can issue only its promised native DrawTwo followed by the optional Slash.");
        if (kind == CardKind.Slash ? !CanMouDanchiSlash(f.OwnerSeat, target) : _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || target != f.OwnerSeat || _coreCardUseModulesOperations.IsCardUseForbidden(f.OwnerSeat, kind, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(f.OwnerSeat, kind, [])) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, f.OwnerSeat, kind) || _coreHandComparisonOperations.IsSelfTargetForbiddenAction(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], kind, [f.OwnerSeat]) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], kind, Suit.None, null))
            return false;
        SuspendMouDanchiAttack(f.Id);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.MouDanchi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDanchi = r with { Stage = kind == CardKind.DrawTwo ? MouDanchiStage.DrawIssuing : MouDanchiStage.SlashIssuing, Kind = kind, TargetSeat = target, UseId = 0, ActionId = 0, Returned = false, Batches = [] }, PendingMovementContinuation = null });
        if (kind == CardKind.Slash)
            _coreProgramVirtualSlashOffersOperations.BeginOfferedProgramVirtualSlash(f, f.OwnerSeat, target);
        else
        {
            var card = new Card(0, kind, Suit.None, 0);
            var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [f.OwnerSeat], kind, physicalCardIds: []);
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouDanchiOrigin is null)
                throw new InvalidOperationException("Danchi must bind its native zero-material card before effects.");
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, LegalActionKind.DrawTwo, playedCardKind: kind);
        }

        return true;
    }

    internal void SuspendMouDanchiAttack(long frameId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var r = f.MouDanchi!;
        if (r.Suspension is not null)
            return;
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != r.NextActionId)
            throw new InvalidOperationException("Danchi cannot suspend a different next-use window.");
        var actionId = window.CompletedResponseReturn is null ? r.NextActionId : window.Action.ParentActionId;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == window.Id);
        var original = _resolutionStackOperations._resolutionStack.Take(at).OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == actionId);
        if (original?.Action is not { } action)
            return;
        var suspension = new MouDanchiSuspendedUse(original.Id, action.ActionId, original.CardKind, original.SourceSeat, original.TargetIndex, original.CardAttack?.Active == true, original.Continuations.Duel?.Active == true, original.Continuations.GroupCard?.Active == true, original.FireAttackSelection is not null, original.FireAttackSelection?.RevealedCardId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDanchi = r with { Suspension = suspension } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(original.Id, u => u with { CardAttack = suspension.AttackActive ? u.CardAttack!with { Active = false } : u.CardAttack, Continuations = u.Continuations with { Duel = suspension.DuelActive ? u.Continuations.Duel!with { Active = false } : u.Continuations.Duel, GroupCard = suspension.GroupActive ? u.Continuations.GroupCard!with { Active = false } : u.Continuations.GroupCard } });
        // FireAttack's selector is active only while that exact frame is top.
        // Its cursor stays on the mother, without inventing an Active flag.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDanchiCursorSuspendedEvent(f.Id, suspension));
    }

    internal void RestoreMouDanchiAttack(ProgramSkillFrame f)
    {
        if (f.MouDanchi is not { Suspension: { } s } r)
            return;
        if (!MouDanchiSuspensionMatches(f) || !MouDanchi.Valid(f) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null || r.Stage is MouDanchiStage.NativeUse or MouDanchiStage.DrawIssuing or MouDanchiStage.SlashIssuing || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDanchiCursorRestoredEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Danchi lost its exact suspended original attack before returning.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(s.UseId, u => u with { CardAttack = s.AttackActive ? u.CardAttack!with { Active = true } : u.CardAttack, Continuations = u.Continuations with { Duel = s.DuelActive ? u.Continuations.Duel!with { Active = true } : u.Continuations.Duel, GroupCard = s.GroupActive ? u.Continuations.GroupCard!with { Active = true } : u.Continuations.GroupCard } });
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDanchi = r with { Suspension = null } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDanchiCursorRestoredEvent(f.Id, s.UseId, s.ActionId));
    }

    internal bool MouDanchiSuspensionMatches(ProgramSkillFrame f) => f.MouDanchi?.Suspension is not { } s || _coreCardUseLifecycleOperations.LifecycleCardUse(s.UseId)is { Action: { } a } use && a.ActionId == s.ActionId && use.CardKind == s.Kind && use.SourceSeat == s.SourceSeat && use.TargetIndex == s.TargetIndex && (!s.AttackActive || use.CardAttack is { Active: false }) && (!s.DuelActive || use.Continuations.Duel is { Active: false }) && (!s.GroupActive || use.Continuations.GroupCard is { Active: false }) && (use.FireAttackSelection is not null) == s.HasFireSelection && use.FireAttackSelection?.RevealedCardId == s.FireRevealedCardId && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDanchiCursorSuspendedEvent>().Count(e => e.FrameId == f.Id && e.Suspension == s) == 1;

    internal CardActionContext PrepareMouDanchiAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDanchi: { Stage: MouDanchiStage.DrawIssuing } r } f)
            return action;
        if (!MouDanchi.Valid(f) || action.EffectiveKind != CardKind.DrawTwo || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.PhysicalCards.Count != 0 || action.ConversionChain.Count != 0)
            throw new InvalidOperationException("Danchi cannot normalize another producer's native action.");
        return _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: null));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 34101)]
    internal void BindMouDanchiUse(CardUseDeclaredEvent declared)
    {
        if (!HasMouChengYu || _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { } use)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { MouDanchi: { Stage: MouDanchiStage.DrawIssuing or MouDanchiStage.SlashIssuing } r } f)
            return;
        if (!MouDanchi.Valid(f) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardKind != r.Kind || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0 } a || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || !a.TargetSeats.SequenceEqual([r.TargetSeat]))
            throw new InvalidOperationException("Danchi must bind exactly the original beneficiary's neutral native card.");
        var origin = new MouDanchiNativeOrigin(f.Id, r.Source, r.GameplayHash, use.Id, a.ActionId, f.OwnerSeat, use.CardKind);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouDanchi = r with { Stage = MouDanchiStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouDanchiOrigin = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDanchiNativeIssuedEvent(origin));
    }

    internal bool IsMouDanchiUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouDanchiOrigin: { } o, Action: { } a } use && use.Id == o.UseId && a.ActionId == o.ActionId && use.CardId == 0 && use.CardKind == o.Kind && a.EffectiveKind == o.Kind && use.PhysicalCardIds is { Count: 0 } && a.PhysicalCards.Count == 0 && a.ConversionChain.Count == 0 && a.EffectiveSuit == Suit.None && a.EffectiveRank == 0 && a.ProviderSeat == o.ActorSeat && a.RequesterSeat is null && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.ActorSeat, o.ActorSeat) && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is ProgramSkillFrame { MouDanchi: { Stage: MouDanchiStage.NativeUse } r } f && MouDanchi.Valid(f) && r.UseId == use.Id && r.ActionId == a.ActionId && r.Source == o.Source && r.GameplayHash == o.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDanchiNativeIssuedEvent>().Count(e => e.Origin == o) == 1;
    internal bool IsMouDanchiDrawTwo(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.CardKind == CardKind.DrawTwo && IsMouDanchiUse(id);
    internal Card ReadMouDanchiAppearance(long id, CardAppearanceReference appearance)
    {
        if (!IsMouDanchiUse(id) || appearance.Id != 0 || appearance.Kind != _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind || appearance.Suit != Suit.None || appearance.Rank != 0)
            throw new InvalidOperationException("Danchi lost the frozen neutral native appearance.");
        return new(0, appearance.Kind, Suit.None, 0);
    }

    internal bool IsMouDanchiSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => f.MouDanchi is { Stage: MouDanchiStage.Children, Returned: true } r && MouDanchi.Valid(f) && r.Batches.Contains(batch.Id) && batch.ParentFrameId == r.UseId && batch.AwaitingProgramFrameId is null && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId && batch.OriginOwnerSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDanchiNativeBatchEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.BatchId == batch.Id) == 1 && batch.Movements.Count > 0 && batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m));
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 34101)]
    internal bool FinishMouDanchiSlash(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsMouDanchiUse(attack.ResolutionId) || !CoreNativeCardClassificationsRuntime.IsSlashCard(_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!.CardKind))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }
    [NativeUseValidation(34101)]
    internal void ValidateMouDanchiNativeUse(CardUseFrame use)
    {
        if (use.MouDanchiOrigin is not null && (!IsMouDanchiUse(use.Id) || use.Step != ResolutionFrameStep.Completed))
            throw new InvalidOperationException("Danchi must finish its whole actual native card before returning.");
    }

    [NativeUseReturn(34101)]
    internal void ReturnMouDanchiUse(CardUseFrame use)
    {
        if (use.MouDanchiOrigin is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouDanchi!;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || r.UseId != use.Id || r.Returned || r.ActionId != o.ActionId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Danchi cannot return another Use or repeat its reward.");
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat).ToArray();
        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDanchiNativeBatchEvent(f.Id, use.Id, b.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDanchi = r with { Stage = MouDanchiStage.Children, Returned = true, Batches = batches.Select(b => b.Id).ToArray() }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDanchiNativeReturnedEvent(f.Id, use.Id, o.ActionId, use.CardKind));
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 34101)]
    internal bool ReturnMouDanchiAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDanchi: { Stage: MouDanchiStage.Children, Returned: true } r } f || r.UseId != completion.ResolutionId)
            return false;
        if (completion.ProgramFrameId != f.Id)
            throw new InvalidOperationException("Danchi's actual Slash returned to a different producer.");
        MouDanchi.Continue(f.Id);
        return true;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 1000)]
    internal bool WindowSettlement_IsMouDanchiSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsMouDanchiSettlementBatch(frame, batch);
}
