#nullable enable
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineSkillProgramsDefinitions
{
internal static readonly Lazy<ProgramLegalityCatalog> ProgramLegalityMetadata = new(() => ProgramLegalityCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
