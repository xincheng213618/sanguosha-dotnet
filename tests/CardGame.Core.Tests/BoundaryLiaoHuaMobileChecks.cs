using CardGame.Core;
using static BoundaryLiaoHuaScenario;

namespace CardGame.Core.Tests;

internal static class BoundaryLiaoHuaMobileChecks
{
    public static void ActualDiscardSlashGainReturnsBeforeExtraPlayAndEmptyPileStillInsertsItCold()
    {
        var (registry, game) = CreateScene();
        var candidates = Decision(game)!.Choices.SelectMany(choice => choice.Cards).ToArray();
        Need(candidates.Length == 2 && candidates.Distinct().Count() == 2 && candidates.All(id =>
            game.CardMovements.Any(move => move.CardId == id && move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile)),
            "The locked start chooses among the two actual Slash entities previously discarded from the owner's real hand.");
        var selected = candidates[0]; Answer(game, choice => choice.Cards.SequenceEqual([selected]));
        Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Gain);
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Dangxian);
        Need(frame.BoundaryLiaoHuaDiscardSlash is { CardId: { } card } paid && card == selected && paid.BatchId > 0 &&
            frame.PendingMovementContinuation is not null && game.CreateSnapshot(0).Players[0].Hand.Single().Id == selected &&
            !game.Events.Any(item => item.Payload is PhaseChangedEvent { ActorSeat: 0, Phase: TurnPhase.Play }),
            "The exact native discard gain remains paid on its original start frame while the gain child is paused; extra Play has not overtaken that child.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Continue(game); Play(game);
        Need(game.CardMovements.Count(move => move.CardId == selected && move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0) && move.Reason.Value == GainReason) == 1 &&
            game.Events.Select(item => item.Payload).OfType<PhaseChangedEvent>().Count(item => item.ActorSeat == 0 && item.Phase == TurnPhase.Play) == 1 &&
            !game.Events.Any(item => item.Payload is PhaseChangedEvent { ActorSeat: 0, Phase: TurnPhase.Draw }) &&
            game.Events.Select(item => item.Payload).OfType<ProgramPhaseScheduledEvent>().Count(item => item.FrameId == frame.Id && item.Started) == 1,
            "Cold returns the paid gain once and starts the real scheduled extra Play before the normal turn's draw flow.");
        End(game); Play(game);
        Need(game.Events.Select(item => item.Payload).OfType<PhaseChangedEvent>().Count(item => item.ActorSeat == 0 && item.Phase == TurnPhase.Play) == 2 &&
            game.CreateSnapshot(0).Players[0].Hp == 4 && !game.Events.Any(item => item.Payload is DamageAppliedEvent { SourceSeat: 0, TargetSeat: 0 }),
            "Finishing the real extra Play resumes the separate normal turn without the OL edition's no-damage self-damage debt.");

        var (_, empty) = CreateScene(discardSlash: false);
        Need(empty.CreateSnapshot(0).Players[0].HandCount == 0 && empty.Events.Select(item => item.Payload).OfType<PhaseChangedEvent>().Count(item => item.ActorSeat == 0 && item.Phase == TurnPhase.Play) == 1,
            "An empty discard Slash pool still inserts the forced extra Play and never obtains a substitute from the draw pile.");
        End(empty); Play(empty);
        Need(empty.Events.Select(item => item.Payload).OfType<PhaseChangedEvent>().Count(item => item.ActorSeat == 0 && item.Phase == TurnPhase.Play) == 2,
            "The empty-pool extra Play also returns to its normal turn exactly once.");
    }

    public static void FuliRecoversToLivingFactionsAndComparesCurrentHpAfterRecoveryChildrenCold()
    {
        foreach (var raisePeer in new[] { false, true })
        {
            var (registry, game) = CreateScene(discardSlash: false, recovery: true, raisePeer: raisePeer);
            FatalDamage(game);
            Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "program-trigger" && choice.Parameters.GetValueOrDefault("skill-id") == Fuli) == true);
            Need(game.CreateSnapshot(0).Players[0].Hp == -4, "Fuli is offered during a genuine negative-HP dying caused by the actual six-point damage.");
            Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "program-trigger" && choice.Parameters.GetValueOrDefault("skill-id") == Fuli);
            Reach(game, prompt => prompt?.SkillPrompt?.SkillId == (raisePeer ? RecoveryPeer : Recovery));
            var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Fuli);
            Need(game.CreateSnapshot(0).Players[0] is { Hp: 4, MaxHp: 6, IsFaceDown: false } &&
                game.CreateSnapshot(0).Players.Where(player => player.IsAlive).Select(player => registry.Generals[player.GeneralId!].FactionId).Distinct().Count() == 4 &&
                game.CreateSnapshot(0).Players[0].HandCount == 0,
                "The limited skill recovers negative HP up to the four living printed factions, below this actual maximum HP, without the OL edition's hand draw or premature turnover.");
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Continue(game);
            if (raisePeer)
            {
                Reach(game, prompt => prompt?.SkillPrompt?.SkillId == RecoveryPeer && prompt.Choices.Any(choice => choice.Targets.Count == 1));
                Answer(game, choice => choice.Targets.SequenceEqual([1]));
            }
            Play(game);
            Need(game.CreateSnapshot(0).Players[0].Hp == 4 && game.CreateSnapshot(0).Players[0].IsFaceDown == !raisePeer &&
                (!raisePeer || game.CreateSnapshot(0).Players[1].Hp == 4) &&
                game.Events.Select(item => item.Payload).OfType<ProgramBindingResolvedEvent>().Count(item => item.FrameId == original.Id && item.SkillId == Fuli && item.Activated && item.Completed) == 1 &&
                game.Events.Select(item => item.Payload).OfType<RecoveryAppliedEvent>().Count(item => item.TargetSeat == 0) == 1,
                "After the original recovery child returns cold, Fuli flips only for strictly unique current highest HP; a peer healed by that child creates a real tie and prevents the flip without repeating recovery.");
        }
    }

    private static void Need(bool value, string reason)
    { if (!value) throw new InvalidOperationException(reason); }
}
