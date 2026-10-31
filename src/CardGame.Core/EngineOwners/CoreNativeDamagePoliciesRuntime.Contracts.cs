#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeDamagePoliciesCoreProgramDamageHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDamageHooks DamageHooks { get; }
}

internal interface ICoreNativeDamagePoliciesCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}
