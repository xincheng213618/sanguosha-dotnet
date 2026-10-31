#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuiJiOlEntries
{
    internal void AttachQiaoliNoResponse(long resolutionId, CardConversionSource source) => _resolve().AttachQiaoliNoResponse(resolutionId, source);
    internal void ContinueProgramAfterRuiJiDuelUse() => _resolve().ContinueProgramAfterRuiJiDuelUse();
    internal bool HasQiaoliEndingGainPending(int ownerSeat) => _resolve().HasQiaoliEndingGainPending(ownerSeat);
    internal void PresentQiaoliDistributionPrompt(ProgramSkillFrame frame) => _resolve().PresentQiaoliDistributionPrompt(frame);
    internal void PresentQingliangOptionPrompt(ProgramSkillFrame frame) => _resolve().PresentQingliangOptionPrompt(frame);
    internal void PresentQingliangSuitPrompt(ProgramSkillFrame frame) => _resolve().PresentQingliangSuitPrompt(frame);
    internal int QiaoliEndingGainCount(int ownerSeat) => _resolve().QiaoliEndingGainCount(ownerSeat);
    internal SkillProgramStepOutcome QiaoliProgramDuelLaunch(ProgramSkillFrame frame, bool unrespondable) => _resolve().QiaoliProgramDuelLaunch(frame, unrespondable);
    internal SkillProgramStepOutcome QiaoliProgramEndingEquipmentGain(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().QiaoliProgramEndingEquipmentGain(frame, effect);
    internal SkillProgramStepOutcome QiaoliProgramWeaponDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().QiaoliProgramWeaponDamageDraw(frame, effect);
    internal SkillProgramStepOutcome QingliangProgramChooseOption(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().QingliangProgramChooseOption(frame, effect);
    internal void ResolveQiaoliDistributionChoice(PromptChoice selected) => _resolve().ResolveQiaoliDistributionChoice(selected);
    internal void ResolveQingliangOptionChoice(PromptChoice selected) => _resolve().ResolveQingliangOptionChoice(selected);
    internal void ResolveQingliangSuitChoice(PromptChoice selected) => _resolve().ResolveQingliangSuitChoice(selected);
}
