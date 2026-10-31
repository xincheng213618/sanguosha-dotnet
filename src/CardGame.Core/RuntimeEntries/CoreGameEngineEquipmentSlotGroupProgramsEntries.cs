#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime))]
internal sealed partial class CoreGameEngineEquipmentSlotGroupProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> _resolve;
    private CoreGameEngineEquipmentSlotGroupProgramsEntries(Func<EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentSlotGroupProgramsEntries Create(Func<EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> resolve) => new(resolve);
}
