#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(NativeDrawProducerRuntime))]
internal sealed class CoreProgramNativeDrawNativeDrawProducerAdapter(Func<NativeDrawProducerRuntime> targetFactory) : ICoreProgramNativeDrawNativeDrawProducerOperations
{
    private readonly Func<NativeDrawProducerRuntime> _targetFactory = targetFactory;
    public NativeDrawProducerRuntime Get() => this._targetFactory();
}
