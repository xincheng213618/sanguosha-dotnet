#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HuangChengYanProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuangChengYanOlRuntime>> getCharacterHuangChengYanOlRuntime) : IHuangChengYanProgramHost
{
    public SkillProgramStepOutcome JiezhenReplaceSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().Value.JiezhenProgramReplaceSkills(frame, effect);
    public SkillProgramStepOutcome JiezhenRestoreSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().Value.JiezhenProgramRestoreSkills(frame, effect);
    public SkillProgramStepOutcome ZecaiRoundSettlement(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().Value.ZecaiProgramRoundSettlement(frame, effect);
    public SkillProgramStepOutcome YinshiPreventSourcelessDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().Value.YinshiProgramPreventSourcelessDamage(frame, effect);
    public SkillProgramStepOutcome YinshiClaimBaguaJudgmentCard(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterHuangChengYanOlRuntime().Value.YinshiProgramClaimBaguaJudgmentCard(frame, effect);
}
