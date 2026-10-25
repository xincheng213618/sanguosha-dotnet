namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ObtainDeckCardsReason = "skill-program.obtain-deck-cards";

    [ProgramCapability]
    private sealed class DeckCardsCapability(GameEngine engine) : IDeckCardsHost
    {
        public SkillProgramStepOutcome ObtainDeckCards(ProgramSkillFrame frame, SkillProgramEffect effect) =>
            engine.ObtainDeckCards(frame, effect);
    }

    private SkillProgramStepOutcome ObtainDeckCards(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.DeckCardGain is not null || _resolutionStack.LastOrDefault()?.Id != frame.Id)
            throw new InvalidOperationException("A deck gain requires its active unpaid program instruction.");
        if (!_players[frame.OwnerSeat].IsAlive || !EnsureDrawPile()) return SkillProgramStepOutcome.Continue;
        var cards = _cardZones.CardsAt(CardLocation.DrawPile).Reverse().Where(card =>
            effect.CardKinds.Count > 0 ? effect.CardKinds.Contains(card.Kind) :
                MatchesProgramCardCategory(card.Kind, effect.CardCategories)).Take(effect.Amount).ToArray();
        if (cards.Length == 0) return SkillProgramStepOutcome.Continue;
        var receipt = new DeckCardGainReceipt(frame.InstructionIndex,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            effect.Amount, effect.CardCategories.Count == 0 ? null : effect.CardCategories[0], _movementSequence)
        {
            CardIds = cards.Select(card => card.Id).ToArray(), SelectedKinds = cards.Select(card => card.Kind).ToArray(),
            FilterKinds = effect.CardKinds
        };
        ReplaceRuntimeTop(frame with { DeckCardGain = receipt });
        MoveProgramCardsFromMultipleSources(receipt.CardIds, CardLocation.Hand(frame.OwnerSeat), new(ObtainDeckCardsReason),
            (batch, records) =>
            {
                var active = GetActiveProgramFrame(frame.Id);
                ReplaceRuntimeTop(active with { DeckCardGain = receipt with { BatchId = batch, SequenceAfter = _movementSequence } });
                AdvanceEventRulesAndQueueFact(new DeckCardsObtainedEvent(frame.Id, receipt.InstructionIndex, frame.OwnerSeat,
                    records.Count(move => move.To == CardLocation.Hand(frame.OwnerSeat)), batch, receipt.SequenceBefore, _movementSequence));
            });
        var outcome = AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
        if (outcome == SkillProgramStepOutcome.Continue)
            ReplaceRuntimeTop(GetActiveProgramFrame(frame.Id) with { DeckCardGain = null });
        return outcome;
    }

    private bool ValidDeckCardGain(ProgramSkillFrame frame)
    {
        if (frame.DeckCardGain is not { } receipt ||
            receipt.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            receipt.GameplayHash != frame.GameplayHash || receipt.InstructionIndex != frame.InstructionIndex ||
            receipt.BatchId <= 0 || receipt.SequenceBefore < 0 || receipt.SequenceAfter <= receipt.SequenceBefore ||
            receipt.SequenceAfter > _movementSequence || receipt.CardIds.Count is < 1 or > 4 ||
            receipt.CardIds.Count > receipt.Requested || receipt.CardIds.Distinct().Count() != receipt.CardIds.Count ||
            receipt.SelectedKinds.Count != receipt.CardIds.Count)
            return false;
        var moves = _cardMovements.Where(move => move.Sequence > receipt.SequenceBefore && move.Sequence <= receipt.SequenceAfter).ToArray();
        if (moves.Length != receipt.CardIds.Count || !moves.Select(move => move.CardId).SequenceEqual(receipt.CardIds) ||
            !moves.Select(move => move.CardKind).SequenceEqual(receipt.SelectedKinds) ||
            moves.Any(move => move.From != CardLocation.DrawPile || move.Reason.Value != ObtainDeckCardsReason ||
                !(move.To == CardLocation.Hand(frame.OwnerSeat) || IsHappyWeiYanRedirectedGain(move, CardLocation.Hand(frame.OwnerSeat)) ||
                  move.HappyZuoCiPhantomDestination is { RequestedTo: var requested } &&
                  requested == CardLocation.Hand(frame.OwnerSeat) && IsHappyZuoCiDestroyedMaterial(move)) ||
                (receipt.Category is { } category ? !MatchesProgramCardCategory(move.CardKind, [category]) :
                    !receipt.FilterKinds.Contains(move.CardKind)))) return false;
        return ProgramEventHistory<DeckCardsObtainedEvent>().Count(fact => fact.ProgramFrameId == frame.Id &&
            fact.InstructionIndex == receipt.InstructionIndex && fact.OwnerSeat == frame.OwnerSeat && fact.BatchId == receipt.BatchId &&
            fact.Count == moves.Count(move => move.To == CardLocation.Hand(frame.OwnerSeat)) &&
            fact.SequenceBefore == receipt.SequenceBefore && fact.SequenceAfter == receipt.SequenceAfter) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62010)]
    private void AssertDeckCardGain(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        if (frame.DeckCardGain is not { } receipt) return;
        if (frame.InstructionIndex < 1 || frame.InstructionIndex > plan.Instructions.Count ||
            plan.Instructions[frame.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.ObtainDeckCards } effect ||
            effect.Amount != receipt.Requested || !effect.CardKinds.SequenceEqual(receipt.FilterKinds) ||
            (effect.CardCategories.Count == 0 ? null : (SkillProgramCardCategory?)effect.CardCategories[0]) != receipt.Category ||
            receipt.CardIds is not System.Collections.IList { IsReadOnly: true } ||
            receipt.SelectedKinds is not System.Collections.IList { IsReadOnly: true } ||
            receipt.FilterKinds is not System.Collections.IList { IsReadOnly: true } || !ValidDeckCardGain(frame))
            throw new InvalidOperationException("The deck gain lost its original instruction, filter, immutable selection or exact physical movement.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 62010)]
    private bool IsDeckCardGainMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        effect?.Op == SkillProgramEffectOp.ObtainDeckCards && pending.SubjectSeat == frame.OwnerSeat &&
        pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidDeckCardGain(frame);

    [ProgramContinuation(ProgramContinuationKind.Movement, 62010)]
    private bool ReturnDeckCardGain(ProgramSkillFrame frame)
    {
        if (frame.DeckCardGain is null) return false;
        if (!ValidDeckCardGain(frame)) throw new InvalidOperationException("A deck gain lost its exact previously issued movement.");
        ReplaceRuntimeTop(frame with { DeckCardGain = null, PendingMovementContinuation = null });
        AdvanceRuntimeProgram(frame.Id);
        return true;
    }
}
