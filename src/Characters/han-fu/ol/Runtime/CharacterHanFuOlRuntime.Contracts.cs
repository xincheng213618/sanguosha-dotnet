#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHanFuOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
