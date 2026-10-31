#nullable enable
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramPhaseUsedProjectionsDefinitions
{
internal static readonly Lazy<ProgramPhaseUsedProjectionCatalog> PhaseUsedProjectionMetadata = new(() => ProgramPhaseUsedProjectionCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
