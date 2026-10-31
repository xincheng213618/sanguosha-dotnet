#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineModeRulesRuntime))]
internal sealed partial class CoreGameEngineModeRulesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineModeRulesRuntime> _resolve;
    private CoreGameEngineModeRulesEntries(Func<EngineOwners.CoreGameEngineModeRulesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeRulesEntries Create(Func<EngineOwners.CoreGameEngineModeRulesRuntime> resolve) => new(resolve);
}
