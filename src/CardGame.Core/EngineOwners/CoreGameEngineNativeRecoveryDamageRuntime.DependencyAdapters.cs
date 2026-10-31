#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(NativeHealthRecoveryRuntime))]
internal sealed class CoreNativeRecoveryDamageNativeHealthRecoveryAdapter(Func<NativeHealthRecoveryRuntime> targetFactory) : ICoreNativeRecoveryDamageNativeHealthRecoveryOperations
{
    private readonly Func<NativeHealthRecoveryRuntime> _targetFactory = targetFactory;
    public NativeHealthRecoveryRuntime Get() => this._targetFactory();
}
