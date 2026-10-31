#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellDizangProgramsEntries
{
    internal bool AllowsHellDizangTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsHellDizangTrigger(gate);
    internal void AnswerHellDayuan(PromptChoice choice) => _resolve().AnswerHellDayuan(choice);
    internal void AssertEquipmentRecastNativeDraw() => _resolve().AssertEquipmentRecastNativeDraw();
    internal bool CanHellDitingMountRecast(CharacterState actor, Card card) => _resolve().CanHellDitingMountRecast(actor, card);
    internal HellDayuanJudgmentOverride? EffectiveHellDayuanOverride(JudgmentFrame frame) => _resolve().EffectiveHellDayuanOverride(frame);
    internal int EffectiveHellDayuanRank(JudgmentFrame frame, int fallback) => _resolve().EffectiveHellDayuanRank(frame, fallback);
    internal Suit EffectiveHellDayuanSuit(JudgmentFrame frame, Suit fallback) => _resolve().EffectiveHellDayuanSuit(frame, fallback);
    internal SkillProgramStepOutcome ExecuteHellDizang(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().ExecuteHellDizang(supplied, operation);
    internal PromptChoice HellDayuanAiChoice(ProgramSkillFrame frame) => _resolve().HellDayuanAiChoice(frame);
    internal IReadOnlyList<PromptChoice> HellDayuanChoices(ProgramSkillFrame frame) => _resolve().HellDayuanChoices(frame);
    internal bool IsExactHellDayuanBeforeResultProgramChild(ProgramJudgmentTriggerWindowFrame window, ProgramSkillFrame child) => _resolve().IsExactHellDayuanBeforeResultProgramChild(window, child);
    internal void PublishHellDayuanChoice(ProgramSkillFrame frame) => _resolve().PublishHellDayuanChoice(frame);
    internal bool ResumeHellBufo(long frameId) => _resolve().ResumeHellBufo(frameId);
    internal bool RetainHellBufoChildren(ProgramSkillFrame frame) => _resolve().RetainHellBufoChildren(frame);
    internal bool TryContinueEquipmentRecastNativeDraw(EquipmentRecastFrame supplied) => _resolve().TryContinueEquipmentRecastNativeDraw(supplied);
}
