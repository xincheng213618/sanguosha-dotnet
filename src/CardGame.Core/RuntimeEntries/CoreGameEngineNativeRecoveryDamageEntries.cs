#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime))]
internal sealed partial class CoreGameEngineNativeRecoveryDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> _resolve;
    private CoreGameEngineNativeRecoveryDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeRecoveryDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeRecoveryDamageRuntime> resolve) => new(resolve);
}
