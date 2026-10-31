#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRevealRecastProgramsRuntime))]
internal sealed partial class CoreGameEngineRevealRecastProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> _resolve;
    private CoreGameEngineRevealRecastProgramsEntries(Func<EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRevealRecastProgramsEntries Create(Func<EngineOwners.CoreGameEngineRevealRecastProgramsRuntime> resolve) => new(resolve);
}
