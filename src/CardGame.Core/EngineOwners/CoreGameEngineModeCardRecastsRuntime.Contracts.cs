#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreModeCardRecastsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
