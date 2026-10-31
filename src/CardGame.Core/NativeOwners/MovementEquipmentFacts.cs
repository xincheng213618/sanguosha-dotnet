#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementEquipmentFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore) : IMovementEquipmentFacts
{
    public bool UsesStorageEquipment => getCoreNativeDamageHandlesRuntime().UsesFormalWoodenOx;
    public bool UsesRecoveryArmor => getCoreNativeDamageHandlesRuntime().UsesFormalSilverLion;

    public CardMovementRecord? LastMovement(int cardId, CardLocation from, CardLocation to) => cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(move => move.CardId == cardId && move.From == from && move.To == to);
    public bool WasBoneAt(int cardId, CardLocation location, int sequence) => getCharacterWeiYanHappyGodRuntime().HappyBoneAt(cardId, location, sequence)is not null;
    public int Count(CardLocation location) => getCardZoneStore().Count(location);
}
