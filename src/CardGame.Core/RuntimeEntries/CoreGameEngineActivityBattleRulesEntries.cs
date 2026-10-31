#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActivityBattleRulesRuntime))]
internal sealed partial class CoreGameEngineActivityBattleRulesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActivityBattleRulesRuntime> _resolve;
    private CoreGameEngineActivityBattleRulesEntries(Func<EngineOwners.CoreGameEngineActivityBattleRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActivityBattleRulesEntries Create(Func<EngineOwners.CoreGameEngineActivityBattleRulesRuntime> resolve) => new(resolve);
}
