#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreFinalTargetGiftsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
