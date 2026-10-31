#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhenMobileShiRuntime
{
    internal bool ValidShiCaoZ(ProgramSkillFrame f) => f.ShiCaoZhen is { } r && f.SkillId == ShiCaoZSifeng && f.InstructionIndex == 1 && r.Source == ShiCaoZSource(f) && r.Hash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == r.Hash && r.Turn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && r.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && r.ParentId == f.WindowContext?.ParentFrameId && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == r.Operation && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZStartedEvent>().Count(e => e == new ShiCaoZStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.ParentId, r.Operation)) == 1;
    internal bool ShiCaoZMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ShiCaoZhen is { Paid: true } r && ValidShiCaoZ(f) && e?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is ShiCaoZStage.TopChildren or ShiCaoZStage.AssignmentChildren or ShiCaoZStage.UseRemovalChildren or ShiCaoZStage.DiscardChildren or ShiCaoZStage.EndChildren or ShiCaoZStage.CleanupChildren;
    internal bool ShiCaoZOwnMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ShiCaoZhen!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == ShiCaoZStage.TopChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == ShiCaoZTopReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        if (r.Stage == ShiCaoZStage.AssignmentChildren)
            return m.From == CardLocation.Processing && m.To.Zone == CardZoneKind.PrivatePersistentPile && m.Reason.Value == ShiCaoZPlaceReason && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiCaoZPlacedEvent>().Any(e => e.Material.SetupId == f.Id && e.Material.CardId == m.CardId && e.Material.Sequence == m.Sequence);
        if (r.Stage == ShiCaoZStage.DiscardChildren)
            return m.From == CardLocation.Hand(r.Holder) && m.To.Zone is CardZoneKind.DiscardPile or CardZoneKind.OutsideGame && m.Reason.Value == ShiCaoZDiscardReason;
        if (r.Stage == ShiCaoZStage.CleanupChildren)
            return r.Pool.Contains(m.CardId) && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason.Value == ShiCaoZRemoveReason;
        return r.Materials.Any(c => c.CardId == m.CardId && c.Location == m.From) && (m.To == CardLocation.DiscardPile && m.Reason.Value == ShiCaoZRemoveReason || m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ShiCaoZObtainReason);
    }

    internal bool ShiCaoZFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidShiCaoZ(f) || f.ShiCaoZhen is not { Paid: true } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && ShiCaoZMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => ShiCaoZOwnMovement(f, m));
        if (child is DamageFrame damage)
            return r.Stage == ShiCaoZStage.DamageChildren && damage.ParentFrameId == f.Id && damage.SourceSeat == f.OwnerSeat && f.AttackAttempt is { } a && damage.TargetSeat == a.TargetSeat && damage.Amount == a.DamageAmount;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == ShiCaoZStage.DamageChildren && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && f.AttackAttempt is not null;
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ShiCaoZChildEdge(int i) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiCaoZFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiCaoZChildSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => this.ShiCaoZChildEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, dying));
    internal ProgramSkillFrame? ShiCaoZObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.ShiCaoZFirstChild(f, child), (i) => this.ShiCaoZChildSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 49900)]
    internal bool ShiCaoZDyingObserver() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && ShiCaoZObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 49900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 49900)]
    internal bool ShiCaoZRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiCaoZFirstChild(f, recovery))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return i >= 0 && child > i + 1 && ShiCaoZFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && ShiCaoZChildSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 49900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 49900)]
    internal bool KeepShiCaoZIssued(ProgramSkillFrame f) => f.ShiCaoZhen is { Paid: true } && ValidShiCaoZ(f) || f.SkillId == ShiCaoZSifeng && f.ShiCaoZhen is null && f.InstructionIndex is 0 or 1 && f.WindowContext is { } context && IsIssuedShiCaoZCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 49900)]
    internal bool AdvanceShiCaoZChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiCaoZObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
