using CardGame.Core;
using Scene = BoundaryLiaoHuaScenario;

internal static class MobileDangxianChecks
{
    public static void GainAndExtraPlaySurviveColdRestore()
    {
        var (registry, original) = Scene.CreateScene();
        var prompt = Scene.Decision(original)!;
        var candidates = prompt.Choices.Select(choice => choice.Cards.Single()).ToArray();
        Check(candidates.Length == 2 && original.CreateCardZoneDiagnostics()
            .Where(card => candidates.Contains(card.CardId)).All(card => card.Location == CardLocation.DiscardPile),
            "Dangxian must offer the two physical Slashes discarded at game start.");
        Check(original.CreateSnapshot(1).PendingDecision is null, "The discard selection belongs to Liao Hua.");

        var game = Cold(original, registry);
        var cardId = candidates[^1];
        Scene.Answer(game, choice => choice.Cards.SequenceEqual([cardId]));
        Scene.Reach(game, decision => decision?.SkillPrompt?.SkillId == Scene.Gain);
        CheckGain(game, cardId);
        Check(PlayPhaseCount(game) == 0, "The gain child must resolve before the extra Play phase starts.");
        game = Cold(game, registry);
        Scene.Continue(game);
        CheckExtraPlayThenNormalPlay(game);
        CheckGain(game, cardId);
        Check(game.CreateCardZoneDiagnostics().Single(card => card.CardId == candidates[0]).Location == CardLocation.DiscardPile,
            "The other physical Slash stays in the discard pile.");

        var (_, empty) = Scene.CreateScene(discardSlash: false);
        CheckExtraPlayThenNormalPlay(empty);
        Check(!Events<BoundaryLiaoHuaDiscardSlashObtainedEvent>(empty).Any(),
            "An empty discard pile grants the extra Play phase without generating a gain receipt.");
    }

    private static void CheckExtraPlayThenNormalPlay(GameEngine game)
    {
        Scene.Play(game);
        Check(PlayPhaseCount(game) == 1, "The first opportunity starts one Play phase.");
        Scene.End(game);
        Scene.Play(game);
        Check(PlayPhaseCount(game) == 2, "After the extra Play ends, the normal turn reaches its own Play phase.");
    }

    private static int PlayPhaseCount(GameEngine game) =>
        Events<PhaseChangedEvent>(game).Count(phase => phase.ActorSeat == 0 && phase.Phase == TurnPhase.Play);

    private static IEnumerable<T> Events<T>(GameEngine game) =>
        game.Events.Select(envelope => envelope.Payload).OfType<T>();

    private static void CheckGain(GameEngine game, int cardId)
    {
        var movement = game.CardMovements.Single(move => move.Reason.Value == Scene.GainReason);
        var evidence = Events<BoundaryLiaoHuaDiscardSlashObtainedEvent>(game).Single();
        Check(movement.CardId == cardId && movement.From == CardLocation.DiscardPile &&
            movement.To == CardLocation.Hand(0) && evidence.CardId == cardId &&
            evidence.OwnerSeat == 0 && evidence.Before < movement.Sequence && evidence.After == movement.Sequence,
            "The selected Slash must move once from discard into Liao Hua's hand with matching original evidence.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var checkpoint = GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint()));
        var restored = GameReplay.Restore(checkpoint, registry);
        Check(Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold restore must preserve the decision and card visibility for every seat.");
        return restored;
    }

    private static void Check(bool valid, string message)
    {
        if (!valid)
            throw new InvalidOperationException(message);
    }
}
