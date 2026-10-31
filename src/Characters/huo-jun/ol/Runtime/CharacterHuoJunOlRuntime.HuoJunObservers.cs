#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuoJunOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 60)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 50)]
    internal bool IsHuoJunFenruiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.HuoJunFenrui is { Stage: HuoJunFenruiStage.CostChildren, CostCardId: { } id, CostFrom: { Zone: CardZoneKind.Equipment } } r && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == new CardMoveReason(HuoJunDiscardReason) && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<HuoJunFenruiCostPaidEvent>().Any(e => e.FrameId == f.Id && e.Source == r.Source && e.CardId == id);
    internal bool HuoJunFenruiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HuoJunFenrui is not { CostCardId: not null } r)
            return false;
        if (child is CardUseFrame)
            return IsHuoJunFenruiNativeEquipment(child, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == HuoJunFenruiStage.CostChildren && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && m.CardId == r.CostCardId && m.From == r.CostFrom && m.To == CardLocation.DiscardPile && m.Reason.Value == HuoJunDiscardReason && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m);
        if (child is RecoveryReplacementFrame recovery)
            return IsHuoJunFenruiRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == HuoJunFenruiStage.CostChildren && r.CostCardId is { } id && _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.TargetSeat == f.OwnerSeat;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == HuoJunFenruiStage.DamageIssued && f.AttackAttempt is not null && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && CanContinuePaidHuoJunFenrui(f);
    }

    internal bool HuoJunFenruiObserverSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? HuoJunFenruiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.HuoJunFenruiFirstChild(f, child), (i) => this.HuoJunFenruiObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26210)]
    internal bool AdvanceHuoJunFenruiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HuoJunFenruiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
