#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingMouMobileRuntime
{
    internal bool ValidMouHy(ProgramSkillFrame f)
    {
        if (f.MouHuangYueyingMobile is not { } r || r.Source != MouHySource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || r.After < r.Before || !Enum.IsDefined(r.Stage))
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveMouHuangYueyingJizhi)
            return r.Source.SkillId == MouHuangYueyingMobileComposition.Jizhi && r.RecipientSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangYueyingJizhiStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.WindowId == r.WindowId && e.ActualTurn == r.ActualTurn && e.TurnOwnerSeat == r.TurnOwnerSeat) == 1;
        if (r.Operation == SkillProgramEffectOp.PlaceMouHuangYueyingQicaiEquipment)
            return r.Source.SkillId == MouHuangYueyingMobileComposition.Qicai && r.Phase is { } p && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangYueyingQicaiReservedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.RecipientSeat == r.RecipientSeat && e.Phase == p) == 1 && (!r.Paid || r.CardId is not null && r.From is not null);
        return r.Operation == SkillProgramEffectOp.TransferMouHuangYueyingQicaiGains && r.Source.SkillId == MouHuangYueyingMobileComposition.Qicai && r.Source.BindingId == "receive-three" && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHuangYueyingQiGainClaimedEvent>().Count(e => e.GrantId == r.GrantId && e.GainSequence == r.ClaimSequence && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.CardId == r.CardId && e.GainingSeat == r.RecipientSeat && e.BatchId == r.WindowId) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31500)]
    internal bool ContinuePaidMouHy(ProgramSkillFrame f) => f.MouHuangYueyingMobile is { Paid: true } && ValidMouHy(f) || f.WindowContext is { } context && IsIssuedMouHyQiCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), context);
    [ProgramContinuation(ProgramContinuationKind.Resume, 31500)]
    internal bool ResumeMouHy(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouHuangYueyingMobile: not null } f || f.Id != id)
            return false;
        ContinueMouHy(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31500)]
    internal bool ReturnMouHyMovement(ProgramSkillFrame f) => f.MouHuangYueyingMobile is not null && ResumeMouHy(f.Id);
    internal bool MouHyFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouHy(f))
            return false;
        var r = f.MouHuangYueyingMobile!;
        if (f.NativeDraw is not null && CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return true;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
        return child is CardsMovedTriggerWindowFrame moved && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal bool MouHyObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && this.MouHyFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouHyObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouHyFirstChild(f, child), (i) => this.MouHyObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31500)]
    internal bool MouHyDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } d && MouHyObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31500)]
    internal bool MouHyRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMouHy(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && n > i + 1 && MouHyFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MouHyObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[n - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31500)]
    internal bool AdvanceMouHyChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouHyObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
