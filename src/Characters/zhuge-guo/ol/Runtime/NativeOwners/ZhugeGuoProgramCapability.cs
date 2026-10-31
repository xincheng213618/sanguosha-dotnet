#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhugeGuoProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeGuoOlEntries> getCharacterZhugeGuoOlRuntime) : IZhugeGuoProgramHost
{
    public SkillProgramStepOutcome BeginZhugeGuo(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterZhugeGuoOlRuntime().BeginZhugeGuo(f, e);
}
