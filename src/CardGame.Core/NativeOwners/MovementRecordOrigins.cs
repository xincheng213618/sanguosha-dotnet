#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordOrigins(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries> getCharacterZhongHuiAnniversaryGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries> getCoreGameEngineRequestedDeckBasicsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries> getCoreGameEngineDiscardedProvenanceRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries> getCoreGameEngineAdjacentDiscardStorageRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime) : IMovementRecordOrigins
{
    public void RecordLishiDiscardRedirection(CardMovementRecord movement) => getCharacterZhongHuiAnniversaryGodRuntime().RecordLishiDiscardRedirection(movement);
    public void CaptureRequestedDeckBasicPaid(Card card, CardLocation from, CardLocation to) => getCoreGameEngineRequestedDeckBasicsRuntime().CaptureRequestedDeckBasicPaid(card, from, to);
    public void CaptureDiscardedEntityOrigin(Card card, CardMovementRecord movement) => getCoreGameEngineDiscardedProvenanceRuntime().CaptureDiscardedEntityOrigin(card, movement);
    public void CaptureAdjacentDiscardOrigin(Card card, CardMovementRecord movement, CardKind? frozenIdentity) => getCoreGameEngineAdjacentDiscardStorageRuntime().CaptureAdjacentDiscardOrigin(card, movement, frozenIdentity);
    public void CollectFullDiscardPhaseSuit(Card card, CardMovementRecord movement) => getCoreProgramParticipantPhaseOperationsRuntime().CollectFullDiscardPhaseSuit(card, movement);
}
