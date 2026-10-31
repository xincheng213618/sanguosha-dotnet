#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDengAiMobileShiCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
