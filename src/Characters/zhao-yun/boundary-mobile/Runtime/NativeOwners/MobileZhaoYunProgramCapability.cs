#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhaoYunProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhaoYunBoundaryMobileEntries> getCharacterZhaoYunBoundaryMobileRuntime) : IMobileZhaoYunProgramHost
{
    public SkillProgramStepOutcome GiveRevealedCard(ProgramSkillFrame frame, string sourceBind) => getCharacterZhaoYunBoundaryMobileRuntime().GiveMobileZhaoYunRevealedCard(frame, sourceBind);
}
