#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramTargetWindowProtocolsDefinitions
{
internal static readonly Lazy<ProgramTargetWindowCatalog> TargetWindowMetadata = new(() => ProgramTargetWindowCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
