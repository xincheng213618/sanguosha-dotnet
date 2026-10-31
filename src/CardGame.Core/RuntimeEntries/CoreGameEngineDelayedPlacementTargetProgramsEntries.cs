#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> _resolve;
    private CoreGameEngineDelayedPlacementTargetProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDelayedPlacementTargetProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDelayedPlacementTargetProgramsRuntime> resolve) => new(resolve);
}
