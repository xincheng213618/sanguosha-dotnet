#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordMaterials(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries> getCoreHappyWeiYanGainDestinationsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries> getCharacterZuoCiHappyGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries> getCharacterDengAiAnniversaryGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries> getCoreGameEngineWushuangShopCardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries> getCoreGameEngineShanheStartingResourcesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries> getCharacterZhugeLiangWumiaoRuntime) : IMovementRecordMaterials
{
    public void ValidateHappyWeiYanGainDestinationRecord(CardMovementRecord movement) => getCoreHappyWeiYanGainDestinationsRuntime().ValidateHappyWeiYanGainDestinationRecord(movement);
    public void ValidateHappyZuoCiPhantomMovement(CardMovementRecord movement) => getCharacterZuoCiHappyGodRuntime().ValidateHappyZuoCiPhantomMovement(movement);
    public void RecordDiouCarrierDestruction(CardMovementRecord movement) => getCharacterDiaoChanLeRuntime().RecordDiouCarrierDestruction(movement);
    public void RecordShenDaCarrierDestruction(CardMovementRecord movement) => getCharacterDengAiAnniversaryGodRuntime().RecordShenDaCarrierDestruction(movement);
    public void RecordWushuangShopEquipmentDestruction(CardMovementRecord movement) => getCoreGameEngineWushuangShopCardsRuntime().RecordWushuangShopEquipmentDestruction(movement);
    public void RecordShanheStartingResourceDestruction(CardMovementRecord movement) => getCoreGameEngineShanheStartingResourcesRuntime().RecordShanheStartingResourceDestruction(movement);
    public void RecordWuZglCopyMovement(CardMovementRecord movement) => getCharacterZhugeLiangWumiaoRuntime().RecordWuZglCopyMovement(movement);
}
