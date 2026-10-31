#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramDistanceRulesDefinitions
{
internal static readonly Lazy<ProgramDistanceCatalog> DistanceRuleMetadata = new(() => ProgramDistanceCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
