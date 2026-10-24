using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;

internal static class BoundaryWangJiUiChecks
{
    public static void ActualInspectedForeignHandAndPaidCardButtonCold(string output)
    {
        var scene = BoundaryWangJiScenario.CreateScene(); var game = scene.Game;
        var enemy = BoundaryWangJiScenario.EnemySeat(game);
        BoundaryWangJiScenario.ActivateQizhi(game, enemy); BoundaryWangJiScenario.Play(game);
        BoundaryWangJiScenario.UseCard(game, CardKind.DrawTwo);
        BoundaryWangJiScenario.Reach(game, BoundaryWangJiScenario.Inspecting);
        using var model = ShiFifthBatchUiChecks.Model(game, scene.Registry); game = Program.Engine(model);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.BoundaryWangJi?.Stage == BoundaryWangJiStage.Inspect);
        var ids = root.BoundaryWangJi!.Viewed.ToArray();
        Program.Assert(model.HasPrivatelyViewedCards && ids.Length == game.CreateSnapshot(0).Players[enemy].HandCount &&
                model.PrivatelyViewedCards.Select(item => item.Id).ToHashSet().SetEquals(ids) && model.PrivatelyViewedCards.All(item => item.IsPrivateReveal) &&
                game.CreateSnapshot(Enumerable.Range(1, 3).First(seat => seat != enemy)).PrivateRevealedCards is null,
            "The owner's actual inspection surface renders the recipient's whole current hand with private identities after cold loading the original Use.");
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Program.Render(host.Root, 1120, 740, null);
        var surface = (ItemsControl)host.Window.FindName("PrivatelyViewedCards");
        Program.Assert(surface.IsVisible && surface.ActualHeight > 0 && model.PrivatelyViewedCards.All(face =>
                Program.Find<FrameworkElement>(surface).Any(item => ReferenceEquals(item.DataContext, face) && item.IsVisible && item.ActualWidth > 0)),
            "Every inspected entity is visible on the actual native private-card surface.");
        var choice = model.SkillChoices.First(item => item.Parameters.GetValueOrDefault("branch") == "discard" && item.Cards.Count == 1);
        var selected = choice.Cards.Single();
        var button = Program.Find<Button>(host.Root).Single(item => item.IsVisible && item.IsEnabled &&
            (ReferenceEquals(item.Command, model.DockSkillChoiceCommand) || ReferenceEquals(item.Command, model.SelectSkillChoiceCommand)) &&
            ReferenceEquals(item.CommandParameter, choice));
        host.Capture(output, "boundary-wang-ji-mobile-private-hand-card-button", 1120);
        var before = game.AcceptedCommands.Count;
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
        BoundaryWangJiScenario.Play(game); ShiFifthBatchUiChecks.Refresh(model);
        Program.Assert(game.AcceptedCommands.Skip(before).OfType<AnswerPromptCommand>().Single().Choice == choice.Id &&
                game.CardMovements.Count(item => item.CardId == selected && item.From == CardLocation.Hand(enemy) &&
                    item.To == CardLocation.DiscardPile && item.Reason.Value == "skill-program.boundary:qizhi-mobile.discard") == 1 &&
                !model.HasPrivatelyViewedCards && !game.ResolutionStack.Any(item => item.Id == root.Id),
            "The inspected card's rendered choice submits once, discards that exact entity through the native child and retires its original private inspection.");
    }
}
