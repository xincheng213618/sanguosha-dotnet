#nullable enable
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineProgramContinuationsDefinitions
{
internal static readonly Lazy<global::CardGame.Core.GameEngine.ProgramContinuationCatalog> ProgramContinuations = new(() => global::CardGame.Core.GameEngine.ProgramContinuationCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
