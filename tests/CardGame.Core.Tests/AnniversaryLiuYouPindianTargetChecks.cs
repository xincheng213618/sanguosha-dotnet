using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryLiuYouPindianTargetChecks
{
    private const string Hero = "anniversary:liu-you", Kannan = "anniversary:kannan", Activation = "phase-contest";
    private const string Opponent = "fixture:kannan-opponent", Empty = "fixture:kannan-empty", Deck = "fixture:kannan-deck";

    internal static void ActualPlayPublishesAndRestoresOriginalPindian()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(42, 1, "kannan-opponent", Hero, 1, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 8)], [],
            [new(Opponent, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 6)] },
             new(Empty, Hp: 8) { StartingHand = [] }],
            [new(Empty, Hp: 8) { StartingHand = [] }], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(record => record.Payload).OfType<T>().ToArray();
        static void Need(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The published Kannan command was rejected.");
        }
        void Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80 && game.State.Winner == Winner.None; step++)
            {
                if (Prompt() is { } prompt)
                {
                    Need(prompt.PlayerSeat == 0 && ready(prompt), "Kannan reached an unexpected choice.");
                    return;
                }
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Kannan did not reach the requested human choice.");
        }

        Submit(new StartGameCommand());
        Reach(prompt => prompt.Kind == DecisionKind.PlayCard);
        var players = game.CreateSnapshot(0).Players;
        var sourceCard = players[0].Hand.Single(card => card is { Kind: CardKind.Dodge, Rank: 8 }).Id;
        var opponent = players.Single(player => player.GeneralId == Opponent);
        var empty = players.Where(player => player.GeneralId == Empty).ToArray();
        var action = game.GetHumanLegalActions().Single(candidate => candidate.ProgramSkillId == Kannan &&
            candidate.ProgramActivationId == Activation);
        Need(game.State.Phase == TurnPhase.Play && opponent is { IsAlive: true, HandCount: 1 } && empty.Length == 2 &&
            empty.All(player => player is { IsAlive: true, HandCount: 0 }) &&
            action.MinCardCount == 0 && action.SelectableTargetSeats.SequenceEqual([opponent.Seat]),
            "The actual Play action offers only the living opponent with an input and needs no submitted activation card.");

        var revision = game.Revision;
        Need(!game.Submit(new UseProgramSkillCommand(0, Kannan, Activation, [], [empty[0].Seat], revision, Prompt()!.PromptId)).Accepted &&
            game.Revision == revision && Facts<AnniversaryLiuYouStartedEvent>().Length == 0,
            "Submitting a living empty-hand opponent fails before issuing the original activation.");
        Submit(new UseProgramSkillCommand(0, Kannan, Activation, [], [opponent.Seat], revision, Prompt()!.PromptId));
        Reach(prompt => prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-source-card"));
        var start = Facts<AnniversaryLiuYouStartedEvent>().Single();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == start.FrameId);
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        Need(start.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play, ActorSeat: 0, ActualTurnOwnerSeat: 0 } &&
            start.Phase.TurnNumber == game.State.TurnNumber && start.Target == opponent.Seat &&
            root.AnniversaryLiuYou == new AnniversaryLiuYouReceipt(start.Source, start.Hash, start.Phase, opponent.Seat) &&
            root.SelectedCardIds.Count == 0 && root.SelectedTargetSeats.SequenceEqual([opponent.Seat]) &&
            pair is { SourceSeat: 0, PindianStep: PindianStep.ChooseSourceCard, ProgramResultBind: "kannan-contest" } &&
            pair.ParentFrameId == root.Id && pair.OpponentSeat == opponent.Seat && Prompt() is { PlayerSeat: 0, IsPrivate: true },
            "The accepted command binds the original actual Play and chosen opponent to its private native Pindian frame.");
        var choice = Prompt()!.Choices.Single(item => item.Parameters.GetValueOrDefault("action") == "pindian-source-card" &&
            item.Cards.SequenceEqual([sourceCard]));

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            restored.CardMovements.SequenceEqual(game.CardMovements),
            "Cold replay preserves all views, the two original frames and their unpaid card movements.");
        game = restored;
        Submit(new AnswerPromptCommand(0, Prompt()!.PromptId, choice.Id, game.Revision));
        var selectingOpponent = game.ResolutionStack.OfType<PindianFrame>().Single();
        Need(selectingOpponent.Id == pair.Id && selectingOpponent.SourceCardId == sourceCard &&
            selectingOpponent.PindianStep == PindianStep.ChooseOpponentCard && Facts<AnniversaryLiuYouStartedEvent>().SequenceEqual([start]),
            "The restored source choice records the same original hand entity on the same pair without reissuing Kannan.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-kannan-opponent", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Opponent, "固定拼点候选", "supporter", "standard:none", "wei", 8));
            builder.AddGeneral(new(Empty, "固定空手角色", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "戡难固定牌堆", 0, 0, [new("standard:peach", 32)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "戡难原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Opponent, Empty], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
