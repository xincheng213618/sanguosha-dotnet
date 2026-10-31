#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterCaoShuangMouMouCaoShuangAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouCaoShuangAnniversaryRuntime Get();
}

internal interface ICharacterCaoShuangMouCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
