#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(NativeDrawOrdinalTriggerRuntime))]
internal sealed class CoreNativeDrawOrdinalsNativeDrawOrdinalTriggerAdapter(Func<NativeDrawOrdinalTriggerRuntime> targetFactory) : ICoreNativeDrawOrdinalsNativeDrawOrdinalTriggerOperations
{
    private readonly Func<NativeDrawOrdinalTriggerRuntime> _targetFactory = targetFactory;
    public NativeDrawOrdinalTriggerRuntime Get() => this._targetFactory();
}
