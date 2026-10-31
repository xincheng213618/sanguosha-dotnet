#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreEquipmentPlacementAndHpPairCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
