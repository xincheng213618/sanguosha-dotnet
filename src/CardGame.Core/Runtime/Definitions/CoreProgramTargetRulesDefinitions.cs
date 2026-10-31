#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramTargetRulesDefinitions
{
internal static readonly Lazy<ProgramTargetRuleCatalog> TargetRuleMetadata = new(() => ProgramTargetRuleCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
