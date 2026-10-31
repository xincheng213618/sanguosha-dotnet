#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineQibaoDaoRuntime
{
    internal static int QiBaoDaoDamageBonus(bool ownerUsedSlash, bool sourceLess, bool chainPropagation, bool hasWeaponAbility, bool targetHasHp, int targetHp, int targetMaxHp) => ownerUsedSlash && !sourceLess && !chainPropagation && hasWeaponAbility && targetHasHp && targetHp == targetMaxHp ? 1 : 0;
}
