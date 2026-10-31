#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramWindowReturnsDefinitions
{
internal static readonly Lazy<ProgramWindowReturnCatalog> ProgramWindowReturns = new(() => ProgramWindowReturnCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
