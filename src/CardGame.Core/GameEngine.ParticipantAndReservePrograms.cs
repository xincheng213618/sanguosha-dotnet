namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref bool _gameStartingProgramsResolved => ref _matchSetup.ReferenceToGameStartingProgramsResolved;
}
