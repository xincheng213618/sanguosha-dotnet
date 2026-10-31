#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePrepDiscardEndingPromisesCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
