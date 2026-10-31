#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

using System.Runtime.CompilerServices;

using System.Runtime.ExceptionServices;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramSelectionModulesDefinitions
{
internal static readonly Lazy<ProgramSelectionCatalog> ProgramSelectionMetadata = new(() => new(typeof(GameEngine).Assembly.GetTypes()), true);
}
