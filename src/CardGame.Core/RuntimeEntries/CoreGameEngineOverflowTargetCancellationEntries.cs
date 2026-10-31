#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime))]
internal sealed partial class CoreGameEngineOverflowTargetCancellationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> _resolve;
    private CoreGameEngineOverflowTargetCancellationEntries(Func<EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOverflowTargetCancellationEntries Create(Func<EngineOwners.CoreGameEngineOverflowTargetCancellationRuntime> resolve) => new(resolve);
}
