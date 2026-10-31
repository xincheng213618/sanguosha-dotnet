#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreAlternatingSuitAndCompletedTopCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
