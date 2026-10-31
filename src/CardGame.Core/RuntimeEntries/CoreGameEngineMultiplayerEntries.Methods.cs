#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMultiplayerEntries
{
    internal bool IsHumanControlledSeat(int seat) => _resolve().IsHumanControlledSeat(seat);
    internal int HumanDecisionSeat { get => _resolve().HumanDecisionSeat; }
}
