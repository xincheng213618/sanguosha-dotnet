using System.Windows.Input;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    public IReadOnlyList<string> ShanheMarketCategories { get; } = new[] { "战法", "装备", "卡牌", "技能" };
    private string _selectedShanheMarketCategory = "战法";
    public string SelectedShanheMarketCategory
    {
        get => _selectedShanheMarketCategory;
        set
        {
            if (!ShanheMarketCategories.Contains(value) || !SetProperty(ref _selectedShanheMarketCategory, value)) return;
            RaisePropertyChanged(nameof(ShanheMarketGoods)); RaisePropertyChanged(nameof(ShanheMarketCategoryNotice));
            RaisePropertyChanged(nameof(ShanheMarketHasGoods));
            RaisePropertyChanged(nameof(ShanheMarketSurchargeNotice));
        }
    }
    public IEnumerable<ShanheShopItem> ShanheMarketGoods => ShanheShopItems.Where(item => item.MarketCategory == SelectedShanheMarketCategory);
    public bool ShanheMarketHasGoods => ShanheMarketGoods.Any();
    public string ShanheMarketCategoryNotice => SelectedShanheMarketCategory switch
    {
        "技能" => CurrentShanheMarketState is { } state
            ? $"额外技能 {state.ExtraSkillIds.Count}/{state.ExtraSkillSlots} · 满槽可替换" : "额外技能 · 满槽可替换",
        "装备" => IsShanheBattleActive ? "购买即加入手牌" : ShanheStartingResourcesExpireOnUse
            ? "每战开局获得装备牌" : "每战开局装备",
        "卡牌" => IsShanheBattleActive ? "购买即加入手牌" : "每战开局获得随身牌",
        _ => "战法持续本次征程，不占技能槽"
    };
    public string ShanheMarketSurchargeNotice
    {
        get
        {
            var noIncrease = CurrentShanheMarketState?.TacticIds.Any(id => _shanheSession!.Definition.Rewards
                .Any(reward => reward.Id == id && reward.CampaignEconomy?.SuppressShopPriceEscalation == true)) == true;
            var priceNotice = IsShanheShopActive && _shanheSession!.CreateCheckpoint().Options.UseCodexRewardPool &&
                SelectedShanheMarketCategory is "战法" or "装备" or "卡牌" or "技能"
                ? noIncrease ? "同类商品不再加价" : $"每购买一次{SelectedShanheMarketCategory}，同类价格 +100 铜币" : "";
            return CurrentShanheMarketState?.ShopRestockOnPurchase == true
                ? priceNotice + " · 购买后补货"
                : priceNotice;
        }
    }
    public string ShanheMarketRefreshText => CurrentShanheMarketState?.ShopFreeRefreshRemaining > 0 ? "免费刷新" :
        CurrentShanheMarketState?.ShopRefreshCost is int cost && cost > 0 ? $"刷新 · {cost} 铜币" : "刷新";
    public string ShanheMarketFreeRefreshRemaining => $"免费刷新：{CurrentShanheMarketState?.ShopFreeRefreshRemaining ?? 0} 次";
    public string ShanheMarketPaidRefreshRemaining => CurrentShanheMarketState?.ShopPaidRefreshUnlimited == true
        ? "付费刷新：无限" : $"付费刷新：{CurrentShanheMarketState?.ShopPaidRefreshRemaining ?? 0} 次";
    public string ShanheMarketRefreshRemaining => IsShanheShopActive
        ? $"{ShanheMarketFreeRefreshRemaining} · {ShanheMarketPaidRefreshRemaining}" : "开始征程后可打开集市";
    private RelayCommand? _refreshShanheMarketCommand;
    public ICommand RefreshShanheMarketCommand => _refreshShanheMarketCommand ??= new RelayCommand(() =>
    {
        if (_shanheSession is not { } session || !RefreshShanheMarketCommand.CanExecute(null)) return;
        if (IsShanheBattleActive) ApplyShanheBattleMarketCommand(new CardGame.Core.RefreshShanheMarketCommand(0, ShanheMarketActionRevision));
        else ApplyShanheCommand(new RefreshShanheShopCommand(session.Revision));
    }, () => IsShanheShopActive && _shanheBattleGame?.HasPendingShanheMarketPurchase != true &&
        (CurrentShanheMarketState!.ShopPaidRefreshUnlimited || CurrentShanheMarketState.ShopRefreshRemaining > 0) &&
        CurrentShanheMarketState.Gold >= CurrentShanheMarketState.ShopRefreshCost && !IsTutorialActive);
    private void RefreshShanheMarketPresentation()
    {
        foreach (var property in new[] { nameof(ShanheMarketGoods), nameof(ShanheMarketHasGoods), nameof(ShanheMarketCategoryNotice),
            nameof(ShanheMarketSurchargeNotice), nameof(ShanheMarketRefreshText), nameof(ShanheMarketRefreshRemaining),
            nameof(ShanheMarketFreeRefreshRemaining), nameof(ShanheMarketPaidRefreshRemaining) }) RaisePropertyChanged(property);
        _refreshShanheMarketCommand?.NotifyCanExecuteChanged();
        RefreshShanheLiveMarketPresentation();
    }
}
