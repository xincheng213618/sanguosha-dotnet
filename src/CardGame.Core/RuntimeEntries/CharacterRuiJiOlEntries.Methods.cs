#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuiJiOlEntries
{
    internal void AttachQiaoliNoResponse(long resolutionId, global::CardGame.Core.CardConversionSource source) => _resolve().AttachQiaoliNoResponse(resolutionId, source);
    internal void ContinueProgramAfterRuiJiDuelUse() => _resolve().ContinueProgramAfterRuiJiDuelUse();
    internal bool HasQiaoliEndingGainPending(int ownerSeat) => _resolve().HasQiaoliEndingGainPending(ownerSeat);
    internal void PresentQiaoliDistributionPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentQiaoliDistributionPrompt(frame);
    internal void PresentQingliangOptionPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentQingliangOptionPrompt(frame);
    internal void PresentQingliangSuitPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentQingliangSuitPrompt(frame);
    internal int QiaoliEndingGainCount(int ownerSeat) => _resolve().QiaoliEndingGainCount(ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome QiaoliProgramDuelLaunch(global::CardGame.Core.ProgramSkillFrame frame, bool unrespondable) => _resolve().QiaoliProgramDuelLaunch(frame, unrespondable);
    internal global::CardGame.Core.SkillProgramStepOutcome QiaoliProgramEndingEquipmentGain(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().QiaoliProgramEndingEquipmentGain(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome QiaoliProgramWeaponDamageDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().QiaoliProgramWeaponDamageDraw(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome QingliangProgramChooseOption(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().QingliangProgramChooseOption(frame, effect);
    internal void ResolveQiaoliDistributionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveQiaoliDistributionChoice(selected);
    internal void ResolveQingliangOptionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveQingliangOptionChoice(selected);
    internal void ResolveQingliangSuitChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveQingliangSuitChoice(selected);
}
