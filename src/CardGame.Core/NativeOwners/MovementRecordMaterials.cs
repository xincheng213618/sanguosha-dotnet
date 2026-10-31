#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordMaterials(Func<RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries> getCoreHappyWeiYanGainDestinationsRuntime, Func<RuntimeEntries.CharacterZuoCiHappyGodEntries> getCharacterZuoCiHappyGodRuntime, Func<RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime, Func<RuntimeEntries.CharacterDengAiAnniversaryGodEntries> getCharacterDengAiAnniversaryGodRuntime, Func<RuntimeEntries.CoreGameEngineWushuangShopCardsEntries> getCoreGameEngineWushuangShopCardsRuntime, Func<RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries> getCoreGameEngineShanheStartingResourcesRuntime, Func<RuntimeEntries.CharacterZhugeLiangWumiaoEntries> getCharacterZhugeLiangWumiaoRuntime) : IMovementRecordMaterials
{
    public void ValidateHappyWeiYanGainDestinationRecord(CardMovementRecord movement) => getCoreHappyWeiYanGainDestinationsRuntime().ValidateHappyWeiYanGainDestinationRecord(movement);
    public void ValidateHappyZuoCiPhantomMovement(CardMovementRecord movement) => getCharacterZuoCiHappyGodRuntime().ValidateHappyZuoCiPhantomMovement(movement);
    public void RecordDiouCarrierDestruction(CardMovementRecord movement) => getCharacterDiaoChanLeRuntime().RecordDiouCarrierDestruction(movement);
    public void RecordShenDaCarrierDestruction(CardMovementRecord movement) => getCharacterDengAiAnniversaryGodRuntime().RecordShenDaCarrierDestruction(movement);
    public void RecordWushuangShopEquipmentDestruction(CardMovementRecord movement) => getCoreGameEngineWushuangShopCardsRuntime().RecordWushuangShopEquipmentDestruction(movement);
    public void RecordShanheStartingResourceDestruction(CardMovementRecord movement) => getCoreGameEngineShanheStartingResourcesRuntime().RecordShanheStartingResourceDestruction(movement);
    public void RecordWuZglCopyMovement(CardMovementRecord movement) => getCharacterZhugeLiangWumiaoRuntime().RecordWuZglCopyMovement(movement);
}
