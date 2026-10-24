using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CardGame.Core;
using CardGame.Wpf.ViewModels;
using static BoundaryZhouFeiScenario;

internal static class BoundaryZhouFeiUiChecks
{
    public static void ForeignOpaqueStoreOriginalEndingUseAndRemainderCold(string output)
    {
        var scene = CreateScene();
        using var model = ShiFifthBatchUiChecks.Model(scene.Game, scene.Registry);
        var game = Program.Engine(model);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        Click(model, host, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(game, prompt => Branch(prompt, "confirm-store"));
        ShiFifthBatchUiChecks.Refresh(model);
        var store = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.BoundaryZhouFei?.Stage == BoundaryZhouFeiStage.Store);
        var holder = store.BoundaryZhouFei!.Holder;
        var slots = model.SkillChoices.Where(choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand)).ToArray();
        Program.Assert(holder != 0 && slots.Length == 2 && slots.All(choice => choice.Cards.Count == 0 && !choice.Parameters.ContainsKey("card-id")) &&
                game.CreateSnapshot(0).Players[holder].Hand.Count == 0 && !model.HasPrivatelyViewedCards,
            "The real foreign-hand storage prompt offers two opaque slots and keeps their identities hidden from the caster.");
        for (var index = 0; index < 2; index++)
        {
            var slot = index.ToString();
            Click(model, host, choice => choice.Parameters.GetValueOrDefault("branch") == "toggle" &&
                choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand) && choice.Parameters.GetValueOrDefault("slot-index") == slot);
        }
        Program.Assert(!game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiStoredEvent>().Any() &&
                game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == store.Id).BoundaryZhouFei!.Selected.Count == 2,
            "Both actual slot buttons prepare an unpaid two-card batch before the real confirmation.");
        host.Capture(output, "boundary-zhou-fei-mobile-opaque-storage-draft", 1120);
        Click(model, host, choice => choice.Parameters.GetValueOrDefault("branch") == "confirm-store");
        Reach(game, prompt => Optional(prompt, Liangyin));
        var deposit = game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiStoredEvent>().Single();
        var ids = game.CardMovements.Where(move => move.To == deposit.Location).Select(move => move.CardId).ToArray();
        Program.Assert(deposit.Holder == holder && ids.Length == 2 && deposit.Location.Zone == CardZoneKind.PrivatePersistentPile &&
                game.CreateSnapshot(0).PrivateRevealedCards is null,
            "Confirmation stores exactly those original entities on the actual turn holder without revealing their identities.");
        DrawLiangyin(game, holder); Reach(game, prompt => Optional(prompt, Kongsheng)); Skip(game);
        Reach(game, SourceChoice); Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "keep");
        Reach(game, prompt => Branch(prompt, "return"));
        ShiFifthBatchUiChecks.Refresh(model);
        model.SaveGameCommand.Execute(null); model.LoadManualGameCommand.Execute(null); game = Program.Engine(model);
        var ending = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.BoundaryZhouFei?.Stage == BoundaryZhouFeiStage.UseChoice);
        Program.Assert(!model.HasSaveError && ending.BoundaryZhouFei!.Deposits.SequenceEqual([deposit.FrameId]) &&
                model.PrivatelyViewedCards.Select(card => card.Id).ToHashSet().SetEquals(ids) && model.PrivatelyViewedCards.All(card => card.IsPrivateReveal) &&
                game.CreateSnapshot(Enumerable.Range(1, 3).First(seat => seat != holder)).PrivateRevealedCards is null,
            "The actual save/load UI restores the original ending choice and locally reveals its two concealed originals only to their authorized caster.");
        var face = model.PrivatelyViewedCards.Single(card => card.Kind == CardKind.DrawTwo);
        var useChoice = model.SkillChoices.Single(choice => choice.Parameters.GetValueOrDefault("branch") == "use" && choice.Cards.SequenceEqual([face.Id]));
        host.Capture(output, "boundary-zhou-fei-mobile-ending-original-card-cold", 1120);
        Click(model, host, choice => choice.Id == useChoice.Id);
        Reach(game, prompt => Optional(prompt, Liangyin)); Skip(game);
        Reach(game, _ => !game.ResolutionStack.Any(frame => frame.Id == ending.Id));
        ShiFifthBatchUiChecks.Refresh(model);
        var issued = game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiNativeEvent>().Single();
        var remainder = ids.Single(id => id != face.Id);
        Program.Assert(issued.CardId == face.Id && issued.DepositId == deposit.FrameId &&
                game.Events.Select(record => record.Payload).OfType<CardUseFinishedEvent>().Count(item => item.ResolutionId == issued.UseId) == 1 &&
                game.CardMovements.Count(move => move.CardId == face.Id && move.From == deposit.Location && move.To == CardLocation.Processing) == 1 &&
                game.CardMovements.Count(move => move.CardId == remainder && move.From == deposit.Location && move.To == CardLocation.Hand(holder)) == 1 &&
                !model.HasPrivatelyViewedCards,
            "The real use button pays one original entity through its native Use once and returns the untouched remainder to the original holder.");
    }

    private static void Click(MainViewModel model, NativeResponseStatusChecks.NativeHost host, Func<PromptChoice, bool> predicate)
    {
        ShiFifthBatchUiChecks.Refresh(model); Program.Render(host.Root, 1120, 740, null);
        var choice = model.SkillChoices.Single(predicate);
        var button = Program.Find<Button>(host.Root).Single(item => item.IsVisible && item.IsEnabled &&
            (ReferenceEquals(item.Command, model.SelectSkillChoiceCommand) || ReferenceEquals(item.Command, model.DockSkillChoiceCommand)) &&
            ReferenceEquals(item.CommandParameter, choice));
        Program.Assert(button.IsLoaded && button.ActualWidth > 0 && button.ActualHeight > 0,
            "The actual Zhou Fei choice must be rendered and executable before clicking.");
        typeof(ButtonBase).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(button, null);
    }
}
