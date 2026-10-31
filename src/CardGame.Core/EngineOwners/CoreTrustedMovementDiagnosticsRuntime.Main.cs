#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTrustedMovementDiagnosticsRuntime
{
    internal IReadOnlyList<CardMovementRecord> CardMovements => _coreCardMovementJournalCardMovementsAccess._cardMovements.AsReadOnly();
}
