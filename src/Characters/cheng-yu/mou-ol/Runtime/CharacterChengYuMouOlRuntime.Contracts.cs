#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterChengYuMouOlMouLidanOperations : IProgramRuntimeOperations
{
    MouLidanRuntime Get();
}

internal interface ICharacterChengYuMouOlMouDanchiOperations : IProgramRuntimeOperations
{
    MouDanchiRuntime Get();
}

internal interface ICharacterChengYuMouOlCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterChengYuMouOlCoreProgramTargetRulesOperations : IProgramRuntimeOperations
{
    ProgramTargetRules TargetRules { get; }
}
