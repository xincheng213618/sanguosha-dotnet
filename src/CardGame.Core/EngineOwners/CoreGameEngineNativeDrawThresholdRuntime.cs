#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
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
