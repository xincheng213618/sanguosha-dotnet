#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnTrickUsesEntries
{
    internal int ActualTurnTrickUseCount(int ownerSeat) => _resolve().ActualTurnTrickUseCount(ownerSeat);
    internal void ObserveActualTurnTrickUse(IGameEvent payload) => _resolve().ObserveActualTurnTrickUse(payload);
    internal bool TracksActualTurnTrickUses { get => _resolve().TracksActualTurnTrickUses; }
}
