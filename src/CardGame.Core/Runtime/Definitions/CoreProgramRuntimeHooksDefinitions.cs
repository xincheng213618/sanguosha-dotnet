#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramRuntimeHooksDefinitions
{
internal static readonly Lazy<ProgramRuntimeHookCatalog> RuntimeHookMetadata = new(() => ProgramRuntimeHookCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
