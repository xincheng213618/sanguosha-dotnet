#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterSimaHuiOlCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICharacterSimaHuiOlCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}
