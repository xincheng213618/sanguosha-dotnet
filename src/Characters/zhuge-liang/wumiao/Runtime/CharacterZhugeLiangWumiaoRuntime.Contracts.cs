#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhugeLiangWumiaoCoreTrustedCardZoneDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardZoneDiagnostic> CreateCardZoneDiagnostics();
}

internal interface ICharacterZhugeLiangWumiaoCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
