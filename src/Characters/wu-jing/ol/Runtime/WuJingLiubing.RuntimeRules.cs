namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_wu_jing_ol_WuJingLiubing
{
    internal static bool WuJingPhysicalSlash(Card card, CardKind kind, IReadOnlyList<int> ids, IReadOnlyList<CardConversionSource>? conversions) =>
        card.Id > 0 && ids is [var id] && id == card.Id && RuntimeRules_Core_CommonRules.IsSlashCard(card.Kind) && kind == card.Kind && conversions is null or { Count: 0 };
}
