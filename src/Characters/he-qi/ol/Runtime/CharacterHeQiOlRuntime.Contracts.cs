#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHeQiOlCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IProgramSelectionQueries ProgramSelectionQueries { get; }
}

internal interface ICharacterHeQiOlCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}

internal interface ICharacterHeQiOlCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}
