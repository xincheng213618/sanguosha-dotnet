using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShiHuangZuDiscardEntryChecks
{
    private const string Hero = "mobile:shi-huang-zu", Duanyang = "mobile:shi-duanyang";
    private const string Driver = "fixture:duanyang-reentry", Mode = "identity:duanyang-reentry", Deck = "fixture:duanyang-deck";
    private const string OriginalDiscard = "skill-program.fixture:duanyang-reentry.MoveBoundCards";
    private const string Rediscard = "skill-program.fixture:duanyang-reentry.SelectAndMoveOwnedCard";
    private const string Store = "skill-program.mobile-shi-huang-zu.store";
    private static readonly string[] Generals = [Hero, "fixture:duanyang-peer-1", "fixture:duanyang-peer-2", "fixture:duanyang-peer-3"];

    internal static void DuanyangCannotStoreTheNewEntryThroughAnOldDiscardWindowCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        ReachSetupOrPlay();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } setup)
        {
            Accept(game, new SelectGeneralCommand(setup.PlayerSeat, Generals[setup.PlayerSeat], game.Revision, setup.PromptId));
            ReachSetupOrPlay();
        }

        var initial = game.CreateSnapshot(0);
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && initial.DrawPileCount == 0 && initial.DiscardPileCount == 0,
            "The first draw exhausts the fixed deck before the hand Slash is discarded.");
        var card = initial.Players[0].Hand.First(c => c.Kind == CardKind.Slash).Id;
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard", [card], [], game.Revision, P(game)!.PromptId));
        Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Driver } && p.Choices.Any(c => Action(c, "activate")));
        var original = game.CardMovements.Single(m => m.CardId == card && m.Reason.Value == OriginalDiscard);
        Require(original.From == CardLocation.Hand(0) && original.To == CardLocation.DiscardPile &&
            game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(f => f.Batch.Id == original.BatchId).Candidates
                .Any(c => c.SkillId == Duanyang), "The original hand discard queues Duanyang behind the earlier movement observer.");

        var control = Cold(game, registry);
        Skip(control);
        Reach(control, IsDuanyang);
        Require(IsOriginalBatch(control, original.BatchId), "An untouched Slash is offered by its original discard batch.");
        Activate(control);
        Settle(control);
        var grant = Facts<ShiHzStoredEvent>(control).Single();
        Require(grant.CardId == card && grant.BatchId == original.BatchId && grant.DiscardSequence == original.Sequence &&
            grant.CandidateCount == 1 && control.CardMovements.Single(m => m.Reason.Value == Store).To == grant.Pile.Location,
            "Duanyang really stores one original Slash when its original discard entry is untouched.");

        Activate(game);
        Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Driver } && p.Choices.Any(c => Action(c, "select-and-move-owned-card")));
        var returned = game.CardMovements.Where(m => m.CardId == card && m.Sequence > original.Sequence).ToArray();
        Require(returned is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0) && drawn.Reason.Value == $"skill-program.{Driver}.Draw",
            "The earlier movement child draws the same Slash through real deck reshuffling before the new discard is chosen.");
        game = Cold(game, registry);
        Answer(game, c => c.Cards.SequenceEqual([card]));
        var fresh = game.CardMovements.Single(m => m.CardId == card && m.Reason.Value == Rediscard);
        Reach(game, IsDuanyang);
        Require(fresh.From == original.From && fresh.To == original.To && fresh.Sequence > original.Sequence &&
            fresh.BatchId != original.BatchId && IsOriginalBatch(game, fresh.BatchId),
            "The fresh hand discard must still offer Duanyang from its own nested movement batch.");
        game = Cold(game, registry);
        Skip(game);
        Reach(game, p => IsDuanyang(p) || p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        if (P(game) is { } next && IsDuanyang(next))
        {
            Require(IsOriginalBatch(game, original.BatchId), "Any remaining Duanyang offer must be the resumed original batch.");
            Activate(game);
            Settle(game);
        }
        Require(!Facts<ShiHzStoredEvent>(game).Any() && !game.CardMovements.Any(m => m.Reason.Value == Store) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Duanyang cannot store the newer discard entry {fresh.Sequence} through the old window for entry {original.Sequence}.");
        ReplayFourViews(game, registry);

        void ReachSetupOrPlay() => Reach(game, p => p.Kind == DecisionKind.SelectGeneral || p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    }

    private static bool IsDuanyang(PendingDecision prompt) => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Duanyang };
    private static bool Action(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;
    private static bool IsOriginalBatch(GameEngine game, long batchId) =>
        game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()?.Batch.Id == batchId;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay preserves the original movement window, private choice and every physical move.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-duanyang-reentry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new FixtureHeroBuilder(builder), Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"discard","minCards":1,"maxCards":1,"sourceZones":["hand"],
                  "minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                    {"op":"captureSelectedCards","target":"owner","resultBind":"original"},
                    {"op":"moveBoundCards","target":"owner","sourceBind":"original","destination":"discardPile","awaitMovementTriggers":true}]}],
                  "triggers":[{"id":"draw-then-rediscard","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch",
                    "movementReasons":["{{{OriginalDiscard}}}"],"optional":true,"effects":[
                      {"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "摸回重弃", description = "弃置手牌，或摸回并重新弃置。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "六张实体杀", 1, 2, []) { PhysicalCards = Enumerable.Range(1, 6)
                .Select(rank => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, rank)).ToArray() });
            builder.AddMode(new(Mode, "断鞅原入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class FixtureHeroBuilder(IContentRegistryBuilder builder) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => builder.AddGeneral(general.Id == Hero
            ? general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Driver] } : general);
        public void AddSkill(ContentSkillDefinition skill) => builder.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => builder.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => builder.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => builder.AddMode(mode);
    }
}
