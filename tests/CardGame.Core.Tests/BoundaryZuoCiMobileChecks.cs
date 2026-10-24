using CardGame.Core;
using static BoundaryZuoCiScenario;

namespace CardGame.Core.Tests;

internal static class BoundaryZuoCiMobileChecks
{
    public static void EachActualDamagePointAddsOneAvatarToTheOriginalNativeLibrary()
    {
        var (registry, game) = CreateScene();
        Answer(game, choice => choice.Parameters.GetValueOrDefault("action") == "finish"); Play(game);
        var initial = Library(game);
        Hurt(game); Reach(game, prompt => Optional(prompt, Xinsheng)); Activate(game);
        Reach(game, prompt => Optional(prompt, Xinsheng));
        var firstPoint = Library(game);
        Need(firstPoint.Source == initial.Source && firstPoint.Count == initial.Count + 1 &&
            firstPoint.GeneralIds!.Except(initial.GeneralIds!).Count() == 1 &&
            game.CreateSnapshot(0).Players[0].Hp == 1 &&
            game.Events.Select(item => item.Payload).OfType<DamageAppliedEvent>().Count(item => item.TargetSeat == 0 && item.Amount == 2) == 1,
            "This mobile Xinsheng adds exactly one avatar for the first point of an actual two-point damage, leaving the second point independently optional on the original native library.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Activate(game); Play(game);
        var completed = Library(game);
        Need(completed.Source == initial.Source && completed.Count == initial.Count + 2 &&
            completed.GeneralIds!.Distinct().Count() == completed.Count &&
            completed.GeneralIds!.Except(initial.GeneralIds!).Count() == 2 &&
            completed.RevealedGeneralId == initial.RevealedGeneralId && completed.DeclaredSkillId == initial.DeclaredSkillId &&
            game.Events.Select(item => item.Payload).OfType<ProgramBindingResolvedEvent>()
                .Count(item => item.SkillId == Xinsheng && item.BindingId == "damage" && item.Activated && item.Completed) == 2,
            "After cold recovery the second point adds only one more real avatar; the first point is not paid twice and the displayed avatar's skill source remains intact.");
    }

    private static void Need(bool value, string reason)
    { if (!value) throw new InvalidOperationException(reason); }
}
