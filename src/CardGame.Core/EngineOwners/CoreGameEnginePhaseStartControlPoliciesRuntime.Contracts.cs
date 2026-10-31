#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePhaseStartControlPoliciesCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
