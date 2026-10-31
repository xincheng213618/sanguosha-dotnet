#nullable enable
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEnginePipelineHooksDefinitions
{
internal static readonly Lazy<ProgramPipelineHookCatalog> ProgramPipelineHookRegistrations = new(() => ProgramPipelineHookCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
