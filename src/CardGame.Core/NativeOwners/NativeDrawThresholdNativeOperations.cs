#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawThresholdNativeOperations(Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CharacterWeiYanMobileShiEntries> getCharacterWeiYanMobileShiRuntime) : INativeDrawThresholdNativeOperations
{
    public SkillProgramStepOutcome BeginDamage(ProgramSkillFrame frame, int target, int amount) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(frame, target, amount);
    public bool CompletedNativeDrawMatches(ProgramSkillFrame producer, NativeDrawInvocationProof proof) => getCharacterWeiYanMobileShiRuntime().MobileShiKuangguThresholdDrawMatches(producer, proof);
}
