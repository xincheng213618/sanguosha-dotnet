namespace CardGame.Core;

public sealed partial class GameEngine
{

    private void AdvanceToHumanBoundary() => _matchExecutionLoop.AdvanceToHumanBoundary();
}
