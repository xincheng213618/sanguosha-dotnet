using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryHuCheErFuzhongDrawChecks
{
    private const string Hero = "anniversary:hu-che-er";
    private const string Feed = "fixture:hu-che-er-fuzhong-feed";
    private const string Quiet = "fixture:hu-che-er-fuzhong-quiet";
    private const string NoDraw = "fixture:hu-che-er-fuzhong-no-draw";
    private const string Deck = "fixture:hu-che-er-fuzhong-deck";
    private static readonly CardMoveReason FeedReason = new("skill-program." + Feed + ".Draw");

    internal static void OneForeignBatchAddsOneCardToNextNormalDrawCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = CreateGame(registry, interactiveDiscard: false);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        bool AtPlay() => Pending() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
        int Weight() => game.CreateSnapshot(0).Players[0].Markers
            ?.SingleOrDefault(marker => marker.Kind == PlayerMarkerKind.AnniversaryHuCheErZhong)?.Count ?? 0;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

        CardMovementRecord[] DrawCards(int turn, CardMoveReason reason) => game.CardMovements.Where(move =>
            move.TurnNumber == turn && move.Reason == reason && move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(0)).ToArray();

        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
                {
                    game.State.TurnNumber, game.State.CurrentSeat, game.State.Phase, Prompt = Pending(), Weight = Weight(),
                    Recent = game.Events.TakeLast(8).Select(entry => entry.Payload.GetType().Name)
                }));
        }

        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Fuzhong fixture command failed.");
        }

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 120 && !ready(); step++)
            {
                Need(Pending() is null && game.State.Winner == Winner.None, "The quiet table reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The expected Fuzhong boundary was not reached.");
        }

        Submit(new StartGameCommand());
        Reach(AtPlay);
        var openingTurn = game.State.TurnNumber;
        var openingCards = DrawCards(openingTurn, CardMoveReasons.Draw);
        Need(openingCards.Length == 2 && Weight() == 0 && Facts<AnniversaryHuCheErGainRecordedEvent>().Length == 0,
            "Hu Che Er's first actual normal Draw moves two physical cards and earns no weight.");

        Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));
        Reach(() => Pending()?.SkillPrompt?.SkillId == Feed);
        var foreignTurn = game.State.TurnNumber;
        var foreignOwner = game.State.CurrentSeat;
        var giftCards = DrawCards(foreignTurn, FeedReason);
        var invoice = Facts<NativeDrawInvocationRecordedEvent>().Single(draw => draw.Reason == FeedReason);
        var mark = Facts<AnniversaryHuCheErGainRecordedEvent>().Single();
        Need(foreignOwner != 0 && Facts<TurnStartedEvent>().Any(turn => turn.TurnNumber == foreignTurn && turn.ActorSeat == foreignOwner) &&
            giftCards.Length == 2 && giftCards[0].CardId != giftCards[1].CardId &&
            giftCards.All(move => invoice.SequenceBefore < move.Sequence && move.Sequence <= invoice.SequenceAfter) &&
            invoice is { OwnerSeat: 0, Requested: 2, ActualCount: 2 } && invoice.TurnNumber == foreignTurn &&
            invoice.ActualTurnOwnerSeat == foreignOwner && mark.Source.SkillId == "anniversary:fuzhong" && mark.Source.OwnerSeat == 0 &&
            mark is { Before: 0, After: 1 } && mark.BatchId == invoice.LastBatchId && mark.Turn == foreignTurn &&
            mark.TurnOwner == foreignOwner && Weight() == 1,
            "One paid two-card native draw on another actual turn produces exactly one weight for its logical gain.");

        var original = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(original.CreateSnapshot(0)) &&
            JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(original.ResolutionStack) &&
            game.CardMovements.SequenceEqual(original.CardMovements) && Facts<AnniversaryHuCheErGainRecordedEvent>().Single() == mark &&
            Weight() == 1,
            "Cold restore preserves the paid foreign draw, its single weight and the real continuation prompt.");
        var continuePrompt = Pending()!;
        var continueChoice = continuePrompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Submit(new AnswerPromptCommand(0, continuePrompt.PromptId, continueChoice.Id, game.Revision));
        Reach(AtPlay);

        var nextTurn = game.State.TurnNumber;
        var normalCards = DrawCards(nextTurn, CardMoveReasons.Draw);
        Need(Facts<TurnStartedEvent>().Where(turn => turn.ActorSeat == 0).Select(turn => turn.TurnNumber)
                .SequenceEqual([openingTurn, nextTurn]) && nextTurn > foreignTurn && normalCards.Length == 3 &&
            openingCards.Concat(giftCards).Concat(normalCards).Select(move => move.CardId).Distinct().Count() == 7 &&
            Weight() == 1 && Facts<AnniversaryHuCheErGainRecordedEvent>().SequenceEqual([mark]),
            "The next real own normal Draw moves three other physical cards, while the one weight remains unchanged.");
    }

    internal static void ThreeForeignBatchesRetainSevenAfterActualDiscardCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(threeBatches: true));
        var game = CreateGame(registry, interactiveDiscard: true);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        bool AtPlay() => Pending() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
        bool AtFeed() => Pending()?.SkillPrompt?.SkillId == Feed;
        bool AtDiscard() => Pending() is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards };
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
                {
                    game.State.TurnNumber, game.State.CurrentSeat, game.State.Phase, Prompt = Pending(),
                    Owner().HandCount, Recent = game.Events.TakeLast(8).Select(entry => entry.Payload.GetType().Name)
                }));
        }

        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Fuzhong discard fixture command failed.");
        }

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 120 && !ready(); step++)
            {
                Need(Pending() is null && game.State.Winner == Winner.None, "The quiet table reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The expected Fuzhong discard boundary was not reached.");
        }

        Submit(new StartGameCommand());
        Reach(AtPlay);
        var openingTurn = game.State.TurnNumber;
        Need(Owner() is { Hp: 4, HandCount: 2 }, "The controlled opening keeps its two native Draw cards at four health.");
        Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));

        foreach (var expectedMarks in new[] { 1, 2, 3 })
        {
            Reach(AtFeed);
            Need(Facts<AnniversaryHuCheErGainRecordedEvent>().Length == expectedMarks,
                "Each independent native draw adds one weight before its own Feed continuation.");
            var prompt = Pending()!;
            var choice = prompt.Choices.Single(candidate => candidate.Parameters.GetValueOrDefault("option-id") == "continue");
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        }
        Reach(AtPlay);

        var invoices = Facts<NativeDrawInvocationRecordedEvent>().Where(draw => draw.Reason == FeedReason).ToArray();
        var marks = Facts<AnniversaryHuCheErGainRecordedEvent>();
        var foreignCards = game.CardMovements.Where(move => move.Reason == FeedReason &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)).ToArray();
        Need(invoices.Length == 3 && invoices.Select(draw => draw.ActualCount).SequenceEqual([2, 1, 1]) &&
            invoices.Select(draw => draw.InvocationId).Distinct().Count() == 3 &&
            invoices.Select(draw => draw.LastBatchId).Distinct().Count() == 3 &&
            invoices.All(draw => draw.OwnerSeat == 0 && draw.TurnNumber == invoices[0].TurnNumber &&
                draw.ActualTurnOwnerSeat == invoices[0].ActualTurnOwnerSeat) && invoices[0].ActualTurnOwnerSeat != 0 &&
            Facts<TurnStartedEvent>().Any(turn => turn.TurnNumber == invoices[0].TurnNumber && turn.ActorSeat == invoices[0].ActualTurnOwnerSeat) &&
            foreignCards.Length == 4 && foreignCards.Select(move => move.CardId).Distinct().Count() == 4 &&
            invoices.All(draw => foreignCards.Count(move => move.TurnNumber == draw.TurnNumber &&
                draw.SequenceBefore < move.Sequence && move.Sequence <= draw.SequenceAfter) == draw.ActualCount) &&
            marks.Select(mark => mark.BatchId).SequenceEqual(invoices.Select(draw => draw.LastBatchId)) &&
            marks.Select(mark => (mark.Before, mark.After)).SequenceEqual([(0, 1), (1, 2), (2, 3)]),
            "Three real draw invoices in the same other turn physically deliver two, one and one cards and add exactly three weights.");

        var discardTurn = game.State.TurnNumber;
        var hand = Owner().Hand.Select(card => card.Id).ToHashSet();
        Need(Facts<TurnStartedEvent>().Where(turn => turn.ActorSeat == 0).Select(turn => turn.TurnNumber)
                .SequenceEqual([openingTurn, discardTurn]) &&
            game.CardMovements.Count(move => move.TurnNumber == discardTurn && move.Reason == CardMoveReasons.Draw &&
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) == 3 &&
            Owner() is { Hp: 4, HandCount: 9 } && hand.Count == 9 && foreignCards.All(move => hand.Contains(move.CardId)) &&
            Owner().Markers?.SingleOrDefault(marker => marker.Kind == PlayerMarkerKind.AnniversaryHuCheErZhong)?.Count == 3,
            "Four foreign cards and the next normal Draw of three join the two original cards at three weights and four health.");

        Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));
        Reach(AtDiscard);
        var original = game;
        var obligation = game.ResolutionStack.OfType<ManualDiscardFrame>().Single();
        Need(game.State.Phase == TurnPhase.Discard && obligation is { OwnerSeat: 0, InitialRequiredCount: 2 } &&
            obligation.ActualTurnNumber == discardTurn && Pending() is { RequiredCardCount: 2 } promptBefore &&
            hand.SetEquals(promptBefore.ValidCardIds),
            "The native hand-limit obligation asks for two of the nine actual cards at the original discard boundary.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(original.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(original.CreateSnapshot(0)) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<ManualDiscardFrame>().Single()) == JsonSerializer.Serialize(obligation) &&
            game.CardMovements.SequenceEqual(original.CardMovements),
            "Cold restore preserves the unpaid obligation, its two-card choice and the exact physical hand.");
        var discard = Pending()!;
        var chosen = discard.ValidCardIds.Take(2).ToArray();
        Submit(new DiscardCardsCommand(0, chosen, discard.PromptId, game.Revision));
        Reach(() => Facts<TurnEndedEvent>().Any(turn => turn.ActorSeat == 0 && turn.TurnNumber == discardTurn));

        var payments = game.CardMovements.Where(move => move.TurnNumber == discardTurn &&
            move.Reason == CardMoveReasons.HandLimitDiscard && move.From == CardLocation.Hand(0) &&
            move.To == CardLocation.DiscardPile).Select(move => move.CardId).ToArray();
        Need(payments.Length == 2 && chosen.ToHashSet().SetEquals(payments) &&
            Facts<HandLimitDiscardedEvent>().Single(fact => fact.ActorSeat == 0).CardIds.ToHashSet().SetEquals(chosen) &&
            Owner().HandCount == 7 && hand.Except(chosen).ToHashSet().SetEquals(Owner().Hand.Select(card => card.Id)),
            "The public command moves only the two chosen entities to discard and retains exactly the seven original unpaid cards.");
    }

    private static GameEngine CreateGame(ContentRegistry registry, bool interactiveDiscard)
    {
        var setup = new ShanheBattleSetup(17, 1, "hu-che-er-fuzhong-draw", Hero, 4, 0, 0, 0, 0, [Feed],
            [], [], [new(Peer(2), Hp: 8) { StartingHand = [] }, new(Peer(3), Hp: 8) { StartingHand = [] }],
            [new(Peer(1), Hp: 8) { StartingHand = [] }], new string('a', 64));
        return GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = interactiveDiscard,
            AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, registry);
    }

    private static string Peer(int seat) => "fixture:hu-che-er-fuzhong-peer-" + seat;

    private sealed class Fixture(bool threeBatches = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-hu-che-er-fuzhong", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var extraDraws = threeBatches ? """
                ,{"op":"draw","target":"owner","amount":1},{"op":"chooseOption","target":"owner","resultBind":"second-paid","options":[{"id":"continue"}]}
                ,{"op":"draw","target":"owner","amount":1},{"op":"chooseOption","target":"owner","resultBind":"third-paid","options":[{"id":"continue"}]}
                """ : "";
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
                  {"id":"{{NoDraw}}","revision":1,"triggers":[{"id":"none","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{Feed}}","revision":1,"triggers":[{"id":"one-real-batch","window":"turnStartBeforeNormalFlow","subject":"owner","turnOwnerScope":"otherLiving","usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"draw","target":"owner","amount":2},{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}{{extraDraws}}]}]}
                ]}
                """;
            var labels = new Dictionary<string, object>
            {
                [Quiet] = new { name = "安静同桌", description = "跳过出牌阶段" },
                [NoDraw] = new { name = "安静摸牌阶段", description = "本阶段少摸两张" },
                [Feed] = new { name = "回合外摸牌", description = "真实摸两张牌", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = labels
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                {
                    Program = program,
                    ProgramPresentation = catalog.Presentations[id]
                });
            for (var seat = 1; seat < 4; seat++)
                builder.AddGeneral(new(Peer(seat), "固定安静同桌", "supporter", Quiet, "wei", 8, [NoDraw]));
            builder.AddDeck(new(Deck, "负重固定实体闪", 0, 2, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2), 32).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "负重实际摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer(1), Peer(2), Peer(3)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int>
                {
                    [ShanheBattleSetup.PlayerTeamId] = 2,
                    [ShanheBattleSetup.EnemyTeamId] = 2
                }));
        }
    }
}
