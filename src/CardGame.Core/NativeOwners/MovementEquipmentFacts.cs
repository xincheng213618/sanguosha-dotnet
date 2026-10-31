#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementEquipmentFacts(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreNativeDamageHandlesRuntime>> getCoreNativeDamageHandlesRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime, Func<CardZoneStore> getCardZoneStore) : IMovementEquipmentFacts
{
    public bool UsesStorageEquipment => getCoreNativeDamageHandlesRuntime().Value.UsesFormalWoodenOx;
    public bool UsesRecoveryArmor => getCoreNativeDamageHandlesRuntime().Value.UsesFormalSilverLion;

    public CardMovementRecord? LastMovement(int cardId, CardLocation from, CardLocation to) => cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(move => move.CardId == cardId && move.From == from && move.To == to);
    public bool WasBoneAt(int cardId, CardLocation location, int sequence) => getCharacterWeiYanHappyGodRuntime().HappyBoneAt(cardId, location, sequence)is not null;
    public int Count(CardLocation location) => getCardZoneStore().Count(location);
}
