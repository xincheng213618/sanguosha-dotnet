#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseStartCardControlObserversRuntime
{
    internal bool PhaseStartCardControlFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.PhaseStartCardControl is not { Payment: { } payment } receipt || !_corePhaseStartCardControlOperations.HasExactPhaseStartCardControl(root))
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count && (receipt.Stage == PhaseStartCardControlStage.MovementChildren && root.PendingMovementContinuation is not null || receipt.Stage == PhaseStartCardControlStage.GrantChildren && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OtherPlayRestrictionGrantedEvent>().Any(e => e.ProgramFrameId == root.Id && e.Source == receipt.Source));
        if (receipt.Stage != PhaseStartCardControlStage.MovementChildren || root.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.CardId == payment.CardId && m.From == payment.From && m.To == payment.To && m.Sequence > payment.SequenceBefore && m.Sequence <= payment.SequenceAfter && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlRuntime.PhaseStartControlReason(receipt.Kind));
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (payment.Kind != CardKind.SilverLion || payment.From.Zone != CardZoneKind.Equipment || payment.From.OwnerSeat is not { } lionOwner)
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == lionOwner && hp.Change.TargetSeat == lionOwner && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == lionOwner && recovery.Attempt.TargetSeat == lionOwner && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason?.Value == global::CardGame.Core.EngineOwners.CoreGameEnginePhaseStartCardControlRuntime.PhaseStartControlReason(receipt.Kind);
    }

    internal ProgramSkillFrame? PhaseStartCardControlObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _corePhaseStartCardControlObserversOperations.PhaseStartCardControlFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasHandGainDebtObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 110, group: 10)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 220)]
    internal bool IsPhaseStartCardControlDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && PhaseStartCardControlObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1770)]
    internal bool HasPhaseStartCardControlDamageObserver(long id) => PhaseStartCardControlObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 50)]
    internal bool TryAdvancePhaseStartCardControlSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && PhaseStartCardControlObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 20)]
    internal bool IsPhaseStartCardControlMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.PhaseStartCardControl is { Stage: PhaseStartCardControlStage.MovementChildren, Payment: { } payment } r && effect?.Op == (r.Kind == PhaseStartCardControlKind.OtherPlayRestriction ? SkillProgramEffectOp.GrantOtherPlayUseRestriction : SkillProgramEffectOp.ChooseStartingEquipmentControl) && pending == f.PendingMovementContinuation && pending.SubjectSeat == payment.From.OwnerSeat && pending.CoverageResultBind is null && _corePhaseStartCardControlOperations.HasExactPhaseStartCardControl(f);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 80)]
    internal void AssertPhaseStartCardControl(ProgramSkillFrame f)
    {
        if (f.PhaseStartCardControl is not { } r)
            return;
        if (!_corePhaseStartCardControlOperations.HasExactPhaseStartCardControl(f) || r.Stage == PhaseStartCardControlStage.MovementChildren && f.PendingMovementContinuation is null || r.Stage != PhaseStartCardControlStage.MovementChildren && f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Phase-starting card control lost its exact source, boundary or paid child stage.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && r.Stage == PhaseStartCardControlStage.Choosing && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != f.OwnerSeat || prompt.IsPrivate != (r.Kind == PhaseStartCardControlKind.OtherPlayRestriction) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, _corePhaseStartCardControlOperations.PhaseStartCardControlChoices(f))))
            throw new InvalidOperationException("Phase-starting card control lost its exact owning choice.");
    }
}
