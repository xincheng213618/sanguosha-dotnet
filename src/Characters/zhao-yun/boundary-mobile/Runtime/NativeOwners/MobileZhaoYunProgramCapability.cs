#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhaoYunProgramCapability(Func<RuntimeEntries.CharacterZhaoYunBoundaryMobileEntries> getCharacterZhaoYunBoundaryMobileRuntime) : IMobileZhaoYunProgramHost
{
    public SkillProgramStepOutcome GiveRevealedCard(ProgramSkillFrame frame, string sourceBind) => getCharacterZhaoYunBoundaryMobileRuntime().GiveMobileZhaoYunRevealedCard(frame, sourceBind);
}
