#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawPhaseDrawHandLimitEntries
{
    internal int CurrentTurnDrawPhaseDrawCount(int ownerSeat) => _resolve().CurrentTurnDrawPhaseDrawCount(ownerSeat);
}
