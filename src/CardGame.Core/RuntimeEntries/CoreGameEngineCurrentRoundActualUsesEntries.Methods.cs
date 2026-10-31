#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentRoundActualUsesEntries
{
    internal int CurrentRoundActualUseCount(int actorSeat) => _resolve().CurrentRoundActualUseCount(actorSeat);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CurrentUseKindDeduplicationHistory() => _resolve().CurrentUseKindDeduplicationHistory();
    internal bool TracksCurrentRoundActualUses { get => _resolve().TracksCurrentRoundActualUses; }
}
