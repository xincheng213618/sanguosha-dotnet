namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref int? _winnerSeat => ref _matchOutcome.ReferenceToWinnerSeat;
}
