
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_qin_lang_ol_QinLang
{
    internal static CardConversionSource QinLangSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);

    internal static bool SameQinLangSource(CardConversionSource a,CardConversionSource b)=>a.SkillId==b.SkillId&&a.OwnerSeat==b.OwnerSeat&&a.SkillInstanceId==b.SkillInstanceId;

    internal static CardKind QinLangName(CardKind kind)=>RuntimeRules_Core_CommonRules.IsSlashCard(kind)?CardKind.Slash:kind;

    internal static bool QinLangImitable(CardKind kind)=>RuntimeRules_Core_CommonRules.IsSlashCard(kind)||kind is CardKind.Peach or CardKind.Alcohol||RuntimeRules_Core_CommonRules.IsOrdinaryTrick(kind)&&kind!=CardKind.Nullification;
}
