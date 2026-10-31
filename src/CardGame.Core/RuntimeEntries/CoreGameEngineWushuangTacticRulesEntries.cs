#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangTacticRulesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> _resolve;
    private CoreGameEngineWushuangTacticRulesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangTacticRulesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangTacticRulesRuntime> resolve) => new(resolve);
}
