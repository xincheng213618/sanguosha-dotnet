#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineModeMatchLimitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeMatchLimitsRuntime> _resolve;
    private CoreGameEngineModeMatchLimitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeMatchLimitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeMatchLimitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeMatchLimitsRuntime> resolve) => new(resolve);
}
