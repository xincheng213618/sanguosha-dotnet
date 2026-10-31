#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEnginePipelineHooksDefinitions
{
internal static readonly Lazy<ProgramPipelineHookCatalog> ProgramPipelineHookRegistrations = new(() => ProgramPipelineHookCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
