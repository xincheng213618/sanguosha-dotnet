#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AtomicCardMovementFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries> getCoreGameEngineAdjacentDiscardStorageRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries> getCharacterDiaoChanLeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries> getCoreGameEngineWushuangShopCardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries> getCoreGameEngineShanheStartingResourcesRuntime) : IAtomicCardMovementFacts
{
    public bool TracksAdjacentDiscardStorage => getCoreGameEngineAdjacentDiscardStorageRuntime().TracksAdjacentDiscardStorage;

    public bool IsCarrier(int id) => getCharacterDiaoChanLeRuntime().IsDiouCarrier(id);
    public bool IsPurchasedEquipment(int id) => getCoreGameEngineWushuangShopCardsRuntime().IsWushuangShopEquipment(id);
    public bool IsStartingResource(int id) => getCoreGameEngineShanheStartingResourcesRuntime().IsShanheStartingResource(id);
}
