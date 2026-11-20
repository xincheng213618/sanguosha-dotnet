using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MouJuShouMuwangDiscardEntryChecks
{
    private const string Hero = "mou:ju-shou", Muwang = "mou:muwang";
    private const string Driver = "fixture:muwang-discard-entry-driver", Tracker = "fixture:muwang-second-tracker";
    private const string Peer = "fixture:muwang-peer", Deck = "fixture:muwang-discard-entry-deck";

    internal static void MuwangRecoversOnLaterDiscardAfterAnEarlierClaimSettles()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peers = new ShanheCombatantDefinition[]
        {
            new(Tracker, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] },
            new(Peer, Hp: 8) { StartingHand = [] },
            new(Peer, Hp: 8) { StartingHand = [] }
        };
        var setup = new ShanheBattleSetup(7, 1, "muwang-later-entry", Hero, 2, 0, 0, 0, 0, [Driver],
            [new(CardKind.Dodge, Suit.Spade, 3)], [], peers, [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        var play = Next(game);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The first tracker acts during the original public Play phase.");
        var turn = game.State.TurnNumber;

        Submit(game, new UseProgramSkillCommand(0, Driver, "take", [], [1], game.Revision, play.PromptId));
        var pick = Next(game);
        Answer(game, pick, pick.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
        var transfer = game.CardMovements.Single(move => move.From == CardLocation.Hand(1) && move.To == CardLocation.Hand(0));
        var entity = transfer.CardId;
        play = Next(game);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
             Facts<MouJsFirstChosenEvent>(game).Single(e => e.Source.OwnerSeat == 1) is { CardId: var tracked, FirstSequence: var firstLoss } &&
             tracked == entity && firstLoss == transfer.Sequence,
            "The other tracker's first actual loss follows this exact physical card into the acting seat's hand.");

        Submit(game, new UseProgramSkillCommand(0, Driver, "discard", [entity], [], game.Revision, play.PromptId));
        play = Next(game);
        var original = game.CardMovements.Single(move => move.CardId == entity && move.To == CardLocation.DiscardPile);
        var missed = Facts<MouJsMuwangQueuedEvent>(game).Single(e => e.Source.OwnerSeat == 1 && e.Kind == MouJsMuwangKind.Recover);
        Need(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && missed.BatchId == original.BatchId && missed.Turn == turn &&
             Facts<MouJsMuwangSettledEvent>(game).Single(e => e.Ordinal == missed.Ordinal) is not null &&
             Facts<MouJsRecoveredEvent>(game).Single(e => e.CardId == entity).Source.OwnerSeat == 0 &&
             game.CardMovements.Any(move => move.CardId == entity && move.From == CardLocation.DiscardPile &&
                 move.To == CardLocation.Hand(0) && move.Sequence > original.Sequence),
            "The active tracker retrieves the first discard before the second tracker settles without receiving it.");

        Submit(game, new UseProgramSkillCommand(0, Driver, "discard", [entity], [], game.Revision, play.PromptId));
        var penalty = Next(game);
        var reentry = game.CardMovements.Last(move => move.CardId == entity && move.To == CardLocation.DiscardPile);
        Need(penalty is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: 0 } && reentry.Sequence > original.Sequence &&
             reentry.BatchId != original.BatchId && reentry.From == CardLocation.Hand(0) && reentry.TurnNumber == turn &&
             Facts<MouJsRecoveredEvent>(game).Count(e => e.CardId == entity) == 1,
            "The same card actually reenters the discard pile in the tracked turn before the active tracker's paid penalty.");
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(game.CardMovements.SequenceEqual(restored.CardMovements) &&
             SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(restored.CreateSnapshot(0)),
            "Cold replay preserves the later entry and the active tracker's pending penalty.");
        foreach (var run in new[] { game, restored })
        {
            var current = Next(run);
            var cost = current.Choices.First(choice => choice.Parameters.GetValueOrDefault("branch") == "discard" && choice.Cards is [var id] && id != entity);
            Answer(run, current, cost);
            Need(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                 run.CardMovements.Any(move => move.CardId == cost.Cards.Single() && move.From == CardLocation.Hand(0) &&
                     move.To == CardLocation.DiscardPile && move.Reason.Value == "skill-program.mou:muwang.discard"),
                "The active tracker pays the physical second-loss penalty and returns to the same Play phase.");
            Need(Facts<MouJsRecoveredEvent>(run).Where(e => e.CardId == entity).Select(e => e.Source.OwnerSeat).SequenceEqual([0, 1]) &&
                 run.CardMovements.Last(move => move.CardId == entity) is { From: var from, To: var to, Sequence: var sequence } &&
                 from == CardLocation.DiscardPile && to == CardLocation.Hand(1) && sequence > reentry.Sequence,
                "A previously missed receipt does not consume the other tracker's return when its card enters the discard pile again in the same turn.");
        }
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
                return prompt;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Muwang did not reach the next public decision.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, PromptChoice choice) =>
        Submit(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "Muwang command rejected.");
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-muwang-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
                  {"id":"take","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                   "effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},
                    "zones":["hand"],"count":1,"destination":"ownerHand","awaitMovementTriggers":true}]},
                  {"id":"discard","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                   "effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"drop"},
                    {"op":"moveBoundCards","target":"owner","sourceBind":"drop","destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "移动暮往实体", description = "移动一张实际手牌或弃置一张自己的牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            var skill = catalog.Presentations[Driver];
            builder.AddSkill(new(Driver, skill.Name, skill.Description) { Program = catalog.Programs[Driver], ProgramPresentation = skill });
            builder.AddGeneral(new(Tracker, "第二位暮往来源", "supporter", Muwang, "qun", 8));
            builder.AddGeneral(new(Peer, "同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "暮往弃牌", 0, 2, []) { PhysicalCards = Enumerable.Range(2, 12)
                .Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, rank)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "暮往再次入堆", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Tracker, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
