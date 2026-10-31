#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeMatchLimitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineModeMatchLimitsRuntime> _resolve;
    private CoreGameEngineModeMatchLimitsEntries(Func<EngineOwners.CoreGameEngineModeMatchLimitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeMatchLimitsEntries Create(Func<EngineOwners.CoreGameEngineModeMatchLimitsRuntime> resolve) => new(resolve);
}
