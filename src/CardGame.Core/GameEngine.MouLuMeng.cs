namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.HashSet<int> _roundTurnSeats => ref _actualRoundProgress.ReferenceToRoundTurnSeats;
    private ref int _roundNumber => ref _actualRoundProgress.ReferenceToRoundNumber;
}
