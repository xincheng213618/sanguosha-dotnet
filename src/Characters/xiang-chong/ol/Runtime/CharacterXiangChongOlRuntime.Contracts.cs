#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterXiangChongOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
