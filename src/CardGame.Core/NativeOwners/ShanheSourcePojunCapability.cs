#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourcePojunCapability(Func<RuntimeEntries.CoreGameEngineShanheSourcePojunEntries> getCoreGameEngineShanheSourcePojunRuntime) : IShanheSourcePojunHost
{
    public SkillProgramStepOutcome HoldAllTargetHand(ProgramSkillFrame frame) => getCoreGameEngineShanheSourcePojunRuntime().HoldShanheSourcePojunTargetHand(frame);
}
