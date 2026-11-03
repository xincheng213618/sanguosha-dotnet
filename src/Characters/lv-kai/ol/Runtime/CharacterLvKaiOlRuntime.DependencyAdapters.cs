#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterLvKaiOlCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICharacterLvKaiOlCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}
