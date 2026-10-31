#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementEquipmentFacts(Func<RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime, Func<CardZoneStore> getCardZoneStore) : IMovementEquipmentFacts
{
    public bool UsesStorageEquipment => getCoreNativeDamageHandlesRuntime().UsesFormalWoodenOx;
    public bool UsesRecoveryArmor => getCoreNativeDamageHandlesRuntime().UsesFormalSilverLion;

    public CardMovementRecord? LastMovement(int cardId, CardLocation from, CardLocation to) => cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(move => move.CardId == cardId && move.From == from && move.To == to);
    public bool WasBoneAt(int cardId, CardLocation location, int sequence) => getCharacterWeiYanHappyGodRuntime().HappyBoneAt(cardId, location, sequence)is not null;
    public int Count(CardLocation location) => getCardZoneStore().Count(location);
}
