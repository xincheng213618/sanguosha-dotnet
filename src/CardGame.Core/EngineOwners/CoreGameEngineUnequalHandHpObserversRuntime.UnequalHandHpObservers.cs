#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineUnequalHandHpObserversRuntime
{
    internal bool UnequalHandHpFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.UnequalHandHp is not { Payments.Count: > 0 } r || root.PendingMovementContinuation is null)
            return false;
        _coreUnequalHandHpEndingOperations.RequireUnequalHandHpReceipt(root);
        var p = r.Payments[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime.UnequalHandHpReason(root, p.Step));
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        var lion = p.Step == "gift" && p.From == CardLocation.Equipment(root.OwnerSeat) && p.CardId is { } paid && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > p.Before && m.Sequence <= p.After && m.CardId == paid && m.CardKind == CardKind.SilverLion && m.From == p.From && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime.UnequalHandHpReason(root, p.Step));
        if (!lion)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason?.Value == global::CardGame.Core.EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime.UnequalHandHpReason(root, p.Step);
    }

    internal ProgramSkillFrame? UnequalHandHpObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreUnequalHandHpObserversOperations.UnequalHandHpFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasHandGainDebtObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 210)]
    internal bool IsUnequalHandHpDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && UnequalHandHpObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1760)]
    internal bool HasUnequalHandHpDamageObserver(long id) => UnequalHandHpObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 40)]
    internal bool TryAdvanceUnequalHandHpSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && UnequalHandHpObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
