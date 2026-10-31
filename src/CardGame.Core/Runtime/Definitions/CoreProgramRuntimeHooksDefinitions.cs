#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramRuntimeHooksDefinitions
{
internal static readonly Lazy<ProgramRuntimeHookCatalog> RuntimeHookMetadata = new(() => ProgramRuntimeHookCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
