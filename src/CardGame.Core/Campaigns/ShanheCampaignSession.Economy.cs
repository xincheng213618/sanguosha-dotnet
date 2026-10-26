namespace CardGame.Core.Campaigns;

public sealed partial class ShanheCampaignSession
{
    private IEnumerable<ShanheCampaignEconomy> OwnedEconomy(RunState state) =>
        state.TacticIds.Select(id => _rewards[id].CampaignEconomy).OfType<ShanheCampaignEconomy>();

    private int ShopRefreshLimit(RunState state) => Math.Max(1,
        OwnedEconomy(state).Select(e => e.ShopRefreshLimit).DefaultIfEmpty(0).Max());
    private int ShopFreeRefreshRemaining(RunState state) => Math.Max(0,
        OwnedEconomy(state).Sum(effect => effect.FreeShopRefreshes) - state.ShopFreeRefreshCount);
    private int ShopPaidRefreshRemaining(RunState state) => Math.Max(0,
        ShopRefreshLimit(state) - state.ShopPaidRefreshCount);
    private int ExtraShopStock(RunState state) => OwnedEconomy(state).Sum(e => e.ExtraStockPerCategory);

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
        var income = checked(battleGold + (int)((long)battleGold * incomeBonus / 100));
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
