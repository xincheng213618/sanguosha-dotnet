#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFaultBoundaryEntries
{
    internal GameCheckpoint BuildTrustedCheckpoint(long revision) => _resolve().BuildTrustedCheckpoint(revision);
}
