using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlDouyuChecks
{
    private const string Douyu = "ol:douyu", PangJi = "ol:pang-ji", Tianbian = "classic:tianbian";
    private const string Contest = "fixture:ol-douyu-contest", Peer = "fixture:ol-douyu-peer", Deck = "fixture:ol-douyu-deck";

    public static void PrivateSwapPreservesOriginalTopPaymentCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(417, 1, "douyu-top", PangJi, 3, 0, 0, 0, 0,
            [Contest, Tianbian], [], [],
            [new(Peer) { StartingHand = [new(CardKind.Dodge, Suit.Club, 12)] },
                new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }], [], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4,
            HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        var play = Next(game);
        if (play.SkillPrompt?.SkillId == "ol:biguo")
        {
            Answer(game, play, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
            play = Next(game);
        }
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "Pang Ji must reach his ordinary Play phase.");
        var handCard = game.CreateSnapshot(0).Players[0].Hand[0].Id;
        var opponentCard = game.CreateSnapshot(1).Players[1].Hand.Single().Id;
        Submit(game, new UseProgramSkillCommand(0, Contest, "contest", [handCard], [1], game.Revision, play.PromptId));
        var chooseSource = Next(game);
        Answer(game, chooseSource, c => c.Parameters.GetValueOrDefault("action") == "pindian-top");

        var reservation = game.CardMovements.Single(m => m.Reason.Value == "pindian.reserve-top");
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        Require(reservation.From == CardLocation.DrawPile && reservation.To == CardLocation.Processing &&
            pair.SourceCardId == reservation.CardId && pair.SourceTopReservationSequence == reservation.Sequence,
            "The native Pindian frame owns the source's exact top reservation while the opponent chooses.");

        var chooseOpponent = game.CreateSnapshot(1).PendingDecision;
        Require(chooseOpponent is { Kind: DecisionKind.SkillModule, PlayerSeat: 1, IsPrivate: true, Choices: [var offered] } &&
            offered.Cards.SequenceEqual([opponentCard]) &&
            game.CreateSnapshot(0).PendingDecision is null && game.CreateSnapshot(2).PendingDecision is null,
            "The opponent's only held card is offered privately before the AI commits it.");
        Submit(game, new AdvanceOneStepCommand(game.Revision));
        var swap = Next(game);
        if (swap.SkillPrompt?.SkillId == Douyu && swap.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
        {
            Answer(game, swap, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            swap = Next(game);
        }

        var pending = game.ResolutionStack.OfType<PindianFrame>().Single();
        Require(swap is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Douyu } &&
            swap.Choices.All(c => c.Cards.Count == 0) && game.CreateSnapshot(1).PendingDecision is null &&
            game.CreateSnapshot(2).PublicRevealedCards.Count == 0 && pending.DouyuContest is { Paid: false } receipt &&
            pending.SourceTopReservationSequence == reservation.Sequence && receipt.OriginalCards[0].ReservationSequence == reservation.Sequence,
            "Douyu receives the same unpaid reservation in its private swap window without exposing either card.");

        var restored = Restore(game, content);
        foreach (var continuation in new[] { game, restored })
        {
            Answer(continuation, Next(continuation), c => c.Parameters.GetValueOrDefault("branch") == "swap" && c.Targets.SequenceEqual([1]));
            Require(Next(continuation) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The settled pair must resume the original play phase.");
            var result = Facts<PindianResultDeterminedEvent>(continuation).Single();
            var payments = Facts<DouyuNativePaymentEvent>(continuation).Where(e => e.ContestId == pair.Id).ToArray();
            Require(result.FrameId == pair.Id && result.Result is { SourceWon: true } &&
                result.Result.SourceCardId == opponentCard && result.Result.OpponentCardId == reservation.CardId &&
                payments is [var sourcePayment, var opponentPayment] && sourcePayment.AlreadyReserved &&
                sourcePayment.MovementSequence == reservation.Sequence && sourcePayment.CardId == reservation.CardId &&
                !opponentPayment.AlreadyReserved && opponentPayment.CardId == opponentCard &&
                continuation.CardMovements.Count(m => m.CardId == reservation.CardId && m.To == CardLocation.Processing) == 1 &&
                continuation.CardMovements.Count(m => m.CardId == opponentCard && m.Reason == CardMoveReasons.PindianReveal) == 1,
                "The swap changes comparison assignments while native payment retains both original physical entries.");
            var reward = Facts<DouyuWinDrawReturnedEvent>(continuation).Single();
            Require(reward is { RecipientSeat: 0, RequestedCount: 2, ActualCount: 2 } && reward.ContestId == pair.Id &&
                Facts<DouyuRewardClosedEvent>(continuation).Single().DrawStarted,
                "Winning the swapped comparison draws exactly two cards and closes its issued reward once.");
        }
        Require(game.CardMovements.SequenceEqual(restored.CardMovements), "Cold recovery yields the same physical payments, rewards and cleanup.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static GameEngine Restore(GameEngine game, ContentRegistry content)
    {
        var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(copy.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(copy.ResolutionStack),
            "Cold recovery reconstructs the exact private prompt, native reservation and unpaid receipt.");
        return copy;
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var i = 0; i < 100 && game.PendingDecision is null; i++)
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        return game.PendingDecision ?? throw new InvalidOperationException("The Douyu fixture did not reach a prompt.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> choose) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(choose).Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Douyu fixture command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-douyu", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, PangJi, "classic:qin-mi");
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Contest}}","revision":1,
                    "activations":[{"id":"contest","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,
                        "targetKind":"otherLivingWithHand","usesPerTurn":1,"effects":[{"op":"pindian","target":"selectedTarget","amount":1}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Contest] = new { name = "拼点", description = "与一名其他角色拼点。" } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Contest, "拼点", "与一名其他角色拼点。")
                { Program = catalog.Programs[Contest], ProgramPresentation = catalog.Presentations[Contest] });
            builder.AddGeneral(new(Peer, "拼点对手", "", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定牌堆", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 80)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 4)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "斗誉拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [PangJi, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
