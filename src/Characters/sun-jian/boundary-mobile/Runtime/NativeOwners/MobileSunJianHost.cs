#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileSunJianHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunJianBoundaryMobileEntries> getCharacterSunJianBoundaryMobileRuntime) : IMobileSunJianPoluHost
{
    public SkillProgramStepOutcome Draw(ProgramSkillFrame frame) => getCharacterSunJianBoundaryMobileRuntime().BeginMobileSunJianPolu(frame);
}
