#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhuRongMouMobileCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}
