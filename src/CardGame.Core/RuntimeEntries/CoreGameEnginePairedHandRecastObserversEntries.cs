#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedHandRecastObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime> _resolve;
    private CoreGameEnginePairedHandRecastObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedHandRecastObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedHandRecastObserversRuntime> resolve) => new(resolve);
}
