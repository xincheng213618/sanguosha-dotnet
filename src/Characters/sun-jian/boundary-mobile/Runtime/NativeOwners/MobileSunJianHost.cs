#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileSunJianHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterSunJianBoundaryMobileRuntime>> getCharacterSunJianBoundaryMobileRuntime) : IMobileSunJianPoluHost
{
    public SkillProgramStepOutcome Draw(ProgramSkillFrame frame) => getCharacterSunJianBoundaryMobileRuntime().Value.BeginMobileSunJianPolu(frame);
}
