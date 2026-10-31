#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangShiPingOlRuntime
{

    internal bool ZhangShiPingHasMovement(ProgramSkillFrame f, CardsMovedTriggerWindowFrame moved)
    {
        if (f.ZhangShiPingXingGu is not { } r || moved.ResumeProgramFrameId != f.Id || moved.Batch.ParentFrameId != f.Id || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
            return false;
        return moved.Batch.Movements.All(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage switch
        {
            XingGuStage.InitialChildren => m.From == CardLocation.DrawPile && m.To == r.PileLocation && m.Reason.Value == XingGuStoreReason && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangShiPingXingGuCardStoredEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.MovementSequence == m.Sequence),
            XingGuStage.PlacementChildren => m.CardId == r.CardId && m.From == r.PileLocation && m.To == CardLocation.Equipment(r.RecipientSeat!.Value) && m.Reason == CardMoveReasons.EquipmentEnter || m.From == CardLocation.Equipment(r.RecipientSeat!.Value) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.EquipmentReplace,
            XingGuStage.DiamondChildren => m.CardId == r.DiamondCardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == XingGuDiamondReason,
            _ => false
        }));
    }

    internal bool ZhangShiPingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhangShiPingXingGu is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return ZhangShiPingHasMovement(f, moved);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool ZhangShiPingObserverSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? ZhangShiPingObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhangShiPingOlOperations.ZhangShiPingFirstChild(f, child), (i) => _characterZhangShiPingOlOperations.ZhangShiPingObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 460)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 570)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 600)]
    internal bool HasZhangShiPingDamageObserver(long id) => ZhangShiPingObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26610)]
    internal bool AdvanceZhangShiPingSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangShiPingObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
