#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHouChengAnniversaryRuntime
{
    internal bool ValidHouCheng(ProgramSkillFrame f)
    {
        if (f.AnniversaryHouCheng is not { } r || !HouChengParent(f) || r.Source != HouChengSource(f) || r.Hash != f.GameplayHash || r.Hash != _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash || r.ParentId != f.WindowContext?.ParentFrameId || r.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || r.Turn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Obtained is < 0 or > 2 || r.Selected.Count > 2 || r.Selected.Distinct().Count() != r.Selected.Count || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHouChengStartedEvent>().Count(e => e == new AnniversaryHouChengStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Round, r.Turn, r.TurnOwner)) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == f.WindowContext!.Window) != 1)
            return false;
        if (r.Stage != AnniversaryHouChengStage.Source && r.Victim != -1 && !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Victim) || r.Recipient != -1 && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Recipient) || r.Recipient == r.Victim || r.Recipient == f.OwnerSeat))
            return false;
        var obtains = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHouChengMovementEvent>().Where(e => e.FrameId == f.Id && !e.Giving).ToArray();
        if (obtains.Length > 1 || obtains.Any(e => !ValidHouChengMovement(e) || e.CardOwner != r.Victim || e.Recipient != f.OwnerSeat || e.ActualCount != r.Obtained) || obtains.Length == 0 && r.Obtained != 0 || r.Stage is AnniversaryHouChengStage.Recipient or AnniversaryHouChengStage.Give or AnniversaryHouChengStage.GiveChildren && (obtains.Length != 1 || r.Obtained == 0) || r.Stage == AnniversaryHouChengStage.Give && r.Selected.Count > r.Obtained || r.Stage is AnniversaryHouChengStage.Source or AnniversaryHouChengStage.Obtain && obtains.Length != 0)
            return false;
        if (r.Stage is AnniversaryHouChengStage.ObtainChildren or AnniversaryHouChengStage.GiveChildren)
        {
            var giving = r.Stage == AnniversaryHouChengStage.GiveChildren;
            return f.PendingMovementContinuation is not null && r.BatchId > 0 && r.Moves.Count == r.Selected.Count && r.Moves.Select(m => m.CardId).Order().SequenceEqual(r.Selected.Order()) && r.Moves.All(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHouChengMovementEvent>().Where(e => e.FrameId == f.Id && e.Giving == giving).ToArray()is [var paid] && ValidHouChengMovement(paid) && paid.Before == r.Before && paid.After == r.After && paid.BatchId == r.BatchId && paid.CardOwner == (giving ? f.OwnerSeat : r.Victim) && paid.Recipient == (giving ? r.Recipient : f.OwnerSeat) && (giving || paid.ActualCount == r.Obtained);
        }

        if (r.Stage == AnniversaryHouChengStage.HpChildren && r.HpPaid)
            return r.HpBefore - r.HpAfter == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryHouChengHpPaidEvent>().Count(e => e == new AnniversaryHouChengHpPaidEvent(f.Id, r.HpBefore, r.HpAfter, 1)) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == f.OwnerSeat && e.Amount == 1 && e.RemainingHp == r.HpAfter) == 1;
        return f.PendingMovementContinuation is null && !r.HpPaid;
    }
    internal bool HouChengMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => effect?.Op == SkillProgramEffectOp.ResolveAnniversaryHouChengXianniang && f.AnniversaryHouCheng is { Stage: AnniversaryHouChengStage.ObtainChildren or AnniversaryHouChengStage.GiveChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidHouCheng(f);
    internal bool HouChengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidHouCheng(f))
            return false;
        var r = f.AnniversaryHouCheng!;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && HouChengMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.All(r.Moves.Contains);
        if (child is DyingFrame dying)
            return r.Stage == AnniversaryHouChengStage.HpChildren && r.HpPaid && dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.KillerSeat is null && dying.Continuation == DyingContinuationKind.ProgramSkill;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && r.Stage == AnniversaryHouChengStage.HpChildren && r.HpPaid && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool HouChengSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame f && this.HouChengFirstChild(f, _resolutionStackOperations2._resolutionStack()[i]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i, dying) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(i, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(i, dying));
    internal ProgramSkillFrame? HouChengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HouChengFirstChild(f, c), (i) => this.HouChengSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 48700)]
    internal bool HouChengDyingObserver() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && HouChengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 48700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 48700)]
    internal bool HouChengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        var a = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == child.Id);
        return ValidHouCheng(f) && a >= 0 && b > a && HouChengFirstChild(f, _resolutionStackOperations._resolutionStack[a + 1]) && (b == a + 1 || HouChengSuffix(a)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 48700)]
    internal bool AdvanceHouChengChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HouChengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
