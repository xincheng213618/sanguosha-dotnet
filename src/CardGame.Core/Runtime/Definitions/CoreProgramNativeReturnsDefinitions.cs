#nullable enable
using global::CardGame.Core;
using System.Reflection;

using AttackReceipt = CardGame.Core.GameEngine.AttackCompletionReceipt;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramNativeReturnsDefinitions
{
internal static readonly Lazy<ProgramNativeReturnCatalog> NativeReturnMetadata = new(() => new(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
