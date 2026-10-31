#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SuFeiProgramCapability(Func<RuntimeEntries.CharacterSuFeiOlEntries> getCharacterSuFeiOlRuntime) : ISuFeiProgramHost
{
    public SkillProgramStepOutcome BeginSuFei(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterSuFeiOlRuntime().BeginSuFei(f, e);
}
