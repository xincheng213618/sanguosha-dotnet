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
using CardGame.Wpf.Presentation;
using CardGame.Wpf.ViewModels;

internal static class WenheActivityUiChecks
{

    public static void SurvivalLobbyPrivateGoalEightSeatsAndCrossConfigurationRestore(string output)
    {
        var peach = new CardViewModel
        {
            Id = 1, Kind = CardKind.Peach, Name = "桃", KindLabel = "基本牌", SuitGlyph = "♥", Rank = "Q",
            Description = "Real Peach material with a round-scoped poison identity.", IsPlayable = false
        };
        var artworkChanges = new List<string?>();
        peach.PropertyChanged += (_, args) => artworkChanges.Add(args.PropertyName);
        var printedArtwork = peach.Artwork;
        peach.IdentityKind = CardKind.Poison;
        Program.Assert(peach.Kind == CardKind.Peach && peach.IdentityLabel == "毒" && peach.HasArtwork &&
            ReferenceEquals(peach.Artwork, CardArt.Get(CardKind.Poison)) && !ReferenceEquals(peach.Artwork, printedArtwork) &&
            artworkChanges.Contains(nameof(CardViewModel.Artwork)) && artworkChanges.Contains(nameof(CardViewModel.HasArtwork)),
            "A real Peach's round poison identity must display poison art and notify both bindings without changing the printed entity kind.");
        artworkChanges.Clear();
        peach.IdentityKind = null;
        Program.Assert(peach.Kind == CardKind.Peach && !peach.HasIdentity && ReferenceEquals(peach.Artwork, printedArtwork) &&
            artworkChanges.Contains(nameof(CardViewModel.Artwork)) && artworkChanges.Contains(nameof(CardViewModel.HasArtwork)),
            "Ending the poison identity must restore the printed Peach artwork through live binding notifications.");
        peach.IdentityKind = CardKind.FireSlash;
        Program.Assert(ReferenceEquals(peach.Artwork, printedArtwork), "Existing non-poison conversion identities must retain their printed-card artwork behavior.");
        var registry = ComposedSkillContentRegistry.CreateShowcase(includeWenheActivities: true);
        var store = new MemorySaveStore();
        using var vm = new MainViewModel(false, 17, showSetup: true, saveStore: store,
            useExpandedContent: true, contentRegistry: registry)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false, ManualDiscardEnabled = false };
        var window = new MainWindow(vm);
        window.ApplyTemplate();
        var root = (FrameworkElement)window.Content;
        try
        {
            vm.OpenLobbyCategoryCommand.Execute("activity");
            vm.SelectedTableMode = vm.VisibleTableModes.Single(option => option.ModeId == "activity:wenhe-2019");
            Layout();
            var goals = (ListBox)window.FindName("WenheAspirationChoices");
            Program.Assert(vm.IsActivityModeSelection && vm.IsWenheActivityModeSelection &&
                !vm.IsTeamModeSelection && !vm.IsIdentityModeSelection && vm.ShowModeSetupText &&
                vm.SelectedTableMode is { PlayerCount: 8, ModeBadge: "五阶生存" } &&
                vm.WenheAspirations.Select(option => option.GoalRank).SequenceEqual([1, 2, 4, 6]) &&
                vm.SelectedWenheAspiration.GoalRank == 1 && goals.Visibility == Visibility.Visible && goals.Items.Count == 4 &&
                vm.TeamModeSetupText.Contains("七名AI对手") && vm.TeamModeSetupText.Contains("起手五张") &&
                vm.TeamModeSetupText.Contains("体力及上限+2") && vm.TeamModeSetupText.Contains("免费志向") &&
                vm.CurrentGuideTitle.Contains("文和乱武") && vm.CurrentGuideBody.Contains("各自求生") &&
                !vm.CurrentGuideBody.Contains("队友"),
                "The actual Wenhe lobby must expose four free private aspirations and eight independent tier-five survivors, without identity or team selection.");
            goals.SelectedItem = vm.WenheAspirations.Single(option => option.GoalRank == 4);
            Layout();
            Program.Assert(vm.SelectedWenheAspiration.GoalRank == 4 && vm.TeamModeSetupText.Contains("前四"),
                "The bound goal selector must update the actual pregame option rather than only its displayed text.");
            Program.Render(root, 1440, 880, Path.Combine(output, "170-wenhe-survival-setup.png"));
            vm.StartNewGameFromLobbyCommand.Execute(null);
            Program.Assert(vm.IdentityRevealTitle.Contains("各 自 求 生") && vm.IdentityRevealRoster.Contains("七名AI"),
                "The real introduction must explain independent opponents rather than inventing a coalition.");
            vm.ContinueFromIdentityRevealCommand.Execute(null);
            Until(() => vm.IsGeneralSelectionPending);
            Program.Assert(vm.GeneralSelectionContextTitle.Contains("独立求生") &&
                vm.GeneralSelectionContextDetail.Contains("七名AI对手") &&
                vm.GeneralChoices.All(choice => choice.HealthText == $"五阶体力上限 {registry.Generals[choice.GeneralId].BaseHp + 2}"),
                "The native private general draft must preview exact tier-five health and seven AI opponents.");
            vm.ToggleGeneralRosterCommand.Execute(null);
            Program.Assert(vm.CustomGeneralChoices.Count > 0 && vm.CustomGeneralChoices.All(choice =>
                registry.Generals[choice.GeneralId].FactionId != "god" &&
                choice.HealthText == $"五阶体力上限 {registry.Generals[choice.GeneralId].BaseHp + 2}") &&
                vm.CustomGeneralChoices.Any(choice => choice.GeneralId == "classic:gan-ning") &&
                vm.CustomGeneralChoices.All(choice => choice.GeneralId is not ("boundary:gan-ning" or "classic:hua-tuo")),
                "The real expanded roster must retain ordinary Gan Ning while applying official bans and five-tier health previews.");
            vm.PreviewGeneralChoiceCommand.Execute(vm.CustomGeneralChoices.Single(choice => choice.GeneralId == "classic:guan-yu"));
            vm.ConfirmGeneralChoiceCommand.Execute(null);
            Until(() => Program.Engine(vm).Events.Any(entry => entry.Payload is SetupCompletedEvent));
            var game = Program.Engine(vm);
            var snapshot = game.CreateSnapshot(0);
            Program.Assert(snapshot.ModeKind == ContentModeKind.Survival && snapshot.Players.Count == 8 &&
                snapshot.Players.Count(player => player.IsHuman) == 1 && snapshot.Players.All(player =>
                    player.TeamId is null && player.Role == Role.Contestant &&
                    player.MaxHp == registry.Generals[player.GeneralId].BaseHp + 2) &&
                snapshot.WenheAspiration is { GoalRank: 4, CanUseChampionMark: false } &&
                vm.Seats.Count == 8 && !vm.Seats.Any(seat => seat.IsTeammate) &&
                vm.Seats.Where(seat => !seat.IsHuman).All(seat => seat.RoleLabel == "对手" && !seat.CanMarkIdentity) &&
                vm.HumanSkillCards.All(skill => skill.ContentId is not ("wenhe:rules" or "wenhe:chaos-resolution" or "wenhe:champion-mark")) &&
                vm.Seats.All(seat => !seat.SkillName.Contains("乱武结算") && !seat.SkillName.Contains("文和乱武规则") &&
                    !seat.SkillName.Contains("志在必得")) &&
                vm.AliveText.Contains("各自求生") && vm.IdentityObjective.Contains("最后") &&
                vm.WenheAspirationText.Contains("前四") && vm.TableArenaTitle == "文 和 乱 武",
                "The live eight-seat table must use real independent contestants, seven unmarkable opponents and only the human's private goal.");
            Until(() => game.Events.Any(entry => entry.Payload is WenheRoundRuleChangedEvent));
            Layout();
            var rule = game.CreateSnapshot(0).WenheRule!;
            var banner = (FrameworkElement)window.FindName("WenheRuleBanner");
            Program.Assert(rule is { RoundNumber: 1, Rule: WenheRuleKind.Chaos } &&
                banner.Visibility == Visibility.Visible && vm.WenheRoundText.Contains("第 1 轮") &&
                vm.WenheCurrentRuleDescription.Contains("最近") && vm.TableModeText.Contains("乱武"),
                "The actual first-round lifecycle event must feed the visible rule banner independently of the ordinary action-turn counter.");
            Program.Render(root, 1440, 880, Path.Combine(output, "171-wenhe-eight-survivors.png"));
            LegacySaveFixtures.Save(vm);
            var saved = SnapshotJson.Serialize(game.CreateSnapshot(0));
            vm.SelectedTableMode = vm.TableModes.Single(option => option.ModeId == "identity:classic-8");
            LegacySaveFixtures.Load(vm);
            Program.Assert(!vm.HasSaveError && Program.Engine(vm).ModeId == "activity:wenhe-2019" &&
                vm.SelectedWenheAspiration.GoalRank == 4 && vm.IsWenheActivityTable &&
                SnapshotJson.Serialize(Program.Engine(vm).CreateSnapshot(0)) == saved,
                "A real manual restore must recover the exact live rule, private goal and survival view after another lobby mode is selected.");
            using var other = new MainViewModel(false, 17, true, store, contentRegistry: ComposedSkillContentRegistry.CreateShowcase())
                { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
            LegacySaveFixtures.Load(other);
            Program.Assert(!other.HasSaveError && other.IsWenheActivityTable && other.Seats.Count == 8 &&
                other.SelectedTableMode.ModeId == "activity:wenhe-2019" && other.SelectedWenheAspiration.GoalRank == 4,
                "Package metadata must recover a survival save even when the new desktop configuration did not opt in to Wenhe.");
            other.StartNewGameFromLobbyCommand.Execute(null);
            other.ContinueFromIdentityRevealCommand.Execute(null);
            for (var step = 0; step < 80 && !other.IsGeneralSelectionPending; step++) other.StepAiCommand.Execute(null);
            Program.Assert(!other.HasSaveError && other.IsGeneralSelectionPending &&
                Program.Engine(other).ModeId == "activity:wenhe-2019" &&
                other.GeneralSelectionContextDetail.Contains("七名AI对手") && other.SelectedWenheAspiration.GoalRank == 4,
                "Rematching a cross-configuration save must preserve the recovered survival registry and the actual selected private goal.");
        }
        finally { window.Content = null; window.DataContext = null; window.Close(); }

        void Layout()
        {
            root.Measure(new Size(1440, 880)); root.Arrange(new Rect(0, 0, 1440, 880)); root.UpdateLayout();
            root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
        }
        void Until(Func<bool> ready)
        {
            for (var step = 0; step < 160; step++)
            {
                if (ready()) return;
                var boundary = Program.Engine(vm).CreateSnapshot(0);
                if (Program.SetupResponseDeclineCommand(vm) is { } decline &&
                    decline.CanExecute(null))
                {
                    decline.Execute(null);
                    continue;
                }
                Program.Assert(vm.StepAiCommand.CanExecute(null),
                    $"The real survival UI must advance to its bounded setup/rule boundary. Status={boundary.Status}; turn={boundary.TurnNumber}; phase={boundary.Phase}; seat={boundary.CurrentSeat}; pending={boundary.PendingDecision?.Kind}/{boundary.PendingDecision?.PlayerSeat}/{boundary.PendingDecision?.PromptId}; prompt={boundary.PendingDecision?.Prompt}; roster={string.Join(";", boundary.Players.Select(player => $"{player.Seat}:{player.GeneralId}"))}.");
                vm.StepAiCommand.Execute(null);
            }
            throw new InvalidOperationException("The Wenhe UI did not reach its bounded setup/rule boundary.");
        }
    }
}
