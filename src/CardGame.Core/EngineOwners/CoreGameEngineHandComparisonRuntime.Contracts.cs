#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreHandComparisonCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
