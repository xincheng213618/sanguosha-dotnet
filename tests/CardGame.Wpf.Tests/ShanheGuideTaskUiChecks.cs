using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheGuideTaskUiChecks
{
    public static void OriginalConditionsAndPendingScoresRemainReadOnly(string output)
    {
        var account = new MemoryShanheCodexStore();
        const string evidence = "expedition:bc3d7f66d53247cc8fd2a30ca48717b9";
        account.Write(new() { Books = 47, ClaimedRewards = [ShanheGuideTaskSettlement.ClaimKey(200347)],
            GuideTaskProgress = new([new(200347, evidence, 1), new(200352, evidence, 2), new(200352, evidence, 3)]) });
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: account);
        model.SelectedShanheScenario = model.ShanheScenarios.Single(scenario => scenario.Id == StandardShanheCampaignCatalog.Returning2026EditionId);
        var before = JsonSerializer.Serialize(account.Read());
        var panel = new ShanheCodexPanel { DataContext = model };
        try
        {
            Layout(panel);
            var section = (Expander)panel.FindName("CodexGuideTasksExpander");
            Program.Assert(!section.IsExpanded, "Chapter tasks start collapsed so the tactic research tree remains available.");
            Toggle(section); Layout(panel);
            var chapters = (ItemsControl)panel.FindName("CodexGuideTaskChapters");
            var cards = model.ShanheGuideTaskChapters.SelectMany(chapter => chapter.Tasks).ToArray();
            var texts = Program.Find<TextBlock>(chapters).ToArray();
            Program.Assert(section.IsExpanded && chapters.Items.Count == 2 && cards.Length == 6 && chapters.ActualHeight > 0 &&
                cards.All(card => texts.Any(text => text.Text == card.Name) && texts.Any(text => text.Text == card.Condition)) &&
                cards.Select(card => card.SourceId).SequenceEqual(ShanheGuideTaskCatalog.Tasks.Select(task => task.SourceId)) &&
                cards.All(card => card.RewardLabel == "奖励 10 册") &&
                cards.Single(card => card.SourceId == 200347).IsClaimed && cards.All(card => !card.CanProgress),
                "The actual expandable book section preserves source conditions and permanent claimed progress while the selected local edition cannot advance source tasks.");
            Program.Assert(cards.Count(card => card.RequiresScore) == 2 &&
                texts.Count(text => text.DataContext is ShanheGuideTaskCard { RequiresScore: true } && text.Text.Contains("计分依据待核", StringComparison.Ordinal)) == 2 &&
                texts.Any(text => text.Text.Contains("40000", StringComparison.Ordinal)) && texts.Any(text => text.Text.Contains("60000", StringComparison.Ordinal)),
                "Both score tasks retain their real thresholds and explicitly state that the scoring evidence is pending.");
            model.SelectedShanheScenario = model.ShanheScenarios.Single(scenario => scenario.Id == StandardShanheCampaignCatalog.Source2026EditionId);
            model.SelectedShanheDifficulty = model.ShanheDifficulties.Single(tier => tier.Value == 11);
            Layout(panel);
            cards = model.ShanheGuideTaskChapters.SelectMany(chapter => chapter.Tasks).ToArray();
            Program.Assert(cards.Count(card => !card.RequiresScore && (card.IsClaimed || card.CanProgress)) == 4 &&
                cards.Single(card => card.SourceId == 200352) is { CompletedCount: 2, RequiredCount: 3, CanProgress: true } &&
                cards.Where(card => card.RequiresScore).All(card => !card.CanProgress && card.Availability.Contains("计分依据待核", StringComparison.Ordinal)) &&
                model.ShanheGuideTaskNotice.Contains("每项奖励10册", StringComparison.Ordinal),
                "Selecting a qualifying source tier exposes four actionable or completed tasks and actual bounded account progress while the two scores remain pending.");
            model.SelectedShanheDifficulty = model.ShanheDifficulties.Single(tier => tier.Value == 1);
            Program.Assert(!model.ShanheGuideTaskChapters.SelectMany(chapter => chapter.Tasks).Single(card => card.SourceId == 200358).CanProgress,
                "Changing the selected source difficulty refreshes task eligibility against its original threshold.");
            model.SelectedShanheDifficulty = model.ShanheDifficulties.Single(tier => tier.Value == 11); Layout(panel);
            Program.Render(panel, 1400, 714, Path.Combine(output, "shanhe-guide-tasks.png"));
            Toggle(section); Layout(panel);
            Program.Assert(!section.IsExpanded && model.ShanheBookBalance == 47 && JsonSerializer.Serialize(account.Read()) == before,
                "Opening and closing the read-only tasks cannot claim books or rewrite account progress.");
        }
        finally { panel.DataContext = null; }
    }

    private static void Toggle(Expander section)
    {
        var button = Program.Find<ToggleButton>(section).Single(control => control.Name == "GuideTaskToggle");
        typeof(ToggleButton).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }

    private static void Layout(FrameworkElement root)
    {
        root.Measure(new Size(1050, 535)); root.Arrange(new Rect(0, 0, 1050, 535)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
    }
}
