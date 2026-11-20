using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MouXunYuBizuoDiscardEntryChecks
{
    private const string Hero = "mou:xun-yu", Bizuo = "mou:bizuo", Shimou = "mou:shimou";
    private const string Driver = "fixture:bizuo-discard-driver", Peer = "fixture:bizuo-peer", Deck = "fixture:bizuo-deck";
    private const string Discard = "skill-program.fixture:bizuo-discard-driver.DiscardSelected";
    private const string Allocation = "skill-program.mou:bizuo.allocate";

    internal static void BizuoExcludesAnEarlierUseThatReenteredByDiscard()
    {
        var (game, content) = Start();
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Shimou } && prompt.Choices.Any(choice => IsBranch(choice, "yang")));
        Answer(game, choice => IsBranch(choice, "yang"));
        Settle(game);
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        var peach = hand.Single(card => card.Kind == CardKind.Peach).Id;
        var drawTwo = hand.Single(card => card.Kind == CardKind.DrawTwo).Id;
        EndPlay(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Bizuo } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Activate(game);
        Reach(game, prompt => HasBranch(prompt, "beneficiary"));
        Answer(game, choice => IsBranch(choice, "beneficiary") && choice.Targets.SequenceEqual([0]));
        Settle(game);
        var lease = Facts<MouXyBizuoStartedEvent>(game).Single();
        Require(lease is { Actor: 0, Grant.Beneficiary: 0 } && lease.ActualTurn == game.State.TurnNumber &&
                lease.Grant.IssuedTurn < lease.ActualTurn && game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 },
            "The actual granted extra turn begins with an empty draw pile and no competing discard entries.");

        Play(game, LegalActionKind.Peach, peach);
        var original = game.CardMovements.Single(move => move.CardId == peach && move.Reason == CardMoveReasons.UseFinished);
        Play(game, LegalActionKind.DrawTwo, drawTwo);
        var current = game.CardMovements.Single(move => move.CardId == drawTwo && move.Reason == CardMoveReasons.UseFinished);
        var returned = game.CardMovements.Where(move => move.CardId == peach && move.Sequence > original.Sequence).ToArray();
        Require(original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile &&
                returned is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
                shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
                drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0) && drawn.Sequence < current.Sequence &&
                current.From == CardLocation.Processing && current.To == CardLocation.DiscardPile &&
                Facts<MouXyDiscardEntryRecordedEvent>(game).Select(e => (e.Entry.CardId, e.Entry.Sequence))
                    .SequenceEqual([(peach, (long)original.Sequence), (drawTwo, current.Sequence)]),
            "Two genuine Uses create their exact qualifying entries, and Draw Two's real draw reshuffles only the earlier used Peach back to hand.");

        Accept(game, new UseProgramSkillCommand(0, Driver, "discard-one", [peach], [], game.Revision, P(game)!.PromptId));
        Settle(game);
        var later = game.CardMovements.Single(move => move.CardId == peach && move.Reason.Value == Discard);
        var invoice = Facts<NativeLogicalDiscardInvocationRecordedEvent>(game).Single(e => e.Reason.Value == Discard);
        Require(later.From == CardLocation.Hand(0) && later.To == CardLocation.DiscardPile && later.Sequence > current.Sequence &&
                later.BatchId != original.BatchId && invoice.TurnNumber == lease.ActualTurn && invoice.ExecutorSeat == 0 &&
                Facts<NativeLogicalDiscardMaterialCompletedEvent>(game).Count(e => e.InvocationId == invoice.InvocationId && e.CardId == peach &&
                    e.CompletionSequence == later.Sequence && e.From == later.From && e.To == later.To) == 1,
            "The same Peach really reenters the discard pile as a new native logical discard during the granted turn.");

        EndPlay(game);
        Reach(game, prompt => HasBranch(prompt, "allocate-card"));
        game = Cold(game, content);
        Require(Facts<MouXyBizuoEndedEvent>(game).Single().Grant == lease.Grant &&
                P(game)!.Choices.Where(choice => IsBranch(choice, "allocate-card")).SelectMany(choice => choice.Cards).SequenceEqual([drawTwo]),
            "Bizuo must offer only the current non-discard entry and leave the re-discarded Peach in the pile.");
        Answer(game, choice => IsBranch(choice, "allocate-card") && choice.Cards.SequenceEqual([drawTwo]));
        Reach(game, prompt => HasBranch(prompt, "allocate-recipient"));
        Answer(game, choice => IsBranch(choice, "allocate-recipient") && choice.Targets.SequenceEqual([1]));
        var allocated = Facts<MouXyAllocatedEvent>(game).Single();
        Require(allocated.GrantFrameId == lease.Grant.FrameId && allocated.CardId == drawTwo && allocated.SourceSequence == current.Sequence &&
                allocated.Recipient == 1 && allocated.From == CardLocation.DiscardPile && allocated.ZhifouFrameId is null &&
                game.CardMovements.Any(move => move.Sequence == allocated.Sequence && move.CardId == drawTwo && move.To == CardLocation.Hand(1) &&
                    move.Reason.Value == Allocation) && game.CreateCardZoneDiagnostics().Single(card => card.CardId == peach).Location == CardLocation.DiscardPile,
            "The surviving use entry is delivered once under its actual lease while the later discard remains where it entered.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static bool IsBranch(PromptChoice choice, string branch) => choice.Parameters.GetValueOrDefault("branch") == branch;
    private static bool HasBranch(PendingDecision prompt, string branch) =>
        prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Bizuo } && prompt.Choices.Any(choice => IsBranch(choice, branch));
    private static void EndPlay(GameEngine game) => Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));

    private static void Play(GameEngine game, LegalActionKind kind, int cardId)
    {
        var action = game.GetHumanLegalActions().Single(action => action.Kind == kind && action.CardId == cardId);
        Accept(game, new PlayCardCommand(0, cardId, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
        Settle(game);
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, 4).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the original extra turn, current allocation choices and all physical movement entries.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peers = Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer, Hp: 8) { StartingHand = [] }).ToArray();
        var setup = new ShanheBattleSetup(7, 1, "bizuo-original-non-discard-entry", Hero, 2, 0, 0, 0, 0, [Driver],
            [new(CardKind.Peach, Suit.Heart, 7), new(CardKind.DrawTwo, Suit.Heart, 8)], [], peers, [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-bizuo-non-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,
                  "modifiers":[{"id":"keep-original-hand","query":"handLimit","operation":"add","value":8,"priority":0}],
                  "activations":[{"id":"discard-one","minCards":1,"maxCards":1,"minTargets":0,"maxTargets":0,
                    "targetKind":"anyLiving","sourceZones":["hand"],"usesPerTurn":1,
                    "effects":[{"op":"discardSelected","target":"owner","amount":1}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "实际弃牌", description = "弃置一张实际手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Peer, "实际同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "两张起始摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Spade, 5), new("standard:dodge", Suit.Club, 6)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "弼佐当前入堆", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
