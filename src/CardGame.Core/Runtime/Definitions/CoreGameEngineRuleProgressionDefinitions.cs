#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineRuleProgressionDefinitions
{
internal static readonly Lazy<ProgramRuleObserverCatalog> ProgramRuleObserverRegistrations = new(() => ProgramRuleObserverCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
