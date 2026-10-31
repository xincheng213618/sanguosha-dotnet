#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramSelectionModulesDefinitions
{
internal static readonly Lazy<ProgramSelectionCatalog> ProgramSelectionMetadata = new(() => new(typeof(GameEngine).Assembly.GetTypes()), true);
}
