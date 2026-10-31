#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKanZeOlRuntime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 100)]
    internal bool CanContinueIssuedKanZe(ProgramSkillFrame f)
    {
        if (f.XiashuPaid is null)
            return false;
        return true;
    }

    internal bool KanZeFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueIssuedKanZe(root) || root.XiashuPaid is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == (r.Stage == XiashuStage.GiftChildren ? XiashuGiftReason : XiashuClaimReason) && m.From == CardLocation.Hand(r.Stage == XiashuStage.GiftChildren ? root.OwnerSeat : r.Invoice.RecipientSeat) && m.To == CardLocation.Hand(r.Stage == XiashuStage.GiftChildren ? r.Invoice.RecipientSeat : root.OwnerSeat));
    }

    internal ProgramSkillFrame? KanZeObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterKanZeOlOperations.KanZeFirstChild(r, c), (i) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 290)]
    internal bool HasKanZeBeforeDamageObserver(long id) => KanZeObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 80)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 190)]
    internal bool HasKanZeDamageObserver(long id) => KanZeObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 160)]
    internal bool TryAdvanceKanZeSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && KanZeObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
