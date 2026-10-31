#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangAiCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreWushuangAiCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.WushuangSnapshot? CreateWushuangSnapshot(int viewerSeat);
}
