#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanOlMagicRuntime
{
    internal bool HasMoDiaoChan => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoRound) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMoDiaoChanQingshiPrep);

    internal static CardConversionSource MoDcSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_diao_chan_ol_magic_MoDiaoChanHuanhuo.MoDcSource(f);
    internal int? MoDcDueTurn(MoDiaoChanPromiseEvent p) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Skip(p.AfterTurnStartOrdinal).FirstOrDefault(t => t.ActorSeat == p.TargetSeat)?.TurnNumber;
    internal MoDiaoChanPromiseEvent[] ActiveMoDcPromises(int actor) => !HasMoDiaoChan || !_coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn || actor != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != actor ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanPromiseEvent>().Where(p => p.TargetSeat == actor && MoDcDueTurn(p) == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Count(e => e.Claim.PromiseId == p.PromiseId) < 2).ToArray();
    internal int MoDcRemaining(int actor) => ActiveMoDcPromises(actor).Select(p => 2 - _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Count(e => e.Claim.PromiseId == p.PromiseId)).DefaultIfEmpty(0).Max();
    // Only command progression calls this; view/query code merely reads the signed lock.
    [ProgramPipelineHook(ProgramPipelinePhase.PreparePlayPhaseDecision, 1)]
    internal void PrepareMoDiaoChanHandSelection(CharacterState actor)
    {
        if (!HasMoDiaoChan || ActiveMoDcPromises(actor.Seat).Length == 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase)
            return;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Select(c => c.Id).ToHashSet();
        var actions = _coreNativeLegalActionsOperations.BuildLegalActionsCore(actor, includeProgramActions: true, selectedPhysicalCardId: null, nativeOriginalCard: null, ignoreForcedHandSelection: true);
        var pool = actions.Where(a => a.Kind is not (LegalActionKind.EndPlay or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.Recast)).SelectMany(a => a.CardId is { } id ? new[] { id } : a.SelectableCardIds ?? []).Where(hand.Contains).Distinct().Order().ToArray();
        var old = _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanLockEvent>().LastOrDefault(e => e.ActorSeat == actor.Seat && e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber);
        if (old is not null && old.PhaseToken == phase.Token && (old.CardId == 0 && pool.Length == 0 || pool.Contains(old.CardId)))
            return;
        var before = _randomOperations._random.State;
        var index = pool.Length == 0 ? -1 : _randomOperations._random.Next(pool.Length);
        var lockId = _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanLockEvent>().Select(e => e.LockId).DefaultIfEmpty().Max() + 1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanLockEvent(lockId, actor.Seat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, phase.Token, index < 0 ? 0 : pool[index], pool.Length, index, before, _randomOperations._random.State));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ForcedPlayHandSelection, 1)]
    internal ForcedHandSelectionSnapshot? MoDiaoChanHandSelection(int actor)
    {
        if (ActiveMoDcPromises(actor).Length == 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanLockEvent>().LastOrDefault(e => e.ActorSeat == actor && e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.PhaseToken == phase.Token)is not { CardId: > 0 } pick || _cardZonesOperations._cardZones.GetLocation(pick.CardId) != CardLocation.Hand(actor))
            return null;
        return new(MoDiaoChanComposition.Huanhuo, pick.LockId, phase.Token, pick.CardId, MoDcRemaining(actor));
    }

    internal IReadOnlyList<LegalAction> FilterMoDiaoChanHandSelection(CharacterState actor, IEnumerable<LegalAction> actions)
    {
        if (MoDiaoChanHandSelection(actor.Seat)is not { } hint)
            return actions.ToArray();
        return actions.Where(a => a.Kind is LegalActionKind.EndPlay or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.Recast || (a.CardId is> 0 and var id ? _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(actor.Seat) || id == hint.CardId : a.SelectableCardIds is not { Count: > 0 } ids || ids.Contains(hint.CardId))).ToArray();
    }

    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 10)]
    internal void ValidateMoDiaoChanSubmittedUse(ProgramSubmittedUseQuery query)
    {
        if (!MoDcHuanhuoPaymentAllowed(query.ActorSeat, query.CardIds))
            throw new InvalidOperationException("Huanhuo requires its current selected Hand entity in this real Play use.");
    }

    internal bool MoDcHuanhuoPaymentAllowed(int actor, IReadOnlyList<int> ids, long? existingUseId = null)
    {
        // A compulsory Play selection is not a response or a skill-payment ban.
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not null || _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramSkillFrame)
            return true;
        if (existingUseId is { } use && _coreCardUseLifecycleOperations.LifecycleCardUse(use)?.MoDiaoChanClaims?.Any(q => q.ActorSeat == actor) == true)
            return true;
        var hint = MoDiaoChanHandSelection(actor);
        return hint is null || ids.All(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(actor)) || ids.Contains(hint.CardId);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 31400)]
    internal void CaptureMoDiaoChanUses(IGameEvent fact)
    {
        if (!HasMoDiaoChan)
            return;
        CardActionContext? action = null;
        ResolutionFrame? parent = null;
        if (fact is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } a } use)
        {
            action = a;
            parent = use;
        }
        else if (fact is CardActionAcceptedEvent { Action.Type: CardActionType.Response } accepted)
        {
            action = accepted.Action;
            if (action.EffectiveKind == CardKind.Nullification && _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.NullificationResponse))
                parent = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow;
            else if (action.EffectiveKind == CardKind.Dodge && _coreResponseUseCompletionOperations.IsCompletedResponseUse(action, ProgramCardContinuation.Dodge) && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack)
                parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == attack.ResolutionId);
        }

        if (action is null || parent is null || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase)
            return;
        foreach (var p in ActiveMoDcPromises(action.ActorSeat))
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Any(e => e.Claim.PromiseId == p.PromiseId && e.Claim.ActionId == action.ActionId))
                continue;
            var ordinal = _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Count(e => e.Claim.PromiseId == p.PromiseId) + 1;
            var source = p.Source with
            {
                BindingId = "affected-use-completed"
            };
            var q = new MoDiaoChanUseClaim(p.PromiseId, parent.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, phase.Token, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, ordinal, source, p.GameplayHash);
            parent = _resolutionStackOperations._resolutionStack.Single(f => f.Id == parent.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { MoDiaoChanClaims = (parent.MoDiaoChanClaims ?? []).Append(q).ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanUseClaimedEvent(q));
        }
    }

    internal MoDiaoChanUseClaim? MoDcCompletedClaim(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (MoDcDirectClaim(candidate, context)is { } direct)
            return direct;
        if (candidate.SkillId != MoDiaoChanComposition.Huanhuo || candidate.BindingId != "affected-use-completed" || context.Window != SkillProgramTriggerWindow.CardUseCompleted || context.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.Action.ActionId != identity.CardActionId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)?.MoDiaoChanClaims is not { } claims)
            return null;
        return claims.SingleOrDefault(q => q.Source.OwnerSeat == candidate.OwnerSeat && q.Source.SkillInstanceId == candidate.SkillInstanceId && q.GameplayHash == candidate.GameplayHash && q.ActionId == w.Action.ActionId && q.ProviderSeat == w.Action.ProviderSeat && _characterZhouYuOlMouOperations.MouGuquActorLineageMatches(q.ParentFrameId, q.ActionId, q.ActorSeat, w.Action.ActorSeat, q.ProviderSeat, w.Action.Type) && _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Count(e => e.Claim == q) == 1);
    }

    [ProgramIssuedTrigger(40)]
    internal bool IsIssuedMoDiaoChanCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => MoDcCompletedClaim(c, context)is { } q && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanStartedEvent>().Any(e => e.Source == q.Source && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == e.FrameId && f.MoDiaoChan?.ActionId == q.ActionId)) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseSettledEvent>().Any(e => e.PromiseId == q.PromiseId && e.ActionId == q.ActionId);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 23)]
    internal void CollectIssuedMoDiaoChanCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasMoDiaoChan || window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.LastOrDefault()?.MoDiaoChanClaims is not { } claims)
            return;
        foreach (var q in claims.Where(q => q.ActionId == action.ActionId && q.ProviderSeat == action.ProviderSeat && q.ActorSeat == action.ActorSeat))
        {
            result.RemoveAll(c => c.OwnerSeat == q.Source.OwnerSeat && c.SkillId == q.Source.SkillId && c.TriggerId == q.Source.BindingId && c.SkillInstanceId == q.Source.SkillInstanceId);
            result.Add(new(q.Source.OwnerSeat, -1, q.Source.SkillId, q.Source.BindingId, q.GameplayHash, q.Source.SkillInstanceId, 0, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, q.Ordinal, q.Source.OwnerSeat, -1, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[q.Source.OwnerSeat], action))));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ObservesResponseUseCompletion, 3)]
    internal bool HasIssuedMoDiaoChanResponse(CardActionContext action) => HasMoDiaoChan && _resolutionStackOperations._resolutionStack.Any(f => f.MoDiaoChanClaims?.Any(q => q.ActionId == action.ActionId && q.ParentFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Count(e => e.Claim == q) == 1) == true);
    internal Card[] MoDcDiscardable(int owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal void PayMoDcRandomDiscard(ProgramSkillFrame f)
    {
        var r = f.MoDiaoChan!;
        var pool = _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive ? MoDcDiscardable(r.ActorSeat).OrderBy(c => c.Id).ToArray() : [];
        var before = _randomOperations._random.State;
        var index = pool.Length == 0 ? -1 : _randomOperations._random.Next(pool.Length);
        var seq = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r = r with { Paid = true, Stage = MoDiaoChanStage.CostChildren, Before = seq, After = seq }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanRandomDiscardEvent(f.Id, r.ActorSeat, pool.Length, index, before, _randomOperations._random.State, index < 0 ? null : pool[index].Id));
        if (index >= 0)
            MoDcDiscardOne(f, r.ActorSeat, pool[index]);
        SealMoDcMovement(f.Id, seq);
    }

    internal void MoDcDiscardOne(ProgramSkillFrame f, int owner, Card card)
    {
        var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, owner, card, from, new(f.SkillId == MoDiaoChanComposition.Huanhuo ? "skill-program.ol:mo-huanhuo.discard" : "skill-program.ol:mo-qingshi.cost"), parentFrameId: f.Id);
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.From == from && m.Sequence > f.MoDiaoChan!.Before);
        if (move is not null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanCostEvent(f.Id, card.Id, card.Kind, from, move.To, move.Sequence));
    }
}
