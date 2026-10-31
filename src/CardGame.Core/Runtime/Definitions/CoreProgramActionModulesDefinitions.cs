#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramActionModulesDefinitions
{
internal static readonly Lazy<ProgramActionCatalog> ProgramActionMetadata = new(() => new(GameEngine.RuntimeModuleMethods), true);
}
