#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanHuanOlEntries
{
    internal bool AdvanceYuanHuanSubtree() => _resolve().AdvanceYuanHuanSubtree();
    internal void AnswerYuanHuan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerYuanHuan(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginYuanHuan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginYuanHuan(supplied, op);
    internal global::CardGame.Core.PromptChoice ChooseYuanHuan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseYuanHuan(p, f);
    internal bool ContinueIssuedYuanHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedYuanHuan(f);
    internal void ContinueYuanHuan(long id) => _resolve().ContinueYuanHuan(id);
    internal int DeruActualNames(int seat) => _resolve().DeruActualNames(seat);
    internal bool DrainYuanHuanChildren(long id) => _resolve().DrainYuanHuanChildren(id);
    internal void FinishYuanHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishYuanHuan(f);
    internal bool IsYuanHuanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsYuanHuanRecoveryParent(f, recovery);
    internal void IssueDeruTake(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueDeruTake(f);
    internal void IssueYuanHuanDraw(global::CardGame.Core.ProgramSkillFrame f, bool linjie) => _resolve().IssueYuanHuanDraw(f, linjie);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LinjieCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LinjieCards(f);
    internal void PublishYuanHuanChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYuanHuanChoice(f);
    internal bool QualifyYuanHuan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyYuanHuan(c, trigger, context);
    internal void RecordYuanHuanStep(long id, string reason, long before, int actual, int? subject = default, int? card = default, global::CardGame.Core.CardLocation? from = default, global::CardGame.Core.CardLocation? to = default) => _resolve().RecordYuanHuanStep(id, reason, before, actual, subject, card, from, to);
    internal bool ResumeYuanHuan(long id) => _resolve().ResumeYuanHuan(id);
    internal bool ReturnYuanHuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYuanHuanMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YuanHuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YuanHuanChoices(f);
    internal bool YuanHuanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().YuanHuanFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? YuanHuanObserverRoot() => _resolve().YuanHuanObserverRoot();
}
