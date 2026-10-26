using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;

internal static partial class ShanheCodexUiChecks
{
    private static readonly Lazy<ContentRegistry> FreeRefreshRegistry = new(() => ContentRegistry.Build(new StandardContentPackage()));

    public static void FreeRefreshClicksKeepPaidAllowancePricesAndZeroWalletState(string output)
    {
        using var vm = Create(new MemoryShanheCodexStore(), new MemoryShanheCampaignSaveStore());
        var panel = new ShanheShopPanel { DataContext = vm };
        try
        {
            var session = CreateRefreshShopSession([2291, 2292, 2107], 100);
            Attach(vm, session); Layout(panel);
            var refresh = (Button)panel.FindName("MarketRefreshButton");
            var free = (TextBlock)panel.FindName("MarketFreeRefreshRemaining");
            var paid = (TextBlock)panel.FindName("MarketPaidRefreshRemaining");
            Program.Assert(session.Snapshot is { ShopFreeRefreshRemaining: 6, ShopPaidRefreshRemaining: 5, ShopRefreshRemaining: 11, ShopRefreshCost: 0 } &&
                refresh.Content.ToString() == "免费刷新" && free.Text == "免费刷新：6 次" && paid.Text == "付费刷新：5 次" &&
                refresh.IsEnabled && ReferenceEquals(refresh.Command, vm.RefreshShanheMarketCommand),
                "The real merchant labels both tactics' six free refreshes separately from membership's five paid refreshes and binds an enabled free button.");

            var product = vm.ShanheMarketGoods.First(item => item.CanBuy);
            var buy = Program.Find<Button>(panel).Single(button => ReferenceEquals(button.CommandParameter, product) &&
                ReferenceEquals(button.Command, vm.RequestShanheShopPurchaseCommand));
            var purchaseRevision = session.Revision;
            ClickRefreshControl(buy, panel);
            Program.Assert(session.Revision == purchaseRevision + 1 && session.Snapshot.Gold == 0 &&
                vm.ShanheMarketGoods.All(item => item.Price == 200) && refresh.IsEnabled,
                "An actual 100-coin purchase empties the wallet, increments this category's prices, and leaves the zero-cost refresh button available.");
            var retiredProduct = vm.ShanheMarketGoods.First();
            for (var used = 1; used <= 6; used++)
            {
                var revision = session.Revision;
                ClickRefreshControl(refresh, panel);
                Program.Assert(session.Revision == revision + 1 && session.Snapshot.Gold == 0 &&
                    session.Snapshot.ShopFreeRefreshRemaining == 6 - used && session.Snapshot.ShopPaidRefreshRemaining == 5 &&
                    session.Snapshot.ShopRefreshRemaining == 11 - used &&
                    session.Snapshot.ShopItems.All(item => item.ChoiceId.StartsWith("$shop:" + used + ":", StringComparison.Ordinal)) &&
                    vm.ShanheMarketGoods.Any() && vm.ShanheMarketGoods.All(item => item.Price == 200) &&
                    free.Text == $"免费刷新：{6 - used} 次" && paid.Text == "付费刷新：5 次" &&
                    !vm.RequestShanheShopPurchaseCommand.CanExecute(retiredProduct),
                    "Each bound free click commits one real refresh, advances its stock identity, preserves the independent paid quota and keeps the category purchase surcharge.");
                if (used == 1)
                    Program.Render(panel, 1400, 714, Path.Combine(output, "shanhe-market-free-refresh-zero-wallet.png"));
            }
            Program.Assert(session.AcceptedCommands.OfType<RefreshShanheShopCommand>().Count() == 6 &&
                session.Snapshot.ShopRefreshCost == 100 && refresh.Content.ToString() == "刷新 · 100 铜币" &&
                !refresh.IsEnabled && !vm.RefreshShanheMarketCommand.CanExecute(null),
                "Once the free quota is exhausted, the real button switches to the 100-coin paid price and disables for an empty wallet despite five paid refreshes remaining.");
            Program.Render(panel, 1400, 714, Path.Combine(output, "shanhe-market-free-refresh-paid-blocked.png"));
            var before = JsonSerializer.Serialize(session.Snapshot);
            RawRefreshClick(refresh);
            vm.RefreshShanheMarketCommand.Execute(null);
            Layout(panel);
            Program.Assert(JsonSerializer.Serialize(session.Snapshot) == before,
                "A disabled paid click cannot consume a quota, change stock or debit the zero wallet.");

            session = CreateRefreshShopSession([], 100);
            Attach(vm, session); Layout(panel);
            Program.Assert(session.Snapshot is { ShopFreeRefreshRemaining: 0, ShopPaidRefreshRemaining: 1, ShopRefreshRemaining: 1, ShopRefreshCost: 100 } &&
                free.Text == "免费刷新：0 次" && paid.Text == "付费刷新：1 次" &&
                refresh.Content.ToString() == "刷新 · 100 铜币" && refresh.IsEnabled,
                "A run with no refresh perks accurately displays the ordinary zero free and one paid allowance.");
            var paidRevision = session.Revision;
            ClickRefreshControl(refresh, panel);
            Program.Assert(session.Revision == paidRevision + 1 && session.Snapshot.Gold == 0 &&
                session.Snapshot is { ShopFreeRefreshRemaining: 0, ShopPaidRefreshRemaining: 0, ShopRefreshRemaining: 0 } &&
                paid.Text == "付费刷新：0 次" && !refresh.IsEnabled &&
                vm.ShanheMarketGoods.All(item => item.Price == 100) &&
                session.AcceptedCommands.OfType<RefreshShanheShopCommand>().Count() == 1,
                "The ordinary paid refresh button spends exactly 100 once, publishes fresh stock, and disables when its single allowance is consumed.");
        }
        finally { panel.DataContext = null; }
    }

