#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class TangJiProgramCapability(Func<RuntimeEntries.CharacterTangJiOlEntries> getCharacterTangJiOlRuntime) : ITangJiProgramHost
{
    public SkillProgramStepOutcome KanggeChooseTarget(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTangJiOlRuntime().KanggeProgramChooseTarget(frame, effect);
    public SkillProgramStepOutcome KanggeGainDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTangJiOlRuntime().KanggeProgramGainDraw(frame, effect);
    public SkillProgramStepOutcome KanggeHealVictim(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTangJiOlRuntime().KanggeProgramHealVictim(frame, effect);
    public SkillProgramStepOutcome KanggeDeathPrice(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTangJiOlRuntime().KanggeProgramDeathPrice(frame, effect);
    public SkillProgramStepOutcome JieliePreventAndGift(ProgramSkillFrame frame, SkillProgramEffect effect) => getCharacterTangJiOlRuntime().JielieProgramPreventAndGift(frame, effect);
}
