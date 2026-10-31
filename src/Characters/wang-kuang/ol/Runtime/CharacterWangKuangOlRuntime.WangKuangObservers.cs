#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangKuangOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 740)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 680)]
    internal bool IsWangKuangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.WangKuangRenxia is { Stage: WangKuangRenxiaStage.MovementChildren, Branch: WangKuangRenxiaBranch.Discard } r && ValidWangKuang(f) && r.Selected.Any(c => c.From == CardLocation.Equipment(f.OwnerSeat) && _coreCardAttackStateOperations.GetAttackCard(c.CardId).Kind == CardKind.SilverLion) && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == RenxiaDiscardReason && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1;
    internal bool WangKuangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WangKuangRenxia is not { Stage: WangKuangRenxiaStage.MovementChildren } r || !ValidWangKuang(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && moved.ResumeProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (r.Branch == WangKuangRenxiaBranch.Draw ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == RenxiaDrawReason || m.Reason == CardMoveReasons.Reshuffle : m.Reason.Value == RenxiaDiscardReason && r.Selected.Any(c => c.CardId == m.CardId && c.From == m.From && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreCardAttackStateOperations.GetAttackCard(c.CardId), c.From, (c.From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(c.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile))) || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && r.WoodenOxGrainIds.Contains(m.CardId)));
        if (child is RecoveryReplacementFrame recovery)
            return IsWangKuangRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Branch == WangKuangRenxiaBranch.Discard && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1 && r.Selected.Any(c => c.From == CardLocation.Equipment(f.OwnerSeat) && _coreCardAttackStateOperations.GetAttackCard(c.CardId).Kind == CardKind.SilverLion);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? WangKuangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterWangKuangOlOperations.WangKuangFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 550)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 660)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 690)]
    internal bool HasWangKuangDamageObserver(long id) => WangKuangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26910)]
    internal bool AdvanceWangKuangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WangKuangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
