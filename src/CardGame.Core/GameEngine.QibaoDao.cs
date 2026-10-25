namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Shared by the issued weapon hook and proposed/public damage estimates.
    // A chained recipient inherits the origin's finalized amount, without a
    // second full-health bonus of its own.
    private static int QiBaoDaoDamageBonus(bool ownerUsedSlash, bool sourceLess,
        bool chainPropagation, bool hasWeaponAbility, bool targetHasHp, int targetHp, int targetMaxHp) =>
        ownerUsedSlash && !sourceLess && !chainPropagation && hasWeaponAbility &&
        targetHasHp && targetHp == targetMaxHp ? 1 : 0;
}
