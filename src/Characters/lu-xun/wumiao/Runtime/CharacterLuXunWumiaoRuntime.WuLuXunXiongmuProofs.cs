#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool ValidWuXiongmuStarted(ProgramSkillFrame f)
    {
        if (f.Xiongmu is not { } r || f.SkillId != WuXiongmuSkill || f.TriggerId != "round-start" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.InstructionIndex != r.InstructionIndex || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.RoundStarting } context || context.ParentFrameId != r.WindowId || context.OwnerSeat != f.OwnerSeat || r.Source != WuXiongmuSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || !WuXiongmuNativeCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var start] && start == new XiongmuStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.RoundNumber, r.ActualTurn, r.TurnOwner, r.RequestedDraw, r.OriginalHandCount, r.OriginalMaxHp, r.SequenceBefore) && _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuStartedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.RoundNumber == r.RoundNumber) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.OwnerSeat == f.OwnerSeat && e.Window == context.Window) == 1;
    }

    internal bool ValidWuXiongmuDraw(ProgramSkillFrame f)
    {
        if (f.Xiongmu is not { DrawOutcome: { } draw } r || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.RequestedDraw || draw.SequenceBefore < r.SequenceBefore || draw.SequenceAfter < draw.SequenceBefore || draw.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || draw.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || draw.ReplacementGainedIds is not System.Collections.IList { IsReadOnly: true } || draw.ActualDrawIds.Distinct().Count() != draw.ActualDrawIds.Count || draw.ActualDrawIds.Count > r.RequestedDraw || draw.ActualDrawIds.Intersect(draw.ReplacementGainedIds).Any() || draw.Replacement is not null && draw.ActualDrawIds.Count != 0 || draw.ActualDrawIds.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == XiongmuDrawReason) != 1) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == XiongmuDrawReason) != draw.ActualDrawIds.Count)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var returned] && returned == new XiongmuDrawReturnedEvent(f.Id, r.RequestedDraw, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count, draw.SequenceBefore, draw.SequenceAfter, draw.AttemptFrameId);
    }

    internal bool ValidWuXiongmuPut(ProgramSkillFrame f)
    {
        if (f.Xiongmu is not { InsertedCount: >= 0 } r || r.PutBefore < r.DrawOutcome!.SequenceAfter || r.PutAfter < r.PutBefore || r.PutAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.PutMovements.Count != r.PutMaterials.Count || r.PutMaterials.Select(m => m.CardId).Distinct().Count() != r.PutMaterials.Count || !r.PutMaterials.Select(m => m.CardId).SequenceEqual(r.Selected) || r.PutMaterials.Any(m => m.From.OwnerSeat != f.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuDeckEntityFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.CardId).SequenceEqual(r.DeckBefore) == false || r.DeckBefore.Distinct().Count() != r.DeckBefore.Count || r.DeckBefore.Intersect(r.Selected).Any())
            return false;
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuPutSelectedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.PutMaterials.Count)
            return false;
        for (var i = 0; i < selected.Length; i++)
        {
            var original = r.PutMaterials[i];
            var move = r.PutMovements[i];
            if (selected[i] != new XiongmuPutSelectedEvent(f.Id, i, original.CardId, original.PrintedKind, original.From) || move.CardId != original.CardId || move.CardKind != original.PrintedKind || move.From != original.From || move.To == move.From || move.Sequence <= r.PutBefore || move.Sequence > r.PutAfter || move.Reason.Value != XiongmuPutReason || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move))
                return false;
        }

        var inserted = r.PutMovements.Where(m => m.To == CardLocation.DrawPile).Select(m => m.CardId).ToArray();
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuInsertedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (inserted.Length != r.InsertedCount || facts.Length != inserted.Length || r.Insertions.Count != inserted.Length || _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuPutPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.BatchId != r.PutBatchId || paid.SelectedCount != selected.Length || paid.InsertedCount != inserted.Length || paid.Before != r.PutBefore || paid.After != r.PutAfter || (selected.Length == 0) != (r.PutBatchId == 0))
            return false;
        var random = new DeterministicRandom(unchecked((int)paid.RandomBefore));
        for (var i = 0; i < inserted.Length; i++)
        {
            var sample = r.Insertions[i];
            var before = random.State;
            var count = r.DeckBefore.Count + i;
            var position = random.Next(count + 1);
            if (sample != new XiongmuInsertion(i, inserted[i], count, position, before, random.State) || facts[i] != new XiongmuInsertedEvent(f.Id, i, inserted[i], count, position, before, random.State))
                return false;
        }

        return random.State == paid.RandomAfter && r.PutTailAfter >= r.PutAfter && r.PutTailAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuPutTailCapturedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var tail] && tail == new XiongmuPutTailCapturedEvent(f.Id, r.PutBatchId, r.WoodenOxSpill.Count, r.PutTailAfter) && r.WoodenOxSpill.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.PutAfter && m.Sequence <= r.PutTailAfter && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.PutMaterials.Any(p => p.From == CardLocation.Equipment(f.OwnerSeat) && p.PrintedKind == CardKind.WoodenOx));
    }

    internal bool ValidWuXiongmuObtain(ProgramSkillFrame f)
    {
        if (f.Xiongmu is not { ActualObtained: >= 0 } r || !ValidWuXiongmuPut(f) || r.ObtainBefore < r.PutTailAfter || r.ObtainAfter < r.ObtainBefore || r.ObtainAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ObtainMaterials.Count > r.InsertedCount || r.ObtainMaterials.Count != r.ObtainMovements.Count || r.ObtainMaterials.Select(m => m.CardId).Distinct().Count() != r.ObtainMaterials.Count || r.ObtainMaterials.Any(m => m.From != CardLocation.DrawPile && m.From != CardLocation.DiscardPile || _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Rank != 8))
            return false;
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuObtainSelectedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        var obtained = _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuEntityObtainedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.ObtainMaterials.Count || obtained.Length != selected.Length || r.ActualObtained != r.ObtainMovements.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)))
            return false;
        for (var i = 0; i < selected.Length; i++)
        {
            var original = r.ObtainMaterials[i];
            var m = r.ObtainMovements[i];
            if (selected[i] != new XiongmuObtainSelectedEvent(f.Id, i, original.CardId, original.PrintedKind, original.From) || m.CardId != original.CardId || m.CardKind != original.PrintedKind || m.From != original.From || m.Sequence <= r.ObtainBefore || m.Sequence > r.ObtainAfter || m.Reason.Value != XiongmuObtainReason || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || obtained[i] != new XiongmuEntityObtainedEvent(f.Id, r.Source, r.GameplayHash, i, m.CardId, m.CardKind, m.From, m.To, m.Sequence, r.ObtainBatchId))
                return false;
        }

        return _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuObtainPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var paid] && paid == new XiongmuObtainPaidEvent(f.Id, r.ObtainBatchId, r.InsertedCount, r.ActualObtained, r.ObtainBefore, r.ObtainAfter) && (selected.Length == 0) == (r.ObtainBatchId == 0);
    }

    internal bool IsWuXiongmuHandLimitExempt(CharacterState owner, int cardId) => HasWuXiongmuCapability && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(owner.Seat) && _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuEntityObtainedEvent>().Any(e => e.Source.OwnerSeat == owner.Seat && e.CardId == cardId && e.To == CardLocation.Hand(owner.Seat) && ValidWuXiongmuExemption(e));
    internal bool ValidWuXiongmuExemption(XiongmuEntityObtainedEvent e) => e.Source.SkillId == WuXiongmuSkill && e.Source.BindingId == "round-start" && e.From is var from && (from == CardLocation.DrawPile || from == CardLocation.DiscardPile) && _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Rank == 8 && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuStartedEvent>().Count(s => s.FrameId == e.FrameId && s.Source == e.Source && s.GameplayHash == e.GameplayHash) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuObtainPaidEvent>().Count(p => p.FrameId == e.FrameId && p.BatchId == e.BatchId && p.ActualCount > 0 && e.MovementSequence > p.Before && e.MovementSequence <= p.After) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == e.From && m.To == e.To && m.Reason.Value == XiongmuObtainReason) == 1;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 29000)]
    internal void AssertWuXiongmu(ProgramSkillFrame f)
    {
        if (f.Xiongmu is not { } r)
            return;
        if (!ValidWuXiongmuStarted(f) || !Enum.IsDefined(r.Stage) || r.RequestedDraw < 0 || r.OriginalHandCount < 0 || r.OriginalMaxHp < 0 || r.RequestedDraw != Math.Max(0, r.OriginalMaxHp - r.OriginalHandCount) || r.Before < r.SequenceBefore || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Selected is not System.Collections.IList { IsReadOnly: true } || r.PutMaterials is not System.Collections.IList { IsReadOnly: true } || r.ObtainMaterials is not System.Collections.IList { IsReadOnly: true } || r.DeckBefore is not System.Collections.IList { IsReadOnly: true } || r.Insertions is not System.Collections.IList { IsReadOnly: true } || r.PutMovements is not System.Collections.IList { IsReadOnly: true } || r.ObtainMovements is not System.Collections.IList { IsReadOnly: true } || r.WoodenOxSpill is not System.Collections.IList { IsReadOnly: true } || r.Selected.Distinct().Count() != r.Selected.Count || r.Stage == XiongmuStage.Drawing && r.DrawOutcome is not null || r.Stage != XiongmuStage.Drawing && !ValidWuXiongmuDraw(f) || r.Stage is XiongmuStage.InsertChildren or XiongmuStage.ObtainChildren && !ValidWuXiongmuPut(f) || r.Stage == XiongmuStage.ObtainChildren && !ValidWuXiongmuObtain(f))
            throw new InvalidOperationException("Xiongmu lost its exact native round, immutable original material invoices, insertion RNG or typed draw return.");
        if (f.PendingMovementContinuation is { } pending && !IsWuXiongmuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Xiongmu lost its once-paid native movement return.");
        if (r.Stage == XiongmuStage.Selection && r.Selected.Any(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != f.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(id).Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            throw new InvalidOperationException("Xiongmu's selected own HE entity moved before confirmation.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 650)]
    internal bool IsWuXiongmuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.Xiongmu is { } r && ValidWuXiongmuStarted(f) && effect?.Op == SkillProgramEffectOp.ResolveWuLuXunXiongmu && f.PendingMovementContinuation == pending && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && (r.Stage == XiongmuStage.DrawChildren && ValidWuXiongmuDraw(f) || r.Stage == XiongmuStage.InsertChildren && ValidWuXiongmuPut(f) || r.Stage == XiongmuStage.ObtainChildren && ValidWuXiongmuObtain(f));
}
