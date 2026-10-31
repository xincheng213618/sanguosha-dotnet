#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineProgramTargetResolversDefinitions
{
internal static readonly Lazy<ProgramTargetResolverCatalog> ProgramTargetResolverMetadata = new(() => new(typeof(ProgramTargetResolverCatalog).Assembly.GetTypes()), true);
}
