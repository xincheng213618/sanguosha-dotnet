#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreProgramCardUsePaymentsDefinitions
{
internal static readonly Lazy<ProgramCardUsePaymentCatalog> CardUsePaymentMetadata = new(() => ProgramCardUsePaymentCatalog.Discover(GameEngine.RuntimeModuleMethods), true);
}
