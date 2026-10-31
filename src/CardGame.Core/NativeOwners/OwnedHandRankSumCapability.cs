#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OwnedHandRankSumCapability(Func<RuntimeEntries.CoreGameEngineOwnedHandRankSumProgramsEntries> getCoreGameEngineOwnedHandRankSumProgramsRuntime) : IOwnedHandRankSumHost
{
    public SkillProgramStepOutcome BeginOwnedHandRankSum(ProgramSkillFrame frame, int exactRankSum, string resultBind) => getCoreGameEngineOwnedHandRankSumProgramsRuntime().BeginOwnedHandRankSum(frame, exactRankSum, resultBind);
}
