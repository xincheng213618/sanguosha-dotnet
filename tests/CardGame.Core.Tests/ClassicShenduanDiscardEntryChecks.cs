using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicShenduanDiscardEntryChecks
{
    private const string Hero = "classic:han-hao-shi-huan", Shenduan = "classic:shenduan";
    private const string Driver = "fixture:shenduan-discard", Claim = "fixture:shenduan-earlier-claim";
    private const string Discard = "skill-program.fixture:shenduan-discard.SelectAndMoveOwnedCard";
    private const string Take = "skill-program.fixture:shenduan-earlier-claim.ClaimMovedCards";
    private const string Deliver = "skill-program.fixture:shenduan-earlier-claim.SelectAndMoveOwnedCard";
    private const string Mode = "identity:classic-shenduan-entry", Deck = "fixture:shenduan-deck";
    private static readonly string[] Generals = ["fixture:shenduan-claimant", Hero, "fixture:shenduan-slash-user", "fixture:shenduan-peer"];

    internal static void ShenduanOnlyUsesItsOriginalBlackBasicDiscardCold()
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard", [], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Driver });
        var card = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } &&
            prompt.Choices.Any(choice => Action(choice) == "activate"));

        var original = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Discard);
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == original.BatchId);
        Require(original.From == CardLocation.Hand(1) && original.To == CardLocation.DiscardPile && original.CardKind == CardKind.Slash &&
            window.Candidates.Skip(window.CandidateIndex + 1).Any(candidate => candidate.SkillId == Shenduan && candidate.OwnerSeat == 1),
            "The original black basic discard queues Shenduan on the same batch behind the earlier claimant.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Shenduan });
        Activate(control);
        Reach(control, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Shenduan } &&
            prompt.Choices.Any(choice => choice.Targets.SequenceEqual([3])));
        control = Cold(control, content);
        Answer(control, choice => choice.Targets.SequenceEqual([3]));
        Settle(control);
        var converted = CardUses(control).Single(use => use.CardKind == CardKind.SupplyShortage);
        var placement = control.CardMovements.Single(move => move.CardId == card && move.To == CardLocation.Judgment(3));
        Require(converted is { SourceSeat: 1 } && converted.CardId == card &&
            control.Events.Select(entry => entry.Payload).OfType<TargetsConfirmedEvent>()
                .Any(fact => fact.ResolutionId == converted.ResolutionId && fact.TargetSeats.SequenceEqual([3])) &&
            control.CardMovements.Any(move => move.CardId == card && move.From == CardLocation.DiscardPile &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use && move.Sequence > original.Sequence &&
                move.Sequence < placement.Sequence) && placement.From == CardLocation.Processing &&
            control.Events.Select(entry => entry.Payload).OfType<CardUseFinishedEvent>()
                .Any(fact => fact.ResolutionId == converted.ResolutionId && fact.CardId == card && fact.CardKind == CardKind.SupplyShortage),
            "Shenduan uses the original entity as native Supply Shortage on a nonadjacent target and completes its placement.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => choice.Targets.SequenceEqual([2])));
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Claim } && prompt.Choices.Any(choice => Action(choice) == "request-slash"));

        var took = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == Take);
        var delivered = game.CardMovements.Last(move => move.CardId == card);
        Require(took.From == CardLocation.DiscardPile && took.To == CardLocation.Hand(0) && took.Sequence > original.Sequence &&
            delivered.Reason.Value == Deliver && delivered.To == CardLocation.Hand(2) && delivered.Sequence > took.Sequence,
            "The earlier claimant actually takes the original discarded card and transfers it to the native Slash user.");
        game = Cold(game, content);
        Answer(game, choice => Action(choice) == "request-slash" && choice.Cards.SequenceEqual([card]));
        FinishSlash(game);
        var native = CardUses(game).Single(use => use.CardId == card);
        var cleanup = game.CardMovements.Single(move => move.CardId == card && move.Reason == CardMoveReasons.UseFinished);
        Require(native is { SourceSeat: 2, CardKind: CardKind.Slash } &&
            game.Events.Select(entry => entry.Payload).OfType<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == native.ResolutionId) &&
            cleanup.From == CardLocation.Processing && cleanup.To == CardLocation.DiscardPile &&
            cleanup.Sequence > delivered.Sequence && cleanup.BatchId != original.BatchId,
            "The actual Slash has completed, returning the same card to a new entry created by use rather than discard.");

        if (P(game)?.SkillPrompt?.SkillId == Shenduan)
        {
            Require(game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last().Batch.Id == original.BatchId,
                "Any remaining Shenduan opportunity belongs to the original discard batch.");
            Activate(game);
            Reach(game, prompt => prompt.SkillPrompt?.SkillId == Shenduan || prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
            if (P(game)?.SkillPrompt?.SkillId == Shenduan)
                Answer(game, choice => choice.Targets.SequenceEqual([3]));
        }
        Settle(game);
        Require(CardUses(game).All(use => use.CardKind != CardKind.SupplyShortage) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Shenduan cannot turn Slash cleanup entry {cleanup.Sequence} into Supply Shortage using the original discard {original.Sequence}.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<CardUseDeclaredEvent> CardUses(GameEngine game) =>
        game.Events.Select(entry => entry.Payload).OfType<CardUseDeclaredEvent>();

    private static void FinishSlash(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || prompt.SkillPrompt?.SkillId == Shenduan)
                return;
            Require(prompt.Kind == DecisionKind.RespondDodge, $"Unexpected {prompt.Kind}/{prompt.SkillPrompt?.SkillId} while settling the native Slash.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not finish at a public decision boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the original discard and the pending action in all four seat views.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-shenduan-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"discard","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":1,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},
                        "cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"claim-before-shenduan","window":"discardPileReceived","subject":"owner",
                     "discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{Discard}}}"],"optional":true,
                     "effects":[{"op":"claimMovedCards","target":"owner"},
                         {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                         {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                          "zones":["hand"],"count":1,"destination":"selectedTargetHand","targetRef":{"kind":"selectedTarget"},"awaitMovementTriggers":true},
                         {"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"slash"}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "弃牌", description = "令指定角色弃置一张手牌。" },
                    [Claim] = new { name = "先领再用", description = "领取弃牌，将其交给另一名角色并令其使用杀。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });

            builder.AddGeneral(new(Generals[0], "先行领取者", "supporter", Driver, "qun", 8, [Claim]));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体黑杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "慎断原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
