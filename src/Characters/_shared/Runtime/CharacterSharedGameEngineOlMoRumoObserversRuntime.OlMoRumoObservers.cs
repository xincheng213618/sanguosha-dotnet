#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSharedGameEngineOlMoRumoObserversRuntime
{
    internal bool OlMoRumoFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.OlMoRumo is not { } receipt)
            return false;
        if (child is IdentityAppearanceFrame appearance)
            return (receipt.Stage == OlMoRumoStage.LossIssuing || receipt.Stage == OlMoRumoStage.LossChildren && receipt.LossReturned && receipt.AppearancePrevented && _coreProgramEventHistoryOperations.ProgramEventHistory<IdentityHpDeductionPreventedEvent>().Count(item => item.AppearanceFrameId == appearance.Id && item.OwnerSeat == frame.OwnerSeat && item.Entry == IdentityAppearanceEntry.ProgramHpLoss && item.RequestedDeduction == 1) == 1) && !receipt.HadRoundDamage && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == frame.Id && appearance.Receipt.OwnerSeat == frame.OwnerSeat && appearance.Receipt.RequestedDeduction == 1 && appearance.Receipt.OriginalInstructionIndex == frame.InstructionIndex;
        if (child is HpChangedTriggerWindowFrame hp)
            return receipt.Stage == OlMoRumoStage.LossChildren && receipt.LossReturned && !receipt.HadRoundDamage && hp.ResumeFrameId == frame.Id && hp.Change.ParentFrameId == frame.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.TargetSeat == frame.OwnerSeat && hp.Change.Amount == receipt.ActualLoss && hp.Change.HpAfter == receipt.HpAfter && hp.Change.HpBefore - hp.Change.HpAfter == hp.Change.Amount;
        if (child is DyingFrame dying)
            return receipt.Stage == OlMoRumoStage.LossChildren && receipt.LossReturned && !receipt.HadRoundDamage && receipt.HpAfter == 0 && dying.ParentFrameId == frame.Id && dying.VictimSeat == frame.OwnerSeat && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == frame.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(item => item.Change.Id == state.Id && item.Change.ParentFrameId == frame.Id && item.Change.Window == state.Window);
    }

    internal bool OlMoRumoLifecycleEdge(int index) => index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame window && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame frame && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, window.Candidates[window.CandidateIndex]) && frame.WindowContext is { } context && context.ParentFrameId == window.Id && context.Window == window.Window;
    internal bool OlMoRumoObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations.IsMovementObserverEdge(next) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(next) || _characterSharedGameEngineOlMoRumoObserversOperations.OlMoRumoLifecycleEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(next, dying));
    internal ProgramSkillFrame? OlMoRumoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((frame, child) => _characterSharedGameEngineOlMoRumoObserversOperations.OlMoRumoFirstChild(frame, child), (index) => _characterSharedGameEngineOlMoRumoObserversOperations.OlMoRumoObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 390)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 380)]
    internal bool IsOlMoRumoRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery)
    {
        if (frame.OlMoRumo is null)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == recovery.Id);
        return index >= 0 && child > index + 1 && OlMoRumoFirstChild(frame, _resolutionStackOperations._resolutionStack[index + 1]) && OlMoRumoObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28940)]
    internal bool AdvanceOlMoRumoChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && OlMoRumoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
