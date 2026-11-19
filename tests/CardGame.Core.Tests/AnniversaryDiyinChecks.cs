using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class AnniversaryDiyinChecks
{
    private const string Hero = "anniversary:xun-xu", Diyin = "anniversary:diyin";
    private const string Driver = "fixture:diyin-driver", Reentry = "fixture:diyin-reentry", Peer = "fixture:diyin-peer", Deck = "fixture:diyin-deck";
    private const string Claim = "skill.anniversary.diyin.claim";
    private const string InitialDiscard = "skill-program.fixture:diyin-driver.SelectAndMoveOwnedCard";
    private const string LaterDiscard = "skill-program.fixture:diyin-reentry.SelectAndMoveOwnedCard";

    internal static void SeventhLossClaimsOnlyTheNextOriginalDiscardEntry()
    {
        var (game, content) = Start();
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var basics = hand.Where(card => card.Kind == CardKind.Dodge && card.Suit == Suit.Heart).Select(card => card.Id).ToArray();
        var equipment = hand.Single(card => card.Kind == CardKind.Crossbow).Id;
        Require(basics.Length == 7, "The fixture provides seven original basic cards for separate native losses.");
        foreach (var card in basics)
            Discard(game, card);
        var losses = game.Events.Select(e => e.Payload).OfType<AnniversaryXunXuLossEvent>().ToArray();
        var seventh = game.CardMovements.Single(move => move.CardId == basics[^1] && move.To == CardLocation.DiscardPile);
        Require(losses.Select(loss => loss.Occurrence).SequenceEqual(Enumerable.Range(1, 7)) &&
            losses[^1].BatchId == seventh.BatchId && Zone(game, seventh.CardId) == CardLocation.DiscardPile &&
            game.Events.All(e => e.Payload is not AnniversaryXunXuClaimEvent),
            "The seventh actual loss arms Diyin and leaves the seventh batch's own card in the discard pile.");

        var ordinary = Restore(game, content);
        Discard(ordinary, equipment);
        var next = ordinary.CardMovements.Single(move => move.CardId == equipment && move.To == CardLocation.DiscardPile);
        var gain = ordinary.CardMovements.Single(move => move.Reason.Value == Claim);
        AssertClaimBatch(ordinary, next);
        Require(gain.CardId == equipment && gain.From == CardLocation.DiscardPile && gain.To == CardLocation.Hand(0) &&
            gain.Sequence > next.Sequence && Zone(ordinary, equipment) == CardLocation.Hand(0) &&
            Zone(ordinary, seventh.CardId) == CardLocation.DiscardPile,
            "After cold restore, Diyin obtains the next original entry exactly once.");

        Accept(game, new UseProgramSkillCommand(0, Driver, "discard-with-reentry", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Driver } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([equipment])));
        Answer(game, choice => choice.Cards.SequenceEqual([equipment]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Reentry } && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([equipment])));
        var original = game.CardMovements.Single(move => move.CardId == equipment && move.Reason.Value == InitialDiscard);
        Require(game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Batch.Id == original.BatchId &&
                frame.Candidates.Any(candidate => candidate.SkillId == Diyin)) && Zone(game, equipment) == CardLocation.Hand(0),
            "The earlier movement observer obtains the same entity while the original Diyin candidate is queued.");
        AssertClaimBatch(game, original);

        game = Restore(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([equipment]));
        Settle(game);
        var later = game.CardMovements.Single(move => move.CardId == equipment && move.Reason.Value == LaterDiscard);
        AssertClaimBatch(game, original);
        Require(later.From == CardLocation.Hand(0) && later.To == CardLocation.DiscardPile &&
            later.BatchId != original.BatchId && later.Sequence > original.Sequence &&
            game.CardMovements.All(move => move.Reason.Value != Claim) && Zone(game, equipment) == CardLocation.DiscardPile,
            "The original Diyin opportunity cannot reclaim the same entity at its later discard-pile entry.");

        void AssertClaimBatch(GameEngine run, CardMovementRecord entry)
        {
            var claim = run.Events.Select(e => e.Payload).OfType<AnniversaryXunXuClaimEvent>().Single();
            Require(claim.BatchId == entry.BatchId && claim.BatchId != seventh.BatchId && claim.ThroughSeventh == 1 &&
                claim.SequenceBefore >= seventh.Sequence && claim.SequenceAfter == entry.Sequence,
                "Diyin reserves the first later batch for the existing seventh-loss threshold.");
        }
    }

    private static void Discard(GameEngine game, int id)
    {
        Accept(game, new UseProgramSkillCommand(0, Driver, "discard", [id], [], game.Revision, P(game)!.PromptId));
        Settle(game);
    }

    private static CardLocation Zone(GameEngine game, int id) => game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location;

    private static GameEngine Restore(GameEngine game, ContentRegistry content)
    {
        var replay = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(replay.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(replay.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the public command prefix, current prompts and physical movement entries.");
        return replay;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var hand = Enumerable.Range(1, 7).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Heart, rank))
            .Append(new(CardKind.Crossbow, Suit.Club, 8)).ToArray();
        var rivals = Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] }).ToArray();
        var setup = new ShanheBattleSetup(373, 1, "diyin-original-entry", Hero, 3, 0, 0, 0, 0, [Driver, Reentry], hand, [], rivals, [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Settle(game);
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-diyin-original-entry", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"discard","minCards":1,"maxCards":1,"minTargets":0,"maxTargets":0,"sourceZones":["hand"],
                     "targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]},
                    {"id":"discard-with-reentry","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                       "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{Reentry}}}","revision":1,"triggers":[{"id":"recover-before-diyin","window":"cardsMoved","subject":"owner",
                     "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{InitialDiscard}}}"],"optional":false,"effects":[
                       {"op":"selectRandomPileEquipment","target":"owner","amount":1,"resultBind":"recovered","zones":["discardPile"]},
                       {"op":"moveBoundCards","target":"owner","sourceBind":"recovered","destination":"ownerHand","awaitMovementTriggers":true},
                       {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "弃置", description = "以真实手牌产生独立弃牌批。" },
                    [Reentry] = new { name = "取回重弃", description = "取得原弃牌批中的装备，再实际弃置。" }
                } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var id in new[] { Driver, Reentry })
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Peer, "实际同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 10)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "笛音原入堆", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
