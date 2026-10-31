#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime))]
internal sealed partial class CoreGameEngineEquipmentPairPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> _resolve;
    private CoreGameEngineEquipmentPairPaymentsEntries(Func<EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentPairPaymentsEntries Create(Func<EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> resolve) => new(resolve);
}
