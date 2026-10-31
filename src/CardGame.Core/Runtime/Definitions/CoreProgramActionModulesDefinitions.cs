#nullable enable
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramActionModulesDefinitions
{
internal static readonly Lazy<ProgramActionCatalog> ProgramActionMetadata = new(() => new(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
