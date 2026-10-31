#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreShanheMarketApplicationCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}
