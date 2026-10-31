using CardGame.Core;

internal static class BaguaResponseChecks
{
    public static void FailedJudgmentReturnsToDodgeAndColdReplay()
    {
        foreach (var arrows in new[] { false, true })
        {
            var (game, registry) = BaguaResponseScenario.Create(arrows: arrows);
            var hp = game.CreateSnapshot(0).Players[0].Hp;
            var before = game.PendingDecision!.PromptId;
            BaguaResponseScenario.Answer(game, "bagua");
            BaguaResponseScenario.Reach(game, DecisionKind.RespondDodge);
            BaguaResponseScenario.Require(game.Events.Select(item => item.Payload).OfType<JudgmentResolvedEvent>()
                .Any(item => item.Reason == JudgmentReasons.BaguaDefense && !item.Succeeded) &&
                game.CreateSnapshot(0).Players[0].Hp == hp && game.PendingDecision!.PromptId != before &&
                game.PendingDecision.Choices.All(choice => choice.Parameters.GetValueOrDefault("response") is not ("bagua" or "bagua-skip")),
                "A black Bagua judgment must reopen the same Dodge without damage or a second armor attempt.");
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            BaguaResponseScenario.Require(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "The failed-judgment hand response must cold replay with its exact prompt and attack.");
            BaguaResponseScenario.Answer(game, "take-damage");
            BaguaResponseScenario.Require(game.CreateSnapshot(0).Players[0].Hp == hp - 1 &&
                game.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Count(item => item.TargetSeat == 0) == 1,
                "Declining the continued response after a black judgment must apply the incoming damage exactly once.");
            var dodge = restored.PendingDecision!.Choices.First(choice => choice.Parameters.GetValueOrDefault("response") == "dodge").Cards.Single();
            BaguaResponseScenario.Answer(restored, "dodge");
            BaguaResponseScenario.Require(restored.CreateSnapshot(0).Players[0].Hp == hp &&
                restored.CreateCardZoneDiagnostics().Single(card => card.CardId == dodge).Location == CardLocation.DiscardPile &&
                restored.Events.Select(item => item.Payload).OfType<JudgmentRequestedEvent>().Count(item => item.Reason == JudgmentReasons.BaguaDefense) == 1,
                "The real hand Dodge must pay once and prevent damage after the one failed judgment.");
        }
    }
}
