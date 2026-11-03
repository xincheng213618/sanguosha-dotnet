#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLvKaiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
