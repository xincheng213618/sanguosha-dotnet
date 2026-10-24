namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Match options and content are frozen before the first equipment query.
    // The cache contains immutable definitions only, never card or frame state.
    private EquipmentContentRegistry? _equipmentRules;
    private EquipmentContentRegistry EquipmentRules => _equipmentRules ??=
        BuiltInEquipmentContent.CreateForMatch((_modeRuntime.Find<IMatchModeEquipmentProfile>() ??
            throw new InvalidOperationException("The selected mode requires an explicitly installed equipment profile."))
            .EquipmentRecipe, _contentRegistry, _contentRegistry.EquipmentPackages);
}
