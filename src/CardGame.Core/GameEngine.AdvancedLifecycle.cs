namespace CardGame.Core;
public sealed record EquipmentSlotCapacityChangedEvent(int Seat, EquipmentSlot Slot, int Capacity) : IGameEvent;
public sealed record DeckRankCardsObtainedEvent(int Seat, string SkillId, IReadOnlyList<int> CardIds) : IGameEvent;

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
