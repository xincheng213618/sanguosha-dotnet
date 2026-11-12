#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSoleIncomingRangeDrawRuntime
{
    internal static CardMoveReason SoleIncomingRangeDrawReason(SoleIncomingEffectGrant grant) => RuntimeSkills.RuntimeRules_Core_SoleIncomingRangeDraw.SoleIncomingRangeDrawReason(grant);
    internal bool TryBeginSoleIncomingAttackRangeDraw(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { SoleIncomingEffects: { } rounds } use || rounds.DrawIndex >= rounds.Grants.Count)
            return false;
        var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { SoleIncomingEffects = rounds with { AttackCompletionPending = true } });
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        if (TryBeginSoleIncomingRangeDraw(use.Id))
            return true;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { SoleIncomingEffects = current.SoleIncomingEffects!with { AttackCompletionPending = false } });
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = duel;
        return false;
    }

    internal bool TryBeginSoleIncomingRangeDraw(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { SoleIncomingEffects: { } rounds } use)
            return false;
        if (use.SoleIncomingRangeDraw is not null)
        {
            ContinueSoleIncomingRangeDraw(id);
            return true;
        }

        AssertSoleIncomingEffectRounds(use);
        if (_effectEpisodes.HasPending(id) || _effectEpisodes.IsActive(id) ||
            rounds.Grants.Any(grant => !_effectEpisodes.IsSettled(id, grant.EpisodeId))) return false;
        while (rounds.DrawIndex < rounds.Grants.Count)
        {
            var grant = rounds.Grants[rounds.DrawIndex];
            var owner = _coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat];
            // The repetition was paid when targeted. Its later draw has a separate
            // live-source gate, evaluated only after all native effect rounds return.
            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, grant.Source.SkillId, grant.Source.SkillInstanceId))
            {
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { SoleIncomingEffects = rounds = rounds with { DrawIndex = rounds.DrawIndex + 1 } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingRangeDrawCompletedEvent(id, grant.ProgramFrameId, false));
                continue;
            }

            var range = _coreNativeRuleQueriesOperations.GetAttackRange(owner.Seat);
            var count = Math.Min(grant.MaximumDrawCount, Math.Max(0, range));
            var receipt = new SoleIncomingRangeDrawReceipt(grant, rounds.DrawIndex, range, count, SoleIncomingRangeDrawStage.Requesting)
            {
                AwaitingProgramFrameId = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()is { } parent && CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(parent) ? parent.Id : null
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { SoleIncomingRangeDraw = receipt, NativeDraw = new NativeDrawProducerReceipt(0, count, null, SkillProgramCardSetVisibility.Private, SoleIncomingRangeDrawReason(grant)) { Recipients = [owner.Seat], ReturnKind = NativeDrawAttemptReturnKind.SoleIncomingRange } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingRangeDrawRequestedEvent(id, grant.ProgramFrameId, owner.Seat, range, count));
            ContinueSoleIncomingRangeDraw(id);
            return true;
        }

        return false;
    }

    [NativeFrameRoute(typeof(CardUseFrame), 20, typeof(NativeFrameRouteConditions), "SoleIncomingRangeDraw")]
    internal void ContinueSoleIncomingRangeDraw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { SoleIncomingRangeDraw: { } receipt } use || use.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("An incoming range draw lost its exact quiet native use owner.");
        AssertSoleIncomingEffectRounds(use);
        if (receipt.Stage == SoleIncomingRangeDrawStage.Requesting)
        {
            var paid = use.NativeDraw!;
            if (paid.ActiveAttemptId is not null && !paid.AttemptReturned)
                throw new InvalidOperationException("An incoming draw cannot pass its issued pre-draw request.");
            if (!paid.AttemptReturned && _coreNativeDrawAttemptsOperations.TryBeginNativeDrawAttempt(use))
                return;
            var before = _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;
            var grant = receipt.Grant;
            var ids = paid.Replacement is null && _coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat].IsAlive && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat], grant.Source.SkillId, grant.Source.SkillInstanceId) ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat], receipt.RequestedCount, true, SoleIncomingRangeDrawReason(grant)) : Array.AsReadOnly(Array.Empty<int>());
            var outcome = new NativeDrawOutcome(grant.Source.OwnerSeat, receipt.RequestedCount, paid.ActiveAttemptId, before, _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence, paid.Replacement)
            {
                ActualDrawIds = ids
            };
            use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
            _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { NativeDraw = null, SoleIncomingRangeDraw = receipt = receipt with { Stage = SoleIncomingRangeDrawStage.Children, Outcome = outcome } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingRangeDrawPaidEvent(id, grant.ProgramFrameId, grant.Source.OwnerSeat, ids.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.SoleIncomingRangeDraw) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.SoleIncomingRangeDraw) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.SoleIncomingRangeDraw) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        AssertSoleIncomingEffectRounds(use);
        receipt = use.SoleIncomingRangeDraw!;
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { SoleIncomingRangeDraw = null, NativeDraw = null, SoleIncomingEffects = use.SoleIncomingEffects!with { DrawIndex = receipt.GrantIndex + 1 } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingRangeDrawCompletedEvent(id, receipt.Grant.ProgramFrameId, true));
        if (use.SoleIncomingEffects!.AttackCompletionPending)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { SoleIncomingEffects = current.SoleIncomingEffects!with { AttackCompletionPending = false } });
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            var completion = _coreCardAttackStateOperations.CaptureAttackCompletion(attack);
            if (!_coreNativeSlashCompletionOperations.Value.FinishAttack(attack))
                _coreNativeAttackCompletionOperations.CompleteAttackAfterCardResolution(completion);
        }
        else
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, _nativeCardRepresentations.Get(id, use.CardId), use.CardKind);
    }

    internal bool IsSoleIncomingRangeDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => use.SoleIncomingRangeDraw is { Stage: SoleIncomingRangeDrawStage.Children, Outcome: { } result } receipt && batch.ParentFrameId == use.Id && batch.AwaitingProgramFrameId == receipt.AwaitingProgramFrameId && batch.Movements.Count > 0 && batch.Movements.All(m => m.Sequence > result.SequenceBefore && m.Sequence <= result.SequenceAfter && (result.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(result.RecipientSeat) && m.Reason == SoleIncomingRangeDrawReason(receipt.Grant) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle));
    internal bool IsSoleIncomingRangeDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame window) => window.ResumeSoleIncomingRangeDrawFrameId == use.Id && window.Id == window.Batch.Id && window.ResumeProgramFrameId is null && !CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(window with { ResumeSoleIncomingRangeDrawFrameId = null }) && IsSoleIncomingRangeDrawMovement(use, window.Batch);

    internal void AssertSoleIncomingEffectRounds(CardUseFrame use)
    {
        if (use.SoleIncomingEffects is not { } r)
            return;
        if (use.Action is not { Type: CardActionType.Use } action || r.ActionId != action.ActionId ||
            r.Grants.Count == 0 || r.Grants.Select(g => g.ProgramFrameId).Distinct().Count() != r.Grants.Count ||
            r.DrawIndex < 0 || r.DrawIndex > r.Grants.Count ||
            r.Grants.Any(g => g.UseFrameId != use.Id || g.ActionId != action.ActionId ||
                g.TargetSeat != g.Source.OwnerSeat || g.OriginalActorSeat == g.TargetSeat ||
                !_coreTableSeatQueriesOperations.IsValidPlayerSeat(g.TargetSeat) || g.MaximumDrawCount is < 1 or > 20 ||
                g.WindowFrameId <= use.Id || g.ProgramFrameId <= g.WindowFrameId ||
                _effectEpisodes.Get(use.Id, g.EpisodeId) is not { Issue: var issued } ||
                issued.IssuerFrameId != g.ProgramFrameId || issued.Source != g.Source ||
                issued.GameplayHash != g.GameplayHash || issued.Spec != new NativeCardEffectEpisodeSpec(50)))
            throw new InvalidOperationException("An incoming draw lost its original native use or issued episode.");
        if (use.SoleIncomingRangeDraw is not { } draw)
            return;
        if (draw.GrantIndex != r.DrawIndex || draw.GrantIndex < 0 || draw.GrantIndex >= r.Grants.Count ||
            r.Grants[draw.GrantIndex] != draw.Grant || !_effectEpisodes.IsSettled(use.Id, draw.Grant.EpisodeId) ||
            _effectEpisodes.HasPending(use.Id) || _effectEpisodes.IsActive(use.Id) ||
            draw.RequestedCount != Math.Min(draw.Grant.MaximumDrawCount, Math.Max(0, draw.AttackRange)))
            throw new InvalidOperationException("A deferred range draw lost its frozen amount or original finished native use.");
        if (draw.Stage == SoleIncomingRangeDrawStage.Requesting)
        {
            if (draw.Outcome is not null || use.NativeDraw is not { } paid || paid.ReturnKind != NativeDrawAttemptReturnKind.SoleIncomingRange || paid.InstructionIndex != 0 || paid.RequestedCount != draw.RequestedCount || paid.Recipients is not [var recipient] || recipient != draw.Grant.TargetSeat || paid.Reason != SoleIncomingRangeDrawReason(draw.Grant) || paid.RecipientIndex != 0 || paid.RecipientApplied || paid.Operation is not null || paid.ResultBind is not null || paid.Visibility != SkillProgramCardSetVisibility.Private || paid.ActiveAttemptId is null && paid.AttemptReturned || paid.Replacement is not null && !paid.AttemptReturned)
                throw new InvalidOperationException("A deferred range draw lost its unpaid native request.");
            return;
        }
        if (draw.Stage != SoleIncomingRangeDrawStage.Children || use.NativeDraw is not null ||
            draw.Outcome is not { } outcome || outcome.RecipientSeat != draw.Grant.TargetSeat ||
            outcome.RequestedCount != draw.RequestedCount || outcome.SequenceBefore < 0 ||
            outcome.SequenceBefore > outcome.SequenceAfter || outcome.SequenceAfter > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence ||
            outcome.ActualDrawIds.Count > draw.RequestedCount || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count ||
            outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 ||
            !_coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore &&
                m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile &&
                m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == SoleIncomingRangeDrawReason(draw.Grant))
                .OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("A deferred range draw lost its exact paid physical materials.");
    }
    [ProgramDamageHook(ProgramDamagePhase.AttackCompletionPrelude, 30)]
    internal bool FinishSoleIncomingAttackRangeDraw(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = TryBeginSoleIncomingAttackRangeDraw(attack);
        return paused;
    }
}
