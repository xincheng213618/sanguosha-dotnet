#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICorePublicDamagePredictionCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICorePublicDamagePredictionCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICorePublicDamagePredictionCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}
