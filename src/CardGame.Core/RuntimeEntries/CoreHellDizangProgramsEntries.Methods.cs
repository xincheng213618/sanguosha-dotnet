#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreHellDizangProgramsEntries
{
    internal bool AllowsHellDizangTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsHellDizangTrigger(gate);
    internal void AnswerHellDayuan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHellDayuan(choice);
    internal void AssertEquipmentRecastNativeDraw() => _resolve().AssertEquipmentRecastNativeDraw();
    internal bool CanHellDitingMountRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().CanHellDitingMountRecast(actor, card);
    internal global::CardGame.Core.HellDayuanJudgmentOverride? EffectiveHellDayuanOverride(global::CardGame.Core.JudgmentFrame frame) => _resolve().EffectiveHellDayuanOverride(frame);
    internal int EffectiveHellDayuanRank(global::CardGame.Core.JudgmentFrame frame, int fallback) => _resolve().EffectiveHellDayuanRank(frame, fallback);
    internal global::CardGame.Core.Suit EffectiveHellDayuanSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback) => _resolve().EffectiveHellDayuanSuit(frame, fallback);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteHellDizang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ExecuteHellDizang(supplied, operation);
    internal global::CardGame.Core.PromptChoice HellDayuanAiChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HellDayuanAiChoice(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HellDayuanChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HellDayuanChoices(frame);
    internal bool IsExactHellDayuanBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child) => _resolve().IsExactHellDayuanBeforeResultProgramChild(window, child);
    internal void PublishHellDayuanChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishHellDayuanChoice(frame);
    internal bool ResumeHellBufo(long frameId) => _resolve().ResumeHellBufo(frameId);
    internal bool RetainHellBufoChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RetainHellBufoChildren(frame);
    internal bool TryContinueEquipmentRecastNativeDraw(global::CardGame.Core.EquipmentRecastFrame supplied) => _resolve().TryContinueEquipmentRecastNativeDraw(supplied);
}
