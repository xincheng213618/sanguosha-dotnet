#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhugeGuoMobileCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
