namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_AlternativeViewAsCosts
{
    internal static CardKind ProgramBasicCardName(CardKind kind) => RuntimeRules_Core_CommonRules.IsSlashCard(kind) ? CardKind.Slash : kind;
}
