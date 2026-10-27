using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;
using static Program;

internal static class CardExchangeUiChecks
{
    public static void FixedExchangeDiscardAndCancellation(string output)
    {
        var scene = Start();
        using (var model = Restore(scene.G, scene.Registry))
        using (var host = new NativeResponseStatusChecks.NativeHost(model))
        {
            var game = Engine(model);
            var top = model.ExchangeTopCards.Select(card => card.Id).ToArray();
            var hand = model.ExchangeHandCards.Select(card => card.Id).ToArray();
            Assert(model.IsCardExchangeViewPending && top.Length == 5 && hand.Length == 4 && model.CanCancelAction &&
                model.CanConfirmSelected && model.CenterSkillChoices.Count == 0 && !model.HasPrivatelyViewedCards,
                "Guanxu must show five ordered top cards above the complete target hand, with cancel and no raw pair buttons.");
            var pair = game.CreateSnapshot(0).PendingDecision!.Choices.First(choice => choice.Cards.Count == 2 &&
                choice.Cards[1] == top[2] && game.CreateCardZoneDiagnostics().Single(card => card.CardId == choice.Cards[0]).CardKind == CardKind.Duel);
            var commands = game.AcceptedCommands.Count; var movements = game.CardMovements.Count;
            ClickCard(pair.Cards[0]);
            Assert(!model.CanConfirmSelected, "A half-selected exchange cannot commit.");
            ClickCard(pair.Cards[1]);
            Assert(model.CanConfirmSelected && model.ExchangeTopCards.Select(card => card.Id).SequenceEqual(top.Select(id => id == pair.Cards[1] ? pair.Cards[0] : id)) &&
                model.ExchangeHandCards.Select(card => card.Id).SequenceEqual(hand.Select(id => id == pair.Cards[0] ? pair.Cards[1] : id)) &&
                game.AcceptedCommands.Count == commands && game.CardMovements.Count == movements,
                "The unpaid preview swaps only the two original slots without reordering or moving any engine card.");
            host.Capture(output, "guanxu-exchange-preview", 1120);
            host.Capture(output, "guanxu-exchange-preview", 1920);
            Click((Button)host.Window.FindName("PlayCardButton"));
            Reach(model, scene.Registry, () => Engine(model).CreateSnapshot(0).PendingDecision?.Choices.FirstOrDefault()?.Parameters.GetValueOrDefault("option") == "guanxu-discard");
            Assert(model.IsCardExchangeDiscardPending && model.ExchangeTopCards.Count == 0 && model.ExchangeHandCards.Count == hand.Length &&
                !model.CanConfirmSelected && !model.CanCancelAction && Enumerable.Range(1, 3).All(seat => game.CreateSnapshot(seat).PrivateRevealedCards is null or { Count: 0 }),
                "A confirmed exchange must enter an independent mandatory private three-card discard.");
            // Restore at the second stage before making any discard selection.
            using var restored = Restore(game, scene.Registry);
            Assert(restored.IsCardExchangeDiscardPending && restored.ExchangeHandCards.Select(card => card.Id).SequenceEqual(model.ExchangeHandCards.Select(card => card.Id)),
                "Cold loading must retain the complete post-exchange hand and its second-stage prompt.");
            var triple = game.CreateSnapshot(0).PendingDecision!.Choices.First().Cards.ToArray();
            ClickCard(triple[0]); ClickCard(triple[1]);
            Assert(!model.CanConfirmSelected && game.CardMovements.Count == movements + 2,
                "Two selected discards remain unpaid and cannot satisfy the required three-card set.");
            ClickCard(triple[2]);
            Assert(model.CanConfirmSelected, "The exact published same-suit triple must become confirmable.");
            host.Capture(output, "guanxu-discard-selection", 1120);
            Click((Button)host.Window.FindName("PlayCardButton"));
            Reach(model, scene.Registry, () => Engine(model).CreateSnapshot(0).PendingDecision?.Kind == DecisionKind.PlayCard);
            Assert(!model.IsCardExchangePending && game.CreateSnapshot(2).Players[2].HandCount == hand.Length - 3,
                "Confirming the second stage discards exactly three target hand entities once.");
            AssertNoBindingErrors();

            void ClickCard(int id)
            {
                Render(host.Root, 1120, 740, null);
                var picker = (FrameworkElement)host.Window.FindName("CardExchangeSelection");
                var button = Find<Button>(picker).Single(button => button.Command == model.SelectCardExchangeCardCommand &&
                    button.CommandParameter is CardViewModel card && card.Id == id);
                Assert(button.IsEnabled, "The published exchange/discard card must be selectable."); Click(button);
            }
        }

        scene = Start();
        using var canceled = Restore(scene.G, scene.Registry);
        var canceledGame = Engine(canceled); var before = canceledGame.CardMovements.Count;
        canceled.SelectCardExchangeCardCommand.Execute(canceled.ExchangeTopCards[0]);
        canceled.SelectCardExchangeCardCommand.Execute(canceled.ExchangeHandCards[0]);
        canceled.CancelActionCommand.Execute(null);
        Reach(canceled, scene.Registry, () => canceledGame.CreateSnapshot(0).PendingDecision?.Kind == DecisionKind.PlayCard);
        Assert(!canceled.IsCardExchangePending && canceledGame.CardMovements.Count == before && canceledGame.CreateSnapshot(2).Players[2].HandCount == 4,
            "Canceling even a complete preview must make no exchange and skip the second stage despite an existing same-suit triple.");
    }

    private static WushuangOfficialSkillsAChecks.Scene Start()
    {
        var scene = WushuangOfficialSkillsAChecks.Scene.Start([3282]);
        scene.Activate(3282, "inspect-and-exchange"); scene.Reach(() => scene.Stage == "begin");
        scene.Answer(choice => choice.Parameters.GetValueOrDefault("option") == "guanxu-target" && choice.Targets.Single() == 2);
        scene.Reach(() => scene.Stage == "guanxu-view"); return scene;
    }
    private static MainViewModel Restore(GameEngine game, ContentRegistry registry)
    {
        var store = new MemorySaveStore();
        store.Write(GameSaveSlot.Manual, new(GameSaveFile.CurrentFormatVersion, DateTimeOffset.UtcNow, false, game.CreateCheckpoint()));
        var model = new MainViewModel(false, 31, true, store, contentRegistry: registry)
            { IsMotionEnabled = false, IsSoundEnabled = false, IsMusicEnabled = false };
        LegacySaveFixtures.Load(model); Assert(!model.HasSaveError, model.SaveStatus); return model;
    }
    private static void Reach(MainViewModel model, ContentRegistry registry, Func<bool> ready)
    {
        new WushuangOfficialSkillsAChecks.Scene(Engine(model), registry).Reach(ready);
        typeof(MainViewModel).GetMethod("RefreshCurrentView", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(model, null);
    }
    private static void Click(Button button) => typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
}
