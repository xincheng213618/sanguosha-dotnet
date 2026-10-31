#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentRoundActualUsesEntries
{
    internal int CurrentRoundActualUseCount(int actorSeat) => _resolve().CurrentRoundActualUseCount(actorSeat);
    internal IEnumerable<IGameEvent> CurrentUseKindDeduplicationHistory() => _resolve().CurrentUseKindDeduplicationHistory();
    internal bool TracksCurrentRoundActualUses { get => _resolve().TracksCurrentRoundActualUses; }
}
