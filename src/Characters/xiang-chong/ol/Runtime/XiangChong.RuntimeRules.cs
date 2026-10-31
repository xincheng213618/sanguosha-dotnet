
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_xiang_chong_ol_XiangChong
{
    internal static CardConversionSource XiangSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);

    internal static Dictionary<string,string> XiangArgs(string branch)=>new(){{"program-action","xiang-chong"},{"branch",branch}};
}
