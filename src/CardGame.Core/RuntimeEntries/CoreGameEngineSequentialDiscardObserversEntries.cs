#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSequentialDiscardObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardObserversRuntime> _resolve;
    private CoreGameEngineSequentialDiscardObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDiscardObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardObserversRuntime> resolve) => new(resolve);
}
