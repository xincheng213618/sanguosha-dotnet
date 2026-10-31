#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreOutsidePhaseDrawDiscardNativeDrawInvoiceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeDrawInvoiceRuntime Get();
}

internal interface ICoreOutsidePhaseDrawDiscardCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
