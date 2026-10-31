#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGhostActivityProgramsRuntime))]
internal sealed partial class CoreGhostActivityProgramsEntries
{
    private readonly Func<EngineOwners.CoreGhostActivityProgramsRuntime> _resolve;
    private CoreGhostActivityProgramsEntries(Func<EngineOwners.CoreGhostActivityProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGhostActivityProgramsEntries Create(Func<EngineOwners.CoreGhostActivityProgramsRuntime> resolve) => new(resolve);
}
