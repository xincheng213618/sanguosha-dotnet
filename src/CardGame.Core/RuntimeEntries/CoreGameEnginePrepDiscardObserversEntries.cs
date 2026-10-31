#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardObserversRuntime> _resolve;
    private CoreGameEnginePrepDiscardObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardObserversRuntime> resolve) => new(resolve);
}
