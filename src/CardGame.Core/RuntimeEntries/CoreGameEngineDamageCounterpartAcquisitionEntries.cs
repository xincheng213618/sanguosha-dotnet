#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime))]
internal sealed partial class CoreGameEngineDamageCounterpartAcquisitionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> _resolve;
    private CoreGameEngineDamageCounterpartAcquisitionEntries(Func<EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageCounterpartAcquisitionEntries Create(Func<EngineOwners.CoreGameEngineDamageCounterpartAcquisitionRuntime> resolve) => new(resolve);
}
