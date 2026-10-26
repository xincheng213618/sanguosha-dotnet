using System.Text.Json;
using CardGame.Core;
using static WuYiEnhancementScenario;

internal static class WuYiEnhancementChecks
{
    public static void ConfirmedOptionsAndExtraTargetResumeOnce()
    {
        var (game, registry) = Start();
        var pair = CurrentCardEnhancement.IgnoreArmor | CurrentCardEnhancement.Uncancelable;
        Require(!game.PendingDecision!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("enhancement-selection") == "14"),
            "Only subsets within the actual two-option budget are published.");
        Answer(game, Selection(game, pair));
        Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
        Require(Facts(game).Select(fact => fact.Enhancement).SequenceEqual([CurrentCardEnhancement.IgnoreArmor, CurrentCardEnhancement.Uncancelable]) &&
            game.CreateSnapshot(0).Players[3].Hp == 5, "One confirmed pair applies both native effects to the same damaging Slash once.");
        Cold(game, registry);

        foreach (var flags in new[] { CurrentCardEnhancement.ExtraTarget, CurrentCardEnhancement.ExtraTarget | CurrentCardEnhancement.IgnoreArmor })
        {
            (game, registry) = Start();
            Answer(game, Selection(game, flags));
            Require(game.ResolutionStack.OfType<ProgramSkillFrame>().Last().CardEnhancementDraft is
                { ChoosingExtraTarget: true, FinishAfterExtraTarget: true }, "The original frame owns the confirmed extra-target continuation.");
            game = Cold(game, registry);
            Answer(game, game.PendingDecision!.Choices.Single(choice => choice.Targets.SequenceEqual([2])));
            Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard);
            Require(Facts(game).Aggregate(CurrentCardEnhancement.None, (value, fact) => value | fact.Enhancement) == flags &&
                Facts(game).Count(fact => fact.Enhancement == CurrentCardEnhancement.ExtraTarget) == 1 &&
                game.CreateSnapshot(0).Players[2].Hp == 5 && game.CreateSnapshot(0).Players[3].Hp == 5,
                "Cold target confirmation finishes exactly the committed one/two options without reopening effect selection.");
            Cold(game, registry);
        }
    }

    private static CurrentCardEnhancedEvent[] Facts(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<CurrentCardEnhancedEvent>().ToArray();
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(JsonSerializer.Serialize(restored.CreateSnapshot(0)) == JsonSerializer.Serialize(game.CreateSnapshot(0)), "Accepted confirmed selections replay to the same native prompt and view.");
        return restored;
    }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
