#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterHuangQuanMouCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICharacterHuangQuanMouBoundaryYiJiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.BoundaryYiJiRuntime Get();
}

internal interface ICharacterHuangQuanMouMouHuangQuanOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouHuangQuanRuntime Get();
}
