#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime))]
internal sealed partial class CoreGameEngineOriginalHandEntityObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> _resolve;
    private CoreGameEngineOriginalHandEntityObserversEntries(Func<EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandEntityObserversEntries Create(Func<EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> resolve) => new(resolve);
}
