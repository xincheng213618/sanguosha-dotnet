namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_zhou_qun_ol_ZhouQun
{
    internal static CardConversionSource ZhouQunSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static int ZhouQunNameLength(CardKind kind) => RuntimeRules_Core_CommonRules.IsSlashCard(kind) ? 1 : kind == CardKind.BorrowedSword ? 2 : CardCatalog.Get(kind).DisplayName.Length;
}
