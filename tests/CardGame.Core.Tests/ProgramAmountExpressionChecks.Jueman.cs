using System.Reflection;
using CardGame.Core;

internal static partial class ProgramAmountExpressionChecks
{
    private const string Jueman = "ol:jueman", JuemanDrawReason = "skill-program.ol:jueman.Draw";

    private static void JuemanDrawUsesRealBasicPrefixCold()
    {
        var (game, registry) = CreateDaili("jueman-source-loss");
        var dodge = View(game, 0).Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        var slash = View(game, 0).Hand.Single(card => card.Kind == CardKind.Slash).Id;
        Use(game, "give", [1], DailiDriver, [dodge]); Play(game);
        var action = game.GetHumanLegalActions().First(candidate => candidate.Kind == LegalActionKind.Slash &&
            candidate.CardId == slash && candidate.TargetSeats.SequenceEqual([1]) && candidate.ConversionSource is null);
        Accept(game, new PlayCardCommand(0, slash, [1], game.Revision, game.PendingDecision!.PromptId, action.PlayedCardKind, action.TargetCardId));
        Play(game);
        var turn = game.State.TurnNumber;
        Need(game.CardMovements.Any(move => move.CardId == dodge && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing) &&
            Events<CardResponseCompletedEvent>(game).Any(fact => fact.ActorSeat == 1 && fact.ProviderSeat == 1 && fact.EffectiveKind == CardKind.Dodge),
            "The fixed Team game pays a real Slash and the target's own real Dodge instead of fabricating a basic-use prefix.");
        var hand = View(game, 0).HandCount;
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, game.PendingDecision!.PromptId));
        ReachAccepted(game, () => DailiOption(game, "jueman-draw"));
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Jueman);
        var frameId = frame.Id;
        var history = (MatchRuntime)typeof(GameEngine).GetProperty("MatchRuntime", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(game)!;
        var queries = history.Capabilities.Get<IActualCardUseHistoryQueries>();
        var ended = queries.FindTurnEndPoint(turn, 0)!;
        var prefix = queries.ReadCardUseFacts(ended, CardUseHistoryView.ActualUses);
        Need(prefix.Take(2).Select(fact => fact.ActorSeat).SequenceEqual([0, 1]) &&
            prefix[0] is { Type: CardActionType.Use, DeclaredKind: CardKind.Slash } &&
            prefix[1] is { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge, IsSelfNativeResponseUse: true } &&
            frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } && frame.InvocationAuthorization is not null &&
            Events<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == frameId) is { RequestedCount: 1, ActualDrawCount: 1 } &&
            View(game, 0).HandCount == hand + 1,
            "Exactly one of the original completed turn's first two basic actors is the owner, so ordinary JSON Draw1 pays once before its actual child.");
        game = Cold(game, registry); AnswerAccepted(game, "jueman-draw");
        ReachAccepted(game, () => DailiOption(game, "jueman-return"));
        Need(View(game, 0).Skills?.Any(skill => skill.Id == Jueman) != true,
            "The real paid gain child removes Jueman before returning to its original AfterTurnEnded program.");
        game = Cold(game, registry); AnswerAccepted(game, "jueman-return");
        ReachAccepted(game, () => Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == frameId));
        Need(Events<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == frameId).Completed &&
            Events<NativeDrawAttemptStartedEvent>(game).Count(fact => fact.Context.Reason.Value == JuemanDrawReason) == 1 &&
            Events<ProgramDrawCompletedEvent>(game).Count(fact => fact.FrameId == frameId) == 1,
            "The delivered ordinary Draw1 and its declared source-loss permission finish the same original frame once after command cold recovery.");
    }
}
