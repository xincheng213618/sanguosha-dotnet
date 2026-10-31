#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreGuanduEquipmentCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
