#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentPairPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> _resolve;
    private CoreGameEngineEquipmentPairPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentPairPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPairPaymentsRuntime> resolve) => new(resolve);
}
