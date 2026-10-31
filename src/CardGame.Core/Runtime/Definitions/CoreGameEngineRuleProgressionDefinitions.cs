#nullable enable
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineRuleProgressionDefinitions
{
internal static readonly Lazy<ProgramRuleObserverCatalog> ProgramRuleObserverRegistrations = new(() => ProgramRuleObserverCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
