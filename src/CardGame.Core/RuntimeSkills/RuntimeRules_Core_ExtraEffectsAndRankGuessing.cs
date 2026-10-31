
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_ExtraEffectsAndRankGuessing
{
    internal static bool ExtraEffectCardKind(CardKind kind) => CardCatalog.Get(kind).CategoryName == "基本牌" ||
        CardCatalog.Get(kind).CategoryName == "锦囊牌" && !RuntimeRules_Core_CommonRules.IsDelayedCard(kind);
}
