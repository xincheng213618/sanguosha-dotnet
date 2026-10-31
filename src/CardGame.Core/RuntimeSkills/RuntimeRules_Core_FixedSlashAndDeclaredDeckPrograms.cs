namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_FixedSlashAndDeclaredDeckPrograms
{
    internal static bool MatchesDeclaredDeckCriterion(Card card, string criterion) => criterion switch
    {
        "basic" => RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(card.Kind) == SkillProgramCardCategory.Basic,
        "trick" => RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(card.Kind) == SkillProgramCardCategory.Trick,
        "equipment" => RuntimeRules_Core_ProgramCardPayments.GetProgramCardCategory(card.Kind) == SkillProgramCardCategory.Equipment,
        "red" => card.Suit is Suit.Heart or Suit.Diamond,
        "black" => card.Suit is Suit.Spade or Suit.Club,
        _ => throw new InvalidOperationException("Unknown declared deck criterion.")
    };
}
