#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterYangFuOlCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterYangFuOlYangFuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.YangFuRuntime Get();
}

internal interface ICharacterYangFuOlCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}
