#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaiFuRenOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
