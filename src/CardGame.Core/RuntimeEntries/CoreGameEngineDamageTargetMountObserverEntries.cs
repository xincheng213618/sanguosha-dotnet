#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetMountObserverEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime> _resolve;
    private CoreGameEngineDamageTargetMountObserverEntries(Func<EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetMountObserverEntries Create(Func<EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime> resolve) => new(resolve);
}
