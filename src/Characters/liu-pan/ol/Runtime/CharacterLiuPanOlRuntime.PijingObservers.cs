#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuPanOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27410)]
    internal void AssertPijing(ProgramSkillFrame f)
    {
        if (f.Pijing is not { } r)
            return;
        var use = RequirePijingUse(f, out var window);
        var op = r.Reciprocal ? SkillProgramEffectOp.ResolvePijingNextSoleTargetChoice : SkillProgramEffectOp.ResolvePijingTargetTogglesAndRandomGifts;
        if (f.InstructionIndex != 1 || f.SkillId != "ol:pijing" || f.TriggerId != (r.Reciprocal ? "next-sole-use" : "toggle-targets") || r.Source != PijingSource(f) || r.GameplayHash != f.GameplayHash || r.UseId != use.Id || r.ActionId != use.Action!.ActionId || r.WindowId != window.Id || r.ProviderSeat != use.Action.ProviderSeat || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || r.X < 1 || r.Index < 0 || r.Index > r.Toggles.Count || r.Toggles.Count > r.X || r.Toggles.Select(t => t.Seat).Distinct().Count() != r.Toggles.Count || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Count < 0 || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<PijingStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new PijingStartedEvent(f.Id, r.Source, r.GameplayHash, r.UseId, r.ActionId, r.WindowId, r.ActualTurn, r.ActorSeat, r.ProviderSeat, r.X, r.Reciprocal, r.GroupId, r.Original.Count) || !_coreProgramEventHistoryOperations.ProgramEventHistory<PijingTargetSlotEvent>().Where(e => e.FrameId == f.Id && !e.Final).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Original))
            throw new InvalidOperationException("Pijing lost its exact original native designation, source, scalar invoice or owning target cursor.");
        if (!r.Reciprocal && (r.ActorSeat != f.OwnerSeat || r.GroupId != 0 || r.Grants.Count != 0 || r.Toggles.Any(t => t.Seat == f.OwnerSeat) || r.Stage is PijingStage.Reciprocal or PijingStage.DrawChildren || _coreProgramEventHistoryOperations.ProgramEventHistory<PijingStartedEvent>().Count(e => !e.Reciprocal && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.ActualTurn == r.ActualTurn) != 1))
            throw new InvalidOperationException("Pijing cannot exceed one initial activation in an actual turn or select itself.");
        if (r.Reciprocal)
        {
            var group = PijingIssuedGroup(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), f.WindowContext!, out _, out _);
            if (group is null || group.GroupId != r.GroupId || group.HolderSeat != r.ActorSeat || !group.Grants.SequenceEqual(r.Grants) || r.Grants.Any(id => _coreProgramEventHistoryOperations.ProgramEventHistory<PijingGrantConsumedEvent>().Count(e => e == new PijingGrantConsumedEvent(f.Id, r.UseId, r.GroupId, id)) != 1) || r.Stage is not (PijingStage.Reciprocal or PijingStage.DrawChildren))
                throw new InvalidOperationException("Pijing's reciprocal choice lost its original long-lived grants or consumed them twice.");
        }

        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<PijingToggleSelectedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.Toggles.Count)
            throw new InvalidOperationException("Pijing target choices differ from committed scalar selections.");
        for (var i = 0; i < selected.Length; i++)
        {
            var t = r.Toggles[i];
            if (selected[i] != new PijingToggleSelectedEvent(f.Id, i, t.Seat, t.Remove, t.Pair.Count) || !_coreProgramEventHistoryOperations.ProgramEventHistory<PijingTogglePairEvent>().Where(e => e.FrameId == f.Id && e.Index == i).OrderBy(e => e.PairIndex).Select(e => e.Seat).SequenceEqual(t.Pair) || t.Pair.Count != (use.CardKind == CardKind.BorrowedSword ? 2 : 1) || t.Pair[0] != t.Seat)
                throw new InvalidOperationException("Pijing cannot replace a chosen main target or a BorrowedSword victim pair.");
        }

        var children = r.Stage is PijingStage.GiftChildren or PijingStage.DrawChildren;
        var subject = r.Stage == PijingStage.GiftChildren ? r.Toggles[r.Index].Seat : r.ActorSeat;
        if (children ? f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != subject : f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Pijing lost its owning native movement return.");
        if (children && _coreProgramEventHistoryOperations.ProgramEventHistory<PijingMovementPaidEvent>().Where(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Index == r.Index).ToArray()is not [var _ ])
            throw new InvalidOperationException("Pijing lost its exact paid native movement range.");
        if (children && _coreProgramEventHistoryOperations.ProgramEventHistory<PijingMovementPaidEvent>().Single(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Index == r.Index) != new PijingMovementPaidEvent(f.Id, r.Stage, r.Index, subject, r.Before, r.After, r.Count))
            throw new InvalidOperationException("Pijing changed its exact paid native movement range.");
        if (r.Stage == PijingStage.GiftChildren)
        {
            if (r.Gift is not { } g || g.FrameId != f.Id || g.Index != r.Index || g.DonorSeat != subject || g.From.OwnerSeat != subject || g.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || g.Kind == CardKind.GeneralWeapon || _coreProgramEventHistoryOperations.ProgramEventHistory<PijingGiftSelectedEvent>().Count(e => e == g) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<PijingGiftPaidEvent>().Where(e => e.FrameId == f.Id && e.Index == r.Index).ToArray()is not [var paid] || paid.DonorSeat != subject || paid.CardId != g.CardId || paid.From != g.From || paid.RecipientSeat != f.OwnerSeat || paid.Before != r.Before || paid.Sequence <= r.Before || paid.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == g.CardId && m.From == g.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == PijingGiftReason))
                throw new InvalidOperationException("Pijing cannot fabricate or repay a random foreign HE gift.");
        }
        else if (r.Gift is not null)
            throw new InvalidOperationException("Pijing retained a paid entity outside its owning gift stage.");
        if (r.Stage == PijingStage.DrawChildren && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.ActorSeat) && m.Reason.Value == PijingDrawReason) != r.Count)
            throw new InvalidOperationException("Pijing's reciprocal draw cannot count a shuffle or another child gain.");
        if (!r.Reciprocal)
            for (var i = 0; i < r.Index; i++)
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<PijingGiftCompletedEvent>().Where(e => e.FrameId == f.Id && e.Index == i).ToArray()is not [var done] || _coreProgramEventHistoryOperations.ProgramEventHistory<PijingGrantIssuedEvent>().SingleOrDefault(g => g.GrantId == done.GrantId)is not { } grant || grant.HolderSeat != r.Toggles[i].Seat || grant.Source != r.Source || grant.UseId != r.UseId || grant.ActionId != r.ActionId)
                    throw new InvalidOperationException("Pijing lost or repeated a selected person's completed gift/lease boundary.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } prompt && r.Stage is PijingStage.Toggles or PijingStage.Reciprocal && (prompt.PlayerSeat != (r.Reciprocal ? r.ActorSeat : f.OwnerSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, PijingChoices(f))))
            throw new InvalidOperationException("Pijing's chooser cannot answer another or obsolete designation frontier.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 350)]
    internal bool IsLiuPanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.Pijing is { Stage: PijingStage.GiftChildren or PijingStage.DrawChildren } r && pending == f.PendingMovementContinuation && effect?.Op == (r.Reciprocal ? SkillProgramEffectOp.ResolvePijingNextSoleTargetChoice : SkillProgramEffectOp.ResolvePijingTargetTogglesAndRandomGifts) && pending.BeforeCount == 0 && pending.CoverageResultBind is null && pending.SubjectSeat == (r.Stage == PijingStage.GiftChildren ? r.Toggles[r.Index].Seat : r.ActorSeat);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 150)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 140)]
    internal bool IsLiuPanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Pijing is { Stage: PijingStage.GiftChildren, Gift: { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } g } r && recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.TargetSeat == g.DonorSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == PijingGiftReason && _coreProgramEventHistoryOperations.ProgramEventHistory<PijingGiftPaidEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Index && e.CardId == g.CardId);
    internal bool LiuPanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Pijing is not { } r)
            return false;
        AssertPijing(f);
        if (child is RecoveryReplacementFrame recovery)
            return IsLiuPanRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage is PijingStage.GiftChildren or PijingStage.DrawChildren && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is not CardsMovedTriggerWindowFrame moved || f.PendingMovementContinuation is null || !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved))
            return false;
        if (r.Stage == PijingStage.GiftChildren)
            return r.Gift is { } gift && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.CardId == gift.CardId && m.From == gift.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == PijingGiftReason || gift.Kind == CardKind.WoodenOx && gift.From.Zone == CardZoneKind.Equipment && m.From == CardLocation.WoodenOxGrain(gift.DonorSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
        return r.Stage == PijingStage.DrawChildren && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle || m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.ActorSeat) && m.Reason.Value == PijingDrawReason));
    }

    internal ProgramSkillFrame? LiuPanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLiuPanOlOperations.LiuPanFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 860)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 970)]
    internal bool HasLiuPanDamageObserver(long id) => LiuPanObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.Any(x => x.Id == id && x is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1000)]
    internal bool HasLiuPanBeforeDamageObserver(long id) => LiuPanObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.Any(x => x.Id == id && x is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 830)]
    internal bool IsLiuPanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && LiuPanObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27410)]
    internal bool AdvanceLiuPanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuPanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
