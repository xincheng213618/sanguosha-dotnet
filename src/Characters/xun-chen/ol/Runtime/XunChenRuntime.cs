namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class XunChenRuntime(IProgramRuntimeFrames frames, IProgramEventHistoryQueries history, ICardZoneQueries zones,
    IProgramBoundCardMovementOperations movement, IProgramMovementWindows windows,
    IProgramMovementReturns returns) : IXunChenProgramHost
{
    // The native contest's owner's card is claimed from its actual discard
    // location into the original selected counterpart's hand.
    public void GivePindianCard(ProgramSkillFrame frame, int targetSeat)
    {
        var active = frames.ActiveProgram(frame.Id);
        if (active.SelectedTargetSeats is not [var selected] || selected != targetSeat)
            throw new InvalidOperationException("Fenglue gift lost its selected counterpart.");
        var binding = active.PindianResultBindings.SingleOrDefault(item => item.Name == "fenglve-contest");
        if (binding?.NativePindianFrameId is not { } nativeId || binding.SourceSeat != frame.OwnerSeat || binding.OpponentSeat != targetSeat ||
            history.Read<PindianResultDeterminedEvent>().Where(item => item.FrameId == nativeId && item.SkillId == frame.SkillId)
                .ToArray() is not [var fact] || fact.Result.SourceSeat != binding.SourceSeat || fact.Result.OpponentSeat != binding.OpponentSeat ||
            fact.Result.SourceRank != binding.SourceRank || fact.Result.OpponentRank != binding.OpponentRank || fact.Result.SourceWon != binding.SourceWon)
            throw new InvalidOperationException("Fenglue gift lost its exact native pindian result.");
        var cardId = fact.Result.SourceCardId;
        var location = zones.GetLocation(cardId);
        if (location.Zone != CardZoneKind.DiscardPile || !zones.CardsAt(location).Any(card => card.Id == cardId))
            throw new InvalidOperationException("The Fenglue pindian card left the discard pile.");
        var card = zones.CardsAt(location).Single(card => card.Id == cardId);
        frames.ReplaceTop(active with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        movement.MoveCards([card], location, CardLocation.Hand(targetSeat), new($"skill-program.{frame.SkillId}.fenglve-gift"));
        if (!windows.Begin()) returns.ReturnMovement(frame.Id);
    }
}
