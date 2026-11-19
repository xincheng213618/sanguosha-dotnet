using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class FuRongDiscardEntryChecks
{
    private const string Hero = "ol:fu-rong", Xiaosi = "ol:xiaosi";
    private const string Claim = "fixture:fu-rong-claim-payment", Rediscard = "fixture:fu-rong-rediscard-payment";
    private const string Mode = "identity:fu-rong-discard-entry", Deck = "fixture:fu-rong-discard-deck";
    private const string PaymentReason = "skill-program.fu-rong.xiaosi.discard";
    private const string ClaimReason = "skill-program.fixture:fu-rong-claim-payment.ClaimMovedCards";
    private const string RediscardReason = "skill-program.fixture:fu-rong-rediscard-payment.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = [Hero, "fixture:fu-rong-collector", "fixture:fu-rong-peer-2", "fixture:fu-rong-peer-3"];

    public static void XiaosiOffersOnlyTheOriginalPaymentEntries()
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

        var ownerCard = game.CreateSnapshot(0).Players[0].Hand[0].Id;
        var targetCard = game.CreateSnapshot(1).Players[1].Hand[0].Id;
        Accept(game, new UseProgramSkillCommand(0, Xiaosi, "paired-basic-discard", [], [1], game.Revision, P(game)!.PromptId));
        Answer(game, c => Branch(c) == "cost" && c.Cards.SequenceEqual([ownerCard]));
        Reach(game, p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Claim });
        var original = game.CardMovements.Single(m => m.CardId == ownerCard && m.Reason.Value == PaymentReason);
        Require(original.From == CardLocation.Hand(0) && original.To == CardLocation.DiscardPile &&
            Events<FuRongPairPaidEvent>(game).Single() is { Owner: true } paid && paid.Payment.CardId == ownerCard &&
            paid.Payment.Sequence == original.Sequence, "The original Xiaosi payment must authorize its own native discard entry.");

        var control = Cold(game, registry);
        Answer(control, c => Action(c, "skip"));
        PayTarget(control, targetCard);
        Require(UseCards(control).SetEquals([ownerCard, targetCard]),
            "If the original owner payment stays put, both real discard entries must be usable.");

        Answer(game, c => Action(c, "activate"));
        Reach(game, p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Rediscard });
        Require(game.CardMovements.Any(m => m.CardId == ownerCard && m.Reason.Value == ClaimReason && m.Sequence > original.Sequence &&
            m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(1)),
            "A real movement child must obtain the paid Slash before its rediscard prompt opens.");
        var repaid = game;
        game = Cold(game, registry);
        Answer(game, c => c.Cards.SequenceEqual([ownerCard]));
        PayTarget(game, targetCard);
        var reentry = game.CardMovements.Single(m => m.CardId == ownerCard && m.Reason.Value == RediscardReason);
        Require(reentry.From == CardLocation.Hand(1) && reentry.To == CardLocation.DiscardPile && reentry.Sequence > original.Sequence &&
            Events<FuRongPairPaidEvent>(game).Select(p => p.Payment.CardId).SequenceEqual([ownerCard, targetCard]) &&
            UseCards(game).SetEquals([targetCard]), "Xiaosi cannot offer the rediscarded Slash under the original owner's payment.");

        var legacyChoice = UseChoices(control).First(c => c.Cards.SequenceEqual([ownerCard])).Id;
        var obsoleteAnswer = new AnswerPromptCommand(0, P(game)!.PromptId, legacyChoice, game.Revision);
        var checkpoint = game.CreateCheckpoint();
        var obsoleteCheckpoint = checkpoint with
        {
            Revision = checkpoint.Revision + 1,
            Commands = checkpoint.Commands.Append(obsoleteAnswer).ToArray()
        };
        try
        {
            _ = GameReplay.Restore(obsoleteCheckpoint, registry);
            throw new InvalidOperationException("Replay accepted a retired Xiaosi action ordinal.");
        }
        catch (InvalidOperationException error) when (error.Message.StartsWith(
            $"Replay command {checkpoint.Revision} (AnswerPromptCommand) was rejected", StringComparison.Ordinal)) { }

        Answer(game, c => Branch(c) == "use" && c.Cards.SequenceEqual([targetCard]));
        Require(Events<FuRongNativeIssuedEvent>(game).Single().Return.CardId == targetCard,
            "The surviving choice must use the target's actual payment after the legacy choice is rejected.");
        ReplayFourViews(game, registry);

        Answer(repaid, c => c.Cards.SequenceEqual([targetCard]));
        PayTarget(repaid, ownerCard);
        var payments = Events<FuRongPairPaidEvent>(repaid);
        var latest = repaid.CardMovements.Last(m => m.CardId == ownerCard);
        Require(payments.Length == 2 && payments[0].Payment.Sequence == original.Sequence &&
            payments[1] is { Owner: false } && payments[1].Payment.CardId == ownerCard && payments[1].Payment.Sequence == latest.Sequence &&
            latest.From == CardLocation.Hand(1) && latest.To == CardLocation.DiscardPile && latest.Reason.Value == PaymentReason &&
            UseCards(repaid).SetEquals([ownerCard]) && UseChoices(repaid).Count() == UseChoices(control).Count(c => c.Cards.Contains(ownerCard)),
            "When the target pays with the same physical Slash, its new Xiaosi payment must offer each native action once.");
        Answer(repaid, c => Branch(c) == "use" && c.Cards.SequenceEqual([ownerCard]));
        Require(Events<FuRongNativeIssuedEvent>(repaid).Single().Return.CardId == ownerCard,
            "The new payment for the same Slash must bind an actual native use.");

        void ReachSetupOrPlay() => Reach(game, p => p.Kind == DecisionKind.SelectGeneral || p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    }

    private static void PayTarget(GameEngine game, int cardId)
    {
        Reach(game, p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Xiaosi } && p.Choices.Any(c => Branch(c) == "cost"));
        Answer(game, c => Branch(c) == "cost" && c.Cards.SequenceEqual([cardId]));
        Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Xiaosi } && p.Choices.Any(c => Branch(c) == "use"));
    }

    private static IEnumerable<PromptChoice> UseChoices(GameEngine game) => P(game)!.Choices.Where(c => Branch(c) == "use");
    private static HashSet<int> UseCards(GameEngine game) => UseChoices(game).Select(c => c.Cards.Single()).ToHashSet();

    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static bool Action(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;
    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Enumerable.Range(0, 4).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay must preserve both the live payment movement and the original child prompt.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-fu-rong-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Claim}}}","revision":1,"triggers":[{"id":"take-payment","window":"discardPileReceived","subject":"owner",
                    "discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{PaymentReason}}}"],"optional":true,
                    "effects":[{"op":"claimMovedCards","target":"owner"},{"op":"awaitBoundCardMovements","target":"owner"}]}]},
                  {"id":"{{{Rediscard}}}","revision":1,"triggers":[{"id":"rediscard-payment","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{ClaimReason}}}"],"optional":false,
                    "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { Claim, Rediscard }.ToDictionary(id => id, id => new { name = id, description = "真实领取效死付款并可再次弃置。" }) });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Generals[1], "取回效死弃牌", "supporter", Claim, "wei", 6, [Rediscard]));
            foreach (var peer in Generals.Skip(2))
                builder.AddGeneral(new(peer, "测试同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定实体杀", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 64)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(Mode, "效死原始付款", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
