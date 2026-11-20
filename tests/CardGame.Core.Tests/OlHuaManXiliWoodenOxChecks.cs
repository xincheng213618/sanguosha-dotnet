using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlHuaManXiliWoodenOxChecks
{
    private const string HuaMan = "ol:hua-man", Xili = "ol:xili", Carrier = "fixture:xili-damage-source";
    private const string WoodenOx = "classic:wooden-ox", Discard = "skill-program.huaman.discard", Draw = "skill-program.huaman.draw";
    private const string Mode = "identity:classic-ol-xili-wooden-ox", Deck = "fixture:ol-xili-wooden-ox-deck";
    private static readonly string[] Generals = [Carrier, HuaMan, "fixture:xili-victim", "fixture:xili-peer"];

    internal static void XiliCanDiscardTreasureButCannotDirectlyDiscardItsGrainCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var ox = hand.First(card => card.Kind == CardKind.WoodenOx).Id;
        var duels = hand.Where(card => card.Kind == CardKind.Duel).Take(2).Select(card => card.Id).ToArray();
        var grain = duels[0];
        Accept(game, new PlayCardCommand(0, ox, [], game.Revision, P(game)!.PromptId));
        Settle(game);
        Accept(game, new UseEquipmentEffectCommand(0, CardKind.WoodenOx, [grain], [1], game.Revision, P(game)!.PromptId));
        Settle(game);
        var recipient = game.CreateSnapshot(1).Players[1];
        Require(recipient.Equipment.Any(card => card.Id == ox) && recipient.WoodenOxGrain!.Single().Id == grain &&
            game.CreateSnapshot(0).Players[1].WoodenOxGrain!.Count == 0 &&
            game.CardMovements.Single(move => move.CardId == grain && move.Reason == CardMoveReasons.WoodenOxTransfer) is
                { From: var from, To: var to } && from == CardLocation.WoodenOxGrain(0) && to == CardLocation.WoodenOxGrain(1),
            "A real Wooden Ox transfer carries the private actual grain to the other Xili holder.");

        var turn = game.State.TurnNumber;
        var victimHp = game.CreateSnapshot(2).Players[2].Hp;
        var duel = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.Duel &&
            action.CardId == duels[1] && action.TargetSeats.SequenceEqual([2]));
        Accept(game, new PlayCardCommand(0, duels[1], duel.TargetSeats, game.Revision, P(game)!.PromptId, duel.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Xili } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var original = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == duels[1]);
        var window = game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single();
        Require(original is { SourceSeat: 0, CardKind: CardKind.Duel } && original.TargetSeats.SequenceEqual([2]) &&
            window is { SourceSeat: 0, TargetSeat: 2, Amount: 1 } && game.State.TurnNumber == turn &&
            !Facts<DamageAppliedEvent>(game).Any(), "The other holder's actual Duel in his own turn is about to damage a non-holder.");
        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 1, IsPrivate: true, SkillPrompt.SkillId: Xili } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "discard"));
        var choices = P(game)!.Choices;
        Require(choices.Any(choice => choice.Cards.SequenceEqual([ox])), "Xili must offer the payer's actual equipped Wooden Ox.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the private payment window and exact original Duel.");
        Answer(restored, choice => choice.Cards.SequenceEqual([ox]));
        Settle(restored);
        var paid = restored.CardMovements.Single(move => move.Reason.Value == Discard);
        var dropped = restored.CardMovements.Single(move => move.Reason == CardMoveReasons.WoodenOxGrainDiscard);
        var draw = restored.CardMovements.Where(move => move.Reason.Value == Draw).ToArray();
        Require(paid.CardId == ox && paid.From == CardLocation.Equipment(1) && paid.To == CardLocation.DiscardPile &&
            dropped.CardId == grain && dropped.From == CardLocation.WoodenOxGrain(1) && dropped.To == CardLocation.DiscardPile &&
            Facts<XiliDamageIncreasedEvent>(restored).Single() is { PayerSeat: 1, SourceSeat: 0, TargetSeat: 2, Before: 1, After: 2 } increased &&
            increased.BeforeDamageWindowId == window.Id && draw.Length == 4 &&
            draw.Count(move => move.To == CardLocation.Hand(1)) == 2 && draw.Count(move => move.To == CardLocation.Hand(0)) == 2 &&
            Facts<DamageAppliedEvent>(restored).Single() is { SourceSeat: 0, TargetSeat: 2, Amount: 2 } &&
            Facts<CardUseFinishedEvent>(restored).Count(entry => entry.ResolutionId == original.Id) == 1 &&
            restored.CreateSnapshot(2).Players[2].Hp == victimHp - 2,
            "Equipment payment increases the original damage once and both holders draw twice. The grain only falls from the lost treasure.");

        Require(choices.All(choice => !choice.Cards.Contains(grain)) &&
            game.CreateSnapshot(1).Players[1].WoodenOxGrain!.Single().Id == grain,
            "Xili must not offer the real stored Duel as direct discard payment while the treasure remains equipped.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Start(ContentRegistry content)
    {
        for (var seed = 47; seed < 55; seed++)
        {
            var game = GameEngine.CreateStandard(new GameOptions { Seed = seed, PlayerCount = 4, ModeId = Mode,
                HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
            Accept(game, new StartGameCommand());
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
            while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
            {
                Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
                Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
            }

            var hand = game.CreateSnapshot(0).Players[0].Hand;
            if (hand.Any(card => card.Kind == CardKind.WoodenOx) && hand.Count(card => card.Kind == CardKind.Duel) >= 2)
                return game;
        }

        throw new InvalidOperationException("The Xili fixture requires a real Wooden Ox and two distinct Duel cards at setup.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-xili-wooden-ox", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new PayerBuilder(builder), HuaMan);
            new StandardClassicGeneralPackage().Register(new TreasureOnlyBuilder(builder));
            builder.AddGeneral(new(Carrier, "系力同伴", "supporter", Xili, "shu", 8));
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "木牛与决斗", 8, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(index => index % 2 == 0 ? new ContentDeckPhysicalCard(WoodenOx, Suit.Diamond, 5) :
                    new ContentDeckPhysicalCard("standard:duel", Suit.Spade, 1)).ToArray() });
            builder.AddMode(new(Mode, "系力木牛", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class PayerBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => inner.AddGeneral(general.Id == HuaMan ?
            general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Xili] } : general);
        public void AddSkill(ContentSkillDefinition skill) => inner.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => inner.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => inner.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => inner.AddMode(mode);
    }

    private sealed class TreasureOnlyBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddCard(ContentCardDefinition card)
        {
            if (card.Id == WoodenOx)
                inner.AddCard(card);
        }

        public void AddGeneral(ContentGeneralDefinition general) { }
        public void AddSkill(ContentSkillDefinition skill) { }
        public void AddDeck(ContentDeckRecipe deck) { }
        public void AddMode(ContentModeDefinition mode) { }
    }
}
