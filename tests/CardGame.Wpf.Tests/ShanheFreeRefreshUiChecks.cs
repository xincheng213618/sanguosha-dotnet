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
using CardGame.Wpf.ViewModels;

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
                {
                    AssertMarketResourceHints(panel, vm, "铜币不足", "还差 200 铜币");
                    Program.Render(panel, 1400, 714, Path.Combine(output, "shanhe-market-free-refresh-zero-wallet.png"));
                }
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

            // A local variant exercises the already-supported health price. Published
            // source stock has no positive-health shop price and stays unchanged.
            foreach (var (gold, hpCost, expectedStatus) in new[]
                { (100, 4, "体力不足"), (0, 4, "铜币与体力不足"), (100, 1, "购买") })
            {
                session = CreateRefreshShopSession([], gold, hpCost);
                Attach(vm, session); Layout(panel);
                Program.Assert(session.Snapshot.Hp == 4 && session.Snapshot.MaxHp == 4 &&
                    ((TextBlock)panel.FindName("MarketHealth")).Text == vm.ShanheMarketHealthText &&
                    vm.ShanheMarketHealthText == "体力  4/4",
                    "The existing merchant fixture exposes the actual current HP and maximum alongside the wallet.");
                var expectedHint = (gold == 0 ? "还差 100 铜币\n" : "") + (hpCost == 4
                    ? "体力至少 5 点（当前 4），支付后留1点" : "支付后需留1点体力");
                AssertMarketResourceHints(panel, vm, expectedStatus, expectedHint);
                var quoted = vm.ShanheMarketGoods.First();
                Program.Assert(quoted.CanBuy == (hpCost == 1) &&
                    vm.RequestShanheShopPurchaseCommand.CanExecute(quoted) == quoted.CanBuy &&
                    session.Snapshot.ShopItems.All(item => item.State == (hpCost == 1
                        ? ShanheShopItemState.Available : ShanheShopItemState.InsufficientResources)),
                    "Precise resource wording keeps authoritative command availability: HP equal to cost is insufficient and one remaining HP is enough.");
                if (!quoted.CanBuy)
                {
                    var quotedBefore = JsonSerializer.Serialize(session.Snapshot);
                    vm.RequestShanheShopPurchaseCommand.Execute(quoted); Layout(panel);
                    Program.Assert(JsonSerializer.Serialize(session.Snapshot) == quotedBefore,
                        "Reading or invoking an unaffordable quote cannot charge either resource or advance the merchant.");
                }
                if (gold == 0)
                    Program.Render(panel, 1120, 700, Path.Combine(output, "shanhe-market-resource-shortfall.png"));
            }
        }
        finally { panel.DataContext = null; }
    }

    private static void AssertMarketResourceHints(ShanheShopPanel panel, MainViewModel vm, string status, string hint)
    {
        var products = vm.ShanheMarketGoods.Where(item => item.State != ShanheShopItemState.Purchased).ToArray();
        Program.Assert(products.Length > 0 && products.All(item => item.Status == status && item.ResourceHint == hint && item.HasResourceHint),
            "Real shop snapshots distinguish missing coins from the surviving-health requirement with an exact, separate resource hint.");
        foreach (var product in products)
        {
            var text = Program.Find<TextBlock>(panel).Single(block => block.Name == "MarketResourceHint" && ReferenceEquals(block.DataContext, product));
            var cell = (Grid)text.Parent;
            var button = Program.Find<Button>(cell).Single(control => ReferenceEquals(control.CommandParameter, product));
            var bounds = text.TransformToAncestor(cell).TransformBounds(new Rect(text.RenderSize));
            var buyBounds = button.TransformToAncestor(cell).TransformBounds(new Rect(button.RenderSize));
            Program.Assert(text.Visibility == Visibility.Visible && text.Text == hint && text.FontSize >= 12 && text.TextWrapping == TextWrapping.Wrap &&
                cell.RowDefinitions.Count == 3 && Math.Abs(cell.RowDefinitions[2].ActualHeight - 36) < 1 &&
                bounds.Top >= buyBounds.Bottom && bounds.Bottom <= cell.ActualHeight + 1 && text.ActualHeight <= 32.5,
                "The separate two-line resource hint remains readable inside its reserved row and does not cover the real buy button: " +
                JsonSerializer.Serialize(new { product.Id, text.Text, text.IsVisible, text.Visibility, text.FontSize,
                    text.ActualHeight, CellHeight = cell.ActualHeight, Bounds = bounds.ToString(), Buy = buyBounds.ToString(),
                    Rows = cell.RowDefinitions.Select(row => row.ActualHeight).ToArray() }));
            ShanheResponsiveMarketUiChecks.AssertTextGlyphsWithin(text, text, "The merchant resource hint");
        }
    }

    private static ShanheCampaignSession CreateRefreshShopSession(IReadOnlyList<int> sources, int gold, int hpCost = 0)
    {
        var goods = Enumerable.Range(1, 12).Select(index => new ShanheRewardDefinition("refresh-training-" + index,
            "行军操练" + index, Bonuses: new(MaxHp: 1)) { Rarity = 2, Description = "体力上限增加1点。" }).ToArray();
        var perks = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => sources.Contains(source.SourceId)).ToArray();
        Program.Assert(perks.Length == sources.Count, "Every equipped refresh perk must use its actual current source definition.");
        var definition = new ShanheCampaignDefinition("fixture:shanhe-free-refresh-ui", "商店刷新", ["standard:guan-yu"], ["shop"],
        [
            new("shop", "行军集市", ShanheNodeKind.Shop, ["battle"], Choices: goods.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name, GoldCost: 100, HpCost: hpCost, RewardId: reward.Id, LeavesNode: false)).ToArray()),
            new("battle", "下一关", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new("standard:zhang-fei")])])
        ], goods.Concat(perks.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description })).ToArray(), StartingGold: gold);
        var session = ShanheCampaignSession.Create(definition, new(83, "standard:guan-yu")
            { UseBalancedJourneyRewards = true, UseCodexRewardPool = true, EquippedTacticIds = perks.Select(source => source.RewardId).ToArray() }, _ => FreeRefreshRegistry.Value);
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
