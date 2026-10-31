#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime))]
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> _resolve;
    private CoreGameEngineDelayedPlacementTargetProgramsEntries(Func<EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDelayedPlacementTargetProgramsEntries Create(Func<EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> resolve) => new(resolve);
}
