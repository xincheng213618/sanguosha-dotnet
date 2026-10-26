using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Controls;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheCodexPerformanceChecks
{
    public static void OpenSelectAndReopen(string output)
    {
        var timer = Stopwatch.StartNew();
        using var model = new MainViewModel(false, 31, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore(),
            historyStore: new MemoryMatchHistoryStore(), preferencesStore: new MemoryPlayerPreferencesStore());
        Console.WriteLine($"Codex model: {timer.Elapsed.TotalMilliseconds:F1} ms.");
        timer.Restart();
        var panel = new ShanheCampaignPanel { DataContext = model };
        Program.Render(panel, 1440, 900, null);
        Console.WriteLine($"Codex campaign panel: {timer.Elapsed.TotalMilliseconds:F1} ms.");
        var tabs = (TabControl)panel.FindName("ShanheJourneyTabs");
        timer.Restart();
        tabs.SelectedIndex = 1;
        Program.Render(panel, 1440, 900, null);
        var book = (ShanheCodexPanel)panel.FindName("ShanheCodexHost");
        var nodes = (ItemsControl)book.FindName("CodexTreeNodes");
        var scroll = (ScrollViewer)book.FindName("CodexTreeScroll");
        Console.WriteLine($"Codex first tab: {timer.Elapsed.TotalMilliseconds:F1} ms, {Program.Find<Button>(nodes).Count()} nodes realized.");
        AssertBoundedNodes();
        var previous = model.ShanheCodexNodes.ToArray();
        var nodeChanges = 0;
        var nodeResets = 0;
        var linkChanges = 0;
        model.ShanheCodexNodes.CollectionChanged += (_, change) =>
        {
            nodeChanges++;
            if (change.Action == NotifyCollectionChangedAction.Reset) nodeResets++;
        };
        model.ShanheCodexLinks.CollectionChanged += (_, _) => linkChanges++;
        timer.Restart();
        model.SelectCodexNodeCommand.Execute(model.ShanheCodexNodes.Single(node => node.TreeId == 3));
        Program.Render(panel, 1440, 900, null);
        Console.WriteLine($"Codex select: {timer.Elapsed.TotalMilliseconds:F1} ms.");
        Program.Assert(nodeChanges == 2 && nodeResets == 0 && linkChanges == 0 &&
            model.SelectedCodexNode is { TreeId: 3, IsSelected: true } &&
            model.ShanheCodexNodes.Where(node => node.TreeId is not (1 or 3)).All(node => ReferenceEquals(node, previous.Single(old => old.Id == node.Id))),
            "Selecting a node only replaces the previous and current selection, retaining research paths, artwork and links.");
        var tail = model.ShanheCodexNodes.MaxBy(node => node.Column)!;
        scroll.ScrollToHorizontalOffset(tail.X - 100);
        Program.Render(panel, 1440, 900, null);
        AssertBoundedNodes();
        var tailButton = Program.Find<Button>(nodes).Single(button => button.CommandParameter is ShanheCodexNode node && node.Id == tail.Id);
        Program.Assert(tailButton.IsEnabled && tailButton.Command.CanExecute(tailButton.CommandParameter),
            "The final research column must be reachable with a working node command.");
        tailButton.Command.Execute(tailButton.CommandParameter);
        Program.Render(panel, 1440, 900, null);
        Program.Assert(model.SelectedCodexNode?.Id == tail.Id && scroll.HorizontalOffset > 1000,
            "A virtualized tail node must select without jumping back to the start.");
        Program.Render(panel, 1440, 900, Path.Combine(output, "shanhe-book-viewport-tail.png"));
        timer.Restart();
        for (var index = 0; index < 3; index++)
        {
            tabs.SelectedIndex = 0;
            Program.Render(panel, 1440, 900, null);
            tabs.SelectedIndex = 1;
            Program.Render(panel, 1440, 900, null);
        }
        Console.WriteLine($"Codex reopen x3: {timer.Elapsed.TotalMilliseconds:F1} ms.");
        Program.Assert(scroll.HorizontalOffset > 1000 && model.SelectedCodexNode?.Id == tail.Id,
            "Switching tabs preserves the research viewport and selection.");
        var changesBeforeRefresh = nodeChanges;
        timer.Restart();
        model.OpenShanheCommand.Execute(null);
        var refreshMs = timer.Elapsed.TotalMilliseconds;
        Program.Render(panel, 1440, 900, null);
        Console.WriteLine($"Codex campaign refresh: command {refreshMs:F1} ms, render total {timer.Elapsed.TotalMilliseconds:F1} ms.");
        Program.Assert(nodeChanges == changesBeforeRefresh && linkChanges == 0,
            "Reopening the same campaign must not rebuild its unchanged codex.");
        scroll.ScrollToLeftEnd();
        Program.Render(panel, 1440, 900, Path.Combine(output, "shanhe-book-viewport-first.png"));
        AssertBoundedNodes();

        void AssertBoundedNodes()
        {
            var realized = Program.Find<Button>(nodes).Select(button => (ShanheCodexNode)button.CommandParameter).ToArray();
            Program.Assert(realized.Length is > 0 and <= 35 && realized.Length < model.ShanheCodexNodes.Count &&
                model.ShanheCodexNodes.Where(node => node.X + 98 >= scroll.HorizontalOffset &&
                    node.X <= scroll.HorizontalOffset + scroll.ViewportWidth).All(node => realized.Any(item => item.Id == node.Id)),
                "The research tree realizes only the viewport and nearby columns, including every visible node.");
        }
    }
}
