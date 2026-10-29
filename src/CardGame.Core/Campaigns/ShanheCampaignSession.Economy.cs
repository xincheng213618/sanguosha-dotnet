namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private IEnumerable<ShanheCampaignEconomy> OwnedEconomy(RunState state) =>
        state.TacticIds.Select(id => _rewards[id].CampaignEconomy).OfType<ShanheCampaignEconomy>();

    public int ChapterExtraBattleCount => ExtraChapterBattleCount(_state);
    private int ExtraChapterBattleCount(RunState state) =>
        OwnedEconomy(state).Select(effect => effect.ExtraChapterBattles).DefaultIfEmpty(0).Max();

    private bool IsExtraChapterBattleUnlocked(RunState state, ShanheNodeDefinition node) =>
        node.ExtraChapterBattleIndex <= ExtraChapterBattleCount(state);

    private IReadOnlyList<string> AddCampaignTactic(RunState state, ShanheRewardDefinition reward)
    {
        if (reward.CampaignEconomy?.ExtraChapterBattles is not > 0) return ShanheCampaignContent.Freeze(state.TacticIds.Append(reward.Id));
        if (reward.CampaignEconomy.ExtraChapterBattles <= ExtraChapterBattleCount(state)) return state.TacticIds;
        return ShanheCampaignContent.Freeze(state.TacticIds.Where(id => _rewards[id].CampaignEconomy?.ExtraChapterBattles is not > 0).Append(reward.Id));
    }

    private RunState RefreshExtraBattleOptions(RunState state)
    {
        if (state.Phase != ShanheCampaignPhase.Map || state.CurrentNodeId is not { } currentId) return state;
        if (ActiveRouteId(state) is { } route)
            return state with { AvailableNodeIds = RouteAvailableNodes(state, route) };
        var extra = _nodes[currentId].NextNodeIds.Where(id => _nodes[id].ExtraChapterBattleIndex > 0 &&
            IsExtraChapterBattleUnlocked(state, _nodes[id]) && !state.CompletedNodeIds.Contains(id, StringComparer.Ordinal));
        return state with { AvailableNodeIds = ShanheCampaignContent.Freeze(state.AvailableNodeIds.Concat(extra).Distinct(StringComparer.Ordinal)) };
    }

    private int ShopRefreshLimit(RunState state) => Math.Max(1,
        OwnedEconomy(state).Select(e => e.ShopRefreshLimit).DefaultIfEmpty(0).Max());
    private int ShopFreeRefreshRemaining(RunState state) => Math.Max(0,
        OwnedEconomy(state).Sum(effect => effect.FreeShopRefreshes) - state.ShopFreeRefreshCount);
    private bool HasUnlimitedPaidShopRefreshes(RunState state) =>
        IsSchoolComplete(state, 1);
    private int ShopPaidRefreshRemaining(RunState state) => HasUnlimitedPaidShopRefreshes(state) ? 0 : Math.Max(0,
        ShopRefreshLimit(state) - state.ShopPaidRefreshCount);
    private int ExtraShopStock(RunState state) => OwnedEconomy(state).Sum(e => e.ExtraStockPerCategory);
    private bool RestocksShopOnPurchase(RunState state) => OwnedEconomy(state).Any(effect => effect.RestockOnPurchase);

    private int EconomyPurchasePrice(RunState state, int amount, int categoryPurchases)
    {
        var price = OwnedEconomy(state).Any(e => e.SuppressShopPriceEscalation)
            ? amount : checked(amount + categoryPurchases * 100);
        var discount = Math.Min(100, OwnedEconomy(state).Sum(e => e.ShopDiscountPercent));
        return checked((int)((long)price * (100 - discount) / 100));
    }

    // Whole-coin rounding and the order are explicit local execution choices:
    // battle income first, then one interest calculation on the resulting balance.
    private int EconomyBattleBalance(RunState state, int battleGold)
    {
        var incomeBonus = OwnedEconomy(state).Sum(e => e.BattleGoldPercent);
        var income = Math.Max(0, checked(battleGold + (int)((long)battleGold * incomeBonus / 100)));
        income = RandomizeBattleIncome(state, income);
        var principal = checked(state.Gold + income);
        var interest = OwnedEconomy(state).Sum(e => e.PostBattleInterestPercent);
        return checked(principal + (int)((long)principal * interest / 100));
    }

    private int EconomyEquipmentRefund(RunState state, IReadOnlyList<ShanheStartingCard>? additions)
    {
        if (additions is null || additions.Count == 0) return 0;
        var slots = state.StartingEquipment.Select(card => EquipmentCatalog.Get(card.Kind).Slot).ToHashSet();
        var replacements = 0;
        foreach (var card in additions)
            if (!slots.Add(EquipmentCatalog.Get(card.Kind).Slot)) replacements++;
        return checked(replacements * OwnedEconomy(state).Sum(e => e.ReplacedEquipmentGold));
    }
}
