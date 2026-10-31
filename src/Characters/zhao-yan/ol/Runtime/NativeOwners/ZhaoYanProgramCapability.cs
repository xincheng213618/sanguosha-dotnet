#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoYanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhaoYanOlEntries> getCharacterZhaoYanOlRuntime) : IZhaoYanProgramHost
{
    public SkillProgramStepOutcome TongxieArm(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().TongxieProgramArm(frame, effect);
    public SkillProgramStepOutcome TongxieFollowUp(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().TongxieProgramFollowUp(frame, effect);
    public SkillProgramStepOutcome TongxieGuard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().TongxieProgramGuard(frame, effect);
}