    private static ShanheCampaignSession CreateRefreshShopSession(IReadOnlyList<int> sources, int gold)
    {
        var goods = Enumerable.Range(1, 12).Select(index => new ShanheRewardDefinition("refresh-training-" + index,
            "行军操练" + index, Bonuses: new(MaxHp: 1)) { Description = "体力上限增加1点。" }).ToArray();
        var perks = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => sources.Contains(source.SourceId)).ToArray();
        Program.Assert(perks.Length == sources.Count, "Every equipped refresh perk must use its actual current source definition.");
        var definition = new ShanheCampaignDefinition("fixture:shanhe-free-refresh-ui", "商店刷新", ["standard:guan-yu"], ["shop"],
        [
            new("shop", "行军集市", ShanheNodeKind.Shop, ["battle"], Choices: goods.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name, GoldCost: 100, RewardId: reward.Id, LeavesNode: false)).ToArray()),
            new("battle", "下一关", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new("standard:zhang-fei")])])
        ], goods.Concat(perks.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description })).ToArray(), StartingGold: gold);
        var session = ShanheCampaignSession.Create(definition, new(83, "standard:guan-yu")
            { UseCodexRewardPool = true, EquippedTacticIds = perks.Select(source => source.RewardId).ToArray() }, _ => FreeRefreshRegistry.Value);
        var entered = session.Submit(new MoveShanheNodeCommand("shop", session.Revision));
        Program.Assert(entered.Accepted, entered.Error ?? "The refresh UI fixture must enter the actual merchant through a real command.");
        return session;
    }

    private static void ClickRefreshControl(Button button, FrameworkElement root)
    {
        Program.Assert(button.IsEnabled && button.Command?.CanExecute(button.CommandParameter) == true,
            "The actual merchant control must be enabled and correctly bound before its click.");
        RawRefreshClick(button); Layout(root);
    }
    private static void RawRefreshClick(Button button) =>
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
}
