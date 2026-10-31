#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreGuanduGeneralChoicesCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
