#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOverflowTargetCancellationObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> _resolve;
    private CoreGameEngineOverflowTargetCancellationObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOverflowTargetCancellationObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> resolve) => new(resolve);
}
