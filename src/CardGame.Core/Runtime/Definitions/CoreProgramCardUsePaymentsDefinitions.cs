#nullable enable
using global::CardGame.Core;
using System.Collections.ObjectModel;

using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramCardUsePaymentsDefinitions
{
internal static readonly Lazy<ProgramCardUsePaymentCatalog> CardUsePaymentMetadata = new(() => ProgramCardUsePaymentCatalog.Discover(global::CardGame.Core.GameEngine.RuntimeModuleMethods), true);
}
