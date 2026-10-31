#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLuSuMobileShiCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
