#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterZhangLiaoMobileGodCoreTrustedMovementDiagnosticsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<CardMovementRecord> CardMovements { get; }
}
