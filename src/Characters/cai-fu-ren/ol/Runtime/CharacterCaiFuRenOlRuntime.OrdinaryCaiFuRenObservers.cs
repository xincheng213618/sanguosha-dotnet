#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiFuRenOlRuntime
{

    internal bool CanContinueIssuedOrdinaryCaiFuRen(ProgramSkillFrame f)
    {
        if (IsIssuedOrdinaryCaiFuRenXianzhou(f))
            return _coreEquipmentDonationObserversOperations.IsValidAllEquipmentDonation(f);
        if (f.OrdinaryQieting is not { Paid: true })
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27595)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27595)]
    internal bool PermitPaidOrdinaryCaiFuRen(ProgramSkillFrame f) => CanContinueIssuedOrdinaryCaiFuRen(f);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 140)]
    internal bool IsOrdinaryCaiFuRenMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.OrdinaryQieting is { Paid: true, Stage: OrdinaryQietingStage.MovementChildren or OrdinaryQietingStage.DrawChildren } && global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime.IsCaiFuRenQietingOperation(f, effect) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool IsOrdinaryQietingPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.OrdinaryQieting is not { Paid: true } r || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f) && r.Stage == OrdinaryQietingStage.MovementChildren)
            return _characterCaiFuRenBoundaryMobileOperations.IsBoundaryMobileQietingPaidMovement(f, m);
        if (r.Stage == OrdinaryQietingStage.MovementChildren)
            return m.CardId == r.CardId && m.CardKind == r.PrintedKind && m.From == CardLocation.Equipment(r.Qualification.TurnOwnerSeat) && m.To == CardLocation.Equipment(f.OwnerSeat) && m.Reason.Value == OrdinaryQietingMoveReason || r.PrintedKind == CardKind.WoodenOx && r.GrainIds.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(r.Qualification.TurnOwnerSeat) && m.To == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.Reason == CardMoveReasons.WoodenOxTransfer;
        return r.Stage == OrdinaryQietingStage.DrawChildren && r.DrawOutcome is { } o && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == OrdinaryQietingDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    }

    internal bool OrdinaryQietingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.OrdinaryQieting is not { Paid: true } r)
            return false;
        if (r.Stage == OrdinaryQietingStage.Draw && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is RecoveryReplacementFrame recovery)
            return IsOrdinaryCaiFuRenRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame mobileHp && global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f))
            return _characterCaiFuRenBoundaryMobileOperations.IsBoundaryMobileQietingHpChild(f, mobileHp);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == OrdinaryQietingStage.MovementChildren && r.PrintedKind == CardKind.SilverLion && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.Qualification.TurnOwnerSeat && hp.Change.TargetSeat == r.Qualification.TurnOwnerSeat && hp.Change.Amount == 1;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsOrdinaryQietingPaidMovement(f, m));
    }

    internal bool OrdinaryQietingObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? OrdinaryQietingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterCaiFuRenOlOperations.OrdinaryQietingFirstChild(f, child), (index) => _characterCaiFuRenOlOperations.OrdinaryQietingObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1070)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1180)]
    internal bool HasOrdinaryCaiFuRenDamageObserver(long id) => OrdinaryQietingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1210)]
    internal bool HasOrdinaryCaiFuRenBeforeDamageObserver(long id) => OrdinaryQietingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 290)]
    internal bool IsOrdinaryCaiFuRenRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.OrdinaryQieting is not { Paid: true } r)
            return false;
        if (recovery.ParentFrameId == f.Id && global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime.IsBoundaryMobileQieting(f))
            return _characterCaiFuRenBoundaryMobileOperations.IsBoundaryMobileQietingRecovery(f, recovery);
        if (recovery.ParentFrameId == f.Id)
            return r.Stage == OrdinaryQietingStage.MovementChildren && r.PrintedKind == CardKind.SilverLion && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.Qualification.TurnOwnerSeat && recovery.Attempt.TargetSeat == r.Qualification.TurnOwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.CardId == r.CardId && recovery.Attempt.Completion.MoveReason?.Value == OrdinaryQietingMoveReason;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var j = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return i >= 0 && j > i + 1 && OrdinaryQietingFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && OrdinaryQietingObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[j - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27596)]
    internal bool AdvanceOrdinaryQietingChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && OrdinaryQietingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
