#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDrawThresholdRuntimePortsRuntime
{
    SkillProgramStepOutcome INativeDrawThresholdNativeOperations.BeginDamage(ProgramSkillFrame frame, int target, int amount) => _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, target, amount);
    bool INativeDrawThresholdNativeOperations.CompletedNativeDrawMatches(ProgramSkillFrame producer, NativeDrawInvocationProof proof) => _characterWeiYanMobileShiOperations.MobileShiKuangguThresholdDrawMatches(producer, proof);
}
