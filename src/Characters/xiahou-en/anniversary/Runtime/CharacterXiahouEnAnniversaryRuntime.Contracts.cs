#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterXiahouEnAnniversaryCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
