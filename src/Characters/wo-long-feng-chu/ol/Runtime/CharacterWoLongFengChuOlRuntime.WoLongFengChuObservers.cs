#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWoLongFengChuOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 920)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 860)]
    internal bool IsLuanfengRecoveryParent(ProgramSkillFrame producer, RecoveryReplacementFrame recovery)
    {
        if (recovery.ParentFrameId != producer.Id || recovery.Return != new RecoveryReplacementReturn(PostEventContinuation.Program, producer.Id))
            return false;
        if (producer.Luanfeng is { Stage: LuanfengStage.RecoveryChild } r)
            return r.FrameId == producer.Id && r.GameplayHash == producer.GameplayHash && recovery.Attempt.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.Program, producer.InstructionIndex) && recovery.Attempt.SourceSeat == producer.OwnerSeat && recovery.Attempt.TargetSeat == r.VictimSeat && recovery.Attempt.Amount == r.RecoveryAmount && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LuanfengIssuedEvent>().Count(e => e.FrameId == producer.Id && e.DyingFrameId == r.DyingFrameId && e.Source == r.Source) == 1;
        return producer.EquipmentSlotNamedUse?.Payment is { } p && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == new CardMoveReason("equipment.slot-abolished") && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == producer.OwnerSeat && recovery.Attempt.TargetSeat == producer.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > p.Before && m.Sequence <= p.After && m.From == CardLocation.Equipment(producer.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == "equipment.slot-abolished" && _coreCardAttackStateOperations.GetAttackCard(m.CardId).Kind == CardKind.SilverLion);
    }

    internal bool WoLongFengChuFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.EquipmentSlotNamedUse is not { Payment: not null } && root.Luanfeng is null)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsLuanfengRecoveryParent(root, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && (root.Luanfeng is { } lf ? hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == lf.VictimSeat && hp.Change.Amount == lf.RecoveryAmount : hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1);
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            var before = root.EquipmentSlotNamedUse?.Payment?.Before ?? root.Luanfeng!.Before;
            var after = root.EquipmentSlotNamedUse?.Payment?.After ?? root.Luanfeng!.After;
            var reason = root.EquipmentSlotNamedUse is not null ? "equipment.slot-abolished" : "program.luanfeng.draw-to";
            return moved.Batch.ParentFrameId == root.Id && (moved.ResumeProgramFrameId == root.Id || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && m.Reason.Value == reason);
        }

        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && ContinuePaidWoLongFengChu(root);
    }

    internal ProgramSkillFrame? WoLongFengChuObserverRoot()
    {
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
            if (_resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame root && (root.EquipmentSlotNamedUse is not null || root.Luanfeng is not null) && (index == _resolutionStackOperations._resolutionStack.Count - 1 || WoLongFengChuFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]) && _coreObserverSubtreesOperations.HasMovementObserverSuffix(index)))
                return root;
        return null;
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26100)]
    internal bool TryAdvanceWoLongFengChuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WoLongFengChuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
