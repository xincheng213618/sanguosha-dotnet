#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramDamageHooksDefinitions
{
internal static readonly Lazy<ProgramDamageHookCatalog> DamageHookMetadata = new(() => ProgramDamageHookCatalog.Discover(GameEngine.RuntimeModuleMethods, CoreProgramDamageHooksRuntime.DamageHookDelegateType), true);
}
