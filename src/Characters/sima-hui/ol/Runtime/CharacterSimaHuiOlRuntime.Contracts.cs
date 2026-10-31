#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSimaHuiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
