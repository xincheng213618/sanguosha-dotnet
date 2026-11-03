namespace CardGame.Core;

[ProgramRuntimeModule]
internal sealed class BoundaryYiJiRuntime(IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts,
    ICardZoneQueries cards, IProgramCardSnapshotQueries snapshots)
{
    private const string Jijie = "boundary:jijie-mobile";

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
}
