using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheRunRecordUiChecks
{
    public static void CompletedRunsRemainVisibleInTheRealHistoryTab(string output)
    {
        var account = new MemoryShanheCodexStore();
        account.Write(new()
        {
            Books = 10, ClaimedRewards = ["history-win", "history-loss"],
            RunRecords =
            [
                new("history-win", "shanhe:zhuhou-2026-2", "诸侯伐董", "classic:guan-yu", "关羽", 2, true,
                    10, 20, 7, new DateTimeOffset(2026, 10, 25, 12, 0, 0, TimeSpan.Zero), "洛阳破晓"),
                new("history-loss", "shanhe:zhuhou-2026-3", "诸侯伐董", "classic:zhao-yun", "赵云", 3, false,
                    0, 6, 3, new DateTimeOffset(2026, 10, 24, 12, 0, 0, TimeSpan.Zero), "军旗暂歇")
            ]
        });
        using var model = new MainViewModel(false, 2501, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: account)
        { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        Program.Assert(model.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Source2026EditionId &&
            model.SelectedShanheScenario.Name.Contains("推荐"),
            "New preparation defaults to the recommended current content instead of hiding it behind the old local theme.");
        model.OpenShanheCommand.Execute(null);
        var panel = new ShanheCampaignPanel { DataContext = model };
        var window = new Window { Content = panel, Width = 1220, Height = 800, ShowInTaskbar = false };
        try
        {
            window.ApplyTemplate();
            var tab = (TabItem)panel.FindName("ShanheRunHistoryTab");
            tab.IsSelected = true;
            Program.Render(panel, 1200, 750, Path.Combine(output, "shanhe-run-history.png"));
            panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
            panel.UpdateLayout();
            var rows = (ItemsControl)panel.FindName("ShanheRunHistoryRows");
            var texts = Program.Find<TextBlock>(panel).Where(t => t.IsVisible || t.ActualHeight > 0).Select(t => t.Text).ToArray();
            Program.Assert(tab.IsSelected && tab.IsEnabled && rows.Items.Count == 2 && rows.ActualHeight > 0 &&
                texts.Contains("诸侯伐董 · 通关") && texts.Contains("诸侯伐董 · 未通关") &&
                texts.Contains("图册 +10") && texts.Contains("图册 +0") &&
                texts.Any(t => t.Contains("关羽") && t.Contains("难度 2")) &&
                texts.Any(t => t.Contains("洛阳破晓")),
                "The real history tab renders both settled outcomes, build identity, ending and credited books independently of a current expedition.");
            Program.Assert(model.ShanheRunRecordSummary.Contains("2 次征程") && account.Read().Books == 10 &&
                account.Read().RunRecords.Length == 2 && !model.IsShanheRunInProgress,
                "Opening the record view neither starts an expedition nor pays a completed reward again.");
        }
        finally { window.Content = null; panel.DataContext = null; window.Close(); }
    }
}
