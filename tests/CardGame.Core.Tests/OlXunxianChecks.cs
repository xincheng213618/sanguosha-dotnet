using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlXunxianChecks
{
    private const string Skill = "ol:xunxian";
    private const string Mode = "identity:classic-xunxian-test";
    private const string Owner = "fixture:xunxian-owner";
    private const string Recipient = "fixture:xunxian-recipient";
    private const string Peer = "fixture:xunxian-peer";

    public static void SettledEntityIsGivenOnceAfterChoiceRestore()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(),
            new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
            new StandardClassicGeneralPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, UseInteractiveSetup = true, AdvanceAfterHumanCommands = false
        }, content);
        game.DriveStart();
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } setup)
            Submit(game, new SelectGeneralCommand(setup.PlayerSeat,
                setup.PlayerSeat switch { 0 => Owner, 1 => Recipient, _ => $"{Peer}-{setup.PlayerSeat}" },
                game.Revision, setup.PromptId));

        var id = PlayDrawTwo(game);
        var activation = Next(game);
        Require(activation.SkillPrompt?.SkillId == Skill, "The settled physical card must offer 逊贤.");
        Answer(game, activation, activation.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var choicePrompt = Next(game);
        var choices = choicePrompt.Choices.Where(item => item.Parameters.GetValueOrDefault("program-action") == "xunxian-gift").ToArray();
        var players = game.CreateSnapshot(0).Players;
        var owner = players[0];
        var eligible = players.Where(player => player.Seat != 0 && player.IsAlive &&
            (player.HandCount > owner.HandCount || player.Hp > owner.Hp)).Select(player => player.Seat).ToArray();
        var choiceDetails = string.Join(", ", choicePrompt.Choices.Select(item => $"{item.Id} [{string.Join(",", item.Targets)}]"));
        var playerDetails = string.Join(", ", players.Select(player => $"{player.Seat}: HP {player.Hp}, hand {player.HandCount}"));
        Require(choicePrompt is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: 0 } && choicePrompt.SkillPrompt?.SkillId == Skill &&
                eligible.SequenceEqual([1]) && players[1].Hp > owner.Hp && players[1].HandCount <= owner.HandCount &&
                choices.Select(item => item.Targets.SingleOrDefault(-1)).SequenceEqual(eligible) &&
                game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location == CardLocation.DiscardPile,
            $"逊贤 must offer exactly the eligible recipients for the still-discarded card. Prompt {choicePrompt.Kind}, seat {choicePrompt.PlayerSeat}, choices {choiceDetails}. Players {playerDetails}.");
        var choice = choices.Single(item => item.Targets.SequenceEqual([1]));

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(restored.CreateSnapshot(0)),
            "Replay must restore the pending recipient choice and original card in the public discard pile.");
        foreach (var run in new[] { game, restored })
        {
            Answer(run, Next(run), choice);
            var second = PlayDrawTwo(run);
            Require(Next(run) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                    run.CreateCardZoneDiagnostics().Single(card => card.CardId == second).Location == CardLocation.DiscardPile,
                "A second settled card must remain discarded during the same turn.");

            var discard = run.CardMovements.Single(move => move.CardId == id && move.Reason.Value == "card.use-finished");
            var gift = run.CardMovements.Single(move => move.Reason.Value == "skill-program.ol:xunxian.xunxian-gift");
            var giftFact = run.Events.Select(entry => entry.Payload).OfType<ProgramXunxianGiftEvent>().Single();
            var logicalGive = run.Events.Select(entry => entry.Payload).OfType<NativeCardsGivenEvent>().Single();
            Require(gift.CardId == id && gift.From == CardLocation.DiscardPile && gift.To == CardLocation.Hand(1) &&
                    giftFact.CardId == id && giftFact.RecipientSeat == 1 && giftFact.DiscardMovementSequence == discard.Sequence &&
                    logicalGive.Giver == 0 && logicalGive.Recipient == 1 && logicalGive.Count == 1 && logicalGive.CompletionSequence == gift.Sequence &&
                    run.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == id),
                "The original entity must move exactly once and produce one logical Give with the correct receipt.");
        }
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) &&
                SnapshotJson.Serialize(game.CreateSnapshot(1)) == SnapshotJson.Serialize(restored.CreateSnapshot(1)),
            "The restored recipient choice must settle identically.");
    }

    private static int PlayDrawTwo(GameEngine game)
    {
        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The 逊贤 owner must be in Play.");
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.DrawTwo);
        var id = action.CardId!.Value;
        Submit(game, new PlayCardCommand(0, id, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind));
        return id;
    }

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            for (var seat = 0; seat < game.PlayerCount; seat++)
                if (game.CreateSnapshot(seat).PendingDecision is { } prompt)
                    return prompt;
            game.DriveAdvanceOneStep();
        }
        throw new InvalidOperationException("逊贤 did not reach the next public command boundary.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, PromptChoice choice) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "逊贤 command rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-xunxian", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var generals = new[] { Owner, Recipient, $"{Peer}-2", $"{Peer}-3" };
            builder.AddGeneral(new(Owner, "逊贤使用者", "supporter", Skill, "wu", 3));
            builder.AddGeneral(new(Recipient, "高体力角色", "supporter", "standard:none", "wu", 5));
            foreach (var id in generals.Skip(2))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wu", 2));
            const string deck = "fixture:xunxian-deck";
            builder.AddDeck(new(deck, "无中生有牌堆", 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:draw_two", Suit.Heart, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "逊贤规则场景", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: generals));
        }
    }
}
