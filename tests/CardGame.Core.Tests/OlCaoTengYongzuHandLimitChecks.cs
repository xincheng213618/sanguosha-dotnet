using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlCaoTengYongzuHandLimitChecks
{
    private const string CaoTeng = "ol:cao-teng";
    private const string Qingliu = "ol:qingliu";
    private const string Yongzu = "ol:yongzu";
    private const string Recipient = "fixture:yongzu-recipient";
    private const string Mode = "identity:classic-fixture-yongzu-hand-limit";
    private const string Deck = "fixture:yongzu-fixed-dodges";

    internal static void OtherChooserKeepsTheIssuedCardInActualDiscardCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1], HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, registry);
        Submit(game, new StartGameCommand());

        var faction = Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Qingliu });
        Answer(game, faction, choice => choice.Parameters.GetValueOrDefault("branch") == "wei");
        var offer = Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Yongzu } &&
            p.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        var origin = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>()
            .Single(frame => frame.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow);
        var issuerTurn = game.State.TurnNumber;
        Require(game.CreateSnapshot(0).Players[0].FactionId == "wei" &&
            game.CreateSnapshot(1).Players[1] is { GeneralId: Recipient, Hp: 1 } &&
            Facts<CaoTengHandLimitGrantedEvent>(game).Length == 0,
            "The real Qingliu choice has switched Cao Teng to Wei, and the selected one-HP recipient has received no limit grant yet.");

        Answer(game, offer, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        var target = Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Yongzu } && HasBranch(p, "target"));
        Answer(game, target, choice => choice.Parameters.GetValueOrDefault("branch") == "target" && choice.Targets.SequenceEqual([1]));
        var first = Reach(game, p => p is { PlayerSeat: 0, SkillPrompt.SkillId: Yongzu } && HasBranch(p, "restore"));
        Answer(game, first, choice => choice.Parameters.GetValueOrDefault("branch") == "restore");

        var second = Reach(game, p => p is { PlayerSeat: 1, SkillPrompt.SkillId: Yongzu } && HasBranch(p, "limit"));
        var issuedFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Yongzu);
        Require(second.IsPrivate && game.CreateSnapshot(0).PendingDecision is null && !HasBranch(second, "restore") &&
            issuedFrame.WindowContext?.ParentFrameId == origin.Id && issuedFrame.CaoTeng is
                { Stage: CaoTengStage.YongzuChoice, Cursor: 1, TargetSeat: 1, SameFaction: true, FirstOption: "restore" } &&
            Facts<CaoTengRecipientChosenEvent>(game).SequenceEqual([new(issuedFrame.Id, 1, true)]) &&
            Facts<CaoTengHandLimitGrantedEvent>(game).Length == 0,
            "The original other participant alone can choose the unclaimed limit option on the paid preparation frame.");
        Answer(game, second, choice => choice.Parameters.GetValueOrDefault("branch") == "limit");

        var ownerPlay = Reach(game, p => p is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var grant = Facts<CaoTengHandLimitGrantedEvent>(game).Single();
        Require(grant == new CaoTengHandLimitGrantedEvent(issuedFrame.Id, 1, 1) &&
            Facts<CaoTengBenefitIssuedEvent>(game) is
            [
                { Index: 0, RecipientSeat: 0, Option: "restore" },
                { Index: 1, RecipientSeat: 1, Option: "limit" }
            ] &&
            game.CreateSnapshot(0).Players[0].HandLimit == game.CreateSnapshot(0).Players[0].Hp,
            "The single permanent grant belongs to the other chooser and leaves the issuer's ordinary limit intact.");
        Submit(game, new EndPlayPhaseCommand(0, game.Revision, ownerPlay.PromptId));
        var nextPlay = Reach(game, p => p is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        var turn = game.State.TurnNumber;
        var recipient = game.CreateSnapshot(1).Players[1];
        var hand = recipient.Hand.Select(card => card.Id).ToHashSet();
        Require(turn > issuerTurn && recipient is { Hp: 1, HandLimit: 2 } && hand.Count == 3 &&
            game.CardMovements.Count(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(1) && move.Reason == CardMoveReasons.Draw) == 2,
            "The original recipient reaches their own actual Play with three physical cards at one HP after a normal two-card Draw.");
        Submit(game, new EndPlayPhaseCommand(1, game.Revision, nextPlay.PromptId));

        var discard = Reach(game, p => p is { PlayerSeat: 1, Kind: DecisionKind.DiscardCards });
        var obligation = game.ResolutionStack.OfType<ManualDiscardFrame>().Single();
        Require(game.State.Phase == TurnPhase.Discard && obligation.OwnerSeat == 1 && obligation.ActualTurnNumber == turn &&
            obligation.InitialRequiredCount == 1 && discard.RequiredCardCount == 1 && hand.SetEquals(discard.ValidCardIds),
            "The recipient's native Discard owes one of the three original cards under the issued limit of two.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Facts<CaoTengHandLimitGrantedEvent>(restored).SequenceEqual([grant]) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) ==
                SnapshotJson.Serialize(game.CreateSnapshot(seat))) && restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack.OfType<ManualDiscardFrame>().Single()) == JsonSerializer.Serialize(obligation),
            "Cold recovery preserves the issued grant, exact unpaid native Discard, all physical movement and each private view.");

        var payment = restored.CreateSnapshot(1).PendingDecision!.ValidCardIds.First();
        Submit(restored, new DiscardCardsCommand(1, [payment], discard.PromptId, restored.Revision));
        var paid = restored.CardMovements.Where(move => move.TurnNumber == turn && move.From == CardLocation.Hand(1) &&
            move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard).Select(move => move.CardId);
        var kept = restored.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).ToHashSet();
        Require(paid.SequenceEqual([payment]) && kept.Count == 2 && kept.SetEquals(hand.Where(id => id != payment)) &&
            Facts<CaoTengHandLimitGrantedEvent>(restored).SequenceEqual([grant]),
            "The accepted native command discards the chosen original card exactly once and keeps the original other two.");
    }

    private static bool HasBranch(PendingDecision prompt, string branch) =>
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == branch);

    private static PendingDecision Reach(GameEngine game, Func<PendingDecision, bool> predicate)
    {
        for (var step = 0; step < 140; step++)
        {
            var prompt = Enumerable.Range(0, 2).Select(seat => game.CreateSnapshot(seat).PendingDecision)
                .FirstOrDefault(decision => decision is { PlayerSeat: 0 or 1 });
            if (prompt is not null && predicate(prompt))
                return prompt;
            if (prompt is { Kind: DecisionKind.SelectGeneral })
            {
                var general = prompt.PlayerSeat == 0 ? CaoTeng : Recipient;
                Submit(game, new SelectGeneralCommand(prompt.PlayerSeat, general, game.Revision, prompt.PromptId));
            }
            else
            {
                Require(prompt is null, $"Unexpected Yongzu fixture prompt {prompt?.PlayerSeat}/{prompt?.Kind}/{prompt?.SkillPrompt?.SkillId}.");
                Submit(game, new AdvanceOneStepCommand(game.Revision));
            }
        }

        throw new InvalidOperationException("The Yongzu fixture did not reach the selected native prompt.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> select) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Yongzu fixture command failed.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-cao-teng-yongzu-hand-limit", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, CaoTeng, "ol:liu-xie");
            string[] otherGenerals = ["fixture:yongzu-peer-2", "fixture:yongzu-peer-3"];
            builder.AddGeneral(new(Recipient, "原拥族受益者", "supporter", "standard:none", "wei", 1));
            foreach (var general in otherGenerals)
                builder.AddGeneral(new(general, "其他角色", "supporter", "standard:none", "shu", 4));
            builder.AddDeck(new(Deck, "固定拥族实体闪", 1, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 48).Select(index =>
                    new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, index % 13 + 1)).ToArray()
            });
            builder.AddMode(new(Mode, "拥族真实弃牌", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: [CaoTeng, Recipient, .. otherGenerals]));
        }
    }
}
