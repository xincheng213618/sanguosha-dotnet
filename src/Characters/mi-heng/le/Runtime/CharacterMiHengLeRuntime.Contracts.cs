#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterMiHengLeCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
