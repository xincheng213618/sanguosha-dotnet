#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhugeGuoOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
