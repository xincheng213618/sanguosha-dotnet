#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawThresholdNativeOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries> getCharacterWeiYanMobileShiRuntime) : INativeDrawThresholdNativeOperations
{
    public SkillProgramStepOutcome BeginDamage(ProgramSkillFrame frame, int target, int amount) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(frame, target, amount);
    public bool CompletedNativeDrawMatches(ProgramSkillFrame producer, NativeDrawInvocationProof proof) => getCharacterWeiYanMobileShiRuntime().MobileShiKuangguThresholdDrawMatches(producer, proof);
}
