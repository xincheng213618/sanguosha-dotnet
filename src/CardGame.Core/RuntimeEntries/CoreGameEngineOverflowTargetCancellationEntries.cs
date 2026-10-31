#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOverflowTargetCancellationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> _resolve;
    private CoreGameEngineOverflowTargetCancellationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOverflowTargetCancellationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> resolve) => new(resolve);
}
