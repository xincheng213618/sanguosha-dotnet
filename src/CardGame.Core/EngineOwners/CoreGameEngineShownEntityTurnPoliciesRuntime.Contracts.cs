#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreShownEntityTurnPoliciesCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
