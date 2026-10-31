#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourcePojunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourcePojunEntries> getCoreGameEngineShanheSourcePojunRuntime) : IShanheSourcePojunHost
{
    public SkillProgramStepOutcome HoldAllTargetHand(ProgramSkillFrame frame) => getCoreGameEngineShanheSourcePojunRuntime().HoldShanheSourcePojunTargetHand(frame);
}
