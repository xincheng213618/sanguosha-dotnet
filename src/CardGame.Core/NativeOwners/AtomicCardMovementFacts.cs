#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AtomicCardMovementFacts(Func<RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries> getCoreGameEngineAdjacentDiscardStorageRuntime, Func<RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime, Func<RuntimeEntries.CoreGameEngineWushuangShopCardsEntries> getCoreGameEngineWushuangShopCardsRuntime, Func<RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries> getCoreGameEngineShanheStartingResourcesRuntime) : IAtomicCardMovementFacts
{
    public bool TracksAdjacentDiscardStorage => getCoreGameEngineAdjacentDiscardStorageRuntime().TracksAdjacentDiscardStorage;

    public bool IsCarrier(int id) => getCharacterDiaoChanLeRuntime().IsDiouCarrier(id);
    public bool IsPurchasedEquipment(int id) => getCoreGameEngineWushuangShopCardsRuntime().IsWushuangShopEquipment(id);
    public bool IsStartingResource(int id) => getCoreGameEngineShanheStartingResourcesRuntime().IsShanheStartingResource(id);
}
