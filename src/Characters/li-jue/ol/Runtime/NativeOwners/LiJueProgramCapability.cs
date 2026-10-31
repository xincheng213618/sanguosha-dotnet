#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class LiJueProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiJueOlEntries> getCharacterLiJueOlRuntime) : ILiJueProgramHost
{
    public SkillProgramStepOutcome BeginLiJue(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterLiJueOlRuntime().BeginLiJue(f, e);
}
