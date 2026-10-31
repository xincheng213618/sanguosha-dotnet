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
        RequireIssuedKanZe(f);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 160)]
    internal void RequireIssuedKanZe(ProgramSkillFrame f)
    {
        if (f.XiashuPaid is not { } r)
            return;
        var i = r.Invoice;
        if (i.Source != KanZeSource(f) || i.GameplayHash != f.GameplayHash || i.WindowId != f.WindowContext?.ParentFrameId || i.InstructionIndex != f.InstructionIndex || i.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.GiveAllHandAndResolveXiashu || i.GiftCount <= 0 || i.GiftAfter <= i.GiftBefore || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || f.SelectedTargetSeats is not [var target] || target != i.RecipientSeat)
            throw new InvalidOperationException("Xiashu lost its frozen original invoice.");
        if (r.SelectedReveal.Count != r.SelectedReveal.Distinct().Count())
            throw new InvalidOperationException("Xiashu reveal selection contains duplicate entities.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not PlayPhaseStartingBoundaryFrame w || w.Id != i.WindowId || w.OwnerSeat != f.OwnerSeat || w.ItemIndex < 0 || w.ItemIndex >= w.Items.Count || w.Items[w.ItemIndex].Candidate is not { } candidate || candidate.OwnerSeat != f.OwnerSeat || candidate.SkillId != f.SkillId || candidate.SkillInstanceId != f.SkillInstanceId || candidate.BindingId != f.TriggerId || candidate.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Xiashu lost its native selected boundary candidate.");
        var gift = _coreProgramLifecycleOperations.GetProgramCardSet(f, XiashuGiftBind).CardIds;
        if (gift.Count != i.GiftCount || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XiashuGiftPaidEvent>().Count(e => e.FrameId == f.Id && e.Invoice == i) != 1 || gift.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > i.GiftBefore && m.Sequence <= i.GiftAfter && m.CardId == id && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(i.RecipientSeat) && m.Reason.Value == XiashuGiftReason) != 1))
            throw new InvalidOperationException("Xiashu whole-hand gift was not paid exactly once.");
        if (r.Stage == XiashuStage.GiftChildren && (r.Before != i.GiftBefore || r.After != i.GiftAfter || r.Count != i.GiftCount))
            throw new InvalidOperationException("Xiashu gift stage differs from its original payment.");
        if (r.Stage is XiashuStage.RecipientReveal or XiashuStage.OwnerClaim)
        {
            if (f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Xiashu private choice cannot masquerade as movement.");
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } decision && (decision.PlayerSeat != (r.Stage == XiashuStage.OwnerClaim ? f.OwnerSeat : i.RecipientSeat) || !decision.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(decision.Choices, XiashuChoices(f))))
                throw new InvalidOperationException("Xiashu lost its actual private chooser.");
            return;
        }

        if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null, SubjectSeat: var actor } || actor != f.OwnerSeat)
            throw new InvalidOperationException("Xiashu lost its native movement continuation.");
        if (r.Stage == XiashuStage.ClaimChildren && (_coreProgramLifecycleOperations.GetProgramCardSet(f, XiashuClaimBind).CardIds.Count != r.Count || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XiashuClaimPaidEvent>().Count(e => e.FrameId == f.Id && e.Before == r.Before && e.After == r.After && e.Count == r.Count) != 1 || _coreProgramLifecycleOperations.GetProgramCardSet(f, XiashuClaimBind).CardIds.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == CardLocation.Hand(i.RecipientSeat) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == XiashuClaimReason) != 1)))
            throw new InvalidOperationException("Xiashu claim invoice changed.");
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
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 40)]
    internal bool IsKanZeDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && KanZeObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 160)]
    internal bool TryAdvanceKanZeSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && KanZeObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
