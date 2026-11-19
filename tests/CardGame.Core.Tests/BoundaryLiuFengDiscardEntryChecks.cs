using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class BoundaryLiuFengDiscardEntryChecks
{
    private const string Hero = "boundary:liu-feng", Xiansi = "boundary:xiansi", Qinling = "boundary:qinling";
    private const string Watch = "fixture:qinling-reentry", Mode = "identity:qinling-reentry", Deck = "fixture:qinling-deck";
    private const string Cost = "skill-program.boundary:qinling.cost";
    private const string Rediscard = "skill-program.fixture:qinling-reentry.SelectAndMoveOwnedCard";
    private const string Inverse = "skill-program.boundary:liu-feng.inverse";
    private static readonly string[] Generals = [Hero, "fixture:qinling-peer-1", "fixture:qinling-peer-2", "fixture:qinling-peer-3"];

    internal static void QinlingCannotStoreTheLaterEntryOfItsDiscardedCostCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, IsSetupOrPlay);
        while (P(game) is { } prompt && prompt.Kind != DecisionKind.PlayCard)
        {
            if (prompt.Kind == DecisionKind.SelectGeneral)
                Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, Generals[prompt.PlayerSeat], game.Revision, prompt.PromptId));
            else
                Skip(game);
            Reach(game, IsSetupOrPlay);
        }

        var view = game.CreateSnapshot(0);
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && view.DrawPileCount == 0 && view.DiscardPileCount == 0,
            "The real first draw empties the fixed stock, and Xiansi leaves the test card in Liu Feng's hand.");
        var card = view.Players[0].Hand.First(c => c.Kind == CardKind.Dodge).Id;
        Accept(game, new UseProgramSkillCommand(0, Qinling, "discard-for-native-slash", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, p => p.SkillPrompt?.SkillId == Qinling && p.Choices.Any(c => Branch(c) == "target"));
        Answer(game, c => Branch(c) == "target" && c.Targets.SequenceEqual([1]));
        Reach(game, p => p.SkillPrompt?.SkillId == Qinling && p.Choices.Any(c => Branch(c) == "cost"));
        Answer(game, c => Branch(c) == "cost" && c.Cards.SequenceEqual([card]));
        Reach(game, p => p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => Action(c) == "activate"));

        var original = game.CardMovements.Single(m => m.CardId == card && m.Reason.Value == Cost);
        var paid = Facts<BoundaryLiuFengCostPaidEvent>(game).Single();
        Require(original.From == CardLocation.Hand(0) && original.To == CardLocation.DiscardPile && paid.CardId == card &&
            paid.Sequence == original.Sequence && game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == paid.FrameId).BoundaryLiuFeng is
                { Stage: BoundaryLiuFengStage.MovementChildren, UseId: 0 } receipt && receipt.CostSequence == original.Sequence,
            "The exact once-paid native discard is retained while its real movement child pauses before Slash issuance.");

        var control = Cold(game, registry);
        Skip(control);
        var originalConsequence = DodgeAndSettle(control, paid.FrameId);
        var gained = control.CardMovements.Single(m => m.Reason.Value == Inverse);
        Require(originalConsequence.MovedCost && gained.CardId == card && gained.From == CardLocation.DiscardPile && gained.To == CardLocation.Authority(0),
            "After a real Dodge, Qinling must put its untouched original discard into the Inverse pile.");

        Activate(game);
        Reach(game, p => p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => Action(c) == "select-and-move-owned-card"));
        var movements = game.CardMovements.Where(m => m.CardId == card && m.Sequence > original.Sequence).ToArray();
        Require(movements is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0) && drawn.Reason.Value == $"skill-program.{Watch}.Draw",
            "The real draw child reshuffles the only discard and returns the same paid card to hand.");
        game = Cold(game, registry);
        Answer(game, c => c.Cards.SequenceEqual([card]));
        var newer = game.CardMovements.Single(m => m.CardId == card && m.Reason.Value == Rediscard);
        Require(newer.From == original.From && newer.To == original.To && newer.Sequence > original.Sequence && newer.BatchId != original.BatchId,
            "The real second discard is a new entry for the same physical card.");
        var consequence = DodgeAndSettle(game, paid.FrameId);
        Require(!consequence.MovedCost && Facts<BoundaryLiuFengCostPaidEvent>(game).SequenceEqual([paid]) &&
            !game.CardMovements.Any(m => m.Reason.Value == Inverse) &&
            game.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location == CardLocation.DiscardPile,
            $"Qinling may not store entry {newer.Sequence} using its earlier discard payment {original.Sequence}.");
        ReplayFourViews(game, registry);
    }

    private static BoundaryLiuFengConsequenceEvent DodgeAndSettle(GameEngine game, long frameId)
    {
        Reach(game, p => p is { PlayerSeat: 1, Kind: DecisionKind.RespondDodge });
        var defender = game.CreateSnapshot(1).Players[1];
        var dodge = defender.Hand.Single().Id;
        var origin = Facts<BoundaryLiuFengUseIssuedEvent>(game).Single().Origin;
        Require(origin.FrameId == frameId && game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == origin.UseId).Action is
            { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0 },
            "The paid Qinling really issues its one zero-material Slash with native response handling.");
        Answer(game, c => c.Cards.SequenceEqual([dodge]));
        Settle(game);
        var returned = Facts<BoundaryLiuFengUseReturnedEvent>(game).Single();
        Require(returned is { Damaged: false } && returned.UseId == origin.UseId && returned.FrameId == frameId &&
            Facts<CardRespondedEvent>(game).Count(e => e.CardId == dodge && e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            game.CreateSnapshot(1).Players[1].Hp == defender.Hp,
            "The target really Dodges, so the same paid Slash returns without dealing damage.");
        return Facts<BoundaryLiuFengConsequenceEvent>(game).Single();
    }

    private static bool IsSetupOrPlay(PendingDecision prompt) => prompt.Kind == DecisionKind.SelectGeneral ||
        prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || prompt.SkillPrompt?.SkillId == Xiansi;
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay must preserve the actual payment, movement child and every seat's prompt.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-qinling-reentry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new FixtureHeroBuilder(builder), Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"draw-then-rediscard","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{Cost}}}"],"optional":true,"effects":[
                      {"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "摸回重弃", description = "摸一张牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "shu", 4));
            builder.AddDeck(new(Deck, "六张实体闪", 1, 2, []) { PhysicalCards = Enumerable.Range(1, 6)
                .Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, rank)).ToArray() });
            builder.AddMode(new(Mode, "侵凌原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class FixtureHeroBuilder(IContentRegistryBuilder builder) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => builder.AddGeneral(general.Id == Hero
            ? general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Watch] } : general);
        public void AddSkill(ContentSkillDefinition skill) => builder.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => builder.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => builder.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => builder.AddMode(mode);
    }
}
