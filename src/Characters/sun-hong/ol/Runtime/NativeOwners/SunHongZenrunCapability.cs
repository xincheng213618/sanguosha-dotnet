#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SunHongZenrunCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunHongOlEntries> getCharacterSunHongOlRuntime) : ISunHongZenrunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterSunHongOlRuntime().BeginZenrun(f);
}
