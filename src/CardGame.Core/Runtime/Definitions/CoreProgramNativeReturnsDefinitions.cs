#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramNativeReturnsDefinitions
{
internal static readonly Lazy<ProgramNativeReturnCatalog> NativeReturnMetadata = new(() => new(GameEngine.RuntimeModuleMethods), true);
}
