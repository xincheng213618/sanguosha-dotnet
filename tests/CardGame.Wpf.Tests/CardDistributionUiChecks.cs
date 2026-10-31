using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Tests;
using CardGame.Wpf;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class CardDistributionUiChecks
{
    public static void TuoyuMultiSelectRegionsConfirmAndRestore(string output)
    {
        var (game, registry) = ShenDengAiChecks.CreateAllocationTableFixture();
        var store = new MemorySaveStore();
        Save();
        using var model = new MainViewModel(false, game.Seed, true, store, contentRegistry: registry) { IsMotionEnabled = false };
        LegacySaveFixtures.Load(model); game = Program.Engine(model);
        var window = new MainWindow(model);
        try
        {
            var root = (FrameworkElement)window.Content;
            Program.Assert(model.IsCardDistributionPending && !model.IsCardPaymentSelectionPending && !model.IsHandResponsePending &&
                model.DistributionCards.Count == model.Hand.Count && model.DistributionDestinations.Count == 3,
                "The actual allocation shows each hand entity once and three named domains, outside the payment UI.");
            Program.Assert(model.DistributionDestinations.Any(t => t.Label.Contains("无次数与距离限制")) &&
                model.DistributionDestinations.Any(t => t.Label.Contains("伤害与回复值 +1")) &&
                model.DistributionDestinations.Any(t => t.Label.Contains("无法被响应")), "All domain effects must be visible before selecting a card.");
            var staleCard = model.DistributionCards[^1];
            var staleTarget = model.DistributionDestinations[1];
            var ids = model.Hand.TakeLast(2).Select(card => card.Id).ToArray();
            var before = game.Events.Count(e => e.Payload is ShenDengAiAssignedEvent);
            var movements = game.CardMovements.Count;
            foreach (var id in ids) model.SelectCardCommand.Execute(model.Hand.Single(card => card.Id == id));
            model.SelectDistributionTargetCommand.Execute(model.DistributionDestinations.Single(t => t.Key == "domain:2"));
            Program.Assert(model.CanConfirmSelected && model.Hand.Count(card => card.IsSelected) == 2 &&
                model.DistributionCards.Count(card => card.IsSelected) == 2 && game.Events.Count(e => e.Payload is ShenDengAiAssignedEvent) == before,
                "Multi-select and choosing a region only draft the assignment until explicit confirmation.");
            model.SelectDistributionCardCommand.Execute(model.DistributionCards.Single(card => card.Id == ids[0]));
            Program.Assert(model.Hand.Count(card => card.IsSelected) == 1, "Click-again cancellation stays synchronized with the real hand.");
            model.SelectCardCommand.Execute(model.Hand.Single(card => card.Id == ids[0]));
            Program.Render(root, 1120, 740, Path.Combine(output, "tuoyu-batch-selected.png"));
            Program.Assert(((ItemsControl)window.FindName("DistributionCardChoices")).Visibility == Visibility.Collapsed &&
                Program.Find<Button>((ItemsControl)window.FindName("DistributionTargetChoices")).Count() == 3,
                "The rendered picker uses the real hand without duplicate faces and exposes one named button per region.");
            model.ConfirmSelectedCommand.Execute(null);
            Program.Assert(game.Events.Select(e => e.Payload).OfType<ShenDengAiAssignedEvent>().Skip(before).Select(e => e.CardId).ToHashSet().SetEquals(ids) &&
                game.CreateSnapshot(0).Players[0].Hand.Where(card => ids.Contains(card.Id)).All(card => card.SkillMarkLabel == "丰田") &&
                game.CardMovements.Count == movements && !model.CanConfirmSelected,
                "One confirmation assigns exactly the selected cards without physical movement or unintended repeats.");
            Save(); LegacySaveFixtures.Load(model); game = Program.Engine(model);
            model.SelectDistributionCardCommand.Execute(staleCard); model.SelectDistributionTargetCommand.Execute(staleTarget);
            Program.Assert(model.Hand.All(card => !card.IsSelected) && !model.CanConfirmSelected &&
                model.DistributionDestinations.Single(t => t.Key == "domain:2").Label.Contains("2 张"),
                "Restore retains the committed region counts, clears the draft, and rejects retired card and region controls.");
            model.SelectAllDistributionCardsCommand.Execute(null);
            Program.Assert(model.DistributionCards.All(card => card.IsSelected), "Select-all includes every remaining legal candidate.");
            model.ClearSelectionCommand.Execute(null);
            Program.Assert(model.DistributionCards.All(card => !card.IsSelected) && model.Hand.All(card => !card.IsSelected),
                "Esc clears both selection surfaces without ending mandatory allocation.");
            Program.Render(root, 1600, 1000, Path.Combine(output, "tuoyu-batch-restored.png"));
            GameReplay.Restore(game.CreateCheckpoint(), registry);
        }
        finally { window.Content = null; window.Close(); }
        void Save() => store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
    }
}
