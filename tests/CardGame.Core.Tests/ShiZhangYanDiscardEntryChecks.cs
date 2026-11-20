using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShiZhangYanDiscardEntryChecks
{
    private const string Hero = "mobile:shi-zhang-yan", Feijing = "mobile:shi-feijing", Xiaoge = "mobile:shi-xiaoge";
    private const string Watch = "fixture:xiaoge-reentry", Mode = "identity:xiaoge-reentry", Deck = "fixture:xiaoge-deck";
    private const string Discard = "skill.mobile-shi-zhang-yan.feijing.discard";
    private const string Rediscard = "skill-program.fixture:xiaoge-reentry.SelectAndMoveOwnedCard";
    private const string Obtain = "skill.mobile-shi-zhang-yan.xiaoge.obtain";
    private static readonly string[] Generals = [Hero, "fixture:xiaoge-extra", "fixture:xiaoge-target", "fixture:xiaoge-peer"];

    internal static void XiaogeCannotObtainANewerEntryOfTheFeijingPaymentCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, IsSetupOrPlay);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } grand)
        {
            Accept(game, new SelectGeneralCommand(grand.PlayerSeat, Generals[grand.PlayerSeat], game.Revision, grand.PromptId));
            Reach(game, IsSetupOrPlay);
        }

        var start = game.CreateSnapshot(0);
        var extra = game.CreateSnapshot(1).Players[1];
        Require(start.DrawPileCount == 0 && start.DiscardPileCount == 0 && start.Players[0].Hp == start.Players[0].MaxHp - 1 &&
            extra.Hand is [{ Kind: CardKind.Slash }], "The real opening leaves Zhang Yan wounded and one original black Slash in the path player's hand.");
        var card = extra.Hand.Single().Id;
        var slash = game.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([2]));
        Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, P(game)!.PromptId));
        Reach(game, p => p.SkillPrompt?.SkillId == Feijing && p.Choices.Any(c => Action(c) == "activate"));
        Activate(game);
        Answer(game, c => Branch(c) == "path" && Token(c) == "1");
        Reach(game, p => p is { PlayerSeat: 1, IsPrivate: true, SkillPrompt.SkillId: Feijing } && p.Choices.Any(c => Branch(c) == "commit"));
        Answer(game, c => Branch(c) == "commit" && c.Cards.SequenceEqual([card]));
        Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Feijing } && p.Choices.Any(c => Branch(c) == "color"));
        Answer(game, c => Branch(c) == "color" && Token(c) == "black");
        Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && p.Choices.Any(c => Action(c) == "activate"));

        var original = game.CardMovements.Single(m => m.CardId == card && m.Reason.Value == Discard);
        var payment = Facts<ShiZyHandDiscardedEvent>(game).Single();
        var added = Facts<ShiZyTargetAddedEvent>(game).Single();
        var prevented = Facts<ShiZyPreventedEvent>(game).Single();
        var receipt = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == prevented.FrameId).ShiZhangYan;
        Require(original.From == CardLocation.Hand(1) && original.To == CardLocation.DiscardPile && payment.Seat == 1 &&
            payment.CardId == card && payment.Sequence == original.Sequence && added.Target == 1 && added.CardId == card &&
            added.Sequence == original.Sequence && prevented.Target == 1 && prevented.UseId == added.UseId &&
            prevented.CardId == card && prevented.PaymentSequence == original.Sequence &&
            receipt is { Stage: ShiZyStage.RecoverChildren } && receipt.PaymentSequence == original.Sequence &&
            Facts<RecoveryAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 0, Amount: 1 },
            "Feijing pays its exact card and adds the extra target; Xiaoge prevents that damage and really recovers before obtaining.");

        var control = Cold(game, registry);
        Skip(control);
        Settle(control);
        var obtained = control.CardMovements.Single(m => m.Reason.Value == Obtain);
        Require(obtained.CardId == card && obtained.From == CardLocation.DiscardPile && obtained.To == CardLocation.Hand(0),
            "An untouched original Feijing payment is genuinely obtained after Xiaoge's recovery child returns.");

        Activate(game);
        Reach(game, p => p.SkillPrompt?.SkillId == Watch && p.Choices.Any(c => Action(c) == "select-and-move-owned-card"));
        var movements = game.CardMovements.Where(m => m.CardId == card && m.Sequence > original.Sequence).ToArray();
        Require(movements is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0) && drawn.Reason.Value == $"skill-program.{Watch}.Draw",
            "The paid recovery child really draws the original Feijing card out of the discard pile through a reshuffle.");
        game = Cold(game, registry);
        Answer(game, c => c.Cards.SequenceEqual([card]));
        Settle(game);
        var fresh = game.CardMovements.Single(m => m.CardId == card && m.Reason.Value == Rediscard);
        Require(fresh.From == CardLocation.Hand(0) && fresh.To == CardLocation.DiscardPile && fresh.Sequence > original.Sequence &&
            fresh.BatchId != original.BatchId && Facts<ShiZyHandDiscardedEvent>(game).SequenceEqual([payment]) &&
            Facts<ShiZyPreventedEvent>(game).SequenceEqual([prevented]) && Facts<RecoveryAppliedEvent>(game).Count() == 1 &&
            game.CreateSnapshot(1).Players[1].Hp == extra.Hp && !Facts<ShiZyDuelIssuedEvent>(game).Any(),
            "Cold continuation preserves the exact damage prevention and recovery while the newer discard is a separate movement.");
        Require(!game.CardMovements.Any(m => m.Reason.Value == Obtain) &&
            game.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location == CardLocation.DiscardPile,
            $"Xiaoge may not obtain discard entry {fresh.Sequence} using the original Feijing payment at entry {original.Sequence}.");
        ReplayFourViews(game, registry);
    }

    private static bool IsSetupOrPlay(PendingDecision prompt) => prompt.Kind == DecisionKind.SelectGeneral ||
        prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static string? Token(PromptChoice choice) => choice.Parameters.GetValueOrDefault("token");
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay must preserve the live paid recovery child and all visible state.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shi-xiaoge-reentry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new FixtureHeroBuilder(builder), Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,
                  "modifiers":[{"id":"reach-path","query":"attackRange","operation":"add","value":1,"priority":0}],
                  "triggers":[{"id":"opening-wound","window":"gameStarting","subject":"owner","optional":false,
                    "effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"draw-then-rediscard","window":"afterHealthChanged","subject":"owner","optional":true,
                    "condition":{"kind":"healthChangeKindIs","hpChangeKind":"recovery"},"effects":[
                      {"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "回复后摸回重弃", description = "回复后摸一张牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "六张实体杀", 1, 2, []) { PhysicalCards = Enumerable.Range(1, 6)
                .Select(rank => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, rank)).ToArray() });
            builder.AddMode(new(Mode, "骁戈原入堆", 4, 4, new Dictionary<string, int>
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
