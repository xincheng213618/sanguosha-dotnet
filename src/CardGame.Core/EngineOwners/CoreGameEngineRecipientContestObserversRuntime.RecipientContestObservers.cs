#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecipientContestObserversRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 60)]
    internal bool IsRecipientContestMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => pending.BeforeCount == 0 && pending.CoverageResultBind is null && (f.UniqueHpPeer is { Stage: UniqueHpPeerStage.CostChildren or UniqueHpPeerStage.DrawChildren } peer && effect?.Op == SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer && pending.SubjectSeat == peer.ChooserSeat && _coreRecipientContestReceiptsOperations.ValidUniqueHpPeer(f) || f.RecipientContest is { Stage: RecipientContestStage.GiftChildren } && effect?.Op == SkillProgramEffectOp.GiveAllHandAndStartRecipientPindian && pending.SubjectSeat == f.OwnerSeat && _coreRecipientContestReceiptsOperations.ValidRecipientContest(f));
    [ProgramContinuation(ProgramContinuationKind.Movement, 400)]
    internal bool ReturnRecipientContestMovement(ProgramSkillFrame f)
    {
        if (f.UniqueHpPeer is null && f.RecipientContest is null || f.PendingMovementContinuation is not { } pending)
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (!IsRecipientContestMovement(f, effect, pending))
            throw new InvalidOperationException("A discard/draw or atomic gift lost its exact pending invoice.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal bool RecipientContestFirstPaidChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        long before, after;
        int actor;
        string reason;
        bool draw;
        UniqueHpPeerInvoice? invoice = null;
        if (f.UniqueHpPeer is { Stage: UniqueHpPeerStage.CostChildren or UniqueHpPeerStage.DrawChildren } peer && _coreRecipientContestReceiptsOperations.ValidUniqueHpPeer(f))
        {
            invoice = peer.ChooserSeat == f.OwnerSeat ? peer.OwnerInvoice : peer.PeerInvoice;
            if (invoice is null)
                return false;
            actor = invoice.ActorSeat;
            draw = peer.Stage == UniqueHpPeerStage.DrawChildren;
            before = draw ? invoice.DrawBefore : invoice.CostBefore;
            after = draw ? invoice.DrawAfter : invoice.CostAfter;
            reason = draw ? global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime.UniqueHpPeerDrawReason(f, actor) : global::CardGame.Core.EngineOwners.CoreGameEngineUniqueHpPeerProgramsRuntime.UniqueHpPeerCostReason(f);
        }
        else if (f.RecipientContest is { Stage: RecipientContestStage.GiftChildren } gift && _coreRecipientContestReceiptsOperations.ValidRecipientContest(f))
        {
            before = gift.SequenceBefore;
            after = gift.SequenceAfter;
            actor = f.OwnerSeat;
            reason = global::CardGame.Core.EngineOwners.CoreGameEngineRecipientContestsRuntime.RecipientContestGiftReason(f);
            draw = false;
        }
        else
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == f.Id && (moved.ResumeProgramFrameId == f.Id || moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == f.Id) && (moved.Batch.AwaitingProgramFrameId is null || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > before && m.Sequence <= after && (draw ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(actor) && m.Reason.Value == reason : f.RecipientContest is { } g ? g.GiftCardIds.Contains(m.CardId) && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(g.RecipientSeat) && m.Reason.Value == reason : invoice!.CardIds.Select((id, n) => (id, from: invoice.SourceLocations[n])).Any(p => p.id == m.CardId && p.from == m.From && m.Reason.Value == reason && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m))))) || !draw && invoice is not null && m.Sequence > after && m.From == CardLocation.WoodenOxGrain(actor) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && invoice.CardIds.Select((id, n) => (id, from: invoice.SourceLocations[n])).Any(p => p.from == CardLocation.Equipment(actor) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(cost => cost.Sequence > before && cost.Sequence <= after && cost.CardId == p.id && cost.CardKind == CardKind.WoodenOx && cost.From == p.from && cost.To == CardLocation.DiscardPile && cost.Reason.Value == reason))));
        if (draw || invoice is null || !invoice.CardIds.Where((id, n) => invoice.SourceLocations[n] == CardLocation.Equipment(actor)).Any(id => _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && m.CardId == id && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(actor) && m.To == CardLocation.DiscardPile && m.Reason.Value == reason)))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == actor && hp.Change.TargetSeat == actor && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame replaced && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(replaced, f) && replaced.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && replaced.Attempt.SourceSeat == actor && replaced.Attempt.TargetSeat == actor && replaced.Attempt.Amount == 1 && replaced.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && replaced.Attempt.Completion.MoveReason?.Value == reason;
    }

    internal bool RecipientContestPaidPrefix(int rootIndex, int lastIndex)
    {
        if (rootIndex < 0 || lastIndex <= rootIndex || lastIndex >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[rootIndex] is not ProgramSkillFrame f || !RecipientContestFirstPaidChild(f, _resolutionStackOperations._resolutionStack[rootIndex + 1]))
            return false;
        for (var i = rootIndex + 1; i <= lastIndex; i++)
        {
            // The root-to-first incoming edge is already proved above. Every
            // later edge is exact; whole rescue suffixes are used only after it.
            if (i > rootIndex + 1 && !(_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(i) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(i)))
                return false;
            if (_resolutionStackOperations._resolutionStack[i] is DyingFrame d && i < lastIndex && (_coreRoundPricedPileAlcoholOperations.IsRoundPricedPileAlcoholRide(i, d) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(i, d) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(i, d) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(i, d) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(i, d)))
                return lastIndex == _resolutionStackOperations._resolutionStack.Count - 1;
        }

        return true;
    }

    internal ProgramSkillFrame? RecipientContestPaidObserverRoot()
    {
        for (var i = 0; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
            if (RecipientContestPaidPrefix(i, _resolutionStackOperations._resolutionStack.Count - 1))
                return (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[i];
        return null;
    }

    internal bool HasUniqueHpPeerActualTargetObserver(long useId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x is ActualUseTargetWindowFrame w && w.ParentFrameId == useId);
        if (index < 1 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame f || f.UniqueHpPeer is not { CardUseFrameId: var original } || original != useId || !_coreRecipientContestReceiptsOperations.ValidUniqueHpPeer(f))
            return false;
        return index + 2 == _resolutionStackOperations._resolutionStack.Count || RecipientContestPaidPrefix(index + 1, _resolutionStackOperations._resolutionStack.Count - 1);
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 260, group: 10)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 370)]
    internal bool IsRecipientContestProgramDying() => _coreRecipientConsequencesObserversOperations.IsRecipientConsequencesProgramDying() || _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && RecipientContestPaidObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1860)]
    internal bool HasRecipientContestDamageObserver(long windowId)
    {
        if (_coreRecipientConsequencesObserversOperations.HasRecipientConsequencesDamageObserver(windowId))
            return true;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == windowId && f is DamageTriggerWindowFrame);
        return index >= 2 && _resolutionStackOperations._resolutionStack[index] is DamageTriggerWindowFrame window && _resolutionStackOperations._resolutionStack[index - 1] is DamageFrame damage && window.ParentFrameId == damage.Id && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == damage.ParentFrameId)is { AttackAttempt: not null, AttackReturn: not null } attack && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId == attack.Id && RecipientContestPaidObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id) < _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == attack.Id);
    }
}
