#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhuRongProgramCapability(Func<RuntimeEntries.CharacterZhuRongBoundaryMobileEntries> getCharacterZhuRongBoundaryMobileRuntime) : IMobileZhuRongProgramHost
{
    public SkillProgramStepOutcome Exchange(ProgramSkillFrame frame) => getCharacterZhuRongBoundaryMobileRuntime().ExchangeMobileZhuRongCards(frame);
}
