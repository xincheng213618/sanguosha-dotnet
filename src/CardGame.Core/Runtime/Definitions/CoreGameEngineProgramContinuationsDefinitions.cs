#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineProgramContinuationsDefinitions
{
internal static readonly Lazy<GameEngine.ProgramContinuationCatalog> ProgramContinuations = new(() => GameEngine.ProgramContinuationCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
