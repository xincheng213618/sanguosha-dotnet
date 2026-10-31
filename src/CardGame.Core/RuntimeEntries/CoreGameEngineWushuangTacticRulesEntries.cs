#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineWushuangTacticRulesRuntime))]
internal sealed partial class CoreGameEngineWushuangTacticRulesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> _resolve;
    private CoreGameEngineWushuangTacticRulesEntries(Func<EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangTacticRulesEntries Create(Func<EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> resolve) => new(resolve);
}
