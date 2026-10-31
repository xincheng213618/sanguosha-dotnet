#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramTargetWindowProtocolsDefinitions
{
internal static readonly Lazy<ProgramTargetWindowCatalog> TargetWindowMetadata = new(() => ProgramTargetWindowCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
