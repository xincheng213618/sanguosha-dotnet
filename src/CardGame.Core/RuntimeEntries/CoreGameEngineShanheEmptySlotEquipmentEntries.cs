#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheEmptySlotEquipmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheEmptySlotEquipmentRuntime> _resolve;
    private CoreGameEngineShanheEmptySlotEquipmentEntries(Func<EngineOwners.CoreGameEngineShanheEmptySlotEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheEmptySlotEquipmentEntries Create(Func<EngineOwners.CoreGameEngineShanheEmptySlotEquipmentRuntime> resolve) => new(resolve);
}
