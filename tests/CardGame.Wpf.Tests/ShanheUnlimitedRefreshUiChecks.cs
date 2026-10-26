using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;

internal static partial class ShanheCodexUiChecks
{
    public static void UnlimitedPaidRefreshLabelAndButtonFollowTheActualWallet(string output)
    {
        using var vm = Create(new MemoryShanheCodexStore(), new MemoryShanheCampaignSaveStore());
        var panel = new ShanheShopPanel { DataContext = vm };
        try
        {
            var session = CreateSchoolRefreshShopSession(700, freeRefreshes: false);
            Attach(vm, session); Layout(panel);
            var refresh = (Button)panel.FindName("MarketRefreshButton");
            var paid = (TextBlock)panel.FindName("MarketPaidRefreshRemaining");
            Program.Assert(session.Snapshot.ShopPaidRefreshUnlimited && session.Snapshot.ShopRefreshRemaining == 0 &&
                paid.Text == "付费刷新：无限" && refresh.Content.ToString() == "刷新 · 100 铜币" && refresh.IsEnabled,
                "The actual shop displays unlimited paid uses and enables its bound 100-coin button without inventing a finite count.");
            for (var used = 1; used <= 6; used++) ClickRefreshControl(refresh, panel);
            Program.Assert(session.Snapshot.Gold == 100 && refresh.IsEnabled && paid.Text == "付费刷新：无限",
                "The unlimited button stays available after exceeding membership's five uses when the wallet still pays for another use.");
            ClickRefreshControl(refresh, panel);
            Program.Assert(session.Snapshot.Gold == 0 && paid.Text == "付费刷新：无限" &&
                !refresh.IsEnabled && !vm.RefreshShanheMarketCommand.CanExecute(null),
                "Infinity keeps its accurate label but the real paid button disables when the wallet cannot pay 100.");
            var before = session.Snapshot;
            RawRefreshClick(refresh); vm.RefreshShanheMarketCommand.Execute(null); Layout(panel);
            Program.Assert(ReferenceEquals(before, session.Snapshot), "Disabled UI activation cannot issue a free unlimited refresh.");

            session = CreateSchoolRefreshShopSession(0, freeRefreshes: true);
            Attach(vm, session); Layout(panel);
            Program.Assert(refresh.IsEnabled && refresh.Content.ToString() == "免费刷新" && paid.Text == "付费刷新：无限",
                "A zero-copper wallet can use held free refreshes before the unlimited paid allowance.");
            ClickRefreshControl(refresh, panel); ClickRefreshControl(refresh, panel);
            Program.Assert(session.Snapshot.Gold == 0 && refresh.Content.ToString() == "刷新 · 100 铜币" && !refresh.IsEnabled,
                "The actual bound control switches from the final free refresh to an unaffordable paid refresh.");
        }
        finally { panel.DataContext = null; }
    }

    private static ShanheCampaignSession CreateSchoolRefreshShopSession(int gold, bool freeRefreshes)
    {
        int[] carried = [2018, 2092, 2090, 2017, 2107, 2137];
        int[] granted = freeRefreshes ? [2146, 2291] : [2146];
        var sources = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => carried.Concat(granted).Contains(source.SourceId)).ToArray();
        var perks = sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
        {
            CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description,
            SchoolMembership = StandardShanheCampaignCatalog.CurrentTacticSchoolMembershipBySourceId.TryGetValue(source.SourceId, out var member) ? member : null
        }).ToArray();
        var goods = Enumerable.Range(1, 12).Select(index => new ShanheRewardDefinition("school-training-" + index,
            "行军操练" + index, Bonuses: new(MaxHp: 1))).ToArray();
        var grants = granted.Select((id, index) => new ShanheNodeDefinition("grant-" + index, "已取得的战法", ShanheNodeKind.Event,
            [index + 1 < granted.Length ? "grant-" + (index + 1) : "shop"],
            Choices: [new("take", "领取", RewardId: sources.Single(s => s.SourceId == id).RewardId)])).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:school-refresh-ui", "资源流商店", ["standard:guan-yu"], [grants[0].Id],
            grants.Concat(new ShanheNodeDefinition[]
            {
                new("shop", "行军集市", ShanheNodeKind.Shop, ["battle"], Choices: goods.Select(reward =>
                    new ShanheNodeChoiceDefinition("buy-" + reward.Id, reward.Name, 100, RewardId: reward.Id, LeavesNode: false)).ToArray()),
                new("battle", "下一关", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new("standard:zhang-fei")])])
            }).ToArray(), goods.Concat(perks).ToArray(), StartingGold: gold)
            { Schools = StandardShanheCampaignCatalog.CurrentSchools };
        var session = ShanheCampaignSession.Create(definition, new(83, "standard:guan-yu")
            { UseCodexRewardPool = true, EquippedTacticIds = carried.Select(id => sources.Single(s => s.SourceId == id).RewardId).ToArray() }, _ => FreeRefreshRegistry.Value);
        foreach (var grant in grants)
        {
            var moved = session.Submit(new MoveShanheNodeCommand(grant.Id, session.Revision));
            var gained = session.Submit(new ChooseShanheNodeOptionCommand("take", null, session.Revision));
            Program.Assert(moved.Accepted && gained.Accepted, moved.Error ?? gained.Error ?? "Actual school component acquisition failed.");
        }
        var entered = session.Submit(new MoveShanheNodeCommand("shop", session.Revision));
        Program.Assert(entered.Accepted && session.GetSchoolProgress().Single(p => p.School.Id == 1).IsComplete,
            entered.Error ?? "The fixture's seven distinct owned components must complete resource school.");
        return session;
    }
}
