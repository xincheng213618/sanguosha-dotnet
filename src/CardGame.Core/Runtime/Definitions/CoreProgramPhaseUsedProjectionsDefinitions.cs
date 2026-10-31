#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramPhaseUsedProjectionsDefinitions
{
internal static readonly Lazy<ProgramPhaseUsedProjectionCatalog> PhaseUsedProjectionMetadata = new(() => ProgramPhaseUsedProjectionCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
