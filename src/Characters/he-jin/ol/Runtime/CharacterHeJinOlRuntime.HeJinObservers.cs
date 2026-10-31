#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeJinOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 710)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 650)]
    internal bool IsHeJinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.HeJinYanhuo is { Stage: HeJinYanhuoStage.DiscardChildren } r && ValidHeJinYanhuo(f) && r.SelectedCards.Any(s => s.From == CardLocation.Equipment(r.KillerSeat) && _coreCardAttackStateOperations.GetAttackCard(s.CardId).Kind == CardKind.SilverLion) && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == HeJinDiscardReason && recovery.Attempt.SourceSeat == r.KillerSeat && recovery.Attempt.TargetSeat == r.KillerSeat && recovery.Attempt.Amount == 1;
    internal bool HeJinFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HeJinMouzhu is { Stage: HeJinMouzhuStage.GiftChildren } gift && ValidHeJinMouzhu(f) && child is CardsMovedTriggerWindowFrame gained)
            return gained.Batch.ParentFrameId == f.Id && gained.ResumeProgramFrameId == f.Id && gained.Batch.OriginOwnerSeat == f.OwnerSeat && gained.Batch.OriginSkillId == f.SkillId && gained.Batch.OriginSkillInstanceId == f.SkillInstanceId && gained.Batch.Movements is [var m] && m.CardId == gift.GiftId && m.From == CardLocation.Hand(gift.ActorSeat) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == HeJinGiftReason && m.Sequence > gift.Before && m.Sequence <= gift.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m);
        if (f.HeJinYanhuo is { Stage: HeJinYanhuoStage.DiscardChildren } r && ValidHeJinYanhuo(f))
        {
            if (child is CardsMovedTriggerWindowFrame moved)
                return moved.Batch.ParentFrameId == f.Id && moved.ResumeProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.Reason.Value == HeJinDiscardReason && r.SelectedCards.Any(s => s.CardId == m.CardId && s.From == m.From && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreCardAttackStateOperations.GetAttackCard(s.CardId), s.From, (s.From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(s.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile))) || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(r.KillerSeat) && m.To == CardLocation.DiscardPile && r.SelectedCards.Any(s => s.From == CardLocation.Equipment(r.KillerSeat) && _coreCardAttackStateOperations.GetAttackCard(s.CardId).Kind == CardKind.WoodenOx)));
            if (child is RecoveryReplacementFrame recovery)
                return IsHeJinRecoveryParent(f, recovery);
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.KillerSeat && hp.Change.TargetSeat == r.KillerSeat && hp.Change.Amount == 1 && r.SelectedCards.Any(s => s.From == CardLocation.Equipment(r.KillerSeat) && _coreCardAttackStateOperations.GetAttackCard(s.CardId).Kind == CardKind.SilverLion);
        }

        return (f.HeJinMouzhu is { Stage: not HeJinMouzhuStage.ChoosingGift } && ValidHeJinMouzhu(f) || f.HeJinYanhuo is not null && ValidHeJinYanhuo(f)) && child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? HeJinObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHeJinOlOperations.HeJinFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 510)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 620)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 650)]
    internal bool HasHeJinDamageObserver(long id) => HeJinObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26710)]
    internal bool AdvanceHeJinSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HeJinObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
