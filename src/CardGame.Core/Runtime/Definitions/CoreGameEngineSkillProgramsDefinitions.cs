#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineSkillProgramsDefinitions
{
internal static readonly Lazy<ProgramLegalityCatalog> ProgramLegalityMetadata = new(() => ProgramLegalityCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
