#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime))]
internal sealed partial class CoreGameEngineProgramCardUseRulesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> _resolve;
    private CoreGameEngineProgramCardUseRulesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardUseRulesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> resolve) => new(resolve);
}
