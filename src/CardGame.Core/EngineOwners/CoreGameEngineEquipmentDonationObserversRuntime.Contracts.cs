#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreEquipmentDonationObserversCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
