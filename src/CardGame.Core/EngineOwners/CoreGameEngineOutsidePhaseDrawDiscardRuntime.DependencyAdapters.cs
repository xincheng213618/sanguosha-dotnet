#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(NativeDrawInvoiceRuntime))]
internal sealed class CoreOutsidePhaseDrawDiscardNativeDrawInvoiceAdapter(Func<NativeDrawInvoiceRuntime> targetFactory) : ICoreOutsidePhaseDrawDiscardNativeDrawInvoiceOperations
{
    private readonly Func<NativeDrawInvoiceRuntime> _targetFactory = targetFactory;
    public NativeDrawInvoiceRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CoreOutsidePhaseDrawDiscardCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICoreOutsidePhaseDrawDiscardCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
