using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlSilvChecks
{
    private const string LiWan = "ol:li-wan", GuoYouZhi = "mou:guo-you-zhi-mobile";
    private const string Lianju = "ol:lianju", Bique = "mou-mobile:bique";
    private const string Gain = "skill-program.li-wan.lianju.gain", Refund = "skill-program.li-wan.silv.refund";
    private const string Discard = "skill-program.mou-mobile:bique.discard";
    private const string Mode = "identity:ol-silv-test", Deck = "fixture:ol-silv-deck";
    private static readonly string[] Generals = [LiWan, GuoYouZhi, "fixture:ol-silv-peer-2", "fixture:ol-silv-peer-3"];

    internal static void BiqueDiscardRefundsTheMarkedEntityToItsActualHolder()
    {
        var (game, content) = Start();
        var use = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.DrawTwo);
        var cardId = use.CardId!.Value;
        Accept(game, new PlayCardCommand(0, cardId, use.TargetSeats, game.Revision, P(game)!.PromptId, use.PlayedCardKind));
        Settle(game);
        var used = game.CardMovements.Single(move => move.CardId == cardId && move.Reason == CardMoveReasons.UseFinished);
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Lianju } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
        Activate(game);
        Answer(game, choice => IsBranch(choice, "red"));
        Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Answer(game, choice => choice.Cards.SequenceEqual([cardId]));
        Answer(game, choice => IsBranch(choice, "finish"));
        Reach(game, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        var gain = game.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == Gain);
        var mark = game.Events.Select(e => e.Payload).OfType<SilvEntityMarkedEvent>().Single();
        Require(used.To == CardLocation.DiscardPile && used.Sequence < gain.Sequence &&
                gain.From == CardLocation.DiscardPile && gain.To == CardLocation.Hand(1) &&
                mark.Source.OwnerSeat == 0 && mark.CardId == cardId && mark.GainMovementSequence == gain.Sequence &&
                game.CreateSnapshot(1).Players[1].Hand.Single(card => card.Id == cardId).SkillMarkLabel == "闾",
            "Li Wan actually gives the card she used to Guo You Zhi, with the original persistent Silv mark.");

        var turn = game.State.TurnNumber;
        var hand = game.CreateSnapshot(1).Players[1].Hand.Select(card => card.Id).ToArray();
        Accept(game, new UseProgramSkillCommand(1, Bique, "inspect-and-give", [], [2], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => HasBiqueBranch(prompt, "gift-confirm"));
        Answer(game, choice => IsBranch(choice, "gift-confirm"));
        Reach(game, prompt => HasBiqueBranch(prompt, "adjust"));
        Answer(game, choice => IsBranch(choice, "adjust"));
        Reach(game, prompt => HasBiqueBranch(prompt, "discard-card"));
        foreach (var id in hand)
            Answer(game, choice => IsBranch(choice, "discard-card") && choice.Cards.SequenceEqual([id]));

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, 4).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay retains the original mark and the real Bique decision for every viewer.");
        Answer(restored, choice => IsBranch(choice, "discard-confirm"));
        Reach(restored, prompt => prompt is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });

        var facts = restored.Events.Select(e => e.Payload).ToArray();
        var invoice = facts.OfType<NativeLogicalDiscardInvocationRecordedEvent>().Single(e => e.Reason.Value == Discard);
        var material = facts.OfType<NativeLogicalDiscardMaterialCompletedEvent>().Single(e => e.InvocationId == invoice.InvocationId && e.CardId == cardId);
        var discarded = restored.CardMovements.Single(move => move.CardId == cardId && move.Reason.Value == Discard);
        Require(invoice.TurnNumber == turn && invoice is { InitiatorSeat: 1, ExecutorSeat: 1, ActualTurnOwnerSeat: 1 } &&
                invoice.ActualCount == hand.Length && material.From == CardLocation.Hand(1) && material.To == CardLocation.DiscardPile &&
                discarded.Sequence == material.CompletionSequence && discarded.From == material.From && discarded.To == material.To,
            "Bique completes a native discard of the marked entity from its real holder's hand.");

        var refunds = restored.CardMovements.Where(move => move.Reason.Value == Refund).ToArray();
        Require(refunds is [var refund] && refund.CardId == cardId && refund.Sequence > discarded.Sequence &&
                refund.From == CardLocation.DiscardPile && refund.To == CardLocation.Hand(1) &&
                restored.CreateSnapshot(1).Players[1].Hand is [{ Id: var remaining, SkillMarkLabel: "闾" }] && remaining == cardId,
            "Silv must refund the marked Bique discard exactly once to the actual holder.");
        var issued = facts.OfType<LiWanStartedEvent>().Single(e => e.Op == SkillProgramEffectOp.ReturnDiscardedSilvEntity);
        Require(issued.Source.OwnerSeat == 0 && issued.RefundSequence == discarded.Sequence &&
                facts.OfType<LiWanPaidEvent>().Single(e => e.FrameId == issued.FrameId) is { RecipientSeat: 1, Count: 1 } &&
                facts.OfType<LiWanCompletedEvent>().Count(e => e.FrameId == issued.FrameId) == 1,
            "The original Li Wan mark issues one refund for the exact discard entry and returns to Bique.");
    }

    private static bool IsBranch(PromptChoice choice, string branch) => choice.Parameters.GetValueOrDefault("branch") == branch;

    private static bool HasBiqueBranch(PendingDecision prompt, string branch) =>
        prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Bique } && prompt.Choices.Any(choice => IsBranch(choice, branch));

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3],
            HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        ReachSetupBoundary();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachSetupBoundary();
        }
        return (game, content);

        void ReachSetupBoundary() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral ||
            prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-silv-bique-discard", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, LiWan, GuoYouZhi);
            foreach (var id in Generals.Skip(2))
                builder.AddGeneral(new(id, "实际同桌", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体无中生有", 3, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:draw_two", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "思闾返还裨阙弃牌", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
