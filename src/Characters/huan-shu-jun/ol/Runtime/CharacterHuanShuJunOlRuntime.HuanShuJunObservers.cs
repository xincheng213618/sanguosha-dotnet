#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuanShuJunOlRuntime
{
    internal static string HuanShuJunDecisionHash(PendingDecision p) => RuntimeSkills.RuntimeRules_huan_shu_jun_ol_HuanShuJunObservers.HuanShuJunDecisionHash(p);
    internal bool HuanShuJunPaidOrPreparing(ProgramSkillFrame f) => f.HuanShuJun is not null && HuanShuJunComposition.Handles(f.HuanShuJun.Op) && f.HuanShuJun.Source == HuanShuJunSource(f) && f.HuanShuJun.GameplayHash == f.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuanShuJunStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == f.HuanShuJun.Source && e.GameplayHash == f.GameplayHash && e.Op == f.HuanShuJun.Op && e.WindowId == f.HuanShuJun.WindowId) == 1;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28100)]
    internal bool ContinuePaidHuanShuJun(ProgramSkillFrame f)
    {
        if (f.HuanShuJun is not { Paid: true })
            return false;
        return true;
    }
    internal bool HuanShuJunPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.HuanShuJun!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuanpeiCostEntityEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.Sequence == m.Sequence) && m.Reason.Value == HuanpeiDiscardReason)
            return true;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenyiEquipmentGivenEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Sequence == m.Sequence && m.To == CardLocation.Hand(e.RecipientSeat)) && m.Reason.Value == "skill-program.huan-shu-jun.wenyi.give")
            return true;
        if (r.DrawOutcome is { } o && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter)
            return o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == (r.Op == SkillProgramEffectOp.ResolveMiaoxinDamageDrawAndRefresh ? MiaoxinDrawReason : HuanpeiDrawReason) || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        return r.Stage == HuanShuJunStage.EquipmentGiftChildren && r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(f.OwnerSeat)) && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.Sequence > r.Before && m.Sequence <= r.After;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 320)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 310)]
    internal bool IsHuanShuJunRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.HuanShuJun is { Paid: true, Stage: HuanShuJunStage.EquipmentGiftChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(f.OwnerSeat)) && recovery.Attempt.Completion.MoveReason?.Value == "skill-program.huan-shu-jun.wenyi.give";
    internal bool HuanShuJunFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HuanShuJun is not { } r)
            return false;
        if (child is ProgramSkillFrame draft && HuanpeiDraftRidesOn(draft, f))
            return true;
        if (_coreWenyiPeachSuppliesOperations.WenyiEquipmentSlotPaymentRidesOn(f, child))
            return true;
        if (child is DrawFundedDistinctBasicFrame funded)
            return _coreWenyiPeachSuppliesOperations.WenyiDrawFundedParentMatches(f, funded.Payment) && _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicFrameRidesOn(child, f);
        if (child is RequestedDeckBasicFrame view)
            return view.Receipt.Intent == RequestedDeckBasicIntent.WenyiPeach && IsWenyiPeachRequest(f) && _coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicFrameRidesOn(child, f);
        if (child is ProgramSkillFrame { Weijie: { } w } weijie)
            return r.Op == SkillProgramEffectOp.ResolveWenyiHealthOneAid && w.Need.OwnerFrameId == f.Id && w.Need.RequestFrameId == f.Id && w.Need.InstructionIndex == f.InstructionIndex && w.Need.RequestOp == r.Op && w.Need.ActorSeat == f.OwnerSeat && w.Need.ProviderSeat == f.OwnerSeat && w.Need.Targets.SequenceEqual([r.TargetSeat]) && _characterGuoTuOlOperations.WeijieParent(weijie);
        if (!r.Paid)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == HuanShuJunStage.NativeUse && r.Native?.UseId == use.Id && IsIssuedWenyiUse(use);
        if (r.Stage == HuanShuJunStage.Drawing && f.NativeDraw is not null)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is RecoveryReplacementFrame recovery)
            return IsHuanShuJunRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is not CardsMovedTriggerWindowFrame moved || !CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
            return false;
        if (r.Stage == HuanShuJunStage.NativeChildren && r.NativeReturned && r.Native is { } native)
            return moved.Batch.ParentFrameId == native.UseId && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenyiNativeReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == native.UseId && e.ActionId == native.ActionId) == 1;
        return moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.All(m => HuanShuJunPaidMovement(f, m));
    }

    internal bool HuanShuJunReplacementObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        return parent is ProgramSkillFrame { Zenrun: not null } replacement && _characterSunHongOlOperations.ZenrunFirstChild(replacement, child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child);
    }

    internal bool HuanShuJunObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || this.HuanShuJunReplacementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? HuanShuJunObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.HuanShuJunFirstChild(f, c), (i) => this.HuanShuJunObserverSuffix(i));

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28100)]
    internal bool AdvanceHuanShuJunChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuanShuJunObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
