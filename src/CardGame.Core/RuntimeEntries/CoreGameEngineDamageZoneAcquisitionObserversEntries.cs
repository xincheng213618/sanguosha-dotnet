#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageZoneAcquisitionObserversRuntime))]
internal sealed partial class CoreGameEngineDamageZoneAcquisitionObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageZoneAcquisitionObserversRuntime> _resolve;
    private CoreGameEngineDamageZoneAcquisitionObserversEntries(Func<EngineOwners.CoreGameEngineDamageZoneAcquisitionObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageZoneAcquisitionObserversEntries Create(Func<EngineOwners.CoreGameEngineDamageZoneAcquisitionObserversRuntime> resolve) => new(resolve);
}
