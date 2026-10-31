#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreEquipmentSlotGroupProgramsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreEquipmentSlotGroupProgramsProgramTargetSelectionOperations : IProgramRuntimeOperations
{
    ProgramTargetSelectionRuntime Get();
}
