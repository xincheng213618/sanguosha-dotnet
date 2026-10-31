#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhaoYunProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhaoYunBoundaryMobileRuntime>> getCharacterZhaoYunBoundaryMobileRuntime) : IMobileZhaoYunProgramHost
{
    public SkillProgramStepOutcome GiveRevealedCard(ProgramSkillFrame frame, string sourceBind) => getCharacterZhaoYunBoundaryMobileRuntime().Value.GiveMobileZhaoYunRevealedCard(frame, sourceBind);
}
