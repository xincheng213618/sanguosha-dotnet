#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreEquipmentSkillsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
