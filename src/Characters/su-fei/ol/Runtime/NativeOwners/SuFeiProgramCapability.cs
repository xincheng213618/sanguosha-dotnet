#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SuFeiProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSuFeiOlEntries> getCharacterSuFeiOlRuntime) : ISuFeiProgramHost
{
    public SkillProgramStepOutcome BeginSuFei(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterSuFeiOlRuntime().BeginSuFei(f, e);
}
