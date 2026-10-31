#nullable enable
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramWindowReturnsDefinitions
{
internal static readonly Lazy<ProgramWindowReturnCatalog> ProgramWindowReturns = new(() => ProgramWindowReturnCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
