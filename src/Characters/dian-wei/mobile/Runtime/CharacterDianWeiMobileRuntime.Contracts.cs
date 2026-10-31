#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterDianWeiMobileCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
