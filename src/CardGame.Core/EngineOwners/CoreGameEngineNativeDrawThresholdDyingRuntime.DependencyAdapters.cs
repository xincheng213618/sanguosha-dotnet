#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(NativeDrawThresholdRuntime))]
internal sealed class CoreNativeDrawThresholdDyingNativeDrawThresholdAdapter(Func<NativeDrawThresholdRuntime> targetFactory) : ICoreNativeDrawThresholdDyingNativeDrawThresholdOperations
{
    private readonly Func<NativeDrawThresholdRuntime> _targetFactory = targetFactory;
    public NativeDrawThresholdRuntime Get() => this._targetFactory();
}
