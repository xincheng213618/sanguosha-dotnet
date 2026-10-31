#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKeBiNengOlRuntime
{
    internal bool KeBiNengCardPaymentStage(KeBiNengPingduanReceipt r) => r.Stage is KeBiNengPingduanStage.RecastChildren or KeBiNengPingduanStage.EquipmentChildren;
    internal CardMoveReason KeBiNengPaymentReason(KeBiNengPingduanReceipt r) => r.Stage == KeBiNengPingduanStage.RecastChildren ? CardMoveReasons.RecastDiscard : new(PingduanEquipmentReason);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 890)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 830)]
    internal bool IsKeBiNengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.KeBiNengPingduan is { SelectedCard: { } c } r && KeBiNengCardPaymentStage(r) && ValidKeBiNeng(f) && c.From == CardLocation.Equipment(r.ActorSeat) && _coreCardAttackStateOperations.GetAttackCard(c.CardId).Kind == CardKind.SilverLion && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == KeBiNengPaymentReason(r) && recovery.Attempt.SourceSeat == r.ActorSeat && recovery.Attempt.TargetSeat == r.ActorSeat && recovery.Attempt.Amount == 1;
    internal bool KeBiNengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.KeBiNengPingduan is not { } r || !ValidKeBiNeng(f))
            return false;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.Batch.ParentFrameId != f.Id || moved.ResumeProgramFrameId != f.Id || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
                return false;
            return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (KeBiNengCardPaymentStage(r) ? r.SelectedCard is { } c && moved.Batch.Id == r.PaidBatchId && m.CardId == c.CardId && m.From == c.From && m.Reason == KeBiNengPaymentReason(r) && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreCardAttackStateOperations.GetAttackCard(c.CardId), c.From, (r.Stage == KeBiNengPingduanStage.RecastChildren ? _coreCardAttackStateOperations.GetAttackCard(c.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat))) || moved.Batch.ParentBatchId == r.PaidBatchId && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(r.ActorSeat) && m.To == CardLocation.DiscardPile && r.WoodenOxGrainIds.Contains(m.CardId) : r.Stage is KeBiNengPingduanStage.BasicRewardChildren or KeBiNengPingduanStage.RecastDrawChildren or KeBiNengPingduanStage.RecastRewardChildren or KeBiNengPingduanStage.EquipmentRewardChildren && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.ActorSeat) && m.Reason == (r.Stage == KeBiNengPingduanStage.RecastDrawChildren ? CardMoveReasons.RecastDraw : new CardMoveReason(PingduanRewardReason)) || m.Reason == CardMoveReasons.Reshuffle)));
        }

        if (child is RecoveryReplacementFrame recovery)
            return IsKeBiNengRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return KeBiNengCardPaymentStage(r) && r.SelectedCard is { } c && c.From == CardLocation.Equipment(r.ActorSeat) && _coreCardAttackStateOperations.GetAttackCard(c.CardId).Kind == CardKind.SilverLion && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.ActorSeat && hp.Change.TargetSeat == r.ActorSeat && hp.Change.Amount == 1;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? KeBiNengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterKeBiNengOlOperations.KeBiNengFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27040)]
    internal bool AdvanceKeBiNengSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && KeBiNengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
