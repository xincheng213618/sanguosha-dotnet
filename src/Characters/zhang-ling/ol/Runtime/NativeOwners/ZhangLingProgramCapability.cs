#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangLingProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangLingOlEntries> getCharacterZhangLingOlRuntime) : IZhangLingProgramHost
{
    public SkillProgramStepOutcome PlaceCategoryTalisman(ProgramSkillFrame frame, string bind) => getCharacterZhangLingOlRuntime().PlaceCategoryTalisman(frame, bind);
    public SkillProgramStepOutcome ReleaseCategoryTalisman(ProgramSkillFrame frame) => getCharacterZhangLingOlRuntime().ReleaseCategoryTalisman(frame);
    public SkillProgramStepOutcome UseHuqiSlash(ProgramSkillFrame frame, string bind) => getCharacterZhangLingOlRuntime().UseHuqiSlash(frame, bind);
}
