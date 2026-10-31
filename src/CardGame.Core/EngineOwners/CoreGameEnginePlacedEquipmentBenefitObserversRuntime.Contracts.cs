#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePlacedEquipmentBenefitObserversCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
