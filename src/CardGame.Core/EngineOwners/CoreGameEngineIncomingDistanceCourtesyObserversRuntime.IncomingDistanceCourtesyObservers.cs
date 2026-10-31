#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyObserversRuntime
{
    internal bool IncomingDistanceCourtesyFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.IncomingDistanceCourtesy is not { Stage: IncomingDistanceCourtesyStage.MovementChildren, Movements.Count: > 0 } r || root.PendingMovementContinuation is null)
            return false;
        _coreIncomingDistanceCourtesyOperations.RequireIncomingDistanceCourtesyReceipt(root);
        var m = r.Movements[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(move) && move.Sequence > m.Before && move.Sequence <= m.After && move.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime.IncomingDistanceCourtesyReason(root, m.StepIndex));
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        var courtesyRecovery = r.Kind == IncomingDistanceCourtesyKind.Courtesy && r.Benefits[r.NextBenefitIndex] == EndingCourtesyBenefit.TurnOwnerRecoverOne;
        var lionRecovery = r.Kind == IncomingDistanceCourtesyKind.FactionDistance && m.From == CardLocation.Equipment(root.OwnerSeat) && m.CardId is { } id && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.CardId == id && move.CardKind == CardKind.SilverLion && move.Sequence > m.Before && move.Sequence <= m.After && move.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime.IncomingDistanceCourtesyReason(root, 0));
        if (!courtesyRecovery && !lionRecovery)
            return false;
        var source = root.OwnerSeat;
        var target = courtesyRecovery ? r.ActualTurnOwnerSeat : root.OwnerSeat;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == source && hp.Change.TargetSeat == target && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == source && recovery.Attempt.TargetSeat == target && recovery.Attempt.Amount == 1 && (courtesyRecovery ? recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1 : recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime.IncomingDistanceCourtesyReason(root, 0));
    }

    internal ProgramSkillFrame? IncomingDistanceCourtesyObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreIncomingDistanceCourtesyObserversOperations.IncomingDistanceCourtesyFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasHandGainDebtObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 240)]
    internal bool IsIncomingDistanceCourtesyDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && IncomingDistanceCourtesyObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1790)]
    internal bool HasIncomingDistanceCourtesyDamageObserver(long id) => IncomingDistanceCourtesyObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 70)]
    internal bool TryAdvanceIncomingDistanceCourtesySubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && IncomingDistanceCourtesyObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
