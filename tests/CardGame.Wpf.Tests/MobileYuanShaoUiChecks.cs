using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using CardGame.Core;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class MobileYuanShaoUiChecks
{
    public static void EffectiveSuitPairEnablesNativeConfirmation()
    {
        var scene = MobileYuanShaoChecks.Scene.Start(); scene.Play();
        var game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(scene.G.CreateCheckpoint())), scene.Registry);
        using var model = new MainViewModel(false, game.Seed, true, new MemorySaveStore(), contentRegistry: scene.Registry,
            historyStore: new MemoryMatchHistoryStore(), preferencesStore: new MemoryPlayerPreferencesStore())
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        typeof(MainViewModel).GetMethod("ReplaceEngine", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, [game]);
        model.IsNewGameSetupOpen = false;
        var action = model.HumanActiveSkillActions.Single(a => a.ProgramSkillId == MobileYuanShaoChecks.Skill);
        var pair = game.CreateSnapshot(0).Players[0].Hand.Where(c => c.Rank is 2 or 3).Select(c => c.Id).ToArray();
        var weapon = game.CreateSnapshot(0).Players[0].Equipment.Single().Id;
        var commands = game.AcceptedCommands.Count; var moves = game.CardMovements.Count;
        var window = new MainWindow(model); var root = (FrameworkElement)window.Content;
        try
        {
            model.SelectActiveSkillCommand.Execute(action);
            model.SelectCardCommand.Execute(model.Hand.Single(c => c.Id == pair[0]));
            Layout();
            var confirm = (Button)window.FindName("PlayCardButton");
            Program.Assert(model.IsActiveSkillSelectionPending && !model.CanConfirmActiveSkill && !confirm.IsEnabled,
                "One actual material leaves the real ordinary Luanji confirmation disabled.");
            model.SelectCardCommand.Execute(model.Hand.Single(c => c.Id == pair[1]));
            Layout();
            Program.Assert(pair.Select(id => game.CreateSnapshot(0).Players[0].Hand.Single(c => c.Id == id).Suit).Distinct().Count() == 2 &&
                action.SelectableCardSuitOverrides is { } suits && pair.All(id => suits[id] == Suit.Heart) &&
                !action.SelectableCardIds.Contains(weapon) && model.Hand.Count(c => c.IsSelected) == 2 &&
                model.CanConfirmActiveSkill && model.CanConfirmSelected && confirm.IsEnabled &&
                confirm.Command!.CanExecute(confirm.CommandParameter) && game.AcceptedCommands.Count == commands &&
                game.CardMovements.Count == moves,
                "The real button accepts two printed-different owner Hand faces sharing published effective Heart, while the draft performs no early payment.");
            typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(confirm, null);
            Program.Assert(game.AcceptedCommands.Count == commands + 1 && game.AcceptedCommands.Last() is UseProgramSkillCommand paid &&
                paid.SkillId == MobileYuanShaoChecks.Skill && paid.CardIds.Order().SequenceEqual(pair.Order()) &&
                pair.All(id => game.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1) &&
                game.CreateSnapshot(0).Players[0].Equipment.Any(c => c.Id == weapon) &&
                game.Events.Select(e => e.Payload).OfType<CardUseDeclaredEvent>().Count(e => e.SourceSeat == 0 && e.CardKind == CardKind.ArrowBarrage) == 1,
                "Clicking the bound confirmation submits the exact native two-material Arrow once and preserves the equipped original entity.");
        }
        finally { window.Content = null; window.Close(); }

        void Layout()
        {
            root.Measure(new Size(1120, 740)); root.Arrange(new Rect(0, 0, 1120, 740)); root.UpdateLayout();
            root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind); root.UpdateLayout();
        }
    }
}
