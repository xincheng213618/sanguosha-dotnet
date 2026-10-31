#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNextCardTargetAdjustmentsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
