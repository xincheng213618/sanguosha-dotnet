#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterRuiJiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
