#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterWangYunOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
