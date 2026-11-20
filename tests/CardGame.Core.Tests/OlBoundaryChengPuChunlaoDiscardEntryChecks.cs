using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlBoundaryChengPuChunlaoDiscardEntryChecks
{
    private const string Hero = "boundary:cheng-pu", Chunlao = "boundary:chunlao";
    private const string Driver = "fixture:chunlao-discard-driver", Claim = "fixture:chunlao-earlier-claim";
    private const string Mode = "identity:chunlao-discard-entry", Deck = "fixture:chunlao-deck";
    private const string Drop = "skill-program.fixture:chunlao-discard-driver.SelectAndMoveOwnedCard";
    private const string Take = "skill-program.fixture:chunlao-earlier-claim.ClaimMovedCards";
    private const string Deliver = "skill-program.fixture:chunlao-earlier-claim.SelectAndMoveOwnedCard";
    private const string Store = "skill-program.boundary:chunlao.adjacent-discard-store";
    private static readonly string[] Generals = ["fixture:chunlao-claimant", Hero, "fixture:chunlao-discarder", "fixture:chunlao-peer"];

    internal static void ChunlaoCannotStoreANewerSlashCleanupThroughAnOldAdjacentDiscardCold()
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard", [], [2], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Driver });
        var slash = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([slash]));
        ReachClaim(game, 0, choice => Action(choice) == "activate");

        var original = game.CardMovements.Single(move => move.CardId == slash && move.Reason.Value == Drop);
        var origin = Facts<ProgramAdjacentDiscardOriginEvent>(game).Single(fact => fact.MovementSequence == original.Sequence);
        var batch = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == original.BatchId);
        Require(original.From == CardLocation.Hand(2) && original.To == CardLocation.DiscardPile &&
            origin is { SourceSeat: 2, PreviousLivingSeat: 1, NextLivingSeat: 3, EffectiveKind: CardKind.Slash } && origin.CardId == slash &&
            batch.Candidates.Skip(batch.CandidateIndex + 1).Any(candidate => candidate.OwnerSeat == 1 && candidate.SkillId == Chunlao),
            "The real discard freezes both living neighbours and queues the previous neighbour's locked Chunlao behind the earlier claimant.");

        var control = Cold(game, content);
        Skip(control);
        Settle(control);
        var stored = Facts<ProgramAdjacentDiscardStoredEvent>(control).Single();
        var deposit = control.CardMovements.Single(move => move.Reason.Value == Store);
        Require(stored.Source is { OwnerSeat: 1, SkillId: Chunlao } && stored.Zone == CardZoneKind.Chunlao &&
            stored.CardId == slash && stored.DiscardMovementSequence == original.Sequence && stored.StoreMovementSequence == deposit.Sequence &&
            deposit.CardId == slash && deposit.From == CardLocation.DiscardPile && deposit.To == new CardLocation(CardZoneKind.Chunlao, 1),
            "The original unclaimed entry is stored once in the living neighbour's actual Chunlao pile.");

        Activate(game);
        ReachClaim(game, 0, choice => choice.Targets.SequenceEqual([2]));
        Answer(game, choice => choice.Targets.SequenceEqual([2]));
        ReachClaim(game, 0, choice => choice.Cards.SequenceEqual([slash]));
        Answer(game, choice => choice.Cards.SequenceEqual([slash]));
        ReachClaim(game, 2, choice => Action(choice) == "request-slash" && choice.Cards.SequenceEqual([slash]));
        var took = game.CardMovements.Single(move => move.CardId == slash && move.Reason.Value == Take);
        var transfer = game.CardMovements.Where(move => move.CardId == slash && move.Reason.Value == Deliver).ToArray();
        Require(took.From == CardLocation.DiscardPile && took.To == CardLocation.Hand(0) && took.Sequence > original.Sequence &&
            transfer is [var departed, var delivered] && departed.From == took.To && departed.To == CardLocation.Processing &&
            delivered.From == departed.To && delivered.To == CardLocation.Hand(2) && departed.Sequence > took.Sequence &&
            delivered.Sequence > departed.Sequence,
            "The earlier claimant takes the physical Slash and returns it to the original discarder before the old candidate runs.");

        game = Cold(game, content);
        Answer(game, choice => Action(choice) == "request-slash" && choice.Cards.SequenceEqual([slash]));
        SettleSlash(game);
        var declared = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == slash);
        var cleanup = game.CardMovements.Single(move => move.CardId == slash && move.Reason == CardMoveReasons.UseFinished);
        Require(declared is { SourceSeat: 2, CardKind: CardKind.Slash } && cleanup.From == CardLocation.Processing &&
            cleanup.To == CardLocation.DiscardPile && cleanup.BatchId != original.BatchId && cleanup.Sequence > transfer[^1].Sequence &&
            Facts<CardUseFinishedEvent>(game).Any(fact => fact.ResolutionId == declared.ResolutionId && fact.CardId == slash) &&
            Facts<ProgramAdjacentDiscardOriginEvent>(game).Where(fact => fact.CardId == slash).SequenceEqual([origin]),
            "The real native Slash produces a different cleanup entry while only the original discard has frozen neighbour provenance.");
        Require(!Facts<ProgramAdjacentDiscardStoredEvent>(game).Any() && game.CardMovements.All(move => move.Reason.Value != Store) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == slash).Location == CardLocation.DiscardPile,
            $"Chunlao may not store Slash cleanup entry {cleanup.Sequence} through the original adjacent discard entry {original.Sequence}.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static void ReachClaim(GameEngine game, int seat, Func<PromptChoice, bool> predicate) =>
        Reach(game, prompt => prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == Claim && prompt.Choices.Any(predicate));

    private static void SettleSlash(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard })
                return;
            Require(prompt.Kind == DecisionKind.RespondDodge, "Only the native Slash defence may interrupt this discard window.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not return to the original Play phase.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves each seat's pending decision and the exact physical movements.");
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
        public PackageManifest Manifest { get; } = new("fixture-ol-boundary-chunlao-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"discard","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":1,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},
                        "cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"claim-before-chunlao","window":"discardPileReceived","subject":"owner",
                    "discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{Drop}}}"],"optional":true,
                    "effects":[{"op":"claimMovedCards","target":"owner"},
                        {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                          "zones":["hand"],"count":1,"destination":"selectedTargetHand","targetRef":{"kind":"selectedTarget"},"awaitMovementTriggers":true},
                        {"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"slash"}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "弃牌", description = "令指定角色弃置一张手牌。" },
                    [Claim] = new { name = "先行领取", description = "领取弃牌，交给另一名角色，然后令其使用杀。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[0], "先行领取者", "supporter", Driver, "qun", 8, [Claim]));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "醇醪原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
