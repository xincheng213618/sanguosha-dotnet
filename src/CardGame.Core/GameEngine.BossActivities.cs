namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int BossActivitySeat => (Math.Max(0, _options.HumanSeat) + 3) % _playerCount;
    private int BossActivityStageIndex => CompleteProgramEventHistory().OfType<BossActivityStageStartedEvent>()
        .Select(e => e.StageIndex).DefaultIfEmpty(0).Last();
}
