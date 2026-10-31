#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeDrawThresholdNativeDrawThresholdOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeDrawThresholdRuntime Get();
}

internal interface ICoreNativeDrawThresholdNativeDrawThresholdInvoiceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeDrawThresholdInvoiceRuntime Get();
}
