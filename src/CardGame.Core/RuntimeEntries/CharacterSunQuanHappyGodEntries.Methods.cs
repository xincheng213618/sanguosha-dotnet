#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanHappyGodEntries
{
    internal bool AdvanceHappySunQuanChildren() => _resolve().AdvanceHappySunQuanChildren();
    internal PromptChoice AiHappySunQuan(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHappySunQuan(p, f);
    internal void AnswerHappySunQuan(PromptChoice chosen) => _resolve().AnswerHappySunQuan(chosen);
    internal SkillProgramStepOutcome BeginHappySunQuan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHappySunQuan(supplied, op);
    internal bool CompleteHappyLearningStart(ProgramLifecycleTriggerWindowFrame parent) => _resolve().CompleteHappyLearningStart(parent);
    internal bool ContinueHappySunQuan(long id) => _resolve().ContinueHappySunQuan(id);
    internal bool DrainHappySunQuan(ProgramSkillFrame f) => _resolve().DrainHappySunQuan(f);
    internal void FinishHappySunQuan(ProgramSkillFrame f) => _resolve().FinishHappySunQuan(f);
    internal bool HappyDingliQualified(int owner, int holder) => _resolve().HappyDingliQualified(owner, holder);
    internal ProgramTriggerCandidate HappyLearningCandidate(int holder, int turn) => _resolve().HappyLearningCandidate(holder, turn);
    internal ProgramLifecycleTriggerWindowFrame? HappyLearningParent(long id) => _resolve().HappyLearningParent(id);
    internal ProgramCardTriggerWindowFrame? HappyShehuParent(long id) => _resolve().HappyShehuParent(id);
    internal IReadOnlyList<PromptChoice> HappySunQuanChoices(ProgramSkillFrame f) => _resolve().HappySunQuanChoices(f);
    internal bool HappySunQuanDying() => _resolve().HappySunQuanDying();
    internal bool HappySunQuanEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().HappySunQuanEligible(c, t, w);
    internal bool HappySunQuanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HappySunQuanFirstChild(f, child);
    internal bool HappySunQuanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().HappySunQuanMovement(f, effect, pending);
    internal ProgramSkillFrame? HappySunQuanObserverRoot() => _resolve().HappySunQuanObserverRoot();
    internal bool HappySunQuanRecovery(ProgramSkillFrame f, RecoveryReplacementFrame child) => _resolve().HappySunQuanRecovery(f, child);
    internal int HappyXue(int holder) => _resolve().HappyXue(holder);
    internal bool HasHappyLearningPlaySelfOnly(int actor) => _resolve().HasHappyLearningPlaySelfOnly(actor);
    internal bool IsHappyLearningStart(ProgramLifecycleTriggerWindowFrame p) => _resolve().IsHappyLearningStart(p);
    internal bool IsIssuedHappyLearning(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedHappyLearning(c, w);
    internal void RecordHappySunQuanMovement(long id, long before) => _resolve().RecordHappySunQuanMovement(id, before);
    internal bool RetainHappySunQuanPaid(ProgramSkillFrame f) => _resolve().RetainHappySunQuanPaid(f);
    internal void ReturnHappyDingliDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappyDingliDraw(f, outcome);
    internal bool ReturnHappySunQuanMovement(ProgramSkillFrame f) => _resolve().ReturnHappySunQuanMovement(f);
    internal bool TryBeginHappyLearningActualStart(CharacterState current) => _resolve().TryBeginHappyLearningActualStart(current);
    internal bool HasHappySunQuanContent { get => _resolve().HasHappySunQuanContent; }
}
