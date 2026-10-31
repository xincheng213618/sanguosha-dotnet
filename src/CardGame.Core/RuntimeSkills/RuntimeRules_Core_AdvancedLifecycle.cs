namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_AdvancedLifecycle
{
    internal static bool AdvancedIsOrdinaryTrick(CardKind kind) => CardCatalog.Get(kind).CategoryName == "锦囊牌" && !RuntimeRules_Core_CommonRules.IsDelayedCard(kind);
}
