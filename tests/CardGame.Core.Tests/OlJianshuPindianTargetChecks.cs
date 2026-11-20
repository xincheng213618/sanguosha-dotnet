using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlJianshuPindianTargetChecks
{
    private const string Hero = "ol:sp-jia-xu", Jianshu = "ol:jianshu", Yongdi = "ol:yongdi";
    private const string Activation = "limited-black-hand-recipient-contest", Deck = "fixture:jianshu-target-deck";
    private const string Recipient = "fixture:jianshu-target-recipient", Opponent = "fixture:jianshu-target-opponent";
    private const string EmptyPeer = "fixture:jianshu-target-empty-peer";

    internal static void PaidGiftUsesRecipientPindianTargetsCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "jianshu-target", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Spade, 7), new(CardKind.Slash, Suit.Heart, 5)], [],
            [new(Recipient) { StartingHand = [] }, new(EmptyPeer) { StartingHand = [] }],
            [new(Opponent) { StartingHand = [new(CardKind.Crossbow, Suit.Club, 9)] }],
            new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        T Fact<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().Single();
        static void Need(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Jianshu fixture command was rejected.");
        }
        void Answer(PromptChoice choice) =>
            Submit(new AnswerPromptCommand(0, Prompt()!.PromptId, choice.Id, game.Revision));
        void Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80 && game.State.Winner == Winner.None; step++)
            {
                var prompt = Prompt();
                if (prompt is not null && ready(prompt)) return;
                if (prompt is { SkillPrompt.SkillId: Yongdi })
                    Answer(prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"));
                else if (prompt is null)
                    Submit(new AdvanceOneStepCommand(game.Revision));
                else
                    break;
            }
            throw new InvalidOperationException($"Jianshu missed its expected choice in {game.State.Phase}, " +
                $"at {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }

        Submit(new StartGameCommand());
        Reach(prompt => prompt.Kind == DecisionKind.PlayCard);
        var players = game.CreateSnapshot(0).Players;
        var gift = players[0].Hand.Single(card => card.Kind == CardKind.Dodge && card.Suit == Suit.Spade).Id;
        var recipient = players.SingleOrDefault(player => player.GeneralId == Recipient);
        var otherOpponent = players.SingleOrDefault(player => player.GeneralId == Opponent);
        var emptyPeer = players.SingleOrDefault(player => player.GeneralId == EmptyPeer);
        Need(recipient is { IsAlive: true, HandCount: 0 } && otherOpponent is { IsAlive: true, HandCount: 1 } &&
            emptyPeer is { IsAlive: true, HandCount: 0 },
            "Expected an empty gift recipient, one other hand input and one living empty peer. " +
            $"Actual seats and hand counts [{string.Join(", ", players.Select(player => $"{player.Seat} {player.GeneralId}={player.HandCount}"))}].");
        var recipientSeat = recipient!.Seat;
        var expectedTargets = new[] { 0, otherOpponent!.Seat }.Order().ToArray();
        var action = game.GetHumanLegalActions().Single(candidate => candidate.Kind == LegalActionKind.UseProgramSkill &&
            candidate.ProgramSkillId == Jianshu && candidate.ProgramActivationId == Activation);
        Need(action.SelectableCardIds.SequenceEqual([gift]) && action.SelectableTargetSeats.Contains(recipientSeat),
            "The actual activation offers the black hand entity and permits an empty recipient before delivery.");
        Submit(new UseProgramSkillCommand(0, Jianshu, Activation, [gift], [recipientSeat], game.Revision, Prompt()!.PromptId));
        Reach(prompt => prompt.SkillPrompt?.SkillId == Jianshu &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("stage") == "second"));

        var paid = Fact<BlackGiftContestGiftPaidEvent>();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var menu = Prompt()!;
        var receipt = root.BlackGiftContest!;
        Need(receipt is { Stage: BlackGiftContestStage.ChoosingSecond, SecondSeat: null } &&
            receipt.RecipientSeat == recipientSeat && receipt.GiftCardId == gift && paid.RecipientSeat == recipientSeat &&
            game.CardMovements.Count(move => move.CardId == gift && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Hand(recipientSeat) && move.Sequence > paid.Before && move.Sequence <= paid.After) == 1 &&
            game.CreateSnapshot(recipientSeat).Players[recipientSeat].Hand is [var held] && held.Id == gift,
            "The original paid gift is the recipient's sole native Pindian input before choosing the second seat.");
        Need(game.CreateSnapshot(0).Players[0].HandCount > 0 && menu is { PlayerSeat: 0, IsPrivate: true } &&
            menu.ValidTargetSeats.SequenceEqual(expectedTargets) &&
            menu.Choices.SelectMany(choice => choice.Targets).SequenceEqual(expectedTargets),
            "The donor privately chooses the recipient's opponents in seat order, including the donor and omitting the empty peer. " +
            $"Expected [{string.Join(", ", expectedTargets)}]; offered [{string.Join(", ", menu.ValidTargetSeats)}].");
        var donor = menu.Choices.Single(choice => choice.Targets.SequenceEqual([0]));

        var beforeRestore = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(beforeRestore.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(beforeRestore.CardMovements) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id).BlackGiftContest) ==
                JsonSerializer.Serialize(receipt),
            "Cold replay preserves the paid gift and private candidate menu.");
        Answer(donor);

        var started = Fact<BlackGiftContestStartedEvent>();
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var sourcePrompt = game.CreateSnapshot(recipientSeat).PendingDecision;
        Need(started.RecipientSeat == recipientSeat && started.SecondSeat == 0 && started.FrameId == root.Id &&
            started.PindianFrameId == pair.Id && pair.ParentFrameId == root.Id &&
            pair is { PindianStep: PindianStep.ChooseSourceCard, OpponentSeat: 0 } && pair.SourceSeat == recipientSeat &&
            sourcePrompt is { IsPrivate: true, Choices: [var offered] } && sourcePrompt.PlayerSeat == recipientSeat &&
            offered.Parameters.GetValueOrDefault("action") == "pindian-source-card" && offered.Cards.SequenceEqual([gift]),
            "Selecting the donor opens the real recipient's private source-card choice on the paid Jianshu frame.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-jianshu-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Recipient, "固定受赠者", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Opponent, "固定拼点候选", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(EmptyPeer, "固定空手角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "间书固定牌堆", 0, 1, [])
            {
                PhysicalCards = Enumerable.Range(0, 40)
                    .Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "间书原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Recipient, Opponent, EmptyPeer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
