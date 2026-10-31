namespace CardGame.Core;
public sealed record EquipmentSlotCapacityChangedEvent(int Seat, EquipmentSlot Slot, int Capacity) : IGameEvent;
public sealed record DeckRankCardsObtainedEvent(int Seat, string SkillId, IReadOnlyList<int> CardIds) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> _inheritedWeaponAbilities => ref _advancedLifecycle.ReferenceToInheritedWeaponAbilities;
    private ref readonly global::System.Collections.Generic.Dictionary<int, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>> _copiedWeaponAbilities => ref _advancedLifecycle.ReferenceToCopiedWeaponAbilities;
    private ref readonly global::System.Collections.Generic.HashSet<int> _weaponCopyConsumed => ref _advancedLifecycle.ReferenceToWeaponCopyConsumed;
}

internal static class RankSubsetSearch
{
    internal static bool CanComplete(IEnumerable<int> ranks, int target)
    {
        if (target < 0)
            return false;
        var possible = new bool[target + 1];
        possible[0] = true;
        foreach (var rank in ranks.Where(rank => rank > 0 && rank <= target))
            for (var sum = target; sum >= rank; sum--)
                possible[sum] |= possible[sum - rank];
        return possible[target];
    }
}
