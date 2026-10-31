#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

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

    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 360)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 470)]
    internal bool HasWoLongFengChuDamageObserver(long id) => WoLongFengChuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    internal bool HasWoLongFengChuUseObserver(long id) => WoLongFengChuObserverRoot()is { EquipmentSlotNamedUse: { } draft } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is CardUseFrame or ProgramSkillFrame)is var at && at >= 0 && at < _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id) && draft.OriginalParentId is { } original && _resolutionStackOperations._resolutionStack.Any(f => f.Id == original) && draft.Intent != EquipmentSlotNamedUseIntent.Play;
    internal bool HasWoLongFengChuQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (WoLongFengChuObserverRoot()is not { EquipmentSlotNamedUse: { Intent: EquipmentSlotNamedUseIntent.Qinglong } d } root || d.OriginalParentId != attack.ResolutionId || root.OwnerSeat != attack.SourceSeat || !_coreNativeDamageHandlesOperations.UsesFormalQinglongCrescentBlade || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || attack.SuccessfulDodgeResponses != attack.RequiredDodgeResponses || !_coreAdvancedWeaponsOperations.HasWeaponAbility(_coreMatchParticipantsPlayersAccess._players[root.OwnerSeat], CardKind.QinglongCrescentBlade) || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Step: ResolutionFrameStep.AwaitingResponse, Action: not null } parent || parent.SourceSeat != root.OwnerSeat || parent.CardKind != attack.EffectiveCardKind || d.ResponseChoice is not { Cards.Count: 0 } selected || !selected.Targets.SequenceEqual([attack.TargetSeat]) || selected.Parameters.GetValueOrDefault("action") != "tiered-round-zero-forced-slash" || selected.Parameters.GetValueOrDefault("forced-use") != "qinglong" || d.OriginalDecision is not { Kind: DecisionKind.QinglongCrescentBlade, IsPrivate: true } original || original.PlayerSeat != root.OwnerSeat || original.SourceSeat != root.OwnerSeat || original.TargetSeat != attack.TargetSeat || original.IncomingCard != attack.EffectiveCardKind || !original.ValidTargetSeats.SequenceEqual([attack.TargetSeat]) || !original.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            return false;
        AssertWoLongFengChuFrame(root);
        return true;
    }

    internal bool HasWoLongFengChuBorrowedPaymentRide(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowed)
    {
        if (WoLongFengChuObserverRoot()is not { EquipmentSlotNamedUse: { Intent: EquipmentSlotNamedUseIntent.BorrowedSword } d } root || !borrowed.AwaitingSlashChoice || borrowed.ActiveAttack is not null || root.OwnerSeat != borrowed.WeaponOwnerSeat || _coreNativeBorrowedSwordOperations.GetWeapon(_coreMatchParticipantsPlayersAccess._players[root.OwnerSeat])is null || _coreCardUseLifecycleOperations.LifecycleCardUse(borrowed.ResolutionId)is not { CardKind: CardKind.BorrowedSword, Action: not null } parent || _resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault(w => w.ParentFrameId == parent.Id)is not { } window || d.OriginalParentId != window.Id || window.ResponderSeat != root.OwnerSeat || d.ResponseChoice is not { Cards.Count: 0 } selected || !selected.Targets.SequenceEqual([borrowed.SlashTargetSeat]) || selected.Parameters.GetValueOrDefault("response") != "tiered-round-zero-forced-slash" || selected.Parameters.GetValueOrDefault("forced-use") != "borrowed-sword" || d.OriginalDecision is not { Kind: DecisionKind.RespondSlash } original || original.PlayerSeat != root.OwnerSeat || original.IncomingCard != CardKind.BorrowedSword || original.TargetSeat != borrowed.SlashTargetSeat || !original.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            return false;
        AssertWoLongFengChuFrame(root);
        return true;
    }

    internal bool IsExactLuanfengDyingRide(long dyingId)
    {
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dyingId && f is DyingFrame);
        if (at < 0 || at + 2 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[at] is not DyingFrame dying || _resolutionStackOperations._resolutionStack[at + 1] is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingEntering } entry || entry.ResumeDyingFrameId != dyingId || entry.Continuation != ProgramLifecycleContinuation.ResumeDyingEntry || _resolutionStackOperations._resolutionStack[at + 2] is not ProgramSkillFrame { Luanfeng: { } r } root || r.DyingFrameId != dyingId || r.VictimSeat != dying.VictimSeat || root.WindowContext?.ParentFrameId != entry.Id || entry.CandidateIndex >= entry.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, entry.Candidates[entry.CandidateIndex]))
            return false;
        return at + 2 == _resolutionStackOperations._resolutionStack.Count - 1 || WoLongFengChuFirstChild(root, _resolutionStackOperations._resolutionStack[at + 3]) && _coreObserverSubtreesOperations.HasMovementObserverSuffix(at + 2);
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 330)]
    internal bool IsWoLongFengChuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && (IsExactLuanfengDyingRide(dying.FrameId) || WoLongFengChuObserverRoot()is { } root && (_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id) || root.EquipmentSlotNamedUse is { Intent: EquipmentSlotNamedUseIntent.DyingPeach or EquipmentSlotNamedUseIntent.DyingAlcohol } d && d.OriginalParentId == dying.FrameId));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26100)]
    internal bool TryAdvanceWoLongFengChuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WoLongFengChuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
