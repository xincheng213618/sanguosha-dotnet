#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime))]
internal sealed partial class CoreGameEngineNativeRecoveryDamageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> _resolve;
    private CoreGameEngineNativeRecoveryDamageEntries(Func<EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeRecoveryDamageEntries Create(Func<EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> resolve) => new(resolve);
}
