#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineMatchingRecastObserversRuntime))]
internal sealed partial class CoreGameEngineMatchingRecastObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineMatchingRecastObserversRuntime> _resolve;
    private CoreGameEngineMatchingRecastObserversEntries(Func<EngineOwners.CoreGameEngineMatchingRecastObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMatchingRecastObserversEntries Create(Func<EngineOwners.CoreGameEngineMatchingRecastObserversRuntime> resolve) => new(resolve);
}
