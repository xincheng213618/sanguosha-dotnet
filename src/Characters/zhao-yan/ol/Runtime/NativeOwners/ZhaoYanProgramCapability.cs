#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoYanProgramCapability(Func<RuntimeEntries.CharacterZhaoYanOlEntries> getCharacterZhaoYanOlRuntime) : IZhaoYanProgramHost
{
    public SkillProgramStepOutcome TongxieArm(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().TongxieProgramArm(frame, effect);
    public SkillProgramStepOutcome TongxieFollowUp(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().TongxieProgramFollowUp(frame, effect);
    public SkillProgramStepOutcome TongxieGuard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().TongxieProgramGuard(frame, effect);
}
