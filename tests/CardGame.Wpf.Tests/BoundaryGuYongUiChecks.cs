using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;

internal static class BoundaryGuYongUiChecks
{
    public static void MixedColorSameTypeRevealAndNativeSelfRecipientDraft(string output)
    {
        var scene = BoundaryGuYongScenario.CreateScene(atBingyi: true);
        var game = scene.Game;
        BoundaryGuYongScenario.Reach(game, BoundaryGuYongScenario.SelectingRecipients);
        using var model = ShiFifthBatchUiChecks.Model(game, scene.Registry);
        game = Program.Engine(model);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Program.Render(host.Root, 1120, 740, null);
        var revealed = game.Events.Select(item => item.Payload).OfType<ProgramCardsRevealedEvent>()
            .Single(item => item.SkillId == BoundaryGuYongScenario.Bingyi).Cards.Select(card => card.Id).Order().ToArray();
        var hands = game.CreateSnapshot(0).Players.Select(player => player.HandCount).ToArray();
        Program.Assert(model.IsSkillTargetSelectionPending && model.HasPublicRevealedCards && revealed.Length == 2 &&
                model.PublicRevealedCards.Select(card => card.Id).Order().SequenceEqual(revealed) &&
                game.CreateSnapshot(2).PublicRevealedCards.Select(card => card.Id).Order().SequenceEqual(revealed),
            "The restored mixed-color same-type Bingyi visibly exposes the complete real hand and its recipient choice to the owner, with the same public reveal for other viewers.");
        var choice = model.SkillChoices.Single(item => item.Targets.SequenceEqual([0, 2]));
        var accepted = game.AcceptedCommands.Count;
        foreach (var seat in new[] { 0, 2 })
        {
            var target = model.Seats.Single(player => player.Seat == seat);
            var button = Program.Find<Button>(host.Root).Single(item => item.IsVisible &&
                ReferenceEquals(item.Command, model.SelectTargetCommand) && ReferenceEquals(item.CommandParameter, target));
            Program.Assert(target.IsLegalTarget && button.IsEnabled, "Bingyi offers an actual selectable portrait for self and the other recipient.");
            if (!target.IsSelectedTarget) Click(button);
        }
        Program.Assert(model.Seats.Where(seat => seat.IsSelectedTarget).Select(seat => seat.Seat).Order().SequenceEqual([0, 2]) &&
                model.CanConfirmSelected && game.AcceptedCommands.Count == accepted,
            "The exact two-recipient choice, including self, remains an editable unpaid native draft.");
        host.Capture(output, "boundary-gu-yong-mobile-public-reveal-self-recipients", 1120);
        Program.Assert(model.PublicRevealedCards.All(card => Program.Find<ContentPresenter>(host.Root).Any(item =>
                item.IsVisible && item.ActualWidth > 0 && item.ActualHeight > 0 && ReferenceEquals(item.DataContext, card))),
            "The loaded native tree visibly renders each actual public reveal object while selecting recipients.");
        var confirm = (Button)host.Window.FindName("PlayCardButton");
        Program.Assert(ReferenceEquals(confirm.Command, model.ConfirmSelectedCommand) && confirm.Command!.CanExecute(null),
            "The visible primary action confirms the actual committed recipient set.");
        Click(confirm);
        BoundaryGuYongScenario.Reach(game, _ => !game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == BoundaryGuYongScenario.Bingyi));
        Program.Assert(game.AcceptedCommands.Skip(accepted).OfType<AnswerPromptCommand>().Count() == 1 &&
                game.AcceptedCommands.Skip(accepted).OfType<AnswerPromptCommand>().Single().Choice == choice.Id &&
                game.CreateSnapshot(0).Players.Select(player => player.HandCount)
                    .SequenceEqual(hands.Select((count, seat) => count + (seat is 0 or 2 ? 1 : 0))),
            "Native confirmation submits the published choice once and draws once for exactly the two frozen recipients.");
    }

    private static void Click(Button button)
    {
        Program.Assert(button.IsLoaded && button.IsVisible && button.IsEnabled && button.ActualWidth > 0 && button.ActualHeight > 0,
            "The actual Bingyi button must be rendered and enabled before clicking.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }
}
