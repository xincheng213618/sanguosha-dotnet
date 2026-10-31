using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Audio;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class GuanduActivityUiChecks
{
    public static void TwoArmiesCommanderGeneralEventAndCrossConfigurationRestore(string output)
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(includeGuanduActivities: true);
        var mode = registry.Modes[StandardGuanduActivityPackage.ModeId];
        var activity = mode.GuanduActivity ?? throw new InvalidOperationException("The actual Guandu mode is missing its activity definition.");
        foreach (var (family, edition) in StandardGuanduActivityPackage.OrdinaryVersions)
        {
            var id = "guandu:" + family;
            var source = GeneralArt.GetSkin(edition);
            Program.Assert(source is not null && GeneralArt.NormalizeKey(id) == GeneralArt.NormalizeKey(edition) &&
                GeneralArt.GetSkin(id)?.LocalPath == source.LocalPath &&
                new GeneralPortraitViewModel(id).Image is System.Windows.Media.Imaging.BitmapSource { PixelWidth: > 0, PixelHeight: > 0 },
                $"The actual Guandu clone {id} must decode the exact {edition} local portrait rather than fall back to its name.");
        }
        Program.Assert(GeneralArt.GetSkin("guandu:cao-cao")?.LocalPath == GeneralArt.GetSkin("boundary:cao-cao")?.LocalPath &&
            GeneralArt.GetSkin("guandu:guan-yu")?.LocalPath == GeneralArt.GetSkin("sp:guan-yu")?.LocalPath &&
            new[] { "xu-you", "gao-lan", "shen-pei", "zhang-he", "xun-chen" }.All(family =>
                new GeneralPortraitViewModel("guandu:" + family).Image is System.Windows.Media.Imaging.BitmapSource { PixelWidth: > 0, PixelHeight: > 0 }),
            "Guandu must retain the Boundary Cao Cao and SP Guan Yu editions and reuse each available same-character activity portrait.");
        var store = new MemorySaveStore();
        using var vm = new MainViewModel(false, 17, true, store, useExpandedContent: true, contentRegistry: registry)
        { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false, ManualDiscardEnabled = false };
        var window = new MainWindow(vm); window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        try
        {
            vm.OpenLobbyCategoryCommand.Execute("activity");
            vm.SelectedTableMode = vm.VisibleTableModes.Single(option => option.ModeId == mode.Id);
            Layout(root);
            var teams = (ListBox)window.FindName("TeamChoices");
            var roles = (ListBox)window.FindName("GuanduRoleChoices");
            Program.Assert(vm.IsGuanduActivityModeSelection && vm.IsActivityModeSelection && vm.IsTeamModeSelection &&
                !vm.IsIdentityModeSelection && vm.SelectedGuanduRole.Role == GuanduSeatRole.General &&
                teams.Visibility == Visibility.Visible && teams.Items.Count == 2 && roles.Visibility == Visibility.Visible && roles.Items.Count == 2 &&
                vm.StartingTeams.Select(team => team.TeamId).SequenceEqual(["team:cao", "team:yuan"]),
                "The actual Guandu lobby must offer both armies and the default general/fixed commander role without identity choices.");
            teams.SelectedItem = vm.StartingTeams.Single(team => team.TeamId == "team:cao");
            roles.SelectedItem = vm.GuanduRoles.Single(role => role.Role == GuanduSeatRole.Commander);
            Layout(root);
            Program.Assert(vm.SelectedGuanduRole.Role == GuanduSeatRole.Commander && vm.TeamModeSetupText.Contains("曹操军") &&
                vm.TeamModeSetupText.Contains("三名人机") && vm.TeamModeSetupText.Contains("主帅") && vm.TeamModeSetupText.Contains("牌堆耗尽") &&
                vm.TeamModeSetupText.Contains("本地五阶") && vm.CurrentGuideTitle.Contains("官渡"),
                "Bound commander and army choices must explain three AI allies, commander victory and the actual deck-loss boundary.");
            Program.Render(root, 1440, 880, Path.Combine(output, "184-guandu-army-and-role.png"));
            vm.StartNewGameFromLobbyCommand.Execute(null);
            vm.ContinueFromIdentityRevealCommand.Execute(null);
            Until(vm, () => vm.IsGeneralSelectionPending);
            Program.Assert(vm.GeneralChoices is [{ GeneralId: var commander }] && commander == activity.Commanders["team:cao"],
                "Choosing commander must expose only the fixed army commander, with no ordinary substitute.");
            AssertFixedDraft();
            Click(root, (Button)window.FindName("ConfirmGeneralChoiceButton"));
            Until(vm, () => Program.Engine(vm).Events.Any(entry => entry.Payload is SetupCompletedEvent));
            var game = Program.Engine(vm); var snapshot = View(game); var state = snapshot.GuanduActivity!;
            var human = snapshot.Players.Single(player => player.IsHuman);
            Layout(root);
            Program.Assert(vm.Seats.Single(seat => seat.IsHuman).Portrait.Image is System.Windows.Media.Imaging.BitmapSource { PixelWidth: > 0, PixelHeight: > 0 },
                "The real bound Guandu commander seat must display its decoded portrait on the actual table.");
            Program.Assert(state is { HumanRole: GuanduSeatRole.Commander, PlayerTeamId: "team:cao", DeckExhausted: false } &&
                human.Seat == state.PlayerCommanderSeat && human.GeneralId == activity.Commanders["team:cao"] &&
                snapshot.Players[state.OpponentCommanderSeat].GeneralId == activity.Commanders["team:yuan"] &&
                snapshot.Players.Count == 8 && snapshot.Players.GroupBy(player => player.TeamId).All(team => team.Count() == 4) &&
                vm.Seats.Count(seat => seat.IsTeammate) == 3 && vm.Seats.Count(seat => !seat.IsHuman && !seat.IsTeammate) == 4 &&
                vm.Seats.Count(seat => seat.RoleLabel.EndsWith("主帅")) == 2 &&
                snapshot.Players.Where(player => !player.IsHuman).All(player => player.Hand.Count == 0) &&
                vm.TableArenaTitle == "官 渡 之 战" && vm.IdentityObjective.Contains("敌方主帅") &&
                vm.GuanduRoundText.Contains("轮") && vm.GuanduEventText.Length > 0 && vm.GuanduCommanderText.Contains("玩家方失败") &&
                ((FrameworkElement)window.FindName("GuanduEventBanner")).Visibility == Visibility.Visible &&
                vm.IsGuanduGuide && !vm.IsTeamGuide && !vm.IsIdentityGuide,
                "The real commander game must render eight public army seats, two fixed leaders, one of nine events and no teammate hands or Jiange labels.");
            Program.Render(root, 1440, 880, Path.Combine(output, "185-guandu-commanders-and-event.png"));
            LegacySaveFixtures.Save(vm); var saved = SnapshotJson.Serialize(View(game));
            using var restored = new MainViewModel(false, 17, true, store, contentRegistry: ComposedSkillContentRegistry.CreateShowcase())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
            LegacySaveFixtures.Load(restored);
            Program.Assert(!restored.HasSaveError && restored.IsGuanduActivityTable && restored.SelectedStartingTeam.TeamId == "team:cao" &&
                restored.SelectedGuanduRole.Role == GuanduSeatRole.Commander && SnapshotJson.Serialize(View(Program.Engine(restored))) == saved,
                "Cross-configuration recovery must restore the actual Guandu package, army, commander role and fixed event without substituting another game.");
            restored.StartNewGameFromLobbyCommand.Execute(null); restored.ContinueFromIdentityRevealCommand.Execute(null);
            Until(restored, () => restored.IsGeneralSelectionPending);
            Program.Assert(restored.GeneralChoices is [{ GeneralId: var fixedId }] && fixedId == activity.Commanders["team:cao"],
                "A restored commander rematch must retain the same single fixed commander choice.");
            restored.ConfirmGeneralChoiceCommand.Execute(null);
            Until(restored, () => Program.Engine(restored).Events.Any(entry => entry.Payload is SetupCompletedEvent));
            Program.Assert(!restored.HasSaveError && View(Program.Engine(restored)).GuanduActivity is { HumanRole: GuanduSeatRole.Commander, PlayerTeamId: "team:cao" },
                "Rematching after recovery must retain the restored army and fixed commander role.");
            vm.SelectedStartingTeam = vm.StartingTeams.Single(team => team.TeamId == "team:yuan");
            vm.SelectedGuanduRole = vm.GuanduRoles.Single(role => role.Role == GuanduSeatRole.General);
            vm.StartNewGameFromLobbyCommand.Execute(null); vm.ContinueFromIdentityRevealCommand.Execute(null);
            Until(vm, () => vm.IsGeneralSelectionPending);
            game = Program.Engine(vm); snapshot = View(game);
            Program.Assert(snapshot.GuanduActivity is { HumanRole: GuanduSeatRole.General, PlayerTeamId: "team:yuan" } &&
                snapshot.Players.Single(player => player.IsHuman).Seat != snapshot.GuanduActivity.PlayerCommanderSeat &&
                vm.GeneralChoices.Count == activity.CandidateCount &&
                vm.GeneralChoices.All(choice => activity.GeneralPools["team:yuan"].Contains(choice.GeneralId) &&
                    choice.GeneralId != activity.Commanders["team:yuan"]) &&
                vm.GeneralSelectionSubtitle.Contains("三名AI队友") && vm.GeneralChoices.All(choice => choice.HealthText.Contains("五阶体力上限")),
                "A real Yuan general opening must expose its private faction pool rather than offer either fixed commander.");
            AssertFixedDraft();
            LegacySaveFixtures.Save(vm); saved = SnapshotJson.Serialize(View(game));
            LegacySaveFixtures.Load(vm);
            Program.Assert(!vm.HasSaveError && vm.IsGeneralSelectionPending && vm.SelectedGuanduRole.Role == GuanduSeatRole.General &&
                vm.SelectedStartingTeam.TeamId == "team:yuan" && SnapshotJson.Serialize(View(Program.Engine(vm))) == saved,
                "Saving during the actual general draft must retain its prompt, army, role and event exactly.");
            Program.AssertNoBindingErrors();
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }

        void AssertFixedDraft()
        {
            Layout(root);
            var engine = Program.Engine(vm);
            var before = SnapshotJson.Serialize(View(engine));
            var commands = engine.AcceptedCommands.Count;
            Program.Assert(!vm.CanExpandGeneralRoster && !vm.ToggleGeneralRosterCommand.CanExecute(null) &&
                ((Button)window.FindName("ToggleGeneralRosterButton")).Visibility == Visibility.Collapsed,
                "Both fixed commanders and six-candidate generals must hide and disable the expanded-roster action.");
            vm.ToggleGeneralRosterCommand.Execute(null);
            Program.Assert(engine.AcceptedCommands.Count == commands && SnapshotJson.Serialize(View(engine)) == before,
                "Invoking the unavailable action must preserve the exact fixed draft and its prompt.");
        }
    }
    private static void Click(FrameworkElement root, Button button)
    {
        Layout(root);
        Program.Assert(button.IsEnabled && button.Command!.CanExecute(button.CommandParameter), "The real Guandu button must be enabled before its routed click.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        Layout(root);
    }
    private static GameSnapshot View(GameEngine game) => game.CreateSnapshot(game.State.HumanSeat);
    private static void Layout(FrameworkElement root)
    {
        root.Measure(new Size(1440, 880)); root.Arrange(new Rect(0, 0, 1440, 880)); root.UpdateLayout();
        root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
    }
    private static void Until(MainViewModel vm, Func<bool> done)
    {
        for (var step = 0; step < 240; step++)
        {
            if (done()) return;
            Program.Assert(vm.StepAiCommand.CanExecute(null), "The real Guandu setup must advance to its exact native human boundary.");
            vm.StepAiCommand.Execute(null);
        }
        throw new InvalidOperationException("The bounded Guandu UI missed its native setup boundary.");
    }
}
