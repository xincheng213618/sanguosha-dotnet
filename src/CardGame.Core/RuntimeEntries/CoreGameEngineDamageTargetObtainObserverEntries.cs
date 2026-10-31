#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetObtainObserverEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainObserverRuntime> _resolve;
    private CoreGameEngineDamageTargetObtainObserverEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainObserverRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetObtainObserverEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetObtainObserverRuntime> resolve) => new(resolve);
}
