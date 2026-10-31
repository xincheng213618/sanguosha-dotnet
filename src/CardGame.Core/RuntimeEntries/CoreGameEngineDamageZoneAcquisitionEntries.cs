#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime))]
internal sealed partial class CoreGameEngineDamageZoneAcquisitionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime> _resolve;
    private CoreGameEngineDamageZoneAcquisitionEntries(Func<EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageZoneAcquisitionEntries Create(Func<EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime> resolve) => new(resolve);
}
