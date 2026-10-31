#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramDamageHooksDefinitions
{
internal static readonly Lazy<ProgramDamageHookCatalog> DamageHookMetadata = new(() => ProgramDamageHookCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods, global::CardGame.Core.EngineOwners.CoreProgramDamageHooksRuntime.DamageHookDelegateType), true);
}
