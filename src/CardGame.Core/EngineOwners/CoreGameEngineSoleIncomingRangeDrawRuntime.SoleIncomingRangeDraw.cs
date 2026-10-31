#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSoleIncomingRangeDrawRuntime
{
    internal static CardMoveReason SoleIncomingRangeDrawReason(SoleIncomingEffectGrant grant) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_SoleIncomingRangeDraw.SoleIncomingRangeDrawReason(grant);
    internal bool TryBeginSoleIncomingAttackRangeDraw(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
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
                AwaitingProgramFrameId = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()is { } parent && global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(parent) ? parent.Id : null
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
            if (!_coreNativeSlashCompletionOperations.FinishAttack(attack))
                _coreNativeAttackCompletionOperations.CompleteAttackAfterCardResolution(completion);
        }
        else
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId), use.CardKind);
    }

    internal bool IsSoleIncomingRangeDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => use.SoleIncomingRangeDraw is { Stage: SoleIncomingRangeDrawStage.Children, Outcome: { } result } receipt && batch.ParentFrameId == use.Id && batch.AwaitingProgramFrameId == receipt.AwaitingProgramFrameId && batch.Movements.Count > 0 && batch.Movements.All(m => m.Sequence > result.SequenceBefore && m.Sequence <= result.SequenceAfter && (result.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(result.RecipientSeat) && m.Reason == SoleIncomingRangeDrawReason(receipt.Grant) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle));
    internal bool IsSoleIncomingRangeDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame window) => window.ResumeSoleIncomingRangeDrawFrameId == use.Id && window.Id == window.Batch.Id && window.ResumeProgramFrameId is null && !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(window with { ResumeSoleIncomingRangeDrawFrameId = null }) && IsSoleIncomingRangeDrawMovement(use, window.Batch);

    internal void AssertSoleIncomingEffectRounds(CardUseFrame use)
    {
        if (use.SoleIncomingEffects is not { } r)
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (use.Action is not { Type: CardActionType.Use } action || r.ActionId != action.ActionId || r.Grants.Count == 0 || r.Grants.Select(g => g.ProgramFrameId).Distinct().Count() != r.Grants.Count || r.Started < 0 || r.Started > r.Grants.Count || r.DrawIndex < 0 || r.DrawIndex > r.Grants.Count || r.Active && r.Started == 0 || r.Native.ActionId != action.ActionId || r.Native.WholeGroupRepeated != (r.Started > 0) || !global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(use.CardKind) || r.Native.OriginalAttacks.GroupBy(s => s.TargetIndex).Any(g => g.Count() != 1) || r.Native.OriginalAttacks.Any(s => s.TargetIndex < 0 || s.TargetIndex >= use.TargetSeats.Count) || r.Grants.Any(g => g.UseFrameId != use.Id || g.ActionId != action.ActionId || g.TargetSeat != g.Source.OwnerSeat || g.OriginalActorSeat == g.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(g.TargetSeat) || g.MaximumDrawCount is < 1 or > 20 || g.WindowFrameId <= use.Id || g.ProgramFrameId <= g.WindowFrameId || !global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(g.CardKind) || history.OfType<SoleIncomingEffectIssuedEvent>().Count(e => e.Grant == g) != 1 || history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == g.ProgramFrameId && e.SkillId == g.Source.SkillId && e.SkillInstanceId == g.Source.SkillInstanceId && e.OwnerSeat == g.Source.OwnerSeat && e.BindingId == g.Source.BindingId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) != 1 || _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program is not { } p || p.GameplayHash != g.GameplayHash || ProgramInstructionResolver.Default.FindTrigger(p, g.Source.BindingId)?.Effects is not [{ Op: SkillProgramEffectOp.RepeatIncomingSoleCardEffectAndDrawByRange, Amount: var cap }] || cap != g.MaximumDrawCount) || !Enumerable.Range(1, r.Started).SequenceEqual(history.OfType<SoleIncomingEffectRoundStartedEvent>().Where(e => e.UseFrameId == use.Id).Select(e => e.Round)))
            throw new InvalidOperationException("An incoming extra effect lost its accepted native use, source or ordered issued grants.");
        for (var index = 0; index < r.Grants.Count; index++)
        {
            var grant = r.Grants[index];
            if (index < r.Started && history.OfType<SoleIncomingEffectRoundStartedEvent>().Count(e => e.UseFrameId == use.Id && e.ActionId == action.ActionId && e.ProgramFrameId == grant.ProgramFrameId && e.Round == index + 1) != 1)
                throw new InvalidOperationException("An incoming effect round lost its precise ordered source.");
            if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == grant.WindowFrameId)is { } window)
            {
                if (window.ParentFrameId != use.Id || window.Action.ActionId != action.ActionId || window.Candidates.Count(c => c.OwnerSeat == grant.Source.OwnerSeat && c.SkillId == grant.Source.SkillId && c.TriggerId == grant.Source.BindingId && c.SkillInstanceId == grant.Source.SkillInstanceId && c.GameplayHash == grant.GameplayHash && c.FrozenContext?.CardUse?.ParentCardUseFrameId == use.Id && c.FrozenContext.TargetSeat == grant.TargetSeat) != 1)
                    throw new InvalidOperationException("An incoming effect changed its accepted target-owner candidate.");
            }
            else if (history.OfType<ProgramCardTriggerResolvedEvent>().Count(e => e.FrameId == grant.WindowFrameId && e.SkillId == grant.Source.SkillId && e.TriggerId == grant.Source.BindingId && e.OwnerSeat == grant.Source.OwnerSeat && e.Activated) != 1)
                throw new InvalidOperationException("An incoming effect lost its original completed native target window.");
        }

        if (use.SoleIncomingRangeDraw is not { } draw)
            return;
        if (draw.GrantIndex != r.DrawIndex || draw.GrantIndex < 0 || draw.GrantIndex >= r.Grants.Count || r.Grants[draw.GrantIndex] != draw.Grant || draw.RequestedCount != Math.Min(draw.Grant.MaximumDrawCount, Math.Max(0, draw.AttackRange)) || history.OfType<SoleIncomingRangeDrawRequestedEvent>().Count(e => e.UseFrameId == use.Id && e.ProgramFrameId == draw.Grant.ProgramFrameId && e.RecipientSeat == draw.Grant.TargetSeat && e.AttackRange == draw.AttackRange && e.RequestedCount == draw.RequestedCount) != 1)
            throw new InvalidOperationException("A deferred range draw lost its once-frozen amount or issued native-use debt.");
        if (draw.Stage == SoleIncomingRangeDrawStage.Requesting)
        {
            if (draw.Outcome is not null || use.NativeDraw is not { } paid || paid.ReturnKind != NativeDrawAttemptReturnKind.SoleIncomingRange || paid.InstructionIndex != 0 || paid.RequestedCount != draw.RequestedCount || paid.Recipients is not [var recipient] || recipient != draw.Grant.TargetSeat || paid.Reason != SoleIncomingRangeDrawReason(draw.Grant) || paid.RecipientIndex != 0 || paid.RecipientApplied || paid.Operation is not null || paid.ResultBind is not null || paid.Visibility != SkillProgramCardSetVisibility.Private || paid.ActiveAttemptId is null && paid.AttemptReturned || paid.Replacement is not null && !paid.AttemptReturned)
                throw new InvalidOperationException("A deferred range draw lost its unpaid native request.");
            return;
        }

        if (draw.Stage != SoleIncomingRangeDrawStage.Children || use.NativeDraw is not null || draw.Outcome is not { } outcome || outcome.RecipientSeat != draw.Grant.TargetSeat || outcome.RequestedCount != draw.RequestedCount || outcome.SequenceBefore > outcome.SequenceAfter || outcome.SequenceAfter > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence || outcome.ActualDrawIds.Count > draw.RequestedCount || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == SoleIncomingRangeDrawReason(draw.Grant)).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds) || history.OfType<SoleIncomingRangeDrawPaidEvent>().Count(e => e.UseFrameId == use.Id && e.ProgramFrameId == draw.Grant.ProgramFrameId && e.RecipientSeat == outcome.RecipientSeat && e.ActualCount == outcome.ActualDrawIds.Count && e.SequenceBefore == outcome.SequenceBefore && e.SequenceAfter == outcome.SequenceAfter) != 1 || outcome.AttemptFrameId is { } attempt && history.OfType<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == use.Id && e.RecipientSeat == outcome.RecipientSeat && e.RequestedCount == draw.RequestedCount && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("A deferred range draw lost its exact paid physical materials.");
    }

    [ProgramDamageHook(ProgramDamagePhase.AttackCompletionPrelude, 30)]
    internal bool FinishSoleIncomingAttackRangeDraw(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = TryBeginSoleIncomingAttackRangeDraw(attack);
        return paused;
    }
}
