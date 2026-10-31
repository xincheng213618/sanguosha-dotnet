#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SunHongZenrunCapability(Func<RuntimeEntries.CharacterSunHongOlEntries> getCharacterSunHongOlRuntime) : ISunHongZenrunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterSunHongOlRuntime().BeginZenrun(f);
}
