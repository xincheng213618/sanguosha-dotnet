using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlTongxieChecks
{
    private const string ZhaoYan = "ol:zhao-yan", YanJun = "ol:yan-jun";
    private const string Tongxie = "ol:tongxie", Xunxian = "ol:xunxian", Guanchao = "ol:guanchao";
    private const string Mode = "identity:tongxie-settled-use", Deck = "fixture:tongxie-deck";
    private const int LeadSeat = 0, FollowUpSeat = 1, RecipientSeat = 2, TargetSeat = 3;
    private static readonly string[] Generals = [YanJun, ZhaoYan, "fixture:tongxie-recipient", "fixture:tongxie-target"];

    internal static void TongxieFollowsCompletedSlashAfterXunxianGivesItsMaterialCold()
    {
        var (game, content) = Start();
        ReachYanJunPlay(game);
        EndPlay(game, LeadSeat);
        Reach(game, prompt => IsSkill(prompt, FollowUpSeat, Tongxie, "activate"));
        Activate(game);
        Reach(game, prompt => IsSkill(prompt, FollowUpSeat, Tongxie, "tongxie-arm"));
        Answer(game, choice => Action(choice) == "tongxie-arm" && choice.Targets.SequenceEqual([LeadSeat]));
        Reach(game, prompt => IsPlay(prompt, FollowUpSeat));
        var arm = Facts<ProgramTongxieArmedEvent>(game).Single();
        Require(arm.OwnerSeat == FollowUpSeat && arm.MemberSeats.Order().SequenceEqual([LeadSeat, FollowUpSeat]),
            "Zhao Yan and Yan Jun form the real Tongxie group in Zhao Yan's Play phase.");
        var followUpCard = game.CreateSnapshot(FollowUpSeat).Players[FollowUpSeat].Hand.First(card => card.Kind == CardKind.Slash).Id;
        EndPlay(game, FollowUpSeat);
        Reach(game, prompt => IsPlay(prompt, RecipientSeat));
        EndPlay(game, RecipientSeat);
        Reach(game, prompt => IsPlay(prompt, TargetSeat));
        EndPlay(game, TargetSeat);
        ReachYanJunPlay(game);

        var slash = game.GetLegalActions(LeadSeat).First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([TargetSeat]));
        var leadCard = slash.CardId!.Value;
        Accept(game, new PlayCardCommand(LeadSeat, leadCard, slash.TargetSeats, game.Revision, P(game)!.PromptId, slash.PlayedCardKind));
        FinishSlash(game, prompt => IsSkill(prompt, LeadSeat, Xunxian, "activate"));

        var original = game.CardMovements.Single(move => move.CardId == leadCard && move.Reason == CardMoveReasons.UseFinished);
        var lead = Facts<TongxieSettledSlashUseEvent>(game).Single();
        var settled = Facts<SettledActionCardDiscardEvent>(game).Single();
        var window = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == original.BatchId);
        Require(lead is { ActorSeat: LeadSeat, TargetSeat: TargetSeat, EffectiveKind: CardKind.Slash, ParentProgramFrameId: null } &&
            lead.CardId == leadCard && lead.BatchId == original.BatchId && lead.MovementSequence == original.Sequence &&
            settled.BatchId == original.BatchId && settled.MovementSequence == original.Sequence &&
            Facts<CardUseFinishedEvent>(game).Single().ResolutionId == lead.CardUseFrameId &&
            window.Candidates.Skip(window.CandidateIndex + 1).Any(candidate => candidate.SkillId == Tongxie && candidate.OwnerSeat == FollowUpSeat),
            "A real completed single-target Slash records the shared settlement and queues Tongxie behind Xunxian.");

        var control = Cold(game, content);
        Skip(control);
        Reach(control, prompt => IsSkill(prompt, FollowUpSeat, Tongxie, "tongxie-follow-up"));
        Answer(control, choice => Action(choice) == "tongxie-follow-up-decline");
        Reach(control, prompt => IsPlay(prompt, LeadSeat));
        Require(Zone(control, leadCard) == CardLocation.DiscardPile &&
            Facts<ProgramTongxieFollowUpResolvedEvent>(control).Single().DeclinedBy.SequenceEqual([FollowUpSeat]),
            "Without the earlier gift, Tongxie still offers the original completed Slash's follow-up.");

        Activate(game);
        Reach(game, prompt => IsSkill(prompt, LeadSeat, Xunxian, "xunxian-gift"));
        game = Cold(game, content);
        Answer(game, choice => Action(choice) == "xunxian-gift" && choice.Targets.SequenceEqual([RecipientSeat]));
        Reach(game, prompt => IsSkill(prompt, FollowUpSeat, Tongxie, "tongxie-follow-up") || IsPlay(prompt, LeadSeat));
        var gift = Facts<ProgramXunxianGiftEvent>(game).Single();
        Require(gift.CardId == leadCard && gift.RecipientSeat == RecipientSeat && gift.DiscardMovementSequence == original.Sequence &&
            Zone(game, leadCard) == CardLocation.Hand(RecipientSeat) && Facts<TongxieSettledSlashUseEvent>(game).SequenceEqual([lead]),
            "Xunxian legitimately gifts the original Slash while the same completed native use remains recorded.");
        Require(P(game) is { PlayerSeat: FollowUpSeat, SkillPrompt.SkillId: Tongxie } followUp &&
            followUp.Choices.Any(choice => Action(choice) == "tongxie-follow-up" && choice.Cards.SequenceEqual([followUpCard]) &&
                choice.Targets.SequenceEqual([TargetSeat])) && followUp.Choices.All(choice => !choice.Cards.Contains(leadCard)),
            "Tongxie must still offer Zhao Yan's own Slash against the original target after the triggering card was given away.");

        Answer(game, choice => Action(choice) == "tongxie-follow-up" && choice.Cards.SequenceEqual([followUpCard]));
        Require(FinishSlash(game, prompt => IsPlay(prompt, LeadSeat) || IsSkill(prompt, LeadSeat, Tongxie, "tongxie-follow-up")) is
            { PlayerSeat: LeadSeat, Kind: DecisionKind.PlayCard }, "The follow-up Slash returns to the original Play phase without reopening the chain.");
        var resolved = Facts<ProgramTongxieFollowUpResolvedEvent>(game).Single();
        var child = Facts<TongxieSettledSlashUseEvent>(game).Single(fact => fact.CardId == followUpCard);
        Require(resolved is { OwnerSeat: FollowUpSeat, ActorSeat: LeadSeat, TargetSeat: TargetSeat } && resolved.UsedBy.SequenceEqual([FollowUpSeat]) &&
            resolved.UsedCardIds.SequenceEqual([followUpCard]) && child is { ActorSeat: FollowUpSeat, TargetSeat: TargetSeat } &&
            child.ActionId != lead.ActionId && child.ParentProgramFrameId == resolved.FrameId &&
            Facts<CardUseFinishedEvent>(game).Any(fact => fact.ResolutionId == child.CardUseFrameId && fact.CardId == followUpCard) &&
            Zone(game, leadCard) == CardLocation.Hand(RecipientSeat),
            "Only Zhao Yan's separate hand card pays for the real follow-up Slash, whose completion is marked as part of the original chain.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static bool IsSkill(PendingDecision prompt, int seat, string skill, string action) =>
        prompt.PlayerSeat == seat && prompt.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice => Action(choice) == action);

    private static bool IsPlay(PendingDecision prompt, int seat) => prompt.PlayerSeat == seat && prompt.Kind == DecisionKind.PlayCard;

    private static bool IsYanJunPlayBoundary(PendingDecision prompt) =>
        IsSkill(prompt, LeadSeat, Guanchao, "skip") || IsPlay(prompt, LeadSeat);

    private static void ReachYanJunPlay(GameEngine game)
    {
        Reach(game, IsYanJunPlayBoundary);
        if (P(game) is { SkillPrompt.SkillId: Guanchao }) Skip(game);
        Reach(game, prompt => IsPlay(prompt, LeadSeat));
    }

    private static void EndPlay(GameEngine game, int seat) => Accept(game, new EndPlayPhaseCommand(seat, game.Revision, P(game)!.PromptId));

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static CardLocation Zone(GameEngine game, int cardId) =>
        game.CreateCardZoneDiagnostics().Single(card => card.CardId == cardId).Location;

    private static PendingDecision FinishSlash(GameEngine game, Func<PendingDecision, bool> done)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is not { } prompt)
            {
                Accept(game, new AdvanceOneStepCommand(game.Revision));
                continue;
            }
            if (done(prompt)) return prompt;
            Require(prompt.Kind == DecisionKind.RespondDodge,
                $"Unexpected {prompt.Kind}/{prompt.SkillPrompt?.SkillId} for seat {prompt.PlayerSeat} with actions {string.Join(", ", prompt.Choices.Select(Action))} while settling a native Slash.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
        }
        throw new InvalidOperationException("The native Slash did not finish at a decision boundary.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the public original use, the pending decision and all four seat views.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral || IsYanJunPlayBoundary(prompt));
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral || IsYanJunPlayBoundary(prompt));
        }
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-tongxie-settlement", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, ZhaoYan, YanJun);
            builder.AddGeneral(new(Generals[RecipientSeat], "受赠者", "supporter", "standard:none", "qun", 8));
            builder.AddGeneral(new(Generals[TargetSeat], "受击者", "supporter", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "同协原用牌结算", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
