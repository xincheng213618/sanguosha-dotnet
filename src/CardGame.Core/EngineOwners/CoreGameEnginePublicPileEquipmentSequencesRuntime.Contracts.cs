#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICorePublicPileEquipmentSequencesCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
