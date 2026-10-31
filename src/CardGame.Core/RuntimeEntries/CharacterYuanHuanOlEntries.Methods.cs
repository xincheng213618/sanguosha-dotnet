#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanHuanOlEntries
{
    internal bool AdvanceYuanHuanSubtree() => _resolve().AdvanceYuanHuanSubtree();
    internal void AnswerYuanHuan(PromptChoice choice) => _resolve().AnswerYuanHuan(choice);
    internal SkillProgramStepOutcome BeginYuanHuan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginYuanHuan(supplied, op);
    internal PromptChoice ChooseYuanHuan(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseYuanHuan(p, f);
    internal bool ContinueIssuedYuanHuan(ProgramSkillFrame f) => _resolve().ContinueIssuedYuanHuan(f);
    internal void ContinueYuanHuan(long id) => _resolve().ContinueYuanHuan(id);
    internal int DeruActualNames(int seat) => _resolve().DeruActualNames(seat);
    internal bool DrainYuanHuanChildren(long id) => _resolve().DrainYuanHuanChildren(id);
    internal void FinishYuanHuan(ProgramSkillFrame f) => _resolve().FinishYuanHuan(f);
    internal bool IsYuanHuanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsYuanHuanRecoveryParent(f, recovery);
    internal void IssueDeruTake(ProgramSkillFrame f) => _resolve().IssueDeruTake(f);
    internal void IssueYuanHuanDraw(ProgramSkillFrame f, bool linjie) => _resolve().IssueYuanHuanDraw(f, linjie);
    internal IReadOnlyList<PromptChoice> LinjieCards(ProgramSkillFrame f) => _resolve().LinjieCards(f);
    internal void PublishYuanHuanChoice(ProgramSkillFrame f) => _resolve().PublishYuanHuanChoice(f);
    internal bool QualifyYuanHuan(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().QualifyYuanHuan(c, trigger, context);
    internal void RecordYuanHuanStep(long id, string reason, long before, int actual, int? subject = default, int? card = default, CardLocation? from = default, CardLocation? to = default) => _resolve().RecordYuanHuanStep(id, reason, before, actual, subject, card, from, to);
    internal bool ResumeYuanHuan(long id) => _resolve().ResumeYuanHuan(id);
    internal bool ReturnYuanHuanMovement(ProgramSkillFrame f) => _resolve().ReturnYuanHuanMovement(f);
    internal IReadOnlyList<PromptChoice> YuanHuanChoices(ProgramSkillFrame f) => _resolve().YuanHuanChoices(f);
    internal bool YuanHuanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().YuanHuanFirstChild(f, child);
    internal ProgramSkillFrame? YuanHuanObserverRoot() => _resolve().YuanHuanObserverRoot();
}
