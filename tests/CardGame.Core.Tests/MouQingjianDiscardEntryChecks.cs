using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MouQingjianDiscardEntryChecks
{
    private const string Owner = "mou:xiahou-dun-mobile", Qingjian = "mou-mobile:qingjian";
    private const string Driver = "fixture:mou-qingjian-reentry", Peer = "fixture:mou-qingjian-peer";
    private const string Deck = "fixture:mou-qingjian-deck";
    private const string InitialDiscard = "skill-program.fixture:mou-qingjian-reentry.MoveBoundCards";
    private const string Rediscard = "skill-program.fixture:mou-qingjian-reentry.SelectAndMoveOwnedCard";

    public static void QingjianCannotCollectTheNewEntryThroughAnOldDiscardWindow()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(47, 1, "mou-qingjian-old-entry", Owner, 3, 0, 0, 0, 0,
            [Driver], [new(CardKind.DrawTwo, Suit.Diamond, 1), new(CardKind.Dodge, Suit.Heart, 2),
                new(CardKind.BaguaFormation, Suit.Club, 3)], [], [new(Peer, Hp: 6), new(Peer, Hp: 6), new(Peer, Hp: 6)], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Settle(game);

        var use = game.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.DrawTwo && a.ConversionSource is null);
        Accept(game, new PlayCardCommand(0, use.CardId!.Value, use.TargetSeats, game.Revision, P(game)!.PromptId));
        Settle(game);
        var used = game.CardMovements.Single(m => m.CardId == use.CardId && m.To == CardLocation.DiscardPile);
        Require(Events<MouXiahouDunMobileDiscardOriginEvent>(game).Contains(new(used.BatchId, used.Sequence, used.CardId, true)) &&
            Events<MouXiahouDunMobileCollectedEvent>(game).Length == 0, "A true Draw Two use must settle in the discard pile without entering Qingjian.");

        var seed = game.CreateSnapshot(0).Players[0].Hand.Single(c => c is { Kind: CardKind.Dodge, Suit: Suit.Heart }).Id;
        Use(game, "prime", seed);
        Settle(game);
        var initialClaim = Events<MouXiahouDunMobileCollectedEvent>(game).Single();
        Require(initialClaim is { Capacity: 2 } && initialClaim.CardId == seed,
            "At HP 3 the first real non-use discard fills one of Qingjian's two slots.");

        var probe = game.CreateSnapshot(0).Players[0].Hand.Single(c => c.Kind == CardKind.BaguaFormation).Id;
        Use(game, "discard-probe", probe);
        Reach(game, p => p.SkillPrompt?.SkillId == Driver && p.Choices.Any(c => Action(c, "activate")));
        var original = game.CardMovements.Single(m => m.CardId == probe && m.Reason.Value == InitialDiscard);
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(f => f.Batch.Id == original.BatchId);
        Require(window.Candidates.Any(c => c.SkillId == Qingjian && window.Batch.Movements[c.OccurrenceIndex] == original),
            "The original non-use discard must freeze Qingjian behind the earlier recovery trigger.");

        var control = Cold(game, content);
        Answer(control, c => Action(c, "skip"));
        Settle(control);
        Require(Events<MouXiahouDunMobileCollectedEvent>(control) is [_, { Capacity: 2 } collect] &&
                collect.CardId == probe && collect.DiscardSequence == original.Sequence,
            "Without recovery, Qingjian must collect the original probe and record its original discard sequence.");

        Answer(game, c => Action(c, "activate"));
        Reach(game, p => p.PlayerSeat == 0 && p.Choices.Any(c => Action(c, "select-and-move-owned-card")));
        Require(game.CreateSnapshot(0).Players[0].Hp == 2 && game.CreateSnapshot(0).Players[0].Hand.Any(c => c.Id == probe) &&
            game.CardMovements.Any(m => m.CardId == probe && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(0)),
            "The earlier child must lower the capacity to one and deliver the exact outer discard back to hand.");
        game = Cold(game, content);
        Answer(game, c => c.Cards.SequenceEqual([probe]));
        Settle(game);
        var later = game.CardMovements.Single(m => m.CardId == probe && m.Reason.Value == Rediscard);
        Require(later.From == CardLocation.Hand(0) && later.To == CardLocation.DiscardPile && later.Sequence > original.Sequence &&
                Events<MouXiahouDunMobileDiscardOriginEvent>(game).Contains(new(later.BatchId, later.Sequence, probe, false)) &&
                game.CreateSnapshot(0).Players[0].Hp == 3 && Events<MouXiahouDunMobileCollectedEvent>(game).SequenceEqual([initialClaim]) &&
                game.CreateCardZoneDiagnostics().Single(c => c.CardId == probe).Location == CardLocation.DiscardPile,
            "The fresh entry was ineligible at full capacity and the resumed outer window cannot take it after HP recovers.");
        ReplayFourViews(game, content);
    }

    private static void Use(GameEngine game, string action, int card) =>
        Accept(game, new UseProgramSkillCommand(0, Driver, action, [card], [], game.Revision, P(game)!.PromptId));

    private static bool Action(PromptChoice choice, string action) => choice.Parameters.GetValueOrDefault("program-action") == action;

    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) &&
                Enumerable.Range(0, game.PlayerCount).All(seat =>
                    SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold replay must preserve the unpaid nested choice and original discard movements.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-qingjian-reentry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Owner);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,
                  "activations":[
                    {"id":"prime","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                    {"id":"discard-probe","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                      "effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"original"},
                        {"op":"moveBoundCards","target":"owner","sourceBind":"original","destination":"discardPile","awaitMovementTriggers":true}]}],
                  "triggers":[{"id":"recover-and-rediscard","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch",
                    "movementReasons":["{{{InitialDiscard}}}"],"optional":true,"effects":[
                      {"op":"loseHp","target":"owner","amount":1},
                      {"op":"selectRandomPileEquipment","target":"owner","amount":1,"resultBind":"recovered","zones":["discardPile"]},
                      {"op":"moveBoundCards","target":"owner","sourceBind":"recovered","destination":"ownerHand","awaitMovementTriggers":true},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true},
                      {"op":"recoverTo","target":"owner","numberExpression":"integerConstant","minimumValue":3,"clampToMaxHp":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "取回重弃", description = "暂时降低体力并取回弃牌再弃。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Peer, "测试同桌", "", "standard:none", "qun", 6));
            builder.AddDeck(new(Deck, "固定摸牌", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 10)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "清俭原弃置窗口", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
