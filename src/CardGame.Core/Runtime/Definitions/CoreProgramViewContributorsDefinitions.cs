#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramViewContributorsDefinitions
{
internal static readonly Lazy<ProgramViewContributionCatalog> ViewContributionMetadata = new(() => ProgramViewContributionCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
