#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedColorResponseObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> _resolve;
    private CoreGameEnginePairedColorResponseObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedColorResponseObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> resolve) => new(resolve);
}
