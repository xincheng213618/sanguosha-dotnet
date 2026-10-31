#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime))]
internal sealed partial class CoreGameEngineDamageZoneAcquisitionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime> _resolve;
    private CoreGameEngineDamageZoneAcquisitionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageZoneAcquisitionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageZoneAcquisitionRuntime> resolve) => new(resolve);
}
