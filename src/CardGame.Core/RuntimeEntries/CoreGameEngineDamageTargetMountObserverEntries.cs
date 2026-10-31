#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetMountObserverEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime> _resolve;
    private CoreGameEngineDamageTargetMountObserverEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetMountObserverEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime> resolve) => new(resolve);
}
