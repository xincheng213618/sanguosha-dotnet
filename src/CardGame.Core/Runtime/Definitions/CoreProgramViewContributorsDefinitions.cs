#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramViewContributorsDefinitions
{
internal static readonly Lazy<ProgramViewContributionCatalog> ViewContributionMetadata = new(() => ProgramViewContributionCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
