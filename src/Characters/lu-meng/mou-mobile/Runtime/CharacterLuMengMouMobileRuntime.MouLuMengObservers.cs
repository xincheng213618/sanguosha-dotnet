#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengMouMobileRuntime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29200)]
    internal bool KeepPaidMouLuMeng(ProgramSkillFrame f) => f.MouLuMengKeji is not null && ValidMouKeji(f) || f.MouLuMengDujiang is not null && ValidMouDujiang(f) || f.MouLuMengDuojing is { Stage: MouLuMengDuojingStage.Taking or MouLuMengDuojingStage.MovementChildren or MouLuMengDuojingStage.Complete } && ValidMouDuojing(f);

    internal bool MouLuMengObserverOrigin(ProgramSkillFrame f) => f.MouLuMengKeji is not null && ValidMouKeji(f) || f.MouLuMengDujiang is not null && ValidMouDujiang(f) || f.MouLuMengDuojing is { Stage: MouLuMengDuojingStage.MovementChildren } && ValidMouDuojing(f);
    internal bool MouLuMengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouLuMengObserverOrigin(f))
            return false;
        if (CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && (f.MouLuMengKeji is null || hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1 && f.InstructionIndex == 2);
        if (child is DyingFrame dying)
            return f.MouLuMengKeji is not null && f.ActivationId == "lose-hp" && f.InstructionIndex == 2 && dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.Continuation == DyingContinuationKind.ProgramSkill && MouKejiCostWasPaid(f);
        if (child is not CardsMovedTriggerWindowFrame moved || !CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
            return false;
        if (f.MouLuMengDuojing is { Stage: MouLuMengDuojingStage.MovementChildren } r)
            return r.TakenCardId is { } card && r.MovementSequence is { } sequence && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.CardId == card && m.Sequence == sequence && m.From == CardLocation.Hand(r.Targets[r.TargetIndex]) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouDuojingTakeReason);
        return f.MouLuMengKeji is { } cost && f.ActivationId == "discard-hand" && f.InstructionIndex == 2 && f.SelectedCardPayment is { MovementCommitted: true } payment && payment.CardIds.SequenceEqual(f.SelectedCardIds) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > cost.MovementBefore && (m.CardId == cost.SelectedCardId || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)?.OwnerSeat == f.OwnerSeat) && (m.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DiscardSelected)}" || m.Reason.Value.StartsWith($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DiscardSelected)}-", StringComparison.Ordinal)));
    }

    internal bool MouLuMengObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame parent && this.MouLuMengFirstChild(parent, _resolutionStackOperations2._resolutionStack()[child]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? MouLuMengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouLuMengFirstChild(f, child), (index) => this.MouLuMengObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 810)]
    internal bool IsMouLuMengDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouLuMengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 580)]
    internal bool IsMouLuMengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!MouLuMengObserverOrigin(f) || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouLuMengFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouLuMengObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29200)]
    internal bool AdvanceMouLuMengChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouLuMengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
