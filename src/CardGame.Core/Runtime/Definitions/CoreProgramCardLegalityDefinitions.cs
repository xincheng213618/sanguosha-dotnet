#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramCardLegalityDefinitions
{
internal static readonly Lazy<ProgramCardLegalityCatalog> CardLegalityMetadata = new(() => ProgramCardLegalityCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
