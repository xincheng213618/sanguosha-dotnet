namespace CardGame.Core;

[ProgramRuntimeModule]
internal sealed class BoundaryYiJiRuntime(IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts,
    ISelectionParticipantQueries participants, IProgramTriggerQueries triggers, IProgramContentQueries content,
    ICardZoneQueries cards, IProgramCardSnapshotQueries snapshots, INativeGivenCardQueries givenCards)
{
    private const string Jijie = "boundary:jijie-mobile";
    private const string Jiyuan = "boundary:jiyuan-mobile";

    internal bool Enabled => content.HasSkill(Jiyuan);
    internal static bool IsGiveCandidate(ProgramTriggerCandidate candidate) =>
        candidate.SkillId == Jiyuan && candidate.BindingId == "native-give-draw";

    // Give producers retain their exact recipient and physical batch. A hand-loss
    // observer would also count cards taken away and miss Jijie's deck source.
    internal IEnumerable<ProgramTriggerCandidate> CollectGiveCandidates(CardMovementBatchContext batch)
    {
        foreach (var owner in participants.Seats.Where(participants.IsAlive).ToArray())
        foreach (var candidate in triggers.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved).Where(IsGiveCandidate))
        foreach (var gift in givenCards.GiftsForBatch(batch, owner))
        {
            if (gift.Recipient == owner || !participants.IsAlive(gift.Recipient)) continue;
            yield return candidate with
            {
                OccurrenceIndex = Array.FindIndex(batch.Movements.ToArray(), movement => movement.Sequence == gift.CompletionSequence)
            };
        }
    }

    internal ProgramSkillWindowContext CreateGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (!IsGiveCandidate(candidate) || candidate.OccurrenceIndex < 0 || candidate.OccurrenceIndex >= frame.Batch.Movements.Count)
            throw new InvalidOperationException("Jiyuan lost its actual native Give occurrence.");
        var gift = givenCards.GiftsForBatch(frame.Batch, candidate.OwnerSeat).Single(gift =>
            gift.CompletionSequence == frame.Batch.Movements[candidate.OccurrenceIndex].Sequence);
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat,
            SourceSeat: gift.Giver, TargetSeat: gift.Recipient, Amount: gift.Movements.Count,
            OccurrenceIndex: candidate.OccurrenceIndex, Facts: triggers.CaptureProgramTriggerFacts(candidate.OwnerSeat),
            MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
    }

    internal bool IsCurrentGive(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        IsGiveCandidate(candidate) && context.MovementBatch is { } batch &&
        context.Window == SkillProgramTriggerWindow.CardsMoved && context.SourceSeat == candidate.OwnerSeat &&
        context.MovementIndex == candidate.OccurrenceIndex && context.OccurrenceIndex == candidate.OccurrenceIndex &&
        candidate.OccurrenceIndex >= 0 && candidate.OccurrenceIndex < batch.Movements.Count &&
        context.TargetSeat is { } recipient && recipient != candidate.OwnerSeat && participants.IsValidSeat(recipient) && participants.IsAlive(recipient) &&
        givenCards.GiftsForBatch(batch, candidate.OwnerSeat).Any(gift => gift.Recipient == recipient && gift.Movements.Count == context.Amount &&
            gift.CompletionSequence == batch.Movements[candidate.OccurrenceIndex].Sequence);

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 600)]
    private void CollectWindow_CollectBoundaryYiJiGiveCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) =>
        result.AddRange(CollectGiveCandidates(batch));

    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 100)]
    private bool ExcludeOrdinaryMovement_IsBoundaryYiJiGiveCandidate(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) =>
        IsGiveCandidate(candidate);

    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 400)]
    private ProgramSkillWindowContext? TryCreateBoundaryYiJiGiveContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) =>
        IsGiveCandidate(candidate) ? CreateGiveContext(frame, candidate) : null;

    // No cards move and no child runs during this private decision. Command
    // replay therefore reconstructs the same actual bottom card.
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 51000)]
    private CardSnapshot[] PrivateBottomCard(int viewer)
    {
        if (frames.Top is not ProgramSkillFrame
            { SkillId: Jijie, ActivationId: "view-and-give", TriggerId: null, InstructionIndex: 1, SelectedTargetSeats.Count: 0 } frame ||
            frame.OwnerSeat != viewer || prompts.Current is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt ||
            prompt.PlayerSeat != viewer || prompt.SkillPrompt?.SkillId != Jijie || prompt.Choices.Count == 0 ||
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") != "select-target" ||
                choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture)))
            return [];
        return cards.CardsAt(CardLocation.DrawPile).FirstOrDefault() is { } card ? [snapshots.Snapshot(card)] : [];
    }
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 10, ProgramTriggerProofMatch.Decision)]
    private bool? ProveGivenMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        IsGiveCandidate(candidate) ? IsCurrentGive(candidate, context) : null;
}
