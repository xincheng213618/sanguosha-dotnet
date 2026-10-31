#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordOrigins(Func<RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime, Func<RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries> getCoreGameEngineRequestedDeckBasicsRuntime, Func<RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries> getCoreGameEngineDiscardedProvenanceRuntime, Func<RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries> getCoreGameEngineAdjacentDiscardStorageRuntime, Func<RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime) : IMovementRecordOrigins
{
    public void RecordLishiDiscardRedirection(CardMovementRecord movement) => getCharacterZhongHuiAnniversaryGodRuntime().RecordLishiDiscardRedirection(movement);
    public void CaptureRequestedDeckBasicPaid(Card card, CardLocation from, CardLocation to) => getCoreGameEngineRequestedDeckBasicsRuntime().CaptureRequestedDeckBasicPaid(card, from, to);
    public void CaptureDiscardedEntityOrigin(Card card, CardMovementRecord movement) => getCoreGameEngineDiscardedProvenanceRuntime().CaptureDiscardedEntityOrigin(card, movement);
    public void CaptureAdjacentDiscardOrigin(Card card, CardMovementRecord movement, CardKind? frozenIdentity) => getCoreGameEngineAdjacentDiscardStorageRuntime().CaptureAdjacentDiscardOrigin(card, movement, frozenIdentity);
    public void CollectFullDiscardPhaseSuit(Card card, CardMovementRecord movement) => getCoreProgramParticipantPhaseOperationsRuntime().CollectFullDiscardPhaseSuit(card, movement);
}
