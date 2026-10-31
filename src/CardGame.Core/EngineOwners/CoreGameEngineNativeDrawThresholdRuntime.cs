#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineNativeDrawThresholdRuntime
{
    private readonly ICoreNativeDrawThresholdNativeDrawThresholdInvoiceOperations _nativeDrawThresholdInvoiceOperations;
    private readonly ICoreNativeDrawThresholdNativeDrawThresholdOperations _nativeDrawThresholdOperations;
    internal CoreGameEngineNativeDrawThresholdRuntime(ICoreNativeDrawThresholdNativeDrawThresholdInvoiceOperations nativeDrawThresholdInvoiceOperations, ICoreNativeDrawThresholdNativeDrawThresholdOperations nativeDrawThresholdOperations)
    {
        this._nativeDrawThresholdInvoiceOperations = nativeDrawThresholdInvoiceOperations;
        this._nativeDrawThresholdOperations = nativeDrawThresholdOperations;
    }
}
