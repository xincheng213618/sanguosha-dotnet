#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileSunJianHost(Func<RuntimeEntries.CharacterSunJianBoundaryMobileEntries> getCharacterSunJianBoundaryMobileRuntime) : IMobileSunJianPoluHost
{
    public SkillProgramStepOutcome Draw(ProgramSkillFrame frame) => getCharacterSunJianBoundaryMobileRuntime().BeginMobileSunJianPolu(frame);
}
