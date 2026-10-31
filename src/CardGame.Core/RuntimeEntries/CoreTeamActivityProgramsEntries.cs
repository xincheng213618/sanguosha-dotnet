#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreTeamActivityProgramsRuntime))]
internal sealed partial class CoreTeamActivityProgramsEntries
{
    private readonly Func<EngineOwners.CoreTeamActivityProgramsRuntime> _resolve;
    private CoreTeamActivityProgramsEntries(Func<EngineOwners.CoreTeamActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreTeamActivityProgramsEntries Create(Func<EngineOwners.CoreTeamActivityProgramsRuntime> resolve) => new(resolve);
}
