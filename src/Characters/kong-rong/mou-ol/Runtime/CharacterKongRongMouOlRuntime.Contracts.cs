#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterKongRongMouOlMouKongRongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouKongRongRuntime Get();
}

internal interface ICharacterKongRongMouOlCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
