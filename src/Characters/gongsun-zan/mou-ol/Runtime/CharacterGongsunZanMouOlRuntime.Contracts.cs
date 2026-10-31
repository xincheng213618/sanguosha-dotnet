#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterGongsunZanMouOlCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetAttackRange(int sourceSeat);
}

internal interface ICharacterGongsunZanMouOlCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
