using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlMizhaoPindianTargetChecks
{
    private const string Hero = "ol:liu-xie", Mizhao = "ol:mizhao", Activation = "recipient-contest";
    private const string Recipient = "fixture:mizhao-target-recipient", Opponent = "fixture:mizhao-target-opponent";
    private const string EmptyPeer = "fixture:mizhao-target-empty", Deck = "fixture:mizhao-target-deck";

    internal static void WholeHandGiftOpensRecipientPindianCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "mizhao-target", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Spade, 7), new(CardKind.Slash, Suit.Heart, 5)], [],
            [new(Recipient) { StartingHand = [] }, new(EmptyPeer) { StartingHand = [] }],
            [new(Opponent) { StartingHand = [new(CardKind.Crossbow, Suit.Club, 9)] }], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        T Fact<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().Single();
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Mizhao fixture command was rejected.");
        }
        PendingDecision Reach(Func<PendingDecision, bool> ready)
        {
            for (var step = 0; step < 80 && game.State.Winner == Winner.None; step++)
            {
                var prompt = game.CreateSnapshot(0).PendingDecision;
                if (prompt is not null)
                {
                    if (ready(prompt)) return prompt;
                    break;
                }
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException($"Mizhao missed its expected choice in {game.State.Phase}, " +
                $"at {game.CreateSnapshot(0).PendingDecision?.Kind}/{game.CreateSnapshot(0).PendingDecision?.SkillPrompt?.SkillId}.");
        }

        Submit(new StartGameCommand());
        var play = Reach(prompt => prompt.Kind == DecisionKind.PlayCard);
        var players = game.CreateSnapshot(0).Players;
        var gift = players[0].Hand.Select(card => card.Id).Order().ToArray();
        var recipient = players.Single(player => player.GeneralId == Recipient);
        var opponent = players.Single(player => player.GeneralId == Opponent);
        var emptyPeer = players.Single(player => player.GeneralId == EmptyPeer);
        Need(gift.Length >= 2 && recipient is { IsAlive: true, HandCount: 0 } &&
            opponent is { IsAlive: true, HandCount: 1 } && emptyPeer is { IsAlive: true, HandCount: 0 },
            $"Unexpected starting hands [{string.Join(", ", players.Select(player => $"{player.Seat} {player.GeneralId}={player.HandCount}"))}].");
        var action = game.GetHumanLegalActions().Single(candidate => candidate.Kind == LegalActionKind.UseProgramSkill &&
            candidate.ProgramSkillId == Mizhao && candidate.ProgramActivationId == Activation);
        Need(action.MaxCardCount == 0 && action.SelectableTargetSeats.Contains(recipient.Seat),
            "The public activation selects the original recipient before the whole hand is paid.");
        Submit(new UseProgramSkillCommand(0, Mizhao, Activation, [], [recipient.Seat], game.Revision, play.PromptId));
        var menu = Reach(prompt => prompt.SkillPrompt?.SkillId == Mizhao &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == Activation));

        var paid = Fact<RecipientContestGiftPaidEvent>();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var receipt = root.RecipientContest!;
        var moves = game.CardMovements.Where(move => move.Sequence > paid.Before && move.Sequence <= paid.After).ToArray();
        Need(receipt is { Stage: RecipientContestStage.ChoosingThird, ThirdSeat: null, PindianFrameId: null } &&
            root.SelectedTargetSeats.SequenceEqual([recipient.Seat]) && receipt.RecipientSeat == recipient.Seat &&
            paid.RecipientSeat == recipient.Seat && paid.CardCount == gift.Length && receipt.GiftCardIds.Order().SequenceEqual(gift) &&
            moves.Select(move => move.CardId).Order().SequenceEqual(gift) &&
            moves.All(move => move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(recipient.Seat)) &&
            game.CreateSnapshot(0).Players[0].HandCount == 0 &&
            game.CreateSnapshot(recipient.Seat).Players[recipient.Seat].Hand.Select(card => card.Id).Order().SequenceEqual(gift),
            "All original hand entities move to the first recipient on the same paid receipt before Pindian is issued.");
        Need(menu is { PlayerSeat: 0, IsPrivate: true } && menu.ValidTargetSeats.SequenceEqual([opponent.Seat]) &&
            menu.Choices is [var third] && third.Cards.Count == 0 && third.Targets.SequenceEqual([opponent.Seat]),
            "The third-person choice offers the one peer with input and excludes the donor, recipient, and empty peer.");

        var beforeRestore = game;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) ==
            SnapshotJson.Serialize(beforeRestore.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(beforeRestore.CardMovements) &&
            JsonSerializer.Serialize(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == root.Id).RecipientContest) ==
                JsonSerializer.Serialize(receipt),
            "Cold replay retains the original gift receipt and the private third-person menu across all four views.");
        var restoredMenu = game.CreateSnapshot(0).PendingDecision!;
        Submit(new AnswerPromptCommand(0, restoredMenu.PromptId, restoredMenu.Choices.Single().Id, game.Revision));

        var started = Fact<RecipientContestStartedEvent>();
        var pair = game.ResolutionStack.OfType<PindianFrame>().Single();
        var sourcePrompt = game.CreateSnapshot(recipient.Seat).PendingDecision;
        Need(started.FrameId == root.Id && started.RecipientSeat == recipient.Seat && started.ThirdSeat == opponent.Seat &&
            started.PindianFrameId == pair.Id && pair.ParentFrameId == root.Id && pair.SourceSeat == recipient.Seat &&
            pair.OpponentSeat == opponent.Seat && pair.PindianStep == PindianStep.ChooseSourceCard && pair.ProgramResultBind == Activation &&
            sourcePrompt is { Kind: DecisionKind.SkillModule, IsPrivate: true } && sourcePrompt.PlayerSeat == recipient.Seat &&
            sourcePrompt.Choices.SelectMany(choice => choice.Cards).Order().SequenceEqual(gift),
            "The chosen third person opens the original recipient's native Pindian source choice on the paid frame.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mizhao-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Recipient, "固定受赠者", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Opponent, "固定拼点候选", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(EmptyPeer, "固定空手角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "密诏固定牌堆", 0, 1, [])
            {
                PhysicalCards = Enumerable.Range(0, 40)
                    .Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "密诏原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Recipient, Opponent, EmptyPeer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
