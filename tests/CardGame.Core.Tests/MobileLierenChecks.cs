using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileLierenChecks
{
    private const string Lieren = "boundary:lieren-mobile", Hero = "boundary:zhu-rong-mobile";
    private const string DamageLieren = "boundary:lieren-current", DamageHero = "boundary:zhu-rong";
    private const string Observer = "fixture:mobile-lieren-observer", Mode = "identity:mobile-lieren-test";
    private const string Deck = "fixture:mobile-lieren-deck", Exchange = "skill-program.boundary:lieren-mobile.exchange";
    private const string Zongshi = "boundary:zongshih-mobile", ZongshiObtain = "skill-program.boundary:zongshih-mobile.obtain";
    private const string Rediscard = "fixture:mobile-lieren-rediscard", RediscardReason = "skill-program.fixture:mobile-lieren-rediscard.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = [Hero, "fixture:mobile-lieren-target", "fixture:mobile-lieren-peer-2", "fixture:mobile-lieren-peer-3"];

    internal static void TiedPindianExchangesOriginalCardsAndResumes()
    {
        var (game, content, source, opponent) = StartTiedPindian(new Fixture());
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Observer);
        Require(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Lieren) is
            { PendingMovementContinuation: not null, PindianResultBindings: [{ SourceWon: false, SourceRank: 5, OpponentRank: 5 }] },
            "A tied contest must suspend the original invocation while its exchange movement child is active.");
        CheckExchange(game);
        ReplayFourViews(game, content);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Answer(restored, choice => choice.Parameters.GetValueOrDefault("option-id") == "finish");
        Settle(restored);
        CheckExchange(restored);

        void CheckExchange(GameEngine run)
        {
            var moves = run.CardMovements.Where(move => move.Reason.Value == Exchange);
            Require(moves.Select(move => (move.CardId, move.From, move.To)).SequenceEqual([
                (source, CardLocation.DiscardPile, CardLocation.Hand(1)),
                (opponent, CardLocation.DiscardPile, CardLocation.Hand(0))]),
                "Each original contest card moves once from discard to the opposite hand, in source-then-opponent order.");
        }
    }

    internal static void TiedPindianSkipsCardClaimedAndRediscardedBeforeFinish()
    {
        var (game, content, source, opponent) = StartTiedPindian(new Fixture(rediscard: true));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Zongshi });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "contest" && choice.Cards.SequenceEqual([opponent]));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Rediscard });
        var taken = game.CardMovements.Single(move => move.Reason.Value == ZongshiObtain);
        Require(taken.CardId == opponent && taken.From == CardLocation.Processing && taken.To == CardLocation.Hand(1) &&
            !game.CardMovements.Any(move => move.Reason == CardMoveReasons.PindianFinish || move.Reason.Value == Exchange),
            "Zongshi's actual gain must open a hand discard child before the original contest finishes.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        foreach (var run in new[] { game, restored })
        {
            Answer(run, choice => choice.Cards.SequenceEqual([opponent]));
            Reach(run, prompt => prompt.SkillPrompt?.SkillId == Observer);
            var returned = run.CardMovements.Single(move => move.CardId == opponent && move.Reason.Value == RediscardReason);
            var finished = run.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianFinish).ToArray();
            Require(returned.From == CardLocation.Hand(1) && returned.To == CardLocation.DiscardPile && returned.Sequence > taken.Sequence &&
                finished is [var original] && original.CardId == source && original.From == CardLocation.Processing &&
                original.To == CardLocation.DiscardPile && original.Sequence > returned.Sequence,
                "Only the unclaimed card enters the discard pile through the original Pindian finish.");
            var exchanges = run.CardMovements.Where(move => move.Reason.Value == Exchange).ToArray();
            Require(exchanges is [var exchange] && exchange.CardId == source && exchange.From == CardLocation.DiscardPile &&
                exchange.To == CardLocation.Hand(1) &&
                run.CreateCardZoneDiagnostics().Single(card => card.CardId == opponent).Location == CardLocation.DiscardPile,
                "Lieren may exchange the original finish entry, while the Zongshi card stays at its later discard entry.");
            Answer(run, choice => choice.Parameters.GetValueOrDefault("option-id") == "finish");
            Settle(run);
        }
    }

    internal static void DirectDamageOpensOriginalPindianCold()
    {
        var (game, content, slash) = PlayOpeningSlash(new Fixture(sourceGeneral: DamageHero));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: DamageLieren } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash);
        var damage = game.Events.Select(entry => entry.Payload).OfType<DamageAppliedEvent>().Single();
        var window = game.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single();
        Require(damage is { SourceSeat: 0, TargetSeat: 1, Amount: 1, SourceLess: false } &&
            damage.SourceCardUseResolutionId == use.Id &&
            window is { TriggerWindow: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: 0, TargetSeat: 1 } &&
            window.SourceCardId == slash && game.CreateSnapshot(1).Players[1] is { IsAlive: true, HandCount: > 0 },
            "The original physical Slash already damaged the living opponent before the generic Lieren offer.");
        Activate(game);

        Reach(game, prompt => prompt is { Kind: DecisionKind.SkillModule, PlayerSeat: 0, IsPrivate: true } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card"));
        var program = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == DamageLieren);
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var source = game.CreateSnapshot(0).Players[0].Hand.First().Id;
        Require(program.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: 0, TargetSeat: 1 } context &&
            context.ParentFrameId == window.Id && context.DamageFrameId == window.ParentFrameId &&
            pair is { SourceSeat: 0, OpponentSeat: 1, PindianStep: PindianStep.ChooseSourceCard, ProgramResultBind: "contest" } &&
            pair.ParentFrameId == program.Id && P(game)!.Choices.Any(choice => choice.Cards.SequenceEqual([source])) &&
            P(game)!.Choices.All(choice => !choice.Cards.Contains(slash)),
            "The original damage trigger opens the native contest against its own damage target with a separate physical source card.");
        Answer(game, choice => choice.Cards.SequenceEqual([source]));
        var selected = game.ResolutionStack.OfType<PindianFrame>().Single();
        var opponent = game.CreateSnapshot(1).Players[1].Hand.First().Id;
        Require(selected.Id == pair.Id && selected.SourceCardId == source && selected.PindianStep == PindianStep.ChooseOpponentCard &&
            P(game) is { Kind: DecisionKind.SkillModule, PlayerSeat: 1, IsPrivate: true } opponentPrompt &&
            opponentPrompt.Choices.Any(choice => choice.Cards.SequenceEqual([opponent])),
            "The selected source card stays on the original pair when the damaged opponent receives the native prompt.");
        ReplayFourViews(game, content);
    }

    private static (GameEngine Game, ContentRegistry Content, int Source, int Opponent) StartTiedPindian(Fixture fixture)
    {
        var (game, content, slash) = PlayOpeningSlash(fixture);
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Lieren &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Activate(game);

        Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card"));
        Require(P(game)!.Choices.All(choice => !choice.Cards.Contains(slash)),
            "The played Slash has already left its owner's hand at the target-finalized contest.");
        var source = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([source]));
        Reach(game, prompt => prompt.PlayerSeat == 1 &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-card"));
        var opponent = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        ReplayFourViews(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([opponent]));
        return (game, content, source, opponent);
    }

    private static (GameEngine Game, ContentRegistry Content, int Slash) PlayOpeningSlash(Fixture fixture)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), fixture);
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        ReachSetupBoundary();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, fixture.GeneralPool[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachSetupBoundary();
        }
        var slash = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash &&
            action.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        return (game, content, slash.CardId.Value);

        void ReachSetupBoundary() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral ||
            prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    }

    private sealed class Fixture(bool rediscard = false, string sourceGeneral = Hero) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-lieren", new(1, 0, 0), []);
        internal string[] GeneralPool { get; } = [sourceGeneral, .. Generals.Skip(1)];

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, DamageHero);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Observer}}}","revision":1,"triggers":[
                    {"id":"after-exchange","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementOccurrence":"perBatch","movementReasons":["{{{Exchange}}}"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"finish","options":[{"id":"finish"}]}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Observer] = new
                    { name = "交换观察", description = "交换手牌后等待。", optionLabels = new Dictionary<string, string> { ["finish"] = "继续" } } }
            });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Observer, catalog.Presentations[Observer].Name, catalog.Presentations[Observer].Description)
                { Program = catalog.Programs[Observer], ProgramPresentation = catalog.Presentations[Observer] });
            if (rediscard)
                RegisterRediscard(builder);
            builder.AddGeneral(new(Generals[1], "拼点目标", "supporter", Observer, "wei", 4, rediscard ? [Zongshi, Rediscard] : []));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "同点数牌", 4, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 64)
                    .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 5)).ToArray()
            });
            builder.AddMode(new(Mode, "烈刃规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: GeneralPool));
        }

        private static void RegisterRediscard(IContentRegistryBuilder builder)
        {
            var production = CharacterFixtureContent.EmbeddedProgramBundle("boundary-jian-yong-mobile");
            var zongshi = production.Presentations[Zongshi];
            builder.AddSkill(new(Zongshi, zongshi.Name, zongshi.Description)
                { Program = production.Programs[Zongshi], ProgramPresentation = zongshi });
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Rediscard}}}","revision":1,"triggers":[
                    {"id":"rediscard-after-zongshi","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                     "movementOccurrence":"perBatch","movementReasons":["{{{ZongshiObtain}}}"],"optional":false,
                     "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},
                        "cardOwnerRef":{"kind":"owner"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Rediscard] = new { name = "重弃", description = "纵适获得牌后弃置一张手牌。" } }
            });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            var definition = catalog.Presentations[Rediscard];
            builder.AddSkill(new(Rediscard, definition.Name, definition.Description)
                { Program = catalog.Programs[Rediscard], ProgramPresentation = definition });
        }
    }
}
