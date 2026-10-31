#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunChenMobileShiRuntime
{
    internal bool ValidShiSc(ProgramSkillFrame f) => f.ShiSunChen is { } r && f.InstructionIndex == 1 && r.Source == ShiScSource(f) && r.Hash == f.GameplayHash && r.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && r.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && r.WindowId == f.WindowContext?.ParentFrameId && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && Enum.IsDefined(r.Stage) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiScStartedEvent>().Count(e => e == new ShiScStartedEvent(f.Id, r.Source, r.Hash, r.Op, r.Turn, r.TurnOwner, r.PhaseId, r.WindowId, r.UseId, r.ActionId)) == 1;

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 49100)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 49100)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 49100)]
    internal bool ShiScMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => ValidShiSc(f) && f.ShiSunChen is { Stage: ShiScStage.DiscardChildren or ShiScStage.GiftChildren or ShiScStage.DrawChildren } r && effect?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool ShiScFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidShiSc(f) || f.ShiSunChen is not { } r)
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == ShiScStage.Drawing && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == 1 && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && ShiScMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return r.Stage is ShiScStage.DiscardChildren or ShiScStage.GiftChildren && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardKind == CardKind.SilverLion && m.From.Zone == CardZoneKind.Equipment && m.From.OwnerSeat == recovery.Attempt.TargetSeat);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && r.Stage is ShiScStage.DiscardChildren or ShiScStage.GiftChildren;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool ShiScChildEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiScFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiScChildSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterSunChenMobileShiOperations.ShiScChildEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? ShiScObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterSunChenMobileShiOperations.ShiScFirstChild(f, child), (i) => _characterSunChenMobileShiOperations.ShiScChildSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 49100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 49100)]
    internal bool ShiScDamageObserver(long id) => ShiScObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 49100)]
    internal bool ShiScBeforeDamageObserver(long id) => ShiScObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 49100)]
    internal bool ShiScDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ShiScObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 49100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 49100)]
    internal bool ShiScRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiScFirstChild(f, recovery))
            return true;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return at >= 0 && child > at + 1 && ShiScFirstChild(f, _resolutionStackOperations._resolutionStack[at + 1]) && ShiScChildSuffix(at) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 49100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 49100)]
    internal bool KeepShiScPaid(ProgramSkillFrame f) => f.ShiSunChen is not null && ValidShiSc(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 49100)]
    internal bool AdvanceShiScChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiScObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
