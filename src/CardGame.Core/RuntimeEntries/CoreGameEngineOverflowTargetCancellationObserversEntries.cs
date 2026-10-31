#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime))]
internal sealed partial class CoreGameEngineOverflowTargetCancellationObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> _resolve;
    private CoreGameEngineOverflowTargetCancellationObserversEntries(Func<EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOverflowTargetCancellationObserversEntries Create(Func<EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime> resolve) => new(resolve);
}
