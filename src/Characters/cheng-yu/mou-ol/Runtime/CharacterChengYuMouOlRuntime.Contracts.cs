#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterChengYuMouOlMouLidanOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouLidanRuntime Get();
}

internal interface ICharacterChengYuMouOlMouDanchiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouDanchiRuntime Get();
}

internal interface ICharacterChengYuMouOlCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterChengYuMouOlCoreProgramTargetRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramTargetRules TargetRules { get; }
}
