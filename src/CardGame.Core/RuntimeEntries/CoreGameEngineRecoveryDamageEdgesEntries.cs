#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime))]
internal sealed partial class CoreGameEngineRecoveryDamageEdgesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> _resolve;
    private CoreGameEngineRecoveryDamageEdgesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecoveryDamageEdgesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> resolve) => new(resolve);
}
