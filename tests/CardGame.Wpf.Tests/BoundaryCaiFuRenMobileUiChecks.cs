using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;

internal static class BoundaryCaiFuRenMobileUiChecks
{
    public static void ActualPrivateTwoCardViewAndPaidChoiceCold(string output)
    {
        var scene = BoundaryCaiFuRenScenario.CreateScene(singleHuman: true);
        var game = scene.Game;
        BoundaryCaiFuRenScenario.BeginListening(game);
        BoundaryCaiFuRenScenario.Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "hand");
        using var model = ShiFifthBatchUiChecks.Model(game, scene.Registry);
        game = Program.Engine(model);
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item =>
            item.SkillId == BoundaryCaiFuRenScenario.Listen && item.OrdinaryQieting?.Stage == OrdinaryQietingStage.Viewing);
        var receipt = frame.OrdinaryQieting!;
        var viewed = receipt.MobileSelection!.Viewed.ToArray();
        var source = receipt.Qualification.TurnOwnerSeat;
        var observer = Enumerable.Range(1, 3).First(seat => seat != source);
        Program.Assert(viewed.Length == 2 && model.HasPrivatelyViewedCards &&
                model.PrivatelyViewedCards.Select(card => card.Id).ToHashSet().SetEquals(viewed) &&
                model.PrivatelyViewedCards.All(card => card.IsPrivateReveal) &&
                game.CreateSnapshot(observer).PrivateRevealedCards is null &&
                game.CreateSnapshot(0).Players[source].Hand.Count == 0,
            "Cold loading Qieting renders only its two privately viewed foreign entities and keeps the remaining hand hidden.");
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Program.Render(host.Root, 1120, 740, null);
        var surface = (ItemsControl)host.Window.FindName("PrivatelyViewedCards");
        Program.Assert(surface.IsVisible && surface.ActualHeight > 0 && model.PrivatelyViewedCards.All(face =>
                Program.Find<FrameworkElement>(surface).Any(item => ReferenceEquals(item.DataContext, face) &&
                    item.IsVisible && item.ActualWidth > 0)),
            "The actual private-card surface shows both inspected entities.");
        var choice = model.SkillChoices.First(item => item.Parameters.GetValueOrDefault("option") == "take-hand");
        var selected = choice.Cards.Single();
        var button = Program.Find<Button>(host.Root).Single(item => item.IsVisible && item.IsEnabled &&
            (ReferenceEquals(item.Command, model.DockSkillChoiceCommand) || ReferenceEquals(item.Command, model.SelectSkillChoiceCommand)) &&
            ReferenceEquals(item.CommandParameter, choice));
        host.Capture(output, "boundary-cai-fu-ren-mobile-private-hand-choice", 1120);
        var before = game.AcceptedCommands.Count;
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        BoundaryCaiFuRenScenario.Reach(game, prompt => prompt?.SkillPrompt?.SkillId == BoundaryCaiFuRenScenario.Watch);
        Program.Assert(game.AcceptedCommands.Skip(before).OfType<AnswerPromptCommand>().Single().Choice == choice.Id &&
                game.CardMovements.Count(move => move.CardId == selected && move.From == CardLocation.Hand(source) &&
                    move.To == CardLocation.Hand(0) && move.Reason.Value == BoundaryCaiFuRenScenario.HandReason) == 1,
            "The real visible button submits its exact choice once and gains that viewed entity through the original movement child.");
        BoundaryCaiFuRenScenario.Continue(game);
        BoundaryCaiFuRenScenario.Reach(game, _ => !game.ResolutionStack.Any(item => item.Id == frame.Id));
        ShiFifthBatchUiChecks.Refresh(model);
        Program.Assert(!model.HasPrivatelyViewedCards,
            "Returning from the paid child retires the original private view.");
    }
}
