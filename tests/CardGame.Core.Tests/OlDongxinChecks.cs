using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlDongxinChecks
{
    private const string Dongxin = "ol:dongxin", Driver = "fixture:ol-dongxin-driver", Reentry = "fixture:ol-dongxin-reentry";
    private const string Mode = "identity:ol-dongxin-test", Deck = "fixture:ol-dongxin-deck";
    private const string PaymentReason = "skill-program.sun-yuan.dongxin.discard";
    private const string ClaimReason = "skill-program.fixture:ol-dongxin-reentry.ClaimMovedCards";
    private const string ReentryReason = "skill-program.fixture:ol-dongxin-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] Generals = ["fixture:ol-dongxin-owner", "fixture:ol-dongxin-peer", "fixture:ol-dongxin-other-2", "fixture:ol-dongxin-other-3"];

    public static void ReclaimRetainsOriginalPaidEntry()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Submit(game, new StartGameCommand());
        var prompt = Next(game);
        while (prompt.Kind == DecisionKind.SelectGeneral)
        {
            Submit(game, new SelectGeneralCommand(prompt.PlayerSeat, Generals[prompt.PlayerSeat], game.Revision, prompt.PromptId));
            prompt = Next(game);
        }

        Require(prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "Sun Yuan must reach the original play phase.");
        Submit(game, new UseProgramSkillCommand(0, Driver, "take-damage", [], [1], game.Revision, prompt.PromptId));
        Answer(game, 0, Dongxin, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        for (var index = 0; index < 2; index++)
            Answer(game, 0, Dongxin, choice => choice.Parameters.GetValueOrDefault("branch") == "card", firstMatch: true);

        prompt = Next(game);
        var paid = game.CardMovements.Where(move => move.Reason.Value == PaymentReason).ToArray();
        var taken = game.CardMovements.Single(move => move.Reason.Value == ClaimReason);
        var frame = game.Events.Select(e => e.Payload).OfType<SunYuanPaidEvent>().Single();
        Require(paid.Length == 2 && paid.All(move => move.To == CardLocation.DiscardPile) &&
            frame is { Direction: "received", Count: 2 } &&
            prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Reentry } &&
            !game.Events.Any(e => e.Payload is DongxinDamageIssuedEvent or DongxinEntityAcquiredEvent),
            "The two paid Slashes enter the pile before the discard child can re-discard one of them, retort, or reclaim.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        var survivor = paid.Single(move => move.CardId != taken.CardId);
        foreach (var run in new[] { game, restored })
        {
            Answer(run, 1, Reentry, choice => choice.Cards.SequenceEqual([taken.CardId]));
            Answer(run, 0, Dongxin, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
            Require(Next(run) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The original play phase resumes after the retort.");
            var newEntry = run.CardMovements.Single(move => move.CardId == taken.CardId && move.Reason.Value == ReentryReason);
            var grant = run.Events.Select(e => e.Payload).OfType<DongxinEntityAcquiredEvent>().Single();
            Require(newEntry.To == CardLocation.DiscardPile && newEntry.Sequence > taken.Sequence &&
                run.CreateCardZoneDiagnostics().Single(card => card.CardId == taken.CardId).Location == CardLocation.DiscardPile &&
                grant.FrameId == frame.FrameId && grant.CardId == survivor.CardId && grant.DiscardSequence == survivor.Sequence &&
                run.CardMovements.Any(move => move.CardId == grant.CardId && move.Sequence == grant.AcquiredSequence &&
                    move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0)) &&
                run.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == grant.CardId && card.SkillMarkLabel == "恫心"),
                "Only the surviving original entry yields a reclaim movement and a mark bound to its original paid sequence.");
        }
    }

    private static void Answer(GameEngine game, int seat, string skill, Func<PromptChoice, bool> predicate, bool firstMatch = false)
    {
        var prompt = Next(game);
        Require(prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == skill, $"Expected {skill} at seat {seat}. Got {prompt.SkillPrompt?.SkillId} at seat {prompt.PlayerSeat}.");
        var choice = firstMatch ? prompt.Choices.First(predicate) : prompt.Choices.Single(predicate);
        Submit(game, new AnswerPromptCommand(seat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            for (var seat = 0; seat < game.PlayerCount; seat++)
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Dongxin did not reach the next public command boundary.");
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "Dongxin fixture command rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-dongxin", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            RegisterSkill(builder, CharacterFixtureContent.EmbeddedProgramBundle("ol-sun-yuan"), Dongxin);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                    {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"take-damage","minCards":0,"maxCards":0,
                        "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,"effects":[
                        {"op":"damage","target":"owner","amount":1,"sourceRef":{"kind":"selectedTarget"}}]}]},
                    {"id":"{{{Reentry}}}","revision":1,"triggers":[{"id":"claim-and-rediscard","window":"discardPileReceived",
                        "subject":"owner","discardOwnerScope":"other","movementOccurrence":"perCard","movementReasons":["{{{PaymentReason}}}"],
                        "optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"claimMovedCards","target":"owner"},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                         "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "承伤", description = "令其他角色对你造成一点伤害。" },
                    [Reentry] = new { name = "取回重弃", description = "获得首张恫心弃牌，然后弃置一张手牌。" }
                } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            RegisterSkill(builder, catalog, Driver);
            RegisterSkill(builder, catalog, Reentry);
            builder.AddGeneral(new(Generals[0], "恫心角色", "supporter", Dongxin, "wu", 8, [Driver]));
            builder.AddGeneral(new(Generals[1], "伤害来源", "supporter", Reentry, "wei", 8));
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "恫心弃牌入区", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }

        private static void RegisterSkill(IContentRegistryBuilder builder, SkillProgramCatalog catalog, string id)
        {
            var definition = catalog.Presentations[id];
            builder.AddSkill(new(id, definition.Name, definition.Description)
                { Program = catalog.Programs[id], ProgramPresentation = definition });
        }
    }
}
