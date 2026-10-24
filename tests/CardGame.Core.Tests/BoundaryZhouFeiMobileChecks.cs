using System.Text.Json;
using CardGame.Core;
using static BoundaryZhouFeiScenario;

internal static class BoundaryZhouFeiMobileChecks
{
    public static void LiangyinGlobalPileBothDirectionsAndUniqueRoundOutcomes()
    {
        foreach (var recover in new[] { true, false })
        {
            var scene = CreateScene(foreign: false); var game = scene.Game;
            if (recover) { UseControl(game, "wound"); Reach(game, prompt => Optional(prompt, Kongsheng)); Skip(game); Play(game); }
            foreach (var recipient in recover ? new[] { 0 } : new[] { 0, 1, 2 })
            { UseDrawTwo(game); DrawLiangyin(game, recipient); Play(game); }
            var placements = game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiTargetEvent>().Where(entry => entry.Draw).ToArray();
            Need(placements.Length == (recover ? 1 : 3) && !game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiStoredEvent>().Any(),
                "Liangyin observes another skill's actual public general pile globally, with one draw-target record per actual placement batch, without depending on Kongsheng.");
            UseControl(game, "obtain"); Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "public-pile-flow") == true);
            var obtained = Decision(game)!.Choices.First().Cards.Single(); Answer(game, choice => choice.Cards.SequenceEqual([obtained]));
            Reach(game, prompt => Optional(prompt, Liangyin)); Activate(game); Reach(game, prompt => Branch(prompt, "target")); Answer(game, choice => choice.Targets.SequenceEqual([0]));
            Reach(game, prompt => Branch(prompt, "discard")); var discarded = Decision(game)!.Choices.First().Cards.Single(); Answer(game, choice => choice.Cards.SequenceEqual([discarded])); Play(game);
            Need(game.CardMovements.Any(move => move.CardId == discarded && move.To == CardLocation.DiscardPile && move.Reason.Value == "skill.boundary-zhou-fei.discard") &&
                game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiTargetEvent>().Count(entry => !entry.Draw && entry.Target == 0) == 1,
                "Actually obtaining a card from that other general pile opens the selected recipient's real one-card discard, rather than reacting to arbitrary pile exits or uses.");
            Accept(game, new EndPlayPhaseCommand(0, game.Revision, Decision(game)!.PromptId)); Reach(game, prompt => Optional(prompt, Liangyin));
            Activate(game); Reach(game, prompt => Branch(prompt, recover ? "recover" : "lose-hp"));
            var target = recover ? 0 : 3; var before = game.CreateSnapshot(0).Players[target].Hp;
            Cold(ref game, scene.Registry); Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == (recover ? "recover" : "lose-hp"));
            Reach(game, _ => game.CreateSnapshot(0).Players[target].Hp == before + (recover ? 1 : -1));
            Need(game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiPaidEvent>().Count(entry => entry.Branch == (recover ? "recover" : "lose-hp") && entry.Target == target) == 1,
                "The round remembers distinct targets across draw and discard and applies exactly the unique selected recovery or unique unselected HP loss after cold loading.");
        }
    }

    public static void KongshengForeignOpaqueHeSeparateDepositsNativeUsesAndLostSourceCold()
    {
        {
            var scene = CreateScene(); var game = scene.Game;
            StoreAll(game); Reach(game, prompt => Optional(prompt, Liangyin));
            var first = game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiStoredEvent>().Single(); var holder = first.Holder;
            var originalCards = game.CardMovements.Where(move => move.To == first.Location).Select(move => move.CardId).ToArray();
            Need(holder != 0 && originalCards.Length == 2 && game.CreateSnapshot(0).PrivateRevealedCards is null &&
                game.CreateSnapshot(2).Players[holder].NamedAmbushPiles!.Single(pile => pile.SkillId == Kongsheng).Count == 2,
                "Direct HP loss in a foreign turn stores that actual turn owner's two selected entities on that holder, without publishing the concealed identities to the caster or another spectator.");
            var paidFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == first.FrameId);
            Need(paidFrame.BoundaryZhouFei is { Stage: BoundaryZhouFeiStage.StoreChildren, Payment.Count: 2 }, "The real whole-batch storage cost remains on its own original frame while Liangyin interrupts it.");
            Cold(ref game, scene.Registry); DrawLiangyin(game, holder);
            Reach(game, prompt => Optional(prompt, Kongsheng)); StoreAll(game); DrawLiangyin(game, 0);
            Reach(game, SourceChoice);
            var deposits = game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiStoredEvent>().ToArray();
            Need(deposits.Length == 2 && deposits.All(deposit => deposit.Holder == holder) && deposits[0].FrameId != deposits[1].FrameId && deposits[0].Location != deposits[1].Location &&
                game.CardMovements.Count(move => move.To == deposits[0].Location) == 2 && game.CardMovements.Count(move => move.To == deposits[1].Location) == 1,
                "Recovery after the first payment issues a second independent deposit for the same actual turn holder; cold recovery neither merges nor repays the first batch.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "keep");
            Reach(game, prompt => Branch(prompt, "return"));
            var ending = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.BoundaryZhouFei?.Stage == BoundaryZhouFeiStage.UseChoice);
            Need(ending.BoundaryZhouFei!.Deposits.SequenceEqual(deposits.Select(deposit => deposit.FrameId)) && game.CreateSnapshot(0).PrivateRevealedCards!.Count == 2 && game.CreateSnapshot(2).PrivateRevealedCards is null,
                "Only the caster's original ending choice locally authorizes viewing the current deposit's entire concealed pool.");
            var use = Decision(game)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("branch") == "use" && choice.Cards is [var id] && game.CreateSnapshot(0).PrivateRevealedCards!.Single(card => card.Id == id).Kind == CardKind.DrawTwo);
            var nativeCard = use.Cards.Single(); Cold(ref game, scene.Registry); Answer(game, choice => choice.Id == use.Id);
            Reach(game, prompt => Optional(prompt, Liangyin)); Skip(game); Reach(game, prompt => Branch(prompt, "return"));
            var secondUse = Decision(game)!.Choices.First(choice => choice.Parameters.GetValueOrDefault("branch") == "use"); Answer(game, choice => choice.Id == secondUse.Id);
            Reach(game, _ => game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiDepositReturnedEvent>().Count() == 2);
            var issued = game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiNativeEvent>().ToArray();
            Need(issued.Length == 2 && issued.Select(entry => entry.DepositId).SequenceEqual(deposits.Select(deposit => deposit.FrameId)) && issued[0].CardId == nativeCard &&
                issued.All(entry => game.CardMovements.Count(move => move.CardId == entry.CardId && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1) &&
                game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiReturnedUseEvent>().Count() == 2 &&
                game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiDepositReturnedEvent>().Select(entry => entry.Count).SequenceEqual([1, 0]),
                "Each independently issued deposit permits one real native entity Use, then returns only its remaining entities to its original holder through the same ending frame and typed use return.");
        }
        {
            var scene = CreateScene(foreign: false, atPreparation: true); var game = scene.Game;
            StoreAll(game); DrawLiangyin(game, 0); Play(game);
            var deposit = game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiStoredEvent>().Single();
            var original = game.CardMovements.Where(move => move.To == deposit.Location).Select(move => move.CardId).ToArray();
            UseControl(game, "drop-source"); Play(game); Cold(ref game, scene.Registry);
            Need(!game.CreateSnapshot(0).Players[0].Skills.Any(skill => skill.ContentId == Kongsheng), "The existing terminal skill-change activation really removes the caster's printed source before its delayed return.");
            Accept(game, new EndPlayPhaseCommand(0, game.Revision, Decision(game)!.PromptId)); Reach(game, prompt => Optional(prompt, Liangyin));
            var returning = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.BoundaryZhouFei?.Stage == BoundaryZhouFeiStage.ReturnChildren);
            Need(returning.SkillId == Kongsheng && returning.BoundaryZhouFei!.Payment.Count == original.Length, "The lost source's original deposit still owns a paid return invoice for the living holder.");
            Cold(ref game, scene.Registry); Skip(game);
            Reach(game, _ => !game.ResolutionStack.Any(frame => frame.Id == returning.Id));
            Need(!game.Events.Select(record => record.Payload).OfType<BoundaryZhouFeiNativeEvent>().Any() && original.All(id => game.CardMovements.Count(move => move.CardId == id && move.From == deposit.Location && move.To == CardLocation.Hand(0) && move.Reason.Value == "skill.boundary-zhou-fei.return") == 1),
                "Source loss revokes optional use but preserves the original holder's complete entity return once, including actual cold continuation after the gain child.");
        }
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static void Cold(ref GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(JsonSerializer.Serialize(game.CreateSnapshot(0)) == JsonSerializer.Serialize(restored.CreateSnapshot(0)), "The accepted command prefix restores the exact original owner, private pool, paid batch and delayed cursor."); game = restored;
    }
}
