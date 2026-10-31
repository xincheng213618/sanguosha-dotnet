#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheEmptySlotEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheEmptySlotEquipmentRuntime> _resolve;
    private CoreGameEngineShanheEmptySlotEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheEmptySlotEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheEmptySlotEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheEmptySlotEquipmentRuntime> resolve) => new(resolve);
}
