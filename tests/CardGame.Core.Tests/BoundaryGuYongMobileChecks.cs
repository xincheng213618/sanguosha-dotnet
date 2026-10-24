using System.Text.Json;
using CardGame.Core;
using static BoundaryGuYongScenario;

internal static class BoundaryGuYongMobileChecks
{
    public static void ShenxingOriginalMixedHePaymentFrozenColorsAndCurrentHpQuotaCold()
    {
        var scene = CreateScene("cost", withWatch: true); var game = scene.Game;
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var equipment = game.CreateSnapshot(0).Players[0].Equipment.Single(card => card.Kind == CardKind.Crossbow).Id;
        var black = hand.First(card => card.Suit == Suit.Spade).Id;
        var before = hand.Count;
        Use(game, Shenxing, "discard-pair", [black, equipment]); Reach(game, WatchPending);
        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Shenxing);
        Need(game.CreateSnapshot(0).Players[0].HandCount == before - 1 &&
            game.CardMovements.Count(move => move.CardId == black && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile) == 1 &&
            game.CardMovements.Count(move => move.CardId == equipment && move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile) == 1 &&
            parent.ChoiceBindings.Single(binding => binding.Name == "paid-colors").OptionId == "same",
            "The exact original hand and equipment entities pay once before their real discard child; their original same-color reward is already frozen.");
        Cold(ref game, scene.Registry); ReachRewardDraw(game, parent.Id);
        Need(game.CreateSnapshot(0).Players[0].HandCount == before &&
            game.Events.Select(record => record.Payload).OfType<CardsDrawnEvent>().Last().Count == 1,
            "A same-color pair resumes its original program into one native one-card draw.");
        CompleteRewardChildrenAndPlay(game, parent.Id);
        Use(game, Driver, "wound", []); Play(game); Use(game, Driver, "wound", []); Play(game);
        hand = game.CreateSnapshot(0).Players[0].Hand;
        var pair = new[] { hand.First(card => card.Suit is Suit.Heart or Suit.Diamond).Id, hand.First(card => card.Suit is Suit.Club or Suit.Spade).Id };
        var rejected = game.Submit(new UseProgramSkillCommand(0, Shenxing, "discard-pair", pair, [], game.Revision, Decision(game)!.PromptId));
        Need(!rejected.Accepted && game.CreateSnapshot(0).Players[0].Hp == 1,
            "Lowering current HP to one closes the already-used once phase quota without resetting its ledger.");
        Use(game, Driver, "heal", []); Play(game); before = game.CreateSnapshot(0).Players[0].HandCount;
        Use(game, Shenxing, "discard-pair", pair); Reach(game, WatchPending);
        parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Shenxing);
        Cold(ref game, scene.Registry); ReachRewardDraw(game, parent.Id);
        Need(game.CreateSnapshot(0).Players[0].HandCount == before &&
            game.Events.Select(record => record.Payload).OfType<CardsDrawnEvent>().Last().Count == 2 &&
            game.Events.Select(record => record.Payload).OfType<BoundaryGuYongColorsFrozenEvent>().Last().Outcome == "different" &&
            pair.All(id => game.CardMovements.Count(move => move.CardId == id && move.To == CardLocation.DiscardPile) == 1),
            "Healing to two reopens exactly one use; the mixed original pair produces a single native Draw2 batch after its paid child returns.");
        Cold(ref game, scene.Registry); CompleteRewardChildrenAndPlay(game, parent.Id);
        hand = game.CreateSnapshot(0).Players[0].Hand;
        Need(!game.Submit(new UseProgramSkillCommand(0, Shenxing, "discard-pair", hand.Take(2).Select(card => card.Id).ToArray(), [], game.Revision, Decision(game)!.PromptId)).Accepted,
            "Two completed uses exhaust the current two-HP phase allowance.");
    }

    public static void BingyiRevealsBeforeColorOrTypeQualificationAndFrozenSelfRecipientsCold()
    {
        foreach (var pattern in new[] { "type", "color", "tricks", "fail" })
        {
            var scene = CreateScene(pattern, withWatch: pattern == "type"); var game = scene.Game;
            var shown = game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).Order().ToArray();
            var hands = game.CreateSnapshot(0).Players.Select(player => player.HandCount).ToArray();
            BeginBingyi(game);
            var reveal = game.Events.Select(record => record.Payload).OfType<ProgramCardsRevealedEvent>().Single(fact => fact.SkillId == Bingyi);
            Need(reveal.Cards.Select(card => card.Id).Order().SequenceEqual(shown), "Bingyi publicly reveals the complete original hand before deciding its recipients.");
            if (pattern == "fail")
            {
                Need(!SelectingRecipients(Decision(game)) && !game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Bingyi) &&
                    game.CreateSnapshot(0).Players.Select(player => player.HandCount).SequenceEqual(hands),
                    "A hand that differs in both color and major card type is still revealed, then finishes without a recipient choice or draw.");
                continue;
            }
            Reach(game, SelectingRecipients);
            var prompt = Decision(game)!;
            Need(prompt.Choices.Any(choice => choice.Targets.Count == 0) && prompt.Choices.Max(choice => choice.Targets.Count) == shown.Length &&
                prompt.Choices.Any(choice => choice.Targets.SequenceEqual([0, 2])),
                "Uniform color or a uniform major type (including delayed and instant tricks together) grants the frozen hand-count quota and permits self.");
            if (pattern == "color")
            {
                Answer(game, choice => choice.Targets.Count == 0);
                Reach(game, _ => !game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Bingyi));
                Need(game.CreateSnapshot(0).Players.Select(player => player.HandCount).SequenceEqual(hands),
                    "After a valid public reveal, explicitly committing zero recipients finishes the optional benefit without drawing for anyone.");
                continue;
            }
            if (pattern == "type") Cold(ref game, scene.Registry);
            Answer(game, choice => choice.Targets.SequenceEqual([0, 2]));
            if (pattern == "type")
            {
                Reach(game, WatchPending);
                Need(game.CreateSnapshot(0).Players[0].HandCount == hands[0] + 1 && game.CreateSnapshot(0).Players[2].HandCount == hands[2],
                    "The selected self recipient draws first and its actual native child pauses before the later selected seat.");
                Cold(ref game, scene.Registry); ContinueWatch(game);
            }
            Reach(game, _ => !game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Bingyi));
            Need(game.CreateSnapshot(0).Players.Select(player => player.HandCount).SequenceEqual(hands.Select((count, seat) => count + (seat is 0 or 2 ? 1 : 0))),
                "Exactly the two committed recipients each draw once; self's extra card never expands the frozen original selection.");
        }
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private static void ReachRewardDraw(GameEngine game, long parentId)
    {
        for (var step = 0; step < 8; step++)
        {
            Reach(game, WatchPending);
            var child = game.ResolutionStack.OfType<ProgramSkillFrame>().Last();
            if (child.TriggerId == "native-draw") return;
            var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parentId);
            Need(child.SkillId == Watch && child.TriggerId == "paid-cost" && parent.SkillId == Shenxing &&
                parent.InstructionIndex == 2 && parent.SelectedCardPayment is not null && parent.NativeDraw is null,
                "Each real hand/equipment movement child returns to the same already-paid discard instruction before its reward draw begins.");
            ContinueWatch(game);
        }
        throw new InvalidOperationException("The original paid Shenxing did not return from its bounded movement children to its native reward draw.");
    }
    private static void CompleteRewardChildrenAndPlay(GameEngine game, long parentId)
    {
        for (var step = 0; step < 8; step++)
        {
            Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } || WatchPending(prompt));
            if (Decision(game) is { Kind: DecisionKind.PlayCard }) return;
            var child = game.ResolutionStack.OfType<ProgramSkillFrame>().Last();
            var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parentId);
            Need(child.SkillId == Watch && child.TriggerId == "native-draw" && parent.SkillId == Shenxing &&
                parent.NativeDraw is { RecipientApplied: true },
                "Every actual gained-card observer returns through the same already-applied native reward cursor; no child repays or redraws.");
            ContinueWatch(game);
        }
        throw new InvalidOperationException("The original native Shenxing draw did not finish its bounded gained-card children.");
    }
    private static void Cold(ref GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(JsonSerializer.Serialize(game.CreateSnapshot(0)) == JsonSerializer.Serialize(restored.CreateSnapshot(0)),
            "The actual command prefix restores the original paid program, frozen reward and exact recipient cursor.");
        game = restored;
    }
}
