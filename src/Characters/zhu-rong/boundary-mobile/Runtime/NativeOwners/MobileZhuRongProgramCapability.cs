#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhuRongProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhuRongBoundaryMobileEntries> getCharacterZhuRongBoundaryMobileRuntime) : IMobileZhuRongProgramHost
{
    public SkillProgramStepOutcome Exchange(ProgramSkillFrame frame) => getCharacterZhuRongBoundaryMobileRuntime().ExchangeMobileZhuRongCards(frame);
}
