#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuoJunOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
