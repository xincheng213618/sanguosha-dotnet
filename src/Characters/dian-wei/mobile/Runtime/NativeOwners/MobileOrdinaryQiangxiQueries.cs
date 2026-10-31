#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileOrdinaryQiangxiQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDianWeiMobileEntries> getCharacterDianWeiMobileRuntime) : IMobileOrdinaryQiangxiQueries
{
    public bool AllowsWeaponTarget(int ownerSeat, int weaponId, int targetSeat) => getCharacterDianWeiMobileRuntime().AllowsMobileOrdinaryQiangxiWeaponTarget(ownerSeat, weaponId, targetSeat);
}
