#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuangChengYanProgramCapability(Func<RuntimeEntries.CharacterHuangChengYanOlEntries> getCharacterHuangChengYanOlRuntime) : IHuangChengYanProgramHost
{
    public SkillProgramStepOutcome JiezhenReplaceSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().JiezhenProgramReplaceSkills(frame, effect);
    public SkillProgramStepOutcome JiezhenRestoreSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().JiezhenProgramRestoreSkills(frame, effect);
    public SkillProgramStepOutcome ZecaiRoundSettlement(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().ZecaiProgramRoundSettlement(frame, effect);
    public SkillProgramStepOutcome YinshiPreventSourcelessDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().YinshiProgramPreventSourcelessDamage(frame, effect);
    public SkillProgramStepOutcome YinshiClaimBaguaJudgmentCard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().YinshiProgramClaimBaguaJudgmentCard(frame, effect);
}
