#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramDistanceRulesDefinitions
{
internal static readonly Lazy<ProgramDistanceCatalog> DistanceRuleMetadata = new(() => ProgramDistanceCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
