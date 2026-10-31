#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(NativeDrawAttemptRuntime))]
internal sealed class CoreNativeDrawAttemptsNativeDrawAttemptAdapter(Func<NativeDrawAttemptRuntime> targetFactory) : ICoreNativeDrawAttemptsNativeDrawAttemptOperations
{
    private readonly Func<NativeDrawAttemptRuntime> _targetFactory = targetFactory;
    public NativeDrawAttemptRuntime Get() => this._targetFactory();
}
