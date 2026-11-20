using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MouDianWeiPindianOpponentChecks
{
    private const string Hero = "mou:dian-wei", Kuangzhan = "mou:kuangzhan", Deck = "fixture:kuangzhan-opponents";
    private const string Enemy = "fixture:kuangzhan-enemy", Ally = "fixture:kuangzhan-ally", Empty = "fixture:kuangzhan-empty";

    internal static void ActualKuangzhanDrawAndNativePindianUseSameOpponentsCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "kuangzhan-opponents", Hero, 1, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Spade, 13)], [],
            [
                new(Ally, Hp: 10) { StartingHand = [new(CardKind.Slash, Suit.Club, 3)] },
                new(Empty, Hp: 10) { StartingHand = [] }
            ],
            [new(Enemy, Hp: 10) { StartingHand = [new(CardKind.Slash, Suit.Club, 1)] }],
            new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The actual Kuangzhan command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 80 && !ready(); step++)
            {
                Need(Prompt() is null && game.State.Winner == Winner.None, "The fixed scene reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The fixed scene did not reach its actual choice.");
        }

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        var handBefore = game.CreateSnapshot(0).Players[0].HandCount;
        Submit(new UseProgramSkillCommand(0, Kuangzhan, "draw-and-contest", [], [], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt() is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: 0 } && game.ResolutionStack.OfType<ProgramSkillFrame>()
            .Any(frame => frame.MouDianWei?.Stage == MouDianWeiStage.Opponent));
        var draw = Facts<MouDianWeiDrawReturnedEvent>().Single();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == draw.FrameId);
        var choices = Prompt()!;
        var players = game.CreateSnapshot(0).Players;
        Need(draw is { Requested: 3, ActualDrawn: 3, ReplacementGained: 0, X: 3 } && players[0].HandCount == handBefore + 3 &&
            players[1].HandCount > 0 && players[2].HandCount > 0 && players[3].HandCount == 0 &&
            root.MouDianWei is { Stage: MouDianWeiStage.Opponent, Index: 0 } &&
            choices.Choices.SelectMany(choice => choice.Targets).SequenceEqual([1, 2]) && Facts<MouDianWeiContestIssuedEvent>().Length == 0,
            "After three real draws the original skill offers exactly the two opponents with input and excludes the empty seat.");

        var opponent = choices.Choices.Single(choice => choice.Targets.SequenceEqual([1]));
        Submit(new AnswerPromptCommand(0, choices.PromptId, opponent.Id, game.Revision));
        var issued = Facts<MouDianWeiContestIssuedEvent>().Single();
        var pindian = game.ResolutionStack.OfType<PindianFrame>().Single(frame => frame.Id == issued.ContestId);
        Need(issued is { Opponent: 1, Index: 0 } && issued.FrameId == root.Id && pindian.ParentFrameId == root.Id &&
            pindian is { SourceSeat: 0, OpponentSeat: 1, PindianStep: PindianStep.ChooseSourceCard } &&
            Prompt() is { PlayerSeat: 0, IsPrivate: true },
            "Submitting the displayed opponent creates a private native Pindian under the same Kuangzhan frame.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(restored.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack.OfType<PindianFrame>().Single()) == JsonSerializer.Serialize(pindian),
            "Cold replay keeps all seat views, the actual draw movements and the same pending contest.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-kuangzhan-opponents", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            foreach (var id in new[] { Enemy, Ally, Empty })
                builder.AddGeneral(new(id, "固定同桌", "supporter", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "狂战固定牌堆", 0, 1, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "狂战原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy, Ally, Empty], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
