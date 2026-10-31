#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreFinalTargetTurnCountsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
