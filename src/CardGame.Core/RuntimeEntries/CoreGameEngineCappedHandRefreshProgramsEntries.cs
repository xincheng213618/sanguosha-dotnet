#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime))]
internal sealed partial class CoreGameEngineCappedHandRefreshProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> _resolve;
    private CoreGameEngineCappedHandRefreshProgramsEntries(Func<EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCappedHandRefreshProgramsEntries Create(Func<EngineOwners.CoreGameEngineCappedHandRefreshProgramsRuntime> resolve) => new(resolve);
}
