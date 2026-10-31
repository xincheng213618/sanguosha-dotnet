#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramTargetRulesDefinitions
{
internal static readonly Lazy<ProgramTargetRuleCatalog> TargetRuleMetadata = new(() => ProgramTargetRuleCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
