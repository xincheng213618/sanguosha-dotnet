#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSunShaoMobileShiCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
