#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramCardLegalityDefinitions
{
internal static readonly Lazy<ProgramCardLegalityCatalog> CardLegalityMetadata = new(() => ProgramCardLegalityCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
