using System.Globalization;

namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_FirstSlashDuelBenefits
{
    internal static bool IsFirstSlashDuelKind(CardKind kind) => RuntimeRules_Core_CommonRules.IsSlashCard(kind) || kind == CardKind.Duel;

    internal static PromptChoice FirstSlashDuelChoice(ProgramSkillFrame f, string branch, string label) => new(
        new($"first-slash-duel.{f.Id}.{branch}"), label, [], [], new Dictionary<string, string> {
            ["program-action"] = "first-slash-duel-benefit", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
}
