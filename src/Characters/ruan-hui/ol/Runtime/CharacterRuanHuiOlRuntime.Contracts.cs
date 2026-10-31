#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterRuanHuiOlCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
