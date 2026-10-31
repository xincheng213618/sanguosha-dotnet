#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileOrdinaryQiangxiQueries(Func<RuntimeEntries.CharacterDianWeiMobileEntries> getCharacterDianWeiMobileRuntime) : IMobileOrdinaryQiangxiQueries
{
    public bool AllowsWeaponTarget(int ownerSeat, int weaponId, int targetSeat) => getCharacterDianWeiMobileRuntime().AllowsMobileOrdinaryQiangxiWeaponTarget(ownerSeat, weaponId, targetSeat);
}
