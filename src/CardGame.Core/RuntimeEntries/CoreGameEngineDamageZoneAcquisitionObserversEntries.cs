#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageZoneAcquisitionObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageZoneAcquisitionObserversRuntime> _resolve;
    private CoreGameEngineDamageZoneAcquisitionObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageZoneAcquisitionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageZoneAcquisitionObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageZoneAcquisitionObserversRuntime> resolve) => new(resolve);
}
