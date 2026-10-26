using System.Windows.Controls;
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
            var session = CreateRefreshShopSession([2230], 700);
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

            session = CreateRefreshShopSession([2230, 2291], 0);
            Attach(vm, session); Layout(panel);
            Program.Assert(refresh.IsEnabled && refresh.Content.ToString() == "免费刷新" && paid.Text == "付费刷新：无限",
                "A zero-copper wallet can use held free refreshes before the unlimited paid allowance.");
            ClickRefreshControl(refresh, panel); ClickRefreshControl(refresh, panel);
            Program.Assert(session.Snapshot.Gold == 0 && refresh.Content.ToString() == "刷新 · 100 铜币" && !refresh.IsEnabled,
                "The actual bound control switches from the final free refresh to an unaffordable paid refresh.");
        }
        finally { panel.DataContext = null; }
    }
}
