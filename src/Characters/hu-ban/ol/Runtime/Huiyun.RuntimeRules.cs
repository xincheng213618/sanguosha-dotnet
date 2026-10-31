
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_hu_ban_ol_Huiyun
{
    internal static HuiyunOpportunity HuiyunCurrent(HuiyunReceipt r)=>r.Opportunities[r.Index];
}
