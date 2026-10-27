using System.Reflection;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.Controls;
using CardGame.Wpf.ViewModels;

internal static class ShanheSourceDifficultyUiChecks
{
    public static void SourceSelectionStartsActualBattleAndRestoresTwentyFifthTier(string output)
    {
        var progress = new MemoryShanheCampaignSaveStore();
        var account = new MemoryShanheCodexStore();
        account.Write(new());
        using var vm = Create();
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        Program.Assert(vm.ShanheDifficulties.Select(option => option.Value).SequenceEqual(Enumerable.Range(1, 25)) &&
            vm.ShanheDifficulties[24].Name.Contains("炼狱5", StringComparison.Ordinal),
            "The source-reproduction scenario exposes the genuine twenty-five distinct source identities.");
        vm.SelectedShanheDifficulty = vm.ShanheDifficulties[24];
        vm.ShanheGeneralSearch = "SP刘备·煮酒";
        vm.ToggleShanheGeneralRosterCommand.Execute(null);
        vm.SelectedShanheGeneral = vm.ShanheGenerals.Single(general => general.Id == StandardOlSpLiuBei664Package.GeneralId);
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.LocalEditionId);
        Program.Assert(vm.ShanheDifficulties.Count == 3 && vm.SelectedShanheDifficulty.Value == 1,
            "Returning to the old local scenario safely replaces a source-only selection with a valid local difficulty.");
        vm.SelectedShanheScenario = vm.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        vm.SelectedShanheDifficulty = vm.ShanheDifficulties[24];
        var panel = new ShanheCampaignPanel { DataContext = vm };
        panel.Measure(new Size(1440, 880)); panel.Arrange(new Rect(0, 0, 1440, 880)); panel.UpdateLayout();
        panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
        var selector = (ComboBox)panel.FindName("ShanheDifficultySelector");
        Program.Assert(selector.Items.Count == 25 && selector.SelectedItem is ShanheDifficultyOption { Value: 25 } &&
            selector.ToolTip is string description && description.Contains("炼狱5", StringComparison.Ordinal),
            "The real bound selector shows tier twenty-five and its difficulty explanation.");
        vm.StartShanheCommand.Execute(null);
        var session = Session(vm);
        Program.Assert(session.Definition.SourceDifficultyId == 25 && session.CreateCheckpoint().Options.SourceDifficultyId == 25 &&
            session.Snapshot.PlayerGeneralId == StandardOlSpLiuBei664Package.GeneralId &&
            vm.ShanheGuideTaskChapters.SelectMany(chapter => chapter.Tasks).Where(task => !task.RequiresScore)
                .All(task => !task.Availability.StartsWith("待开放", StringComparison.Ordinal)),
            "The actual start command binds source difficulty and opens the four non-score tasks.");
        panel.UpdateLayout();
        Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-source-25-map.png"));
        var offeredButtons = Program.Find<Button>(panel).Where(button => button.DataContext is ShanheJourneyNode { IsAvailable: true }).ToArray();
        var bounds = offeredButtons.Select(button => button.TransformToAncestor(panel).TransformBounds(new Rect(new Size(button.ActualWidth, button.ActualHeight)))).ToArray();
        Program.Assert(offeredButtons.Select(button => ((ShanheJourneyNode)button.DataContext).Node.Id).ToHashSet()
                .SetEquals(session.Snapshot.AvailableNodes.Select(node => node.Id)) && vm.ShanheSourceRegions.Count == 1 &&
            bounds.SelectMany((rectangle, index) => bounds.Skip(index + 1).Select(other => rectangle.IntersectsWith(other))).All(overlap => !overlap),
            "Every available source destination, including the local elite camp, has a separate clickable rectangle; the unchosen region does not crowd the map.");
        panel.DataContext = null;
        var first = vm.ShanheMapRows.SelectMany(row => row.Nodes).First(node => node.IsAvailable);
        var preview = session.PreviewEncounter(first.Id);
        vm.PreviewShanheNodeCommand.Execute(first);
        Program.Assert(vm.ShanheEncounterEnemyCards.Count == preview.Enemies.Count && preview.Enemies[0].InitialArmor > 0 &&
            vm.ShanheEncounterEnemyCards[0].Stats.Contains($"初始护甲 {preview.Enemies[0].InitialArmor}", StringComparison.Ordinal),
            "The real encounter preview reports the same grown initial armor that will enter battle.");
        vm.ChallengeShanheEncounterCommand.Execute(null);
        Program.Assert(vm.IsShanheBattleSession && session.PendingBattleSetup is { } && vm.FlushShanheProgress(),
            "A real source-tier map command starts and saves the native encounter, including all generated growth.");
        Program.Assert(Program.Engine(vm).CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.ContentId == StandardOlSpLiuBei664Package.ZhujiuSkillId),
            "The newly registered SP version is selected through the real roster and enters battle with its executable brewing skill.");
        var expected = SnapshotJson.Serialize(Program.Engine(vm).CreateSnapshot(0));
        var saved = ShanheCampaignJson.Serialize(progress.Read().Campaign);
        using var cold = Create();
        cold.ContinueShanheCommand.Execute(null);
        Program.Assert(cold.IsShanheBattleRetryReady && !cold.IsShanheBattleSession,
            "Source progress restores the current encounter entry without an in-flight game.");
        cold.ResumeShanheBattleCommand.Execute(null);
        Program.Assert(cold.IsShanheBattleSession && cold.SelectedShanheScenario.Id == StandardShanheCampaignCatalog.Source2026EditionId &&
            cold.SelectedShanheDifficulty.Value == 25 && SnapshotJson.Serialize(Program.Engine(cold).CreateSnapshot(0)) == expected &&
            ShanheCampaignJson.Serialize(progress.Read().Campaign) == saved,
            "Retry preserves source tier twenty-five and reconstructs its initial battle without changing expedition progress.");
        VerifyRegionChoiceOnlyChangesPresentation(output);

        MainViewModel Create() => new(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: progress, shanheCodexStore: account);
    }

    private static void VerifyRegionChoiceOnlyChangesPresentation(string output)
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(11);
        var cities = Enumerable.Range(5011, 7).Concat(Enumerable.Range(5081, 7)).Select(StandardShanheCampaignCatalog.Source2026CityNodeId).ToHashSet();
        var definition = ShanheCampaignContent.FreezeAndValidate(source with
        {
            StartingNodeIds = cities.ToArray(), RandomStartingNode = false,
            Nodes = source.Nodes.Where(node => cities.Contains(node.Id)).Select(node => node with { NextNodeIds = [], EliteUpgradeNodeId = null }).ToArray()
        });
        var session = ShanheCampaignSession.Create(definition,
            new(71, StandardOlSpLiuBei664Package.GeneralId) { SourceDifficultyId = 11, UseCodexRewardPool = true },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: new MemoryShanheCampaignSaveStore(), shanheCodexStore: new MemoryShanheCodexStore());
        model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        model.SelectedShanheDifficulty = model.ShanheDifficulties.Single(option => option.Value == 11);
        typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, session);
        typeof(MainViewModel).GetField("_shanheRunDifficulty", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(model, 11);
        typeof(MainViewModel).GetMethod("RefreshShanhePresentation", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, null);
        var before = session.Snapshot;
        var panel = new ShanheCampaignPanel { DataContext = model };
        try
        {
            panel.Measure(new Size(1440, 880)); panel.Arrange(new Rect(0, 0, 1440, 880)); panel.UpdateLayout();
            var selector = Program.Find<ComboBox>(panel).Single(control => control.Name == "ShanheSourceRegionSelector");
            Program.Assert(selector.Visibility == Visibility.Visible && selector.Items.Count == 2,
                "Multiple available source regions have an actual visible region selector.");
            var selectorBounds = selector.TransformToAncestor(panel).TransformBounds(new Rect(selector.RenderSize));
            var objective = Program.Find<TextBlock>(panel).Single(control => control.Text == model.ShanheChapterObjectiveText);
            var objectiveBounds = objective.TransformToAncestor(panel).TransformBounds(new Rect(objective.RenderSize));
            Program.Assert(!selectorBounds.IntersectsWith(objectiveBounds) &&
                selector.Foreground is System.Windows.Media.SolidColorBrush foreground && foreground.Color.R < 128 && foreground.Color.G < 128 && foreground.Color.B < 128,
                "The region selector stays clear of the chapter objective and has readable dark text.");
            selector.SelectedItem = model.ShanheSourceRegions.Last();
            panel.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); panel.UpdateLayout();
            Program.Assert(model.ShanheMapNodes.Count == 7 && model.ShanheMapNodes.All(node => node.Node.Name.StartsWith("孟津", StringComparison.Ordinal)) &&
                ReferenceEquals(before, session.Snapshot) && session.AcceptedCommands.Count == 0,
                "Switching regions reveals all seven destinations without choosing a route, rerolling an encounter or committing a campaign command.");
            Program.Render(panel, 1440, 880, Path.Combine(output, "shanhe-source-region-choice.png"));
        }
        finally { panel.DataContext = null; }
    }

    private static ShanheCampaignSession Session(MainViewModel model) => (ShanheCampaignSession)typeof(MainViewModel)
        .GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(model)!;
}
