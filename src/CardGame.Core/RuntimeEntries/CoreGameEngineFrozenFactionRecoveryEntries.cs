#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenFactionRecoveryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenFactionRecoveryRuntime> _resolve;
    private CoreGameEngineFrozenFactionRecoveryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenFactionRecoveryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFrozenFactionRecoveryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenFactionRecoveryRuntime> resolve) => new(resolve);
}
