#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetCardPreselectionEntries
{
    internal long TargetHandMovementSequence(int seat) => _resolve().TargetHandMovementSequence(seat);
}
