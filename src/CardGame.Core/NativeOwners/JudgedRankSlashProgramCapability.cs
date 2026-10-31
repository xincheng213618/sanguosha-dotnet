#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class JudgedRankSlashProgramCapability(Func<RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries> getCoreGameEngineJudgedRankSlashPoliciesRuntime) : IJudgedRankSlashProgramHost
{
    public void GrantJudgedRankSplitSlashTurnPolicy(ProgramSkillFrame frame, string bind) => getCoreGameEngineJudgedRankSlashPoliciesRuntime().GrantOriginalJudgedRankSlashPolicy(frame, bind);
    public void DrawFromOtherActualBasicDiscard(ProgramSkillFrame frame) => getCoreGameEngineJudgedRankSlashPoliciesRuntime().DrawFromOriginalOtherBasicDiscard(frame);
}
