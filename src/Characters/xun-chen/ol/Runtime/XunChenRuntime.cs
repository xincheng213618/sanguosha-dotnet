namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class XunChenRuntime(IProgramRuntimeFrames frames, ICardZoneEntryQueries zoneEntries, ICardZoneQueries zones,
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
        if (binding is not { NativePindianFrameId: > 0 } || binding.SourceSeat != active.OwnerSeat || binding.OpponentSeat != targetSeat)
            throw new InvalidOperationException("Fenglue gift lost its exact native pindian result.");
        if (binding.SourceDiscard is not { CardId: > 0, EntrySequence: > 0 } discard ||
            zoneEntries.CurrentEntry(CardLocation.DiscardPile, discard.CardId) != discard.EntrySequence)
            return;
        var card = zones.CardsAt(CardLocation.DiscardPile).Single(card => card.Id == discard.CardId);
        frames.ReplaceTop(active with { PendingMovementContinuation = new(active.OwnerSeat, 0, null) });
        movement.MoveCards([card], CardLocation.DiscardPile, CardLocation.Hand(targetSeat), new($"skill-program.{active.SkillId}.fenglve-gift"));
        if (!windows.Begin()) returns.ReturnMovement(frame.Id);
    }
}
