#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFaultBoundaryEntries
{
    internal global::CardGame.Core.GameCheckpoint BuildTrustedCheckpoint(long revision) => _resolve().BuildTrustedCheckpoint(revision);
}
