#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSimaYiMouCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterSimaYiMouMouSimaYiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouSimaYiRuntime Get();
}
