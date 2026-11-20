#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreEquipmentSlotGroupProgramsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
