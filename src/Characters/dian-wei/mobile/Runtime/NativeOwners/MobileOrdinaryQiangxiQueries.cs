#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileOrdinaryQiangxiQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDianWeiMobileRuntime>> getCharacterDianWeiMobileRuntime) : IMobileOrdinaryQiangxiQueries
{
    public bool AllowsWeaponTarget(int ownerSeat, int weaponId, int targetSeat) => getCharacterDianWeiMobileRuntime().Value.AllowsMobileOrdinaryQiangxiWeaponTarget(ownerSeat, weaponId, targetSeat);
}
