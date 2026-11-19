using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class OlBoundaryZongxuanChecks
{
    private const string CaoXian = "ol:cao-xian", YuFan = "boundary:yu-fan", Zongxuan = "boundary:zongxuan-current";
    private const string Driver = "fixture:ol-zongxuan-driver", Peer = "fixture:ol-zongxuan-peer", Deck = "fixture:ol-zongxuan-deck";
    private const string ZhifouDiscard = "skill-program.ol:lingxi-or-zhifou.discard";
    private const string PutOnTop = "skill-program.boundary:zongxuan-current.PutDiscardedCardsOnDrawPileTop";
    private const string LaterDiscard = "skill-program.fixture:ol-zongxuan-driver.SelectAndMoveOwnedCard";
    private const string ReentryMode = "identity:ol-zongxuan-reentry", ReentryDeck = "fixture:ol-zongxuan-reentry-deck";
    private const string FirstDiscardSkill = "fixture:ol-zongxuan-first-discard", ReentrySkill = "fixture:ol-zongxuan-reentry";
    private const string FirstDiscardReason = "skill-program.fixture:ol-zongxuan-first-discard.SelectAndMoveOwnedCard";
    private const string ReentryDiscardReason = "skill-program.fixture:ol-zongxuan-reentry.SelectAndMoveOwnedCard";
    private static readonly string[] ReentryGenerals = ["fixture:ol-zongxuan-upper-seat", YuFan,
        "fixture:ol-zongxuan-reentry-peer-2", "fixture:ol-zongxuan-reentry-peer-3"];

    internal static void FirstPreviousDiscardCannotPlaceTheSameCardAtANewerEntryCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new ReentryFixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = ReentryMode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, ReentryGenerals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }

        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The upper seat starts the actual Play phase.");
        Accept(game, new UseProgramSkillCommand(0, FirstDiscardSkill, "discard", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: FirstDiscardSkill });
        var card = P(game)!.Choices.First(choice => choice.Cards.Count == 1).Cards.Single();
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: ReentrySkill });

        var original = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == FirstDiscardReason);
        var originalWindow = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(window => window.Batch.Id == original.BatchId);
        Require(original.From == CardLocation.Hand(0) && original.To == CardLocation.DiscardPile &&
            originalWindow.Candidates.Any(candidate => candidate.OwnerSeat == 1 && candidate.SkillId == Zongxuan),
            "The next seat's Zongxuan is queued on the upper seat's first discard batch.");

        var control = ColdReentry(game, content);
        Skip(control);
        Reach(control, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Zongxuan });
        Activate(control);
        Reach(control, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Zongxuan } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        Answer(control, choice => choice.Cards.SequenceEqual([card]));
        Answer(control, choice => choice.Parameters.GetValueOrDefault("program-action") == "discard-top-finish");
        Settle(control);
        var placed = control.CardMovements.Single(move => move.Reason.Value == PutOnTop);
        Require(placed.CardId == card && placed.From == CardLocation.DiscardPile && placed.To == CardLocation.DrawPile &&
            placed.Sequence > original.Sequence && control.CreateCardZoneDiagnostics().Where(item => item.Location == CardLocation.DrawPile)
                .MaxBy(item => item.ZoneIndex)!.CardId == card,
            "Zongxuan can choose and confirm the original discard while it is still in the pile, making it the next draw.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: ReentrySkill } &&
            prompt.Choices.Any(choice => choice.Cards.SequenceEqual([card])));
        var recovered = game.CardMovements.Last(move => move.CardId == card);
        Require(recovered.From == CardLocation.DiscardPile && recovered.To == CardLocation.Hand(0) && recovered.Sequence > original.Sequence,
            "The earlier movement trigger actually takes the original card back before the Zongxuan decision.");
        game = ColdReentry(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Zongxuan || prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        var newer = game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == ReentryDiscardReason);
        var relations = game.Events.Select(e => e.Payload).OfType<DiscardNeighborhoodFrozenEvent>().Where(fact => fact.OwnerSeat == 1).ToArray();
        var firstFact = game.Events.Select(e => e.Payload).OfType<ActualTurnFirstOwnedDiscardBatchEvent>()
            .Single(fact => fact.ActualTurnNumber == original.TurnNumber && fact.DiscardOwnerSeat == 0);
        Require(newer.From == original.From && newer.To == original.To && newer.Sequence > original.Sequence &&
            newer.BatchId != original.BatchId && newer.TurnNumber == original.TurnNumber && firstFact.BatchId == original.BatchId &&
            relations.Single(fact => fact.BatchId == original.BatchId) is { PreviousLivingSeat: 0, IncludesFirstPreviousDiscard: true } &&
            relations.Single(fact => fact.BatchId == newer.BatchId) is { PreviousLivingSeat: 0, IncludesFirstPreviousDiscard: false },
            "The same physical card enters the discard pile a second time, and only the outer batch remains the first upper-seat discard.");

        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard },
            "The outer batch no longer offers Zongxuan once its only eligible original entry has left the pile.");
        Require(!game.CardMovements.Any(move => move.Reason.Value == PutOnTop) &&
            game.CreateCardZoneDiagnostics().Single(item => item.CardId == card).Location == CardLocation.DiscardPile,
            $"Zongxuan must leave the card at its newer entry {newer.Sequence}; the first batch referred to entry {original.Sequence}.");
    }

    private static GameEngine ColdReentry(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves both the private decision and the original batch in the physical movement journal.");
        return restored;
    }

    internal static void ZhifouFirstPreviousDiscardOffersOriginalBatchOnce()
    {
        var (game, content) = Start();
        var turn = game.State.TurnNumber;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "2" && choice.Targets.SequenceEqual([0]));
        Reach(game, _ => game.ResolutionStack.LastOrDefault() is ProgramSkillFrame
            { CaoXian: { Stage: CaoXianStage.EffectCards, Option: 2, Target: 0, Required: 2 } });
        Answer(game, choice => choice.Cards.Count == 1);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the original unpaid Zhifou selection in every participant view.");
        game = restored;
        Answer(game, choice => choice.Cards.Count == 1);

        var paid = game.CardMovements.Where(move => move.Reason.Value == ZhifouDiscard).ToArray();
        var invoice = game.Events.Select(e => e.Payload).OfType<NativeLogicalDiscardInvocationRecordedEvent>()
            .Single(e => e.Reason.Value == ZhifouDiscard);
        var materials = game.Events.Select(e => e.Payload).OfType<NativeLogicalDiscardMaterialCompletedEvent>()
            .Where(e => e.InvocationId == invoice.InvocationId).ToArray();
        Require(invoice is { InitiatorSeat: 0, ExecutorSeat: 0, ActualCount: 2 } && paid is [var first, var second] &&
            first.BatchId is > 0 && first.BatchId == second.BatchId && paid.All(move => move.TurnNumber == turn &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile && materials.Any(material =>
                    material.CardId == move.CardId && material.CompletionSequence == move.Sequence && material.From == move.From && material.To == move.To)),
            "Zhifou creates the first actual discard as a single, exactly receipted physical batch of two cards.");

        Settle(game);
        var resolutions = game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>()
            .Where(e => e is { SkillId: Zongxuan, OwnerSeat: 1, Window: SkillProgramTriggerWindow.CardsMoved } &&
                e.BindingId == "own-or-previous-first-discard").ToArray();
        Require(resolutions.Length == 1,
            "The genuine first Zhifou discard must offer Zongxuan to the original next living seat.");
        var batchId = paid[0].BatchId;
        var firstFact = game.Events.Select(e => e.Payload).OfType<ActualTurnFirstOwnedDiscardBatchEvent>()
            .Single(e => e.ActualTurnNumber == turn && e.DiscardOwnerSeat == 0);
        var relation = game.Events.Select(e => e.Payload).OfType<DiscardNeighborhoodFrozenEvent>()
            .Single(e => e.BatchId == batchId && e.OwnerSeat == 1);
        Require(firstFact.BatchId == batchId && relation is { PreviousLivingSeat: 0, IncludesFirstPreviousDiscard: true, IncludesOwnDiscard: false } &&
            relation.ActualTurnNumber == turn,
            "The original batch owns both the first actual discard fact and the frozen upper-seat relation.");

        var resolution = resolutions[0];
        var placed = game.CardMovements.Where(move => move.Reason.Value == PutOnTop).ToArray();
        if (resolution.Activated)
        {
            var ordered = placed.Select(move => move.CardId).ToArray();
            Require(resolution.Completed && ordered.Order().SequenceEqual(paid.Select(move => move.CardId).Order()) &&
                placed.All(move => move.From == CardLocation.DiscardPile && move.To == CardLocation.DrawPile &&
                    move.Sequence > paid.Single(payment => payment.CardId == move.CardId).Sequence) &&
                game.CreateCardZoneDiagnostics().Where(card => card.Location == CardLocation.DrawPile)
                    .OrderByDescending(card => card.ZoneIndex).Take(2).Select(card => card.CardId).SequenceEqual(ordered),
                "The AI's accepted Zongxuan puts the exact original Zhifou cards on top in its chosen draw order.");
        }
        else
            Require(resolution.FrameId == batchId && placed.Length == 0, "The AI's refusal settles the original Zhifou movement window.");

        Accept(game, new UseProgramSkillCommand(0, Driver, "draw-and-discard", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, next => next is { PlayerSeat: 0, SkillPrompt.SkillId: Driver } && next.Choices.Any(choice => choice.Cards.Count == 1));
        var nextCard = P(game)!.Choices.Single(choice => choice.Cards.Count == 1).Cards.Single();
        Require(placed.Length == 0 || nextCard == placed[0].CardId, "The next actual draw returns the first card Zongxuan placed on top.");
        Answer(game, choice => choice.Cards.SequenceEqual([nextCard]));
        Settle(game);
        var later = game.CardMovements.Single(move => move.Reason.Value == LaterDiscard);
        Require(later.CardId == nextCard &&
            later.From == CardLocation.Hand(0) && later.To == CardLocation.DiscardPile && later.TurnNumber == turn && later.BatchId != batchId &&
            game.Events.Select(e => e.Payload).OfType<ActualTurnFirstOwnedDiscardBatchEvent>()
                .Count(e => e.ActualTurnNumber == turn && e.DiscardOwnerSeat == 0) == 1 &&
            game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>()
                .Count(e => e.SkillId == Zongxuan && e.OwnerSeat == 1 && e.Window == SkillProgramTriggerWindow.CardsMoved) == 1 &&
            game.Events.Select(e => e.Payload).OfType<DiscardNeighborhoodFrozenEvent>()
                .Any(e => e.BatchId == later.BatchId && e.OwnerSeat == 1 && e.PreviousLivingSeat == 0 && !e.IncludesFirstPreviousDiscard),
            "The same upper seat's later genuine discard has no first-discard opportunity in this turn.");
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        ShanheStartingCard[] hand = [new(CardKind.Dodge, Suit.Heart, 1), new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.DrawTwo, Suit.Club, 3)];
        var setup = new ShanheBattleSetup(373, 1, "ol-zongxuan-native-first", CaoXian, 3, 0, 0, 0, 0, [Driver], hand, [],
            [new(YuFan, Hp: 3) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: "ol:lingxi" });
        Activate(game);
        var wings = game.CreateSnapshot(0).Players[0].Hand.Where(card => card.Kind == CardKind.Dodge).Select(card => card.Id).ToArray();
        foreach (var id in wings)
            Answer(game, choice => choice.Cards.SequenceEqual([id]));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "finish");
        Settle(game);

        var draw = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.DrawTwo && action.ConversionSource is null);
        var beforeDraw = game.CardMovements.Count;
        Accept(game, new PlayCardCommand(0, draw.CardId!.Value, draw.TargetSeats, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: "ol:zhifou" });
        Activate(game);
        Answer(game, choice => choice.Cards.SequenceEqual([wings[0]]));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "finish");
        Reach(game, _ => game.ResolutionStack.LastOrDefault() is ProgramSkillFrame
            { CaoXian.Stage: CaoXianStage.OptionTarget or CaoXianStage.AdjustCards });
        var drawn = game.CardMovements.Skip(beforeDraw).Where(move => move.Reason == CardMoveReasons.Draw &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)).Select(move => move.CardId).ToArray();
        Require(game.ResolutionStack.LastOrDefault() is ProgramSkillFrame { CaoXian.Stage: CaoXianStage.OptionTarget } &&
            drawn.Length == 2 && drawn.Order().SequenceEqual(game.CreateSnapshot(0).Players[0].Hand.Select(card => card.Id).Order()) &&
            game.Events.Select(e => e.Payload).OfType<CaoXianAdjustedEvent>()
                .Any(e => e is { Owner: 0, RemainingSuits: 1, RequiredHand: 2 }) &&
            game.CardMovements.All(move => move.Reason.Value != ZhifouDiscard),
            "One remaining wing suit sets the hand adjustment to the two cards drawn by Ex Nihilo.");
        return (game, content);
    }

    private sealed class ReentryFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-zongxuan-reentry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, YuFan);
            var rules = $$$"""
                {"skills":[{"id":"{{{FirstDiscardSkill}}}","revision":1,"activations":[
                    {"id":"discard","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]},
                  {"id":"{{{ReentrySkill}}}","revision":1,"triggers":[{"id":"recover-and-discard","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{FirstDiscardReason}}}"],"optional":true,
                    "effects":[{"op":"selectRandomPileEquipment","target":"owner","amount":1,"resultBind":"recovered","zones":["discardPile"]},
                        {"op":"moveBoundCards","target":"owner","sourceBind":"recovered","destination":"ownerHand","awaitMovementTriggers":true},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                         "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [FirstDiscardSkill] = new { name = "首弃", description = "弃置一张手牌。" },
                    [ReentrySkill] = new { name = "取回再弃", description = "取回刚弃置的装备，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });

            builder.AddGeneral(new(ReentryGenerals[0], "上家", "supporter", FirstDiscardSkill, "wei", 8, [ReentrySkill]));
            foreach (var id in ReentryGenerals.Skip(2))
                builder.AddGeneral(new(id, "同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(ReentryDeck, "固定八卦阵", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ReentryMode, "纵玄旧入堆", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, ReentryDeck,
                GeneralCandidateCount: 4, GeneralPoolIds: ReentryGenerals));
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-zongxuan-first", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, CaoXian, YuFan);
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,
                  "triggers":[{"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                    "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}],
                  "activations":[{"id":"draw-and-discard","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                    "effects":[{"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                       "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "实际重弃", description = "摸一张牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Peer, "实际同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体闪", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 10)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "纵玄真实首弃", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [CaoXian, YuFan, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
