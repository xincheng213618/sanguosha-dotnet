#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalHandEntityObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> _resolve;
    private CoreGameEngineOriginalHandEntityObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandEntityObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntityObserversRuntime> resolve) => new(resolve);
}
