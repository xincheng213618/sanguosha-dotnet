#nullable enable
using System.Collections.ObjectModel;
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class OwnedHandRankSumCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedHandRankSumProgramsEntries> getCoreGameEngineOwnedHandRankSumProgramsRuntime) : IOwnedHandRankSumHost
{
    public SkillProgramStepOutcome BeginOwnedHandRankSum(ProgramSkillFrame frame, int exactRankSum, string resultBind) => getCoreGameEngineOwnedHandRankSumProgramsRuntime().BeginOwnedHandRankSum(frame, exactRankSum, resultBind);
}
