namespace CardGame.Core;

// The scheduler frame retains the frozen quota and all completed payments.
// This module sequences the existing prompt, movement and command-session owners.
[ProgramRuntimeModule]
internal sealed class ManualDiscardRuntime(IProgramRuntimeFrames frames, INativeFrameRoutingFacts nativeFrames,
    IMatchParticipantStateAccess participants, IProgramRuntimePrompts prompts, IProgramRuntimeEvents events,
    IProgramTurnQueries turn, IProgramPhaseQueries phase, IProgramBoundCardMovementOperations movement,
    IProgramMovementWindows movementWindows, IManualDiscardFacts facts,
    IManualDiscardTransitions transitions, IManualDiscardCommands commands,
    INativeLogicalDiscardOperations logicalDiscards)
{
    internal void RequestHumanDiscard(CharacterState player)
    {
        var frame = frames.Top as ManualDiscardFrame;
        if (frame is null)
        {
            var required = facts.RequiredDiscard(player);
            frame = new ManualDiscardFrame(frames.NextId(), player.Seat, turn.TurnNumber, required, required)
                { QuotaMultiplier = facts.QuotaMultiplier(player) };
            frames.Push(frame);
        }
        var hand = facts.DiscardableHand(player);
        var handLimit = facts.HandLimit(player);
        var count = Math.Min(frame.RemainingCount, hand.Count);
        transitions.SetPrompt(new PendingDecision(
            DecisionKind.DiscardCards,
            player.Seat,
            $"手牌上限为 {handLimit}，请选择 {count} 张手牌弃置。",
            hand.Select(card => card.Id).ToArray(),
            [])
        {
            PromptId = prompts.NextId(),
            RequiredCardCount = count
        });
        transitions.PublishState();
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 171,
        NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(ManualDiscardFrame), 351)]
    internal void ContinueManualDiscard(long frameId)
    {
        if (frames.Top is not ManualDiscardFrame frame || frame.Id != frameId ||
            frame.OwnerSeat != turn.CurrentSeat || frame.ActualTurnNumber != turn.TurnNumber || phase.Phase != TurnPhase.Discard)
            throw new InvalidOperationException("Manual discard lost its owning turn.");
        if (prompts.Current is not null) return;
        if (movementWindows.Begin(frame.Id)) return;
        var owner = participants.At(frame.OwnerSeat);
        if (facts.Winner == Winner.None && owner.IsAlive && frame.RemainingCount > 0 && facts.DiscardableHand(owner).Count > 0)
        {
            RequestHumanDiscard(owner);
            return;
        }
        frames.Pop(frame.Id, ResolutionFrameKind.ManualDiscard);
        if (facts.Winner != Winner.None) return;
        if (transitions.BeginDiscardPhaseEnded(owner)) return;
        transitions.EndTurn();
    }

    internal bool HasManualDiscardBoundaryFrame()
    {
        if ((nativeFrames.FrameCount == 0 ? null : nativeFrames.FrameAt(0)) is not ManualDiscardFrame frame) return false;
        if (frame.OwnerSeat != turn.CurrentSeat || frame.ActualTurnNumber != turn.TurnNumber || phase.Phase != TurnPhase.Discard ||
            frame.InitialRequiredCount <= 0 || frame.PaidCount < 0 || frame.PaidCount > frame.InitialRequiredCount ||
            frame.RemainingCount < 0 || frame.RemainingCount > frame.InitialRequiredCount - frame.PaidCount ||
            frame.QuotaMultiplier is < 1 or > 20)
            throw new InvalidOperationException("Manual discard lost its frozen remaining quota.");
        return true;
    }

    internal CommandResult SubmitDiscardCards(DiscardCardsCommand command)
    {
        var validation = commands.ValidatePrompt(command.ActorSeat, command.PromptId);
        if (validation is not null) return commands.Reject(validation.Code, validation.Message);

        var prompt = prompts.Current!;
        var actor = participants.At(command.ActorSeat);
        if (phase.Phase != TurnPhase.Discard || turn.CurrentSeat != actor.Seat || !actor.IsAlive)
            return commands.Reject(CommandErrorCode.IllegalAction, "The actor is not in their discard phase.");

        var ids = command.CardIds.ToArray();
        if (ids.Length == 0 || ids.Length > prompt.RequiredCardCount || ids.Distinct().Count() != ids.Length)
            return commands.Reject(CommandErrorCode.InvalidChoice, $"Select between 1 and {prompt.RequiredCardCount} distinct cards.");

        var hand = facts.DiscardableHand(actor);
        var handIds = hand.Select(card => card.Id).ToHashSet();
        if (ids.Any(id => !prompt.ValidCardIds.Contains(id) || !handIds.Contains(id)))
            return commands.Reject(CommandErrorCode.InvalidCard, "Every selected card must be in the published hand.");

        if (frames.Top is not ManualDiscardFrame frame || frame.OwnerSeat != actor.Seat ||
            frame.ActualTurnNumber != turn.TurnNumber || Math.Min(frame.RemainingCount, hand.Count) != prompt.RequiredCardCount)
            return commands.Reject(CommandErrorCode.IllegalAction, "The remaining discard obligation changed; refresh the prompt.");

        var selected = ids.ToHashSet();
        var cards = hand.Where(card => selected.Contains(card.Id)).OrderBy(card => card.Id).ToArray();
        return commands.Accept(() =>
        {
            frames.ReplaceTop(frame with { RemainingCount = frame.RemainingCount - cards.Length,
                PaidCount = checked(frame.PaidCount + cards.Length) });
            var discardInvocation = logicalDiscards.Capture(actor.Seat, actor.Seat,
                cards.Select(card => card.Id).ToArray(), CardMoveReasons.HandLimitDiscard);
            movement.MoveCards(cards, CardLocation.Hand(actor.Seat), CardLocation.DiscardPile,
                CardMoveReasons.HandLimitDiscard);
            logicalDiscards.Complete(discardInvocation);
            prompts.Clear();
            transitions.Log("CardsDiscarded", $"{actor.Name} 弃置 {cards.Length} 张手牌。", actor.Seat);
            events.AdvanceFact(new HandLimitDiscardedEvent(actor.Seat,
                Array.AsReadOnly(cards.Select(card => card.Id).ToArray())));
            ContinueManualDiscard(frame.Id);
        });
    }

    internal void AssertDiscardPromptInvariant()
    {
        if (prompts.Current is { Kind: DecisionKind.DiscardCards } prompt)
        {
            var owner = participants.At(prompt.PlayerSeat);
            var hand = facts.DiscardableHand(owner);
            if (frames.Top is not ManualDiscardFrame frame || frame.OwnerSeat != owner.Seat ||
                frame.ActualTurnNumber != turn.TurnNumber || !HasManualDiscardBoundaryFrame() ||
                !owner.IsHuman || !owner.IsAlive || phase.Phase != TurnPhase.Discard ||
                turn.CurrentSeat != owner.Seat || facts.Status != EngineStatus.AwaitingHumanDiscard ||
                prompt.RequiredCardCount <= 0 || prompt.RequiredCardCount != Math.Min(frame.RemainingCount, hand.Count) ||
                !prompt.ValidCardIds.Order().SequenceEqual(hand.Select(card => card.Id).Order()) ||
                prompt.Choices.Count != 0 || prompt.ValidTargetSeats.Count != 0 || !prompt.IsPrivate)
                throw new InvalidOperationException("The private discard prompt must match its current owner's hand limit.");
        }
        else if (facts.Status == EngineStatus.AwaitingHumanDiscard)
        {
            throw new InvalidOperationException("Discard status requires a private discard prompt.");
        }
    }
}
