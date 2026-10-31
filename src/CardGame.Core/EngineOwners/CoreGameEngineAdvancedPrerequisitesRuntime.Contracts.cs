#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAdvancedPrerequisitesCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
