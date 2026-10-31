#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterWeiYanHappyGodCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}

internal interface ICharacterWeiYanHappyGodCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}
