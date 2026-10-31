#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCardUseRulesRuntime))]
internal sealed partial class CoreGameEngineProgramCardUseRulesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> _resolve;
    private CoreGameEngineProgramCardUseRulesEntries(Func<EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardUseRulesEntries Create(Func<EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> resolve) => new(resolve);
}
