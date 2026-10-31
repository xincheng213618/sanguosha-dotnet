#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreRevealedCardHpComparisonCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
