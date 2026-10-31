#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICharacterSunShangxiangMouMobileMouSunShangxiangOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MouSunShangxiangRuntime Get();
}

internal interface ICharacterSunShangxiangMouMobileCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICharacterSunShangxiangMouMobileCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
