using System.IO;
using System.Windows;
using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;

internal static partial class ShanheCodexUiChecks
{
    public static void RestockPurchaseClicksReplaceOneSlotAndKeepPricesAndRefreshAllowances(string output)
    {
        using var vm = Create(new MemoryShanheCodexStore(), new MemoryShanheCampaignSaveStore());
        var panel = new ShanheShopPanel { DataContext = vm };
        try
        {
            var session = CreateRestockShopSession(true);
            Attach(vm, session); Layout(panel);
            var notice = (TextBlock)panel.FindName("MarketPurchaseNotice");
            var free = (TextBlock)panel.FindName("MarketFreeRefreshRemaining");
            var paid = (TextBlock)panel.FindName("MarketPaidRefreshRemaining");
            Program.Assert(session.Snapshot is { ShopRestockOnPurchase: true, ShopFreeRefreshRemaining: 2, ShopPaidRefreshRemaining: 5 } &&
                notice.Text == vm.ShanheMarketSurchargeNotice && notice.Text.Contains("购买后原位补入同类商品") &&
                notice.Text.Contains("无货可补时保留已购灰位") && notice.Text.Contains("不消耗刷新次数") &&
                notice.Text.Contains("同类商品额外消耗铜币 +100") &&
                notice.ActualHeight > 0 && notice.DesiredSize.Height <= 49 &&
                notice.TranslatePoint(new Point(0, notice.ActualHeight), panel).Y <= panel.ActualHeight,
                "An actually equipped Restock shows its same-slot replacement, exhausted-pool gray-slot fallback and separate refresh policy beside the unchanged category surcharge, within the existing merchant footer.");

            const int slot = 0;
            Program.Assert(session.Snapshot.ShopItems.Count >= 3 && vm.ShanheMarketGoods.All(item => item.Price == 100),
                "The small real random merchant starts with several growth tactics at their authored hundred-copper price.");
            for (var purchase = 1; purchase <= 2; purchase++)
            {
                var before = session.Snapshot;
                var previous = before.ShopItems[slot];
                var product = vm.ShanheMarketGoods.Single(item => item.Id == previous.ChoiceId);
                var buy = Program.Find<Button>(panel).Single(button => ReferenceEquals(button.CommandParameter, product) &&
                    ReferenceEquals(button.Command, vm.RequestShanheShopPurchaseCommand));
                var revision = session.Revision;
                Program.Assert(product.CanBuy && product.Price == purchase * 100,
                    "The actual bound card exposes the current category price before each repeated purchase.");
                ClickRefreshControl(buy, panel);

                var after = session.Snapshot;
                var replacement = after.ShopItems[slot];
                var replacementCard = vm.ShanheMarketGoods.Single(item => item.Id == replacement.ChoiceId);
                Program.Assert(session.Revision == revision + 1 && after.Gold == before.Gold - product.Price &&
                    after.Bonuses.MaxHp == before.Bonuses.MaxHp + 1 && after.ShopItems.Count == before.ShopItems.Count &&
                    replacement.ChoiceId != previous.ChoiceId && replacement.Reward!.Id != previous.Reward!.Id &&
                    replacement.Name != previous.Name && replacementCard.Name == replacement.Name && replacementCard.CanBuy &&
                    replacementCard.Price == (purchase + 1) * 100 && vm.ShanheMarketGoods.All(item => item.Price == (purchase + 1) * 100) &&
                    Program.Find<TextBlock>(panel).Any(block => block.Text == replacementCard.DisplayName) &&
                    after.ShopItems.Where((_, index) => index != slot).Select(item => (item.ChoiceId, item.Reward!.Id))
                        .SequenceEqual(before.ShopItems.Where((_, index) => index != slot).Select(item => (item.ChoiceId, item.Reward!.Id))) &&
                    after.ShopFreeRefreshRemaining == before.ShopFreeRefreshRemaining && after.ShopPaidRefreshRemaining == before.ShopPaidRefreshRemaining &&
                    after.ShopRefreshRemaining == before.ShopRefreshRemaining && free.Text == "免费刷新：2 次" && paid.Text == "付费刷新：5 次" &&
                    session.AcceptedCommands.OfType<ChooseShanheNodeOptionCommand>().Count() == purchase &&
                    !session.AcceptedCommands.OfType<RefreshShanheShopCommand>().Any() &&
                    !vm.RequestShanheShopPurchaseCommand.CanExecute(product),
                    "Each real purchase click pays and grants its growth once, replaces only that same stock position with a visibly new product, retains the category's 200/300 price escalation, and spends neither free nor paid refreshes.");

                var committed = session.Revision;
                RawRefreshClick(buy);
                vm.RequestShanheShopPurchaseCommand.Execute(product);
                Layout(panel);
                Program.Assert(session.Revision == committed && session.Snapshot.Gold == after.Gold &&
                    session.Snapshot.ShopItems[slot].ChoiceId == replacement.ChoiceId,
                    "The retired bound item cannot be purchased again through its stale button or command parameter after same-slot replacement.");
                Program.Render(panel, 1400, 714, Path.Combine(output, $"shanhe-market-restock-purchase-{purchase}.png"));
            }

            session = CreateRestockShopSession(false);
            Attach(vm, session); Layout(panel);
            Program.Assert(!session.Snapshot.ShopRestockOnPurchase &&
                notice.Text == "每购买一次战法，本店同类商品额外消耗铜币 +100",
                "A merchant without Restock keeps its existing purchase notice and does not advertise an unearned replacement effect.");
        }
        finally { panel.DataContext = null; }
    }

    private static ShanheCampaignSession CreateRestockShopSession(bool restock)
    {
        var goods = Enumerable.Range(1, 12).Select(index => new ShanheRewardDefinition("restock-growth-" + index,
            "军略研习" + index, Bonuses: new(MaxHp: 1)) { Description = "体力上限增加1点。" }).ToArray();
        var sources = restock ? new[] { 2252, 2291, 2107 } : new[] { 2291, 2107 };
        var perks = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => sources.Contains(source.SourceId)).ToArray();
        Program.Assert(perks.Length == sources.Length, "The fixture equips the actual current Restock, Free Refresh and Membership source definitions.");
        var definition = new ShanheCampaignDefinition("fixture:shanhe-restock-ui", "商店原位补货", ["standard:guan-yu"], ["shop"],
        [
            new("shop", "行军集市", ShanheNodeKind.Shop, ["battle"], Choices: goods.Select(reward =>
                new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name, GoldCost: 100, RewardId: reward.Id, LeavesNode: false)).ToArray()),
            new("battle", "下一关", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new("standard:zhang-fei")])])
        ], goods.Concat(perks.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description })).ToArray(), StartingGold: 1000);
        var session = ShanheCampaignSession.Create(definition, new(83, "standard:guan-yu")
            { UseCodexRewardPool = true, EquippedTacticIds = perks.Select(source => source.RewardId).ToArray() }, _ => FreeRefreshRegistry.Value);
        var entered = session.Submit(new MoveShanheNodeCommand("shop", session.Revision));
        Program.Assert(entered.Accepted, entered.Error ?? "The Restock UI fixture must enter its actual merchant through a real command.");
        return session;
    }
}
