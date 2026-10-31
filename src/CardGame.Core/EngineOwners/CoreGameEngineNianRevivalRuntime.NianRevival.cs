#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNianRevivalRuntime
{
    internal const string NianReviveSkill = "nian:revive";
    SkillProgramStepOutcome global::CardGame.Core.INianRevivalHost.ReviveNianAlly(ProgramSkillFrame frame) => BeginNianRevival(frame);
    IReadOnlyList<int> global::CardGame.Core.INianRevivalSelectionQuery.DeadAllies(int ownerSeat) => NianDeadAllies(ownerSeat);
    internal int NianUsedRevives => _coreNianChallengeHostOperations.NianFeature?.UsedRevives ?? 0;

    internal bool IsNianAlliedSeat(int seat) => _coreNianChallengeHostOperations.NianFeature?.IsAlliedSeat(seat) == true;
    internal IReadOnlyList<int> NianDeadAllies(int ownerSeat) => _coreNianChallengeHostOperations.NianFeature?.DeadAllies(ownerSeat) ?? [];
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 27930, SkillProgramEffectOp.ReviveNianAlly)]
    internal bool AllowsNianRevival(ProgramActivationLegalityContext gate) => gate.SkillId == NianReviveSkill && gate.Activation is { Id: "revive", MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.NianDeadAlly } && NianDeadAllies(gate.OwnerSeat).Count != 0;
    internal SkillProgramStepOutcome BeginNianRevival(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.SkillId != NianReviveSkill || f.ActivationId != "revive" || f.TriggerId is not null || f.NianRevival is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var targetSeat] || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != f.OwnerSeat || !NianDeadAllies(f.OwnerSeat).Contains(targetSeat))
            throw new InvalidOperationException("Nian revival lost its living actor, dead ally or remaining shared payment.");
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var ordinal = NianUsedRevives + 1;
        var cards = _cardZonesOperations._cardZones.CreateDiagnostics().Where(c => c.Location.OwnerSeat == targetSeat).GroupBy(c => c.Location).Select(g => (Location: g.Key, Cards: g.Select(c => c.CardId).ToArray())).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NianRevival = new(f.InstructionIndex, targetSeat, ordinal, target.MaxHp, NianRevivalStage.DiscardChildren, before, before, 0) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianRevivalPaidEvent(f.Id, f.OwnerSeat, targetSeat, ordinal, target.MaxHp));
        foreach (var group in cards)
            _corePhysicalMovementFacadesOperations.MoveCards(group.Cards.Select(id => _coreCardAttackStateOperations.GetAttackCard(id)).ToArray(), group.Location, CardLocation.DiscardPile, global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalDefinitions.NianRevivalDiscard);
        CompleteNianSegment(f.Id, cards.Sum(g => g.Cards.Length));
        ContinueNianRevival(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void CompleteNianSegment(long id, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.NianRevival!with
        {
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Count = count
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NianRevival = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianRevivalSegmentEvent(id, r.TargetSeat, r.Stage, r.Before, r.After, count));
    }

    internal bool DrainNianRevival(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal bool IsNianRevivalMovementBatch(ProgramSkillFrame f, CardMovementBatchContext batch)
    {
        if (f.NianRevival is not { } r || batch.ParentFrameId != f.Id || batch.AwaitingProgramFrameId is not null || batch.OriginOwnerSeat != f.OwnerSeat || batch.OriginSkillId != f.SkillId || batch.OriginSkillInstanceId != f.SkillInstanceId || batch.Movements.Count == 0 || r.Stage == NianRevivalStage.StateChildren)
            return false;
        RequireNianRevival(f);
        return batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && (r.Stage == NianRevivalStage.DiscardChildren ? m.From.OwnerSeat == r.TargetSeat && m.To == CardLocation.DiscardPile && m.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalDefinitions.NianRevivalDiscard : m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.TargetSeat) && m.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalDefinitions.NianRevivalDraw));
    }

    internal void ContinueNianRevival(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        RequireNianRevival(f);
        if (DrainNianRevival(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = f.NianRevival!;
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianRevivalCancelledEvent(f.Id, r.TargetSeat, "game-ended"));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NianRevival = null });
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        if (r.Stage == NianRevivalStage.DiscardChildren)
        {
            if (target.IsAlive || target.MaxHp != r.MaximumHp || !target.HasHpValue)
                throw new InvalidOperationException("A paid Nian revival lost its original dead character.");
            var wasFaceDown = _coreProgramLifeRestorationOperations.RestoreProgramCharacterLife(f, target, Math.Min(2, target.MaxHp));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NianRevival = r with { Stage = NianRevivalStage.StateChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = 0 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianAllyRevivedEvent(f.Id, f.OwnerSeat, target.Seat, r.UsedRevives, target.Hp));
            // Direct life restoration intentionally issues no Recovery/HpRecovered
            // event. Actual face-state observers still receive their real change.
            _coreProgramLifeRestorationOperations.PublishProgramRevivalFaceState(target, wasFaceDown);
            CompleteNianSegment(f.Id, 0);
            ContinueNianRevival(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (r.Stage == NianRevivalStage.StateChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NianRevival = r with { Stage = NianRevivalStage.DrawAttempt, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = 0 } });
            CompleteNianSegment(f.Id, 0);
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), target.Seat, 2, global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalDefinitions.NianRevivalDraw);
            return;
        }

        if (r.Stage == NianRevivalStage.DrawAttempt)
            throw new InvalidOperationException("A Nian revival cannot bypass its exact native draw return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianRevivalCompletedEvent(f.Id, r.TargetSeat, r.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NianRevival = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ReviveNianAlly)]
    internal void ReturnNianRevivalDraw(ProgramSkillFrame input, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        RequireNianRevival(frame);
        if (frame.NativeDraw is not null || frame.NianRevival is not { Stage: NianRevivalStage.DrawAttempt } receipt || outcome.RecipientSeat != receipt.TargetSeat || outcome.RequestedCount != 2 || outcome.SequenceBefore < receipt.Before || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.ActualDrawIds.Count > 2 || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(receipt.TargetSeat) && m.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalDefinitions.NianRevivalDraw).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("A Nian revival lost its exact actual draw or replacement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { NianRevival = receipt with { Stage = NianRevivalStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter, Count = outcome.ActualDrawIds.Count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NianRevivalSegmentEvent(frame.Id, receipt.TargetSeat, NianRevivalStage.DrawChildren, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count));
        ContinueNianRevival(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27930)]
    internal bool ResumeNianRevival(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { NianRevival: not null } f || f.Id != id)
            return false;
        ContinueNianRevival(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27930)]
    internal bool ReturnNianRevivalMovement(ProgramSkillFrame f) => f.NianRevival is not null && ResumeNianRevival(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27930)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27930)]
    internal bool CanCompleteNianRevival(ProgramSkillFrame f)
    {
        if (f.NianRevival is null)
            return false;
        RequireNianRevival(f);
        return true;
    }

    internal void RequireNianRevival(ProgramSkillFrame f)
    {
        if (f.NianRevival is not { } r || f.SkillId != NianReviveSkill || f.ActivationId != "revive" || f.TriggerId is not null || r.InstructionIndex != f.InstructionIndex || f.SelectedCardIds.Count != 0 || !f.SelectedTargetSeats.SequenceEqual([r.TargetSeat]) || !IsNianAlliedSeat(r.TargetSeat) || r.MaximumHp <= 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Count < 0 || !Enum.IsDefined(r.Stage) || r.UsedRevives <= 0 || r.UsedRevives > (_coreNianChallengeHostOperations.NianFeature?.SharedRevives ?? 0))
            throw new InvalidOperationException("Nian revival lost its exact owning instruction and original participant.");
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NianRevivalPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (paid is not [var payment] || payment.ActorSeat != f.OwnerSeat || payment.TargetSeat != r.TargetSeat || payment.UsedRevives != r.UsedRevives || payment.MaximumHp != r.MaximumHp || NianUsedRevives < r.UsedRevives)
            throw new InvalidOperationException("Nian revival cannot resume without its once-consumed team payment.");
        var segment = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NianRevivalSegmentEvent>().Where(e => e.FrameId == f.Id && e.Stage == r.Stage).ToArray();
        // Only the owning operation can be observed between payment and issuing
        // its scalar movement receipt; event observers do not execute children.
        if (segment.Length == 0 && r.Before == r.After && r.Count == 0)
            return;
        if (segment is not [var issued] || issued.TargetSeat != r.TargetSeat || issued.Before != r.Before || issued.After != r.After || issued.Count != r.Count)
            throw new InvalidOperationException("Nian revival lost its paid native movement segment.");
        var movements = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).ToArray();
        if (r.Stage == NianRevivalStage.DiscardChildren && movements.Count(m => m.From.OwnerSeat == r.TargetSeat && m.To == CardLocation.DiscardPile && m.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalDefinitions.NianRevivalDiscard) != r.Count || r.Stage == NianRevivalStage.DrawChildren && movements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.TargetSeat) && m.Reason == global::CardGame.Core.EngineOwners.CoreGameEngineNianRevivalDefinitions.NianRevivalDraw) != r.Count)
            throw new InvalidOperationException("Nian revival's physical discard/draw invoice changed while suspended.");
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27930)]
    internal void AssertNianRevival(ProgramSkillFrame f)
    {
        if (f.NianRevival is not null)
            RequireNianRevival(f);
    }
}
