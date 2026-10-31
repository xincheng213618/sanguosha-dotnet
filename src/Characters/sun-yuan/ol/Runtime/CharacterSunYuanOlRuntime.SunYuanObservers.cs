#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunYuanOlRuntime
{
    internal static bool SunYuanMovementStage(SunYuanStage s) => global::CardGame.Core.RuntimeSkills.RuntimeRules_sun_yuan_ol_SunYuanObservers.SunYuanMovementStage(s);
    internal bool IsSunYuanPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.SunYuan!;
        if (m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<SunYuanEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Step == r.Step && e.Stage == r.Stage && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence))
            return true;
        return r.Grain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(r.Holder) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.Materials.Any(c => c.Kind == CardKind.WoodenOx && c.From == CardLocation.Equipment(r.Holder));
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 190)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 180)]
    internal bool IsSunYuanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.SunYuan is { } r && r.Stage is SunYuanStage.ObtainChildren or SunYuanStage.DiscardChildren && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == r.Holder && recovery.Attempt.TargetSeat == r.Holder && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Materials.Any(c => c.Kind == CardKind.SilverLion && c.From == CardLocation.Equipment(r.Holder)) && recovery.Attempt.Completion.MoveReason?.Value == (r.Stage == SunYuanStage.ObtainChildren ? SayingObtainReason : DongxinDiscardReason);
    internal bool SunYuanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.SunYuan is not { Paid: true } r)
            return false;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (_coreSayingTieredSuppliesOperations.SayingEquipmentSlotPaymentRidesOn(f, child))
            return true;
        if (child is ProgramSkillFrame { HuanShuJun: not null } huanpei)
            return _characterHuanShuJunOlOperations.HuanpeiDraftRidesOn(huanpei, f);
        if (child is CardUseFrame use)
            return r.Stage == SunYuanStage.NativeUse && use.SayingSlash == r.Native && IsIssuedSayingUse(use);
        if (child is DrawFundedDistinctBasicFrame funded)
            return IsSayingSlashRequest(f) && TryGetSayingSlashRequest(f, out var actor, out var targets) && funded.Payment.ParentFrameId == f.Id && funded.Payment.RequestFrameId == f.Id && funded.Payment.Cursor == f.InstructionIndex && funded.Payment.Source.OwnerSeat == actor && funded.Payment.TargetSeat is { } target && targets.Contains(target) && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(funded.Payment);
        if (child is ChainedStateBasicFrame chained)
            return IsSayingSlashRequest(f) && TryGetSayingSlashRequest(f, out var actor, out var targets) && chained.Payment.ParentFrameId == f.Id && chained.Payment.RequestFrameId == f.Id && chained.Payment.Cursor == f.InstructionIndex && chained.Payment.Source.OwnerSeat == actor && chained.Payment.TargetSeat is { } target && targets.Contains(target) && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chained.Payment);
        if (child is ProgramSkillFrame { Weijie: { } wj } draft)
            return IsSayingSlashRequest(f) && TryGetSayingSlashRequest(f, out var actor, out var targets) && wj.Need.OwnerFrameId == f.Id && wj.Need.RequestFrameId == f.Id && wj.Need.ProviderSeat == actor && wj.Need.Targets.SequenceEqual(targets) && _characterGuoTuOlOperations.WeijieParent(draft);
        if (child is ProgramSkillFrame { Zonghu: { } zh } gift)
            return IsSayingSlashRequest(f) && TryGetSayingSlashRequest(f, out var actor, out var targets) && gift.ParentFrameId == f.Id && zh.Need.OwnerFrameId == f.Id && zh.Need.RequestFrameId == f.Id && zh.Need.InstructionIndex == f.InstructionIndex && zh.Need.RequestOp == r.Op && zh.Need.ActorSeat == actor && zh.Need.ProviderSeat == actor && zh.Need.Targets.SequenceEqual(targets) && _characterLiuZhangOlOperations.ZonghuParent(gift);
        if (child is DamageFrame damage)
            return r.Stage == SunYuanStage.DamageChildren && damage.ParentFrameId == f.Id && f.AttackAttempt is { } a && damage.SourceSeat == a.SourceSeat && damage.TargetSeat == a.TargetSeat && damage.Amount == a.DamageAmount;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == SunYuanStage.DamageChildren && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && f.AttackAttempt is not null;
        if (child is RecoveryReplacementFrame recovery)
            return IsSunYuanRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return SunYuanMovementStage(r.Stage) && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return r.Stage == SunYuanStage.NativeChildren ? moved.ResumeProgramFrameId == f.Id && IsSayingNativeSettlementBatch(f, moved.Batch) : SunYuanMovementStage(r.Stage) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsSunYuanPaidMovement(f, m));
    }

    internal ProgramSkillFrame? SunYuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterSunYuanOlOperations.SunYuanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27490)]
    internal bool AdvanceSunYuanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SunYuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
