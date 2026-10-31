#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourcePojunCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourcePojunRuntime>> getCoreGameEngineShanheSourcePojunRuntime) : IShanheSourcePojunHost
{
    public SkillProgramStepOutcome HoldAllTargetHand(ProgramSkillFrame frame) => getCoreGameEngineShanheSourcePojunRuntime().Value.HoldShanheSourcePojunTargetHand(frame);
}
