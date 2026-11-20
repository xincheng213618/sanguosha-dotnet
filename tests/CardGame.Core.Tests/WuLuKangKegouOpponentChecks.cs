using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class WuLuKangKegouOpponentChecks
{
    private const string Kegou = "wumiao:kegou", Owner = "fixture:kegou-owner", Ally = "fixture:kegou-ally";
    private const string First = "fixture:kegou-first", Second = "fixture:kegou-second", Deck = "fixture:kegou-deck";

    internal static void OriginalPlayStartRetargetsNativePindianCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "kegou-opponents", Owner, 4, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Spade, 2), new(CardKind.Slash, Suit.Spade, 13), new(CardKind.Dodge, Suit.Spade, 4)], [],
            [new(First, Hp: 12) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 7)] },
             new(Second, Hp: 12) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 9)] }],
            [new(Ally, Hp: 12) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 3)] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(record => record.Payload).OfType<T>().ToArray();
        string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
        bool Offered(string branch) => Prompt()?.Choices.Any(choice => Branch(choice) == branch) == true;
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The published Kegou command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 80 && !ready(); step++)
            {
                Need(Prompt()?.PlayerSeat != 0 && game.State.Winner == Winner.None, "The fixed scene reached an unexpected human choice.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The fixed scene did not reach its actual choice.");
        }
        void Answer(Func<PromptChoice, bool> matches)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(matches);
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        }

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { PlayerSeat: 0, SkillPrompt.SkillId: Kegou } prompt &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var playStart = game.ResolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().Single();
        var sourceHand = game.CreateSnapshot(0).Players[0].Hand;
        var low = sourceHand.Single(card => card.Rank == 2).Id;
        var high = sourceHand.Single(card => card.Rank == 13).Id;
        var first = game.CreateSnapshot(2).Players[2].Hand.Single().Id;
        var second = game.CreateSnapshot(3).Players[3].Hand.Single().Id;

        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(() => Offered("opponent"));
        var start = Facts<WuLuKangKegouStartedEvent>().Single();
        Need(playStart.OwnerSeat == 0 && start.WindowId == playStart.Id && start.Source.BindingId == "play-start" &&
            start.Qualification is { TurnOwner: 0, Used: false, Played: false } && start.Qualification.ActualTurn == game.State.TurnNumber &&
            Prompt()!.Choices.SelectMany(choice => choice.Targets).SequenceEqual([1, 2, 3]),
            "The actual Play-start offers Kegou before any card use and publishes eligible opponents in seat order.");

        void ChoosePindian(int opponent, int cardId)
        {
            Answer(choice => Branch(choice) == "opponent" && choice.Targets.SequenceEqual([opponent]));
            Reach(() => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card") == true);
            var child = game.ResolutionStack.OfType<PindianFrame>().Single();
            Need(child.ParentFrameId == start.FrameId && child.SourceSeat == 0 && child.OpponentSeat == opponent &&
                child.PindianStep == PindianStep.ChooseSourceCard && Prompt() is { PlayerSeat: 0, IsPrivate: true },
                "Each selected opponent opens a private native Pindian under the same Kegou frame.");
            Answer(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card" && choice.Cards.SequenceEqual([cardId]));
        }

        ChoosePindian(2, low);
        Reach(() => Offered("repeat"));
        var firstResult = Facts<WuLuKangKegouPindianReturnedEvent>().Single();
        Need(firstResult.FrameId == start.FrameId && firstResult.Result == new PindianResult(0, 2, low, first, 2, 7) &&
            Facts<WuLuKangKegouSlashDecidedEvent>().Single() is { OpponentSeat: 2, Accepted: false } &&
            Facts<WuLuKangKegouSlashIssuedEvent>().Length == 0 && Prompt() is { PlayerSeat: 0, IsPrivate: true },
            "The first physical pair does not win. The far opponent declines the Slash and the original owner can repeat.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack) &&
            restored.CardMovements.SequenceEqual(game.CardMovements),
            "Cold replay preserves the private repeat, original frames and the first pair's card movements.");
        game = restored;

        Answer(choice => Branch(choice) == "repeat");
        Reach(() => Offered("opponent"));
        Need(game.CreateSnapshot(0).Players[2].HandCount == 0 &&
            Prompt()!.Choices.SelectMany(choice => choice.Targets).SequenceEqual([1, 3]),
            "The repeated choice excludes the first opponent after its last physical card was paid.");
        ChoosePindian(3, high);
        Reach(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });

        var issued = Facts<WuLuKangKegouPindianIssuedEvent>();
        var returned = Facts<WuLuKangKegouPindianReturnedEvent>();
        var native = Facts<PindianResultDeterminedEvent>();
        var complete = Facts<WuLuKangKegouCompletedEvent>().Single();
        Need(issued.All(fact => fact.FrameId == start.FrameId) && issued.Select(fact => fact.OpponentSeat).SequenceEqual([2, 3]) &&
            issued.Select(fact => fact.Iteration).SequenceEqual([0, 1]) && returned.All(fact => fact.FrameId == start.FrameId) &&
            returned.Select(fact => fact.PindianId).SequenceEqual(issued.Select(fact => fact.PindianId)) &&
            native.Select(fact => (fact.FrameId, fact.Result)).SequenceEqual(returned.Select(fact => (fact.PindianId, fact.Result))) &&
            returned[1].Result == new PindianResult(0, 3, high, second, 13, 9) && complete.FrameId == start.FrameId && complete.Iterations == 2,
            "Two native results return to the same Kegou frame in accepted opponent order and complete it once.");

        var revealMoves = game.CardMovements.Where(move => move.Reason == CardMoveReasons.PindianReveal).ToArray();
        var gains = Facts<WuLuKangKegouGainPaidEvent>();
        Need(revealMoves.Select(move => (move.CardId, move.From, move.To)).SequenceEqual(new[]
            {
                (low, CardLocation.Hand(0), CardLocation.Processing), (first, CardLocation.Hand(2), CardLocation.Processing),
                (high, CardLocation.Hand(0), CardLocation.Processing), (second, CardLocation.Hand(3), CardLocation.Processing)
            }) && gains.Select(fact => fact.PrintedRank).SequenceEqual([1, 3]) &&
            gains.All(fact => fact.FrameId == start.FrameId && fact.Iteration == 1 && game.CardMovements.Count(move =>
                move.Sequence == fact.Sequence && move.CardId == fact.CardId && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) == 1),
            "The exact four selected entities enter Processing once and the second win pays the two lowest distinct ranks once.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-kegou-opponents", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "wumiao:lu-kang");
            builder.AddGeneral(new(Owner, "克构技能持有者", "supporter", "standard:none", "wu", 4, [Kegou]));
            foreach (var peer in new[] { Ally, First, Second })
                builder.AddGeneral(new(peer, "固定同桌", "supporter", "standard:none", "wei", 12));
            builder.AddDeck(new(Deck, "克构固定牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 4).SelectMany(_ => new ContentDeckPhysicalCard[]
                {
                    new("standard:dodge", Suit.Spade, 1), new("standard:slash", Suit.Spade, 3),
                    new("standard:peach", Suit.Spade, 5), new("standard:indulgence", Suit.Spade, 13)
                }).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "克构原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, First, Second], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
