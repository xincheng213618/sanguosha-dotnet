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
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class HellActivityUiChecks
{

    public static void FiveTierLobbyRealFiveSeatsDormantAuxiliaryAndRestore(string output)
    {
        var registry = ComposedSkillContentRegistry.CreateShowcase(includeActivities: true,
            includeBossActivities: true, includeHulaoActivities: true, includeShenwuActivities: true,
            includeNianActivities: true, includeHellActivities: true);
        var store = new MemorySaveStore();
        using var vm = new MainViewModel(false, 17, showSetup: true, saveStore: store,
            useExpandedContent: true)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        var window = new MainWindow(vm);
        window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        try
        {
            vm.OpenLobbyCategoryCommand.Execute("activity");
            var options = vm.VisibleTableModes.Where(option => option.ModeId.StartsWith("activity:hell-2020-tier-", StringComparison.Ordinal)).ToArray();
            Program.Assert(options.Length == 5, "The real Hell lobby must expose one entry for each of the five enemy tiers.");
            Program.Assert(registry.Generals.Values.Where(general => general.Id.StartsWith("hell:", StringComparison.Ordinal))
                .All(general => GeneralArt.HasPortrait(general.Id) && new GeneralPortraitViewModel(general.Id).Image is not null) &&
                GeneralArt.NormalizeKey("hell:king-pingdeng") == "hell-pingdeng",
                "Every actual Hell NPC, including the distinct Pingdeng general ID, must load its readable local emblem.");
            for (var tier = 1; tier <= 5; tier++)
            {
                vm.SelectedTableMode = options.Single(option => option.ModeId == $"activity:hell-2020-tier-{tier}");
                Program.Assert(vm.IsActivityModeSelection && vm.IsTeamModeSelection && !vm.IsIdentityModeSelection &&
                    vm.SelectedTableMode.PlayerCount == 5 && vm.StartingTeams.Count == 1 &&
                    vm.SelectedStartingTeam.TeamId == "team:allies" && vm.SelectedStartingTeam.Name == "联合军" &&
                    vm.SelectedTableMode.Description.Contains($"敌方{tier}阶") &&
                    vm.TeamModeSetupText.Contains($"敌方为{tier}阶") && vm.TeamModeSetupText.Contains("玩家固定五阶") &&
                    vm.TeamModeSetupText.Contains("起手牌+2") && vm.TeamModeSetupText.Contains("重生") &&
                    vm.TeamModeSetupText.Contains("两名人机") && vm.CurrentGuideTitle.Contains("十殿阎罗") &&
                    vm.CurrentGuideBody.Contains("地藏王") && !vm.CurrentGuideBody.Contains("涅槃") &&
                    !vm.CurrentGuideBody.Contains("小鬼") && !vm.CurrentGuideBody.Contains("牛头马面"),
                    "Every Hell enemy-tier selection must retain the real five-seat coalition and its own fixed player tier-five and campaign rules.");
            }
            vm.SelectedTableMode = options.Single(option => option.ModeId == "activity:hell-2020-tier-3");
            Program.Render(root, 1440, 880, Path.Combine(output, "166-hell-setup.png"));
            vm.StartNewGameFromLobbyCommand.Execute(null);
            vm.ContinueFromIdentityRevealCommand.Execute(null);
            Until(() => vm.IsGeneralSelectionPending);
            Program.Assert(vm.GeneralSelectionContextTitle.Contains("联合军") &&
                vm.GeneralSelectionContextDetail.Contains("起手六张") && vm.GeneralSelectionSubtitle.Contains("两名队友") &&
                vm.GeneralChoices.All(choice => choice.HealthText.Contains("五阶")) &&
                vm.IdentityRevealRoster.Contains("孟婆") && vm.IdentityObjective.Contains("四殿") &&
                vm.IdentityObjective.Contains("前50回合") && vm.TableArenaTitle == "十 殿 阎 罗",
                "The real private draft must explain the three players, two enemy seats and conditional final boss without Ghost rules.");
            vm.ToggleGeneralRosterCommand.Execute(null);
            Program.Assert(vm.CustomGeneralChoices.Count > 0 && vm.CustomGeneralChoices.All(choice =>
                !choice.GeneralId.StartsWith("hell:", StringComparison.Ordinal) &&
                choice.HealthText == $"五阶体力上限 {registry.Generals[choice.GeneralId].BaseHp + 2}"),
                "The full player roster must exclude NPCs and preview the actual fixed tier-five maximum health.");
            vm.PreviewGeneralChoiceCommand.Execute(vm.CustomGeneralChoices.Single(choice => choice.GeneralId == "classic:guan-yu"));
            vm.ConfirmGeneralChoiceCommand.Execute(null);
            Until(() => Program.Engine(vm).Events.Any(entry => entry.Payload is SetupCompletedEvent));
            var game = Program.Engine(vm);
            var snapshot = game.CreateSnapshot(0);
            var mode = registry.Modes[game.ModeId];
            var auxiliary = snapshot.Players.Single(player => player.GeneralId == mode.HellActivity!.AuxiliaryGeneralId);
            var seat = vm.Seats.Single(player => player.Seat == auxiliary.Seat);
            Program.Assert(snapshot.Players.Count == 5 && snapshot.Players.Count(player => player.IsHuman) == 1 &&
                snapshot.Players.Count(player => player.TeamId == "team:allies") == 3 &&
                snapshot.Players.Count(player => player.TeamId == "team:hell") == 2 &&
                snapshot.Players.Where(player => player.TeamId == "team:allies").All(player =>
                    player.MaxHp == registry.Generals[player.GeneralId].BaseHp + 2 && player.HandCount == 6) &&
                vm.Seats.Count(player => player.IsTeammate && !player.IsHuman) == 2 &&
                vm.Seats.Where(player => player.TeamId == "team:allies").Select(player => player.RoleLabel)
                    .SequenceEqual(["先锋", "中坚", "大将"]) &&
                vm.Seats.Count(player => player.RoleLabel == "首领") == 1 &&
                !auxiliary.IsAlive && !seat.IsAlive && seat.RoleLabel == "孟婆·未登场" &&
                seat.UnavailableStatusText == "未登场" && seat.HpText.Contains("尚未登场") && !seat.HealthImages.Any() &&
                vm.AliveText.Contains("联合军 3") && vm.AliveText.Contains("孟婆未登场") &&
                vm.TableModeText.Contains("第 1/4 殿") && vm.TableModeText.Contains("敌方3阶") &&
                vm.WindowTitle.Contains("十殿阎罗") && !vm.AliveText.Contains("鬼军") &&
                !game.Events.Any(entry => entry.Payload is PlayerDiedEvent died && died.VictimSeat == auxiliary.Seat),
                "The native five-seat table must show two allied AI generals, six-card player hands and a dormant Mengpo rather than a falsely dead character.");
            Program.Render(root, 1440, 880, Path.Combine(output, "167-hell-table.png"));
            var auxiliaryButton = Program.Find<Button>(root).Single(button =>
                ReferenceEquals(button.Command, vm.SelectTargetCommand) && ReferenceEquals(button.CommandParameter, seat));
            var shade = Program.Find<Border>(auxiliaryButton).Single(border => border.Name == "DeathShade");
            Program.Assert(shade.Visibility == Visibility.Visible && !shade.IsHitTestVisible &&
                Program.Find<TextBlock>(shade).Single().Text == "未登场",
                "The actual portrait template must label the dormant seat as not yet appeared, preserving its native dead/non-targetable state.");
            LegacySaveFixtures.Save(vm);
            var saved = SnapshotJson.Serialize(game.CreateSnapshot(0));
            vm.SelectedTableMode = vm.TableModes.Single(option => option.ModeId == "team:standard-2v2");
            LegacySaveFixtures.Load(vm);
            Program.Assert(!vm.HasSaveError && Program.Engine(vm).ModeId == "activity:hell-2020-tier-3" &&
                vm.SelectedStartingTeam.TeamId == "team:allies" && vm.Seats.Count == 5 &&
                vm.TableModeText.Contains("敌方3阶") && vm.AliveText.Contains("孟婆未登场") &&
                SnapshotJson.Serialize(Program.Engine(vm).CreateSnapshot(0)) == saved,
                "A manual Hell checkpoint must restore its selected enemy tier, five seats, dormant auxiliary and exact private snapshot from another setup mode.");

            using var other = new MainViewModel(false, 17, true, store, useExpandedContent: true,
                contentRegistry: ComposedSkillContentRegistry.CreateShowcase(includeNianActivities: true))
                { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
            LegacySaveFixtures.Load(other);
            Program.Assert(!other.HasSaveError && other.SelectedTableMode.ModeId == game.ModeId &&
                other.StartingTeams.Count == 1 && other.SelectedStartingTeam.TeamId == "team:allies" &&
                other.Seats.Count == 5 && other.TableArenaTitle == "十 殿 阎 罗" &&
                SnapshotJson.Serialize(Program.Engine(other).CreateSnapshot(0)) == saved,
                "Checkpoint package recovery must restore Hell even when the receiving desktop's configured registry has no Hell package.");
            other.StartNewGameFromLobbyCommand.Execute(null);
            other.ContinueFromIdentityRevealCommand.Execute(null);
            for (var step = 0; step < 24 && !other.IsGeneralSelectionPending; step++) other.StepAiCommand.Execute(null);
            Program.Assert(!other.HasSaveError && other.IsGeneralSelectionPending && Program.Engine(other).ModeId == game.ModeId &&
                other.GeneralSelectionContextTitle.Contains("联合军") && other.GeneralSelectionContextDetail.Contains("起手六张"),
                "Rematching a recovered Hell save must use its restored registry and reach the native five-seat player draft.");
        }
        finally
        {
            window.Content = null;
            window.DataContext = null;
            window.Close();
        }

        void Until(Func<bool> ready)
        {
            for (var step = 0; step < 64; step++)
            {
                if (ready()) return;
                Program.Assert(vm.StepAiCommand.CanExecute(null), "The real Hell UI must remain able to advance to the bounded setup boundary.");
                vm.StepAiCommand.Execute(null);
            }
            throw new InvalidOperationException("The real Hell UI did not reach its bounded setup boundary.");
        }
    }
}
