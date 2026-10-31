#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActivityBattleRulesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityBattleRulesRuntime> _resolve;
    private CoreGameEngineActivityBattleRulesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityBattleRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActivityBattleRulesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActivityBattleRulesRuntime> resolve) => new(resolve);
}
