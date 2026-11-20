using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class FriendXiaxingDiscardEntryChecks
{
    private const string Hero = "friend:xu-shu", Xiaxing = "friend:xiaxing";
    private const string Driver = "fixture:xiaxing-reentry", Peer = "fixture:xiaxing-peer", Deck = "fixture:xiaxing-reentry-deck";
    private const string InitialDiscard = "skill-program.fixture:xiaxing-reentry.DiscardOwnedZoneCards";
    private const string Rediscard = "skill-program.fixture:xiaxing-reentry.SelectAndMoveOwnedCard";
    private const string Reclaim = "friend.xiaxing.reclaim";

    internal static void XiaxingCannotSpendMarksOnAnOldDiscardEntry()
    {
        var (game, content) = Start();
        var play = Next(game);
        var sword = Facts<FriendXuanjianGeneratedEvent>(game).Single().CardId;
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
             game.CreateSnapshot(0).Players[0].Equipment.Any(card => card.Id == sword && card.Kind == CardKind.Xuanjian) &&
             Facts<FriendXuanjianUseReturnedEvent>(game).Single().CardId == sword && HasMark(game, PlayerMarkerKind.QihuiEquipment),
            "The genuine game-start Xuanjian use finishes and grants the equipment Qihui mark.");
        var slash = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.ConversionSource is null &&
            action.CardId is not null && action.TargetSeats.SequenceEqual([1]));
        Submit(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision, play.PromptId, slash.PlayedCardKind, slash.TargetCardId));
        play = Next(game);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && HasMark(game, PlayerMarkerKind.QihuiBasic) &&
             HasMark(game, PlayerMarkerKind.QihuiEquipment) && !HasMark(game, PlayerMarkerKind.QihuiTrick),
            "A true Slash grants the basic mark alongside the original equipment mark.");
        Submit(game, new UseProgramSkillCommand(0, Driver, "discard-weapon", [], [], game.Revision, play.PromptId));
        var early = Next(game);
        var original = game.CardMovements.Single(move => move.CardId == sword && move.Reason.Value == InitialDiscard);
        Need(early is { SkillPrompt.SkillId: Driver } && early.Choices.Any(choice => Action(choice, "activate")) &&
             original.From == CardLocation.Equipment(0) && original.To == CardLocation.DiscardPile &&
             ActiveBatch(game) == original.BatchId && game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Last().Candidates.Any(c => c.SkillId == Xiaxing),
            "The genuine equipment discard queues Xiaxing behind an earlier movement trigger in the same batch.");

        var control = Cold(game, content);
        Answer(control, choice => Action(choice, "skip"));
        Need(IsXiaxing(Next(control)) && ActiveBatch(control) == original.BatchId,
            "An untouched Xuanjian receives its original discard opportunity.");
        PayMarks(control);
        Need(Next(control) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
             control.CardMovements.Single(move => move.CardId == sword && move.Reason.Value == Reclaim) is { From: var from, To: var to } &&
             from == CardLocation.DiscardPile && to == CardLocation.Hand(0) && SpentMarks(control) == 2 &&
             !HasMark(control, PlayerMarkerKind.QihuiBasic) && !HasMark(control, PlayerMarkerKind.QihuiEquipment),
            "The genuine original reclaim pays two distinct marks and obtains the same physical Xuanjian.");

        Answer(game, choice => Action(choice, "activate"));
        var discard = Next(game);
        var pickup = game.CardMovements.Single(move => move.CardId == sword && move.Sequence > original.Sequence &&
            move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0));
        Need(discard.Choices.Any(choice => choice.Cards.SequenceEqual([sword])) &&
             game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(frame => frame.Batch.Id == original.BatchId),
            "The earlier child really obtains the only discarded equipment while the original claim is suspended.");
        Answer(game, choice => choice.Cards.SequenceEqual([sword]));
        var fresh = game.CardMovements.Single(move => move.CardId == sword && move.Reason.Value == Rediscard && move.To == CardLocation.DiscardPile);
        Need(IsXiaxing(Next(game)) && ActiveBatch(game) == fresh.BatchId && fresh.BatchId != original.BatchId &&
             fresh.Sequence > pickup.Sequence && game.CardMovements.Any(move => move.CardId == sword && move.Reason.Value == Rediscard && move.From == CardLocation.Hand(0)),
            "The real second discard opens its own Xiaxing opportunity before the original batch resumes.");
        game = Cold(game, content);
        Answer(game, choice => Action(choice, "skip"));
        var next = Next(game);
        if (IsXiaxing(next))
        {
            Need(ActiveBatch(game) == original.BatchId, "Any repeated invocation must belong to the resumed original batch.");
            PayMarks(game);
            next = Next(game);
        }
        Need(next is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && SpentMarks(game) == 0 &&
             HasMark(game, PlayerMarkerKind.QihuiBasic) && HasMark(game, PlayerMarkerKind.QihuiEquipment) &&
             !game.CardMovements.Any(move => move.Reason.Value == Reclaim) &&
             game.CreateCardZoneDiagnostics().Single(card => card.CardId == sword).Location == CardLocation.DiscardPile,
            "After the current entry was declined, the older window cannot charge two marks or claim the re-discarded Xuanjian.");
    }

    private static bool HasMark(GameEngine game, PlayerMarkerKind kind) =>
        (game.State.Players[0].Markers ?? []).Any(mark => mark.Kind == kind);
    private static int SpentMarks(GameEngine game) => Facts<PlayerMarkerChangedEvent>(game).Count(e => e.PlayerSeat == 0 && e.Delta < 0 &&
        e.Marker is PlayerMarkerKind.QihuiEquipment or PlayerMarkerKind.QihuiBasic);
    private static long? ActiveBatch(GameEngine game) => game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()?.Batch.Id;
    private static bool IsXiaxing(PendingDecision prompt) => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Xiaxing } &&
        prompt.Choices.Any(choice => Action(choice, "activate"));
    private static bool Action(PromptChoice choice, string value) => choice.Parameters.GetValueOrDefault("program-action") == value;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static void PayMarks(GameEngine game)
    {
        Answer(game, choice => Action(choice, "activate"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("value") == nameof(PlayerMarkerKind.QihuiEquipment));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("value") == nameof(PlayerMarkerKind.QihuiBasic));
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peers = Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray();
        var setup = new ShanheBattleSetup(7, 1, "xiaxing-discard-entry", Hero, 3, 0, 0, 0, 0, [Driver],
            [new(CardKind.Slash, Suit.Heart, 7)], [], peers, [], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        return (game, content);
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the physical movements, marks and pending discard-entry opportunity.");
        return restored;
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt) return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Xiaxing did not reach the next public decision.");
    }

    private static void Answer(GameEngine game, Func<PromptChoice, bool> match)
    {
        var prompt = Next(game);
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(match).Id, game.Revision));
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Xiaxing command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-xiaxing-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "friend:zhuge-liang", "friend:pang-tong", Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,
                  "activations":[{"id":"discard-weapon","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                    "effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["equipment"]}]}],
                  "triggers":[{"id":"obtain-and-discard","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch",
                    "movementReasons":["{{{InitialDiscard}}}"],"optional":true,"effects":[
                    {"op":"selectRandomPileEquipment","target":"owner","amount":1,"resultBind":"recovered","zones":["discardPile"]},
                    {"op":"moveBoundCards","target":"owner","sourceBind":"recovered","destination":"ownerHand","awaitMovementTriggers":true},
                    {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                     "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "取得后再弃置", description = "弃置装备牌，或取得弃牌堆的一张装备后再弃置。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            var driver = catalog.Presentations[Driver];
            builder.AddSkill(new(Driver, driver.Name, driver.Description) { Program = catalog.Programs[Driver], ProgramPresentation = driver });
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "侠行摸牌堆", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Club, 5), new("standard:dodge", Suit.Diamond, 6)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "侠行原入堆", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
