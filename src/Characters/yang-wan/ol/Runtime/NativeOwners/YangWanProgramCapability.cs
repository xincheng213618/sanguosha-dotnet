#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangWanProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterYangWanOlRuntime>> getCharacterYangWanOlRuntime) : IYangWanProgramHost
{
    public SkillProgramStepOutcome YouyanGainSuitCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangWanOlRuntime().Value.YouyanProgramGainSuitCards(frame, effect);
    public SkillProgramStepOutcome ZhuihuanArm(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangWanOlRuntime().Value.ZhuihuanProgramArm(frame, effect);
    public SkillProgramStepOutcome ZhuihuanRetaliate(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangWanOlRuntime().Value.ZhuihuanProgramRetaliate(frame, effect);
}
