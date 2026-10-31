#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhugeJinMouMouZhugeJinOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouZhugeJinRuntime Get();
}

internal interface ICharacterZhugeJinMouCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
