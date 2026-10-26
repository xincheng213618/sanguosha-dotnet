namespace CardGame.Core;

public sealed partial class GameEngine
{
    // 锋略's settlement gift: after the contest resolved, the owner's own pindian
    // card is claimed out of the discard pile into the counterpart's hand.
    private void GiveProgramPindianCard(ProgramSkillFrame frame, int targetSeat)
    {
        var active = GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var selected] || selected != targetSeat)
            throw new InvalidOperationException("Fenglue gift lost its selected counterpart.");
        var binding = active.PindianResultBindings.SingleOrDefault(b => b.Name == "fenglve-contest");
        if (binding?.NativePindianFrameId is not { } nativeId || binding.SourceSeat != frame.OwnerSeat ||
            binding.OpponentSeat != targetSeat ||
            ProgramEventHistory<PindianResultDeterminedEvent>().Where(e => e.FrameId == nativeId && e.SkillId == frame.SkillId)
                .ToArray() is not [var fact] || fact.Result.SourceSeat != binding.SourceSeat ||
            fact.Result.OpponentSeat != binding.OpponentSeat || fact.Result.SourceRank != binding.SourceRank ||
            fact.Result.OpponentRank != binding.OpponentRank || fact.Result.SourceWon != binding.SourceWon)
            throw new InvalidOperationException("Fenglue gift lost its exact native pindian result.");
        var result = fact.Result;
        var cardId = result.SourceCardId;
        var location = _cardZones.GetLocation(cardId);
        if (location.Zone != CardZoneKind.DiscardPile ||
            !_cardZones.CardsAt(location).Any(item => item.Id == cardId))
            throw new InvalidOperationException("The Fenglue pindian card left the discard pile.");
        var card = _cardZones.CardsAt(location).Single(item => item.Id == cardId);
        ReplaceRuntimeTop(active with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        MoveCards([card], location, CardLocation.Hand(targetSeat),
            new CardMoveReason($"skill-program.{frame.SkillId}.fenglve-gift"));
        if (!TryBeginCardsMovedProgramWindow()) ReturnRuntimeProgramMovement(frame.Id);
    }

    [ProgramCapability]
    private sealed class XunChenProgramCapability(GameEngine engine) : IXunChenProgramHost
    {
        public void GivePindianCard(ProgramSkillFrame frame, int targetSeat) =>
            engine.GiveProgramPindianCard(frame, targetSeat);
    }
}
