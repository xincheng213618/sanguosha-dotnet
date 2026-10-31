#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICorePrepDiscardReceiptsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
