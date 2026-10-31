#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(NativeDrawThresholdRuntime))]
internal sealed class CoreNativeDrawThresholdNativeDrawThresholdAdapter(Func<NativeDrawThresholdRuntime> targetFactory) : ICoreNativeDrawThresholdNativeDrawThresholdOperations
{
    private readonly Func<NativeDrawThresholdRuntime> _targetFactory = targetFactory;
    public NativeDrawThresholdRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(NativeDrawThresholdInvoiceRuntime))]
internal sealed class CoreNativeDrawThresholdNativeDrawThresholdInvoiceAdapter(Func<NativeDrawThresholdInvoiceRuntime> targetFactory) : ICoreNativeDrawThresholdNativeDrawThresholdInvoiceOperations
{
    private readonly Func<NativeDrawThresholdInvoiceRuntime> _targetFactory = targetFactory;
    public NativeDrawThresholdInvoiceRuntime Get() => this._targetFactory();
}
