#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangWanProgramCapability(Func<RuntimeEntries.CharacterYangWanOlEntries> getCharacterYangWanOlRuntime) : IYangWanProgramHost
{
    public SkillProgramStepOutcome YouyanGainSuitCards(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangWanOlRuntime().YouyanProgramGainSuitCards(frame, effect);
    public SkillProgramStepOutcome ZhuihuanArm(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangWanOlRuntime().ZhuihuanProgramArm(frame, effect);
    public SkillProgramStepOutcome ZhuihuanRetaliate(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterYangWanOlRuntime().ZhuihuanProgramRetaliate(frame, effect);
}
