#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHaoOlRuntime
{

    internal bool IsSunHaoPaymentMovement(ProgramSkillFrame f, CardMovementRecord m) => f.SunHaoCanshi is { Payment: { } p } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > p.Before && m.Sequence <= p.After && (m.CardId == p.CardId && m.From == p.From && m.To == p.To && m.Reason.Value == SunHaoCanshiDiscardReason || p.Kind == CardKind.WoodenOx && p.From == CardLocation.Equipment(f.OwnerSeat) && r.WoodenOxGrain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 310)]
    internal bool IsSunHaoMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && (f.SunHaoCanshi is { Stage: SunHaoCanshiStage.DiscardChildren, Payment: not null } && e?.Op == SkillProgramEffectOp.ResolveSunHaoCanshiUseDiscard || f.MobileSunHaoDraw is { Outcome: not null } && e?.Op == SkillProgramEffectOp.GrantSunHaoCanshiDrawObligation);
    internal bool SunHaoLion(ProgramSkillFrame f) => f.SunHaoCanshi is { Payment: { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } };
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 110)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 100)]
    internal bool IsSunHaoRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => SunHaoLion(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == SunHaoCanshiDiscardReason;
    internal bool SunHaoFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileSunHaoDraw is not null)
            return _characterSunHaoMobileOperations.MobileSunHaoDrawFirstChild(f, child);
        if (f.SunHaoCanshi is not { Stage: SunHaoCanshiStage.DiscardChildren } r)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is RecoveryReplacementFrame recovery)
            return IsSunHaoRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return SunHaoLion(f) && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsSunHaoPaymentMovement(f, m));
    }

    internal ProgramSkillFrame? SunHaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterSunHaoOlOperations.SunHaoFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 790)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 900)]
    internal bool HasSunHaoDamageObserver(long id) => SunHaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 930)]
    internal bool HasSunHaoBeforeDamageObserver(long id) => SunHaoObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27330)]
    internal bool AdvanceSunHaoObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SunHaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
