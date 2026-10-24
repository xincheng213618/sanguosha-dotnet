using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;
using CardGame.Wpf.Controls;

internal static class BoundarySunLuBanUiChecks
{
    public static void OwnerBranchAndActualForeignHiddenHandPicker(string output)
    {
        var scene = BoundarySunLuBanScenario.CreateScene(); var game = scene.Game;
        var source = BoundarySunLuBanScenario.InvitationSeat(game);
        var target = BoundarySunLuBanScenario.GainPrimaryTarget(game);
        var foreign = game.CreateSnapshot(source).Players.Single(player => player.Seat == source).Hand[0].Id;
        var material = BoundarySunLuBanScenario.UseCard(game, CardKind.Slash, [target]);
        BoundarySunLuBanScenario.BeginZenhui(game);
        BoundarySunLuBanScenario.Reach(game, BoundarySunLuBanScenario.OwnBranch);
        using var model = ShiFifthBatchUiChecks.Model(game, scene.Registry); game = Program.Engine(model);
        var useId = game.ResolutionStack.OfType<CardUseFrame>().Single(frame =>
            frame.Action?.PhysicalCards.Any(cost => cost.CardId == material) == true).Id;
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Program.Render(host.Root, 1120, 740, null);
        var branch = model.DockSkillChoices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "become-user");
        var before = game.AcceptedCommands.Count;
        var button = Program.Find<Button>(host.Root).Single(item => item.IsVisible && item.IsEnabled &&
            ReferenceEquals(item.Command, model.DockSkillChoiceCommand) && ReferenceEquals(item.CommandParameter, branch));
        Program.Assert(game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, IsPrivate: true } &&
                game.CreateSnapshot(source).PendingDecision is null,
            "Sun Lu Ban's actual local dock owns the branch choice; the selected foreign player receives no choice.");
        Click(button);
        Program.Assert(game.AcceptedCommands.Count == before + 1 && game.AcceptedCommands.Last() is AnswerPromptCommand answer && answer.Choice == branch.Id,
            "The visible owner's branch button submits the exact published obtain-and-replace option once.");
        BoundarySunLuBanScenario.Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        ShiFifthBatchUiChecks.Refresh(model); Program.Render(host.Root, 1120, 740, null);
        var slots = model.SkillChoices.Where(choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand)).ToArray();
        Program.Assert(slots.Length == 2 && slots.All(choice => choice.Cards.Count == 0 &&
                choice.Parameters.GetValueOrDefault("card-owner-seat") == source.ToString() && !choice.Parameters.ContainsKey("card-id")) &&
                game.CreateSnapshot(0).Players.Single(player => player.Seat == source).Hand.Count == 0,
            "The owner can choose the foreign hand through real opaque slots while its identities stay hidden in the viewer snapshot and choices.");
        var slot = slots.Single(choice => choice.Parameters.GetValueOrDefault("slot-index") == "0");
        var picker = Program.Find<TargetCardPicker>(host.Root).Single(item => item.IsVisible && item.ActualHeight > 0);
        var cardButton = Program.Find<Button>(picker).Single(item => item.IsVisible && item.IsEnabled &&
            ReferenceEquals(item.Command, model.SelectSkillChoiceCommand) && ReferenceEquals(item.CommandParameter, slot));
        host.Capture(output, "boundary-sun-lu-ban-mobile-owner-hidden-hand", 1120);
        before = game.AcceptedCommands.Count; Click(cardButton); BoundarySunLuBanScenario.Play(game);
        Program.Assert(game.AcceptedCommands.Skip(before).OfType<AnswerPromptCommand>().Single().Choice == slot.Id &&
                game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == foreign) &&
                game.CardMovements.Count(move => move.CardId == foreign && move.From == CardLocation.Hand(source) && move.To == CardLocation.Hand(0)) == 1 &&
                game.Events.Select(item => item.Payload).OfType<ProgramCardUseActorReplacedEvent>().Any(item =>
                    item.SkillId == BoundarySunLuBanScenario.Zenhui && item.CardUseFrameId == useId && item.PreviousActorSeat == 0 && item.ActorSeat == source && item.ProviderSeat == 0) &&
                game.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Any(item =>
                    item.SourceSeat == source && item.TargetSeat == target && item.SourceCardUseResolutionId == useId && item.Amount == 1) &&
                game.CardMovements.Count(move => move.CardId == material && move.To == CardLocation.DiscardPile) == 1,
            "The real opaque card button obtains its exact physical entity once and returns the original material to the native replacement user's use.");
    }

    private static void Click(Button button)
    {
        Program.Assert(button.IsLoaded && button.IsVisible && button.IsEnabled && button.ActualWidth > 0 && button.ActualHeight > 0,
            "The actual Sun Lu Ban button must be rendered and enabled before clicking.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }
}
