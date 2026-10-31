#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime))]
internal sealed partial class CoreGameEngineEquipmentSlotGroupProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> _resolve;
    private CoreGameEngineEquipmentSlotGroupProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEquipmentSlotGroupProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSlotGroupProgramsRuntime> resolve) => new(resolve);
}
