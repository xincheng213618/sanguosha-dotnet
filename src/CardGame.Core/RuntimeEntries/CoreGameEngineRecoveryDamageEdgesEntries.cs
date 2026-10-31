#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime))]
internal sealed partial class CoreGameEngineRecoveryDamageEdgesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> _resolve;
    private CoreGameEngineRecoveryDamageEdgesEntries(Func<EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecoveryDamageEdgesEntries Create(Func<EngineOwners.CoreGameEngineRecoveryDamageEdgesRuntime> resolve) => new(resolve);
}
