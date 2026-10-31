#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterPangJiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
