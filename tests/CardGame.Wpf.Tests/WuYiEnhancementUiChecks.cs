using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class WuYiEnhancementUiChecks
{
    public static void SelectAboveConfirmBelow(string output)
    {
        var (game, registry) = WuYiEnhancementScenario.Start();
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        using var model = new MainViewModel(false, game.Seed, true, store, contentRegistry: registry)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        LegacySaveFixtures.Load(model);
        Program.Assert(!model.HasSaveError, model.SaveStatus);
        game = Program.Engine(model);
        var window = new MainWindow(model);
        var root = (FrameworkElement)window.Content;
        try
        {
            Program.Render(root, 1120, 740, Path.Combine(output, "benxi-options-1120.png"));
            Program.Assert(model.IsCardEnhancementSelectionPending && model.CardEnhancementOptions.Count == 4 &&
                model.SelectionArea.ShowConfirm && model.SelectionArea.ShowPrimaryButtons && !model.HasDockSkillChoices &&
                model.CenterSkillChoices.Count == 0 && !model.HasExpandedSelection,
                "Benxi must offer four staged options and an independent bottom confirmation with no duplicate immediate choices.");
            var revision = game.Revision;
            var commands = game.AcceptedCommands.Count;
            var stale = model.CardEnhancementOptions[0];
            Click(CurrentCardEnhancement.IgnoreArmor);
            Click(CurrentCardEnhancement.Uncancelable);
            Program.Assert(model.CardEnhancementOptions.Count(option => option.IsSelected) == 2 && game.Revision == revision &&
                game.AcceptedCommands.Count == commands && !game.Events.Any(entry => entry.Payload is CurrentCardEnhancedEvent),
                "Selecting two effects only changes the visible draft before confirmation.");
            var third = model.CardEnhancementOptions.Single(option => option.Enhancement == CurrentCardEnhancement.DrawAfterDamage);
            Program.Assert(!model.ToggleCardEnhancementCommand.CanExecute(third), "A third effect is unavailable while both slots are filled.");
            model.ToggleCardEnhancementCommand.Execute(third);
            Program.Assert(model.CardEnhancementOptions.Count(option => option.IsSelected) == 2, "Direct invocation cannot exceed the published selection budget.");
            Click(CurrentCardEnhancement.IgnoreArmor);
            Program.Assert(model.CardEnhancementOptions.Count(option => option.IsSelected) == 1, "Clicking a selected effect unchecks it.");
            model.CancelActionCommand.Execute(null);
            Program.Assert(model.CardEnhancementOptions.All(option => !option.IsSelected) && game.Revision == revision,
                "The lower cancel control clears only the uncommitted selection.");
            Click(CurrentCardEnhancement.IgnoreArmor);
            Click(CurrentCardEnhancement.Uncancelable);
            AssertLayout(1120, 740);
            AssertLayout(1600, 900);
            var confirm = (Button)window.FindName("PlayCardButton");
            ClickButton(confirm);
            Program.Assert(game.Revision == revision + 1 && game.AcceptedCommands.Count == commands + 1 &&
                game.AcceptedCommands.Last() is AnswerPromptCommand &&
                game.Events.Count(entry => entry.Payload is CurrentCardEnhancedEvent) == 2 && !model.IsCardEnhancementSelectionPending,
                "The actual lower confirm button commits both published effects once in one native command.");
            model.ToggleCardEnhancementCommand.Execute(stale);
            Program.Assert(game.Revision == revision + 1, "A retired option button cannot change the following prompt.");

            void Click(CurrentCardEnhancement flag)
            {
                var option = model.CardEnhancementOptions.Single(item => item.Enhancement == flag);
                var panel = (ItemsControl)window.FindName("CardEnhancementOptions");
                ClickButton(Program.Find<Button>(panel).Single(button => ReferenceEquals(button.CommandParameter, option)));
                root.UpdateLayout();
            }
            void AssertLayout(int width, int height)
            {
                Program.Render(root, width, height, Path.Combine(output, $"benxi-selected-{width}.png"));
                var panel = (ItemsControl)window.FindName("CardEnhancementOptions");
                var confirmButton = (Button)window.FindName("PlayCardButton");
                var buttons = Program.Find<Button>(panel).ToArray();
                var lowerTop = confirmButton.TranslatePoint(new Point(), root).Y;
                Program.Assert(buttons.Length == 4 && buttons.All(button => button.ActualWidth >= 70 && button.ActualHeight >= 28 &&
                    button.TranslatePoint(new Point(0, button.ActualHeight), root).Y < lowerTop) &&
                    confirmButton.Visibility == Visibility.Visible && confirmButton.IsEnabled && model.PlayButtonText == "确 定",
                    "All four readable effect options sit above the separate enabled confirmation at both supported sizes.");
            }
        }
        finally { window.Content = null; window.Close(); }
    }

    private static void ClickButton(Button button) => typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
}
