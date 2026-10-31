
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_sima_hui_ol_SimaHuiPrograms
{
    internal static bool IsSimaHuiOp(SkillProgramEffectOp op)=>(int)op is >=23500 and <=23505;
}
