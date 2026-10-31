#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoRenBoundaryCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
