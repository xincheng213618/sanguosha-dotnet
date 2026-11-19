using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class MobileLierenChecks
{
    private const string Lieren = "boundary:lieren-mobile", Hero = "boundary:zhu-rong-mobile";
    private const string Observer = "fixture:mobile-lieren-observer", Mode = "identity:mobile-lieren-test";
    private const string Deck = "fixture:mobile-lieren-deck", Exchange = "skill-program.boundary:lieren-mobile.exchange";
    private static readonly string[] Generals = [Hero, "fixture:mobile-lieren-target", "fixture:mobile-lieren-peer-2", "fixture:mobile-lieren-peer-3"];

    internal static void TiedPindianExchangesOriginalCardsAndResumes()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        ReachSetupBoundary();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachSetupBoundary();
        }
        var slash = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash &&
            action.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Lieren &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Activate(game);

        Reach(game, prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card"));
        Require(P(game)!.Choices.All(choice => !choice.Cards.Contains(slash.CardId.Value)),
            "The played Slash has already left its owner's hand at the target-finalized contest.");
        var source = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([source]));
        Reach(game, prompt => prompt.PlayerSeat == 1 &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-card"));
        var opponent = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        ReplayFourViews(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([opponent]));

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

        void ReachSetupBoundary() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral ||
            prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });

        void CheckExchange(GameEngine run)
        {
            var moves = run.CardMovements.Where(move => move.Reason.Value == Exchange);
            Require(moves.Select(move => (move.CardId, move.From, move.To)).SequenceEqual([
                (source, CardLocation.DiscardPile, CardLocation.Hand(1)),
                (opponent, CardLocation.DiscardPile, CardLocation.Hand(0))]),
                "Each original contest card moves once from discard to the opposite hand, in source-then-opponent order.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-lieren", new(1, 0, 0), []);

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "boundary:zhu-rong");
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
            builder.AddGeneral(new(Generals[1], "拼点目标", "supporter", Observer, "wei", 4));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "同点数牌", 4, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 64)
                    .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 5)).ToArray()
            });
            builder.AddMode(new(Mode, "烈刃规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
