#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenFactionRecoveryEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFrozenFactionRecoveryRuntime> _resolve;
    private CoreGameEngineFrozenFactionRecoveryEntries(Func<EngineOwners.CoreGameEngineFrozenFactionRecoveryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFrozenFactionRecoveryEntries Create(Func<EngineOwners.CoreGameEngineFrozenFactionRecoveryRuntime> resolve) => new(resolve);
}
