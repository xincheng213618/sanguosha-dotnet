#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhaoYanProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhaoYanOlRuntime>> getCharacterZhaoYanOlRuntime) : IZhaoYanProgramHost
{
    public SkillProgramStepOutcome TongxieArm(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().Value.TongxieProgramArm(frame, effect);
    public SkillProgramStepOutcome TongxieFollowUp(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().Value.TongxieProgramFollowUp(frame, effect);
    public SkillProgramStepOutcome TongxieGuard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterZhaoYanOlRuntime().Value.TongxieProgramGuard(frame, effect);
}
