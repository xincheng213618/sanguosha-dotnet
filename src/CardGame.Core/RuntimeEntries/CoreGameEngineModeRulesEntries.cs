#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime))]
internal sealed partial class CoreGameEngineModeRulesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> _resolve;
    private CoreGameEngineModeRulesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeRulesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeRulesRuntime> resolve) => new(resolve);
}
