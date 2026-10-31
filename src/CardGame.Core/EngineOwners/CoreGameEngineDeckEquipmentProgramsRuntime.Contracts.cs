#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreDeckEquipmentProgramsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
