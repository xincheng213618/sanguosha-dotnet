using CardGame.Core.RuntimeSkills;

namespace CardGame.Core;

internal readonly record struct ProgramDeckCard(int Id, CardKind Kind);

internal interface IProgramDeckCardsFacts : IProgramSelectionQuery
{
    bool Alive(int seat);
    long MovementSequence { get; }
    IReadOnlyList<ProgramDeckCard> DrawPile { get; }
    IReadOnlyList<CardMovementRecord> MovementRange(long before, long after);
    IEnumerable<DeckCardsObtainedEvent> ObtainedEvents { get; }
    bool IsRedirectedGain(CardMovementRecord movement, CardLocation requested);
    bool IsDestroyedMaterial(CardMovementRecord movement);
}

internal interface IProgramDeckCardsOperations : IProgramRuntimeOperations
{
    bool EnsureDrawPile();
    void MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason,
        Action<long, IReadOnlyList<CardMovementRecord>> afterPhysicalRecords);
    SkillProgramStepOutcome AwaitMovements(long frameId, int ownerSeat);
}

/// <summary>Owns filtered physical deck gains and their paid movement returns.</summary>
[ProgramRuntimeModule,ProgramCapability]
internal sealed class ProgramDeckCardsRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramDeckCardsFacts facts, IProgramDeckCardsOperations operations) : IDeckCardsHost
{
    private const string ObtainDeckCardsReason = "skill-program.obtain-deck-cards";

    private static CardConversionSource Source(ProgramSkillFrame frame) =>
        new(frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);

    public SkillProgramStepOutcome ObtainDeckCards(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.DeckCardGain is not null || frames.Top?.Id != frame.Id)
            throw new InvalidOperationException("A deck gain requires its active unpaid program instruction.");
        if (!facts.Alive(frame.OwnerSeat) || !operations.EnsureDrawPile()) return SkillProgramStepOutcome.Continue;
        var cards = facts.DrawPile.Reverse().Where(card =>
            effect.CardKinds.Count > 0 ? effect.CardKinds.Contains(card.Kind) :
                RuntimeRules_Core_ProgramCardPayments.MatchesProgramCardCategory(card.Kind, effect.CardCategories)).Take(effect.Amount).ToArray();
        if (cards.Length == 0) return SkillProgramStepOutcome.Continue;
        var receipt = new DeckCardGainReceipt(frame.InstructionIndex, Source(frame), frame.GameplayHash,
            effect.Amount, effect.CardCategories.Count == 0 ? null : effect.CardCategories[0], facts.MovementSequence)
        {
            CardIds = cards.Select(card => card.Id).ToArray(), SelectedKinds = cards.Select(card => card.Kind).ToArray(),
            FilterKinds = effect.CardKinds
        };
        frames.ReplaceTop(frame with { DeckCardGain = receipt });
        operations.MoveCards(receipt.CardIds, CardLocation.Hand(frame.OwnerSeat), new(ObtainDeckCardsReason),
            (batch, records) =>
            {
                var active = frames.ActiveProgram(frame.Id);
                frames.ReplaceTop(active with { DeckCardGain = receipt with { BatchId = batch, SequenceAfter = facts.MovementSequence } });
                events.AdvanceFact(new DeckCardsObtainedEvent(frame.Id, receipt.InstructionIndex, frame.OwnerSeat,
                    records.Count(move => move.To == CardLocation.Hand(frame.OwnerSeat)), batch, receipt.SequenceBefore, facts.MovementSequence));
            });
        var outcome = operations.AwaitMovements(frame.Id, frame.OwnerSeat);
        if (outcome == SkillProgramStepOutcome.Continue)
            frames.ReplaceTop(frames.ActiveProgram(frame.Id) with { DeckCardGain = null });
        return outcome;
    }

    private bool ValidDeckCardGain(ProgramSkillFrame frame)
    {
        if (frame.DeckCardGain is not { } receipt || receipt.Source != Source(frame) ||
            receipt.GameplayHash != frame.GameplayHash || receipt.InstructionIndex != frame.InstructionIndex ||
            receipt.BatchId <= 0 || receipt.SequenceBefore < 0 || receipt.SequenceAfter <= receipt.SequenceBefore ||
            receipt.SequenceAfter > facts.MovementSequence || receipt.CardIds.Count is < 1 or > 4 ||
            receipt.CardIds.Count > receipt.Requested || receipt.CardIds.Distinct().Count() != receipt.CardIds.Count ||
            receipt.SelectedKinds.Count != receipt.CardIds.Count)
            return false;
        var moves = facts.MovementRange(receipt.SequenceBefore, receipt.SequenceAfter);
        if (moves.Count != receipt.CardIds.Count || !moves.Select(move => move.CardId).SequenceEqual(receipt.CardIds) ||
            !moves.Select(move => move.CardKind).SequenceEqual(receipt.SelectedKinds) ||
            moves.Any(move => move.From != CardLocation.DrawPile || move.Reason.Value != ObtainDeckCardsReason ||
                !(move.To == CardLocation.Hand(frame.OwnerSeat) || facts.IsRedirectedGain(move, CardLocation.Hand(frame.OwnerSeat)) ||
                  move.HappyZuoCiPhantomDestination is { RequestedTo: var requested } &&
                  requested == CardLocation.Hand(frame.OwnerSeat) && facts.IsDestroyedMaterial(move)) ||
                (receipt.Category is { } category
                    ? !RuntimeRules_Core_ProgramCardPayments.MatchesProgramCardCategory(move.CardKind, [category])
                    : !receipt.FilterKinds.Contains(move.CardKind)))) return false;
        return facts.ObtainedEvents.Count(fact => fact.ProgramFrameId == frame.Id &&
            fact.InstructionIndex == receipt.InstructionIndex && fact.OwnerSeat == frame.OwnerSeat && fact.BatchId == receipt.BatchId &&
            fact.Count == moves.Count(move => move.To == CardLocation.Hand(frame.OwnerSeat)) &&
            fact.SequenceBefore == receipt.SequenceBefore && fact.SequenceAfter == receipt.SequenceAfter) == 1;
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
        frames.ReplaceTop(frame with { DeckCardGain = null, PendingMovementContinuation = null });
        frames.AdvanceProgram(frame.Id);
        return true;
    }
}
