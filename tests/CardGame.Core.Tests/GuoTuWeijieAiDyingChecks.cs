using CardGame.Core;

internal static partial class GuoTuWeijieChecks
{
    private const string AiDyingWatch = "fixture:wj-ai-dying-paid-watch";

    public static void AiFailedDyingAttemptReturnsToOriginalResponderWithoutLoopCold()
    {
        var (game, registry) = Start("ai-dying-failed");
        Use(game, "wound-peer", [2]); Play(game);
        Need(V(game, 2).Hp == 1 && game.State.CurrentSeat == 0, "The true foreign-turn victim reaches one HP before the original damage.");
        Use(game, "damage-peer", [2]);
        Reach(game, () => Current(game) is { Stage: WeijieStage.Card, Need: { Intent: WeijieIntent.Dying, ProviderSeat: 1 } });
        var original = Current(game)!.Need;
        Need(original.Targets.SequenceEqual([2]) && original.Cursor >= 0 &&
            game.ResolutionStack.OfType<DyingFrame>().Any(frame => frame.Id == original.OwnerFrameId && frame.VictimSeat == 2 && frame.ResponderSeat == 1),
            "The actual AI responder owns this exact native dying request and opaque payment choice.");
        game = Cold(game, registry);
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == AiDyingWatch);
        var paid = E<WeijieCostPaidEvent>(game).Single();
        Need(paid.Source.OwnerSeat == 1 && paid.ActualTurnOwner == 0 && paid.VictimSeat == 0 && paid.EffectiveName == CardKind.Dodge &&
            Current(game) is { Stage: WeijieStage.CostChildren, OutputKind: CardKind.Peach } &&
            game.CardMovements.Count(move => move.Sequence == paid.Sequence && move.CardId == paid.CardId &&
                move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile) == 1,
            "The AI pays one genuine opaque foreign Hand card whose printed Basic name cannot supply the requested Peach; the real movement child pauses before failure returns.");
        game = Cold(game, registry);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Play(game);
        Need(E<WeijieCostPaidEvent>(game).Count(fact => fact.FrameId == paid.FrameId) == 1 &&
            E<WeijieAttemptResolvedEvent>(game).Single(fact => fact.FrameId == paid.FrameId) is { Matched: false, NativeLegal: false } &&
            !E<WeijieUseIssuedEvent>(game).Any(fact => fact.AttemptId == paid.FrameId) &&
            !E<WeijieResponseIssuedEvent>(game).Any(fact => fact.AttemptId == paid.FrameId) &&
            E<PlayerDiedEvent>(game).Count(fact => fact.VictimSeat == 2) == 1 && !V(game, 2).IsAlive &&
            !game.ResolutionStack.Any(frame => frame.Id == original.OwnerFrameId || frame.Id == paid.FrameId) &&
            game.CardMovements.Count(move => move.Sequence == paid.Sequence && move.CardId == paid.CardId) == 1 &&
            P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard },
            "Cold return spends the AI attempt once, advances the original dying responder without a stranded human-style let-die prompt, and completes actual death plus damage before returning to Play.");
        _ = Cold(game, registry);
    }
}
