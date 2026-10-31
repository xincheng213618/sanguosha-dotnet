#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCapturedEquipmentDrawsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
