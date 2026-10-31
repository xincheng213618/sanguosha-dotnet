
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_a_hui_nan_ol_AHuiNan
{
    internal static bool AHuiNanBasicKind(CardKind kind)=>RuntimeRules_Core_CommonRules.IsSlashCard(kind)||kind is CardKind.Dodge or CardKind.Peach or CardKind.Alcohol;

    internal static CardConversionSource AHuiNanSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
}
