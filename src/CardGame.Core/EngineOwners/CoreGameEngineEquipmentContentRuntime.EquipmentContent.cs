#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentContentRuntime
{
    internal EquipmentContentRegistry EquipmentRules => _equipmentRulesOperations._equipmentRules ??= BuiltInEquipmentContent.CreateForMatch((_modeRuntimeOperations._modeRuntime.Find<IMatchModeEquipmentProfile>() ?? throw new InvalidOperationException("The selected mode requires an explicitly installed equipment profile.")).EquipmentRecipe, _contentRegistryOperations._contentRegistry, _contentRegistryOperations._contentRegistry.EquipmentPackages);
}
