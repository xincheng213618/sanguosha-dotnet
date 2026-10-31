#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandResultProjectionEntries
{
    internal EngineRunResult BuildResult() => _resolve().BuildResult();
    internal void RefreshPendingDecisionRevision() => _resolve().RefreshPendingDecisionRevision();
}
