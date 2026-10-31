#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterWenYangOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
