#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanHappyGodEntries
{
    internal bool AdvanceHappySunQuanChildren() => _resolve().AdvanceHappySunQuanChildren();
    internal global::CardGame.Core.PromptChoice AiHappySunQuan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHappySunQuan(p, f);
    internal void AnswerHappySunQuan(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerHappySunQuan(chosen);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappySunQuan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHappySunQuan(supplied, op);
    internal bool CompleteHappyLearningStart(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame parent) => _resolve().CompleteHappyLearningStart(parent);
    internal bool ContinueHappySunQuan(long id) => _resolve().ContinueHappySunQuan(id);
    internal bool DrainHappySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHappySunQuan(f);
    internal void FinishHappySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHappySunQuan(f);
    internal bool HappyDingliQualified(int owner, int holder) => _resolve().HappyDingliQualified(owner, holder);
    internal global::CardGame.Core.ProgramTriggerCandidate HappyLearningCandidate(int holder, int turn) => _resolve().HappyLearningCandidate(holder, turn);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame? HappyLearningParent(long id) => _resolve().HappyLearningParent(id);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? HappyShehuParent(long id) => _resolve().HappyShehuParent(id);
    internal bool HappySunQuanBeforeDamage(long id) => _resolve().HappySunQuanBeforeDamage(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappySunQuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappySunQuanChoices(f);
    internal bool HappySunQuanDamageWindow(long id) => _resolve().HappySunQuanDamageWindow(id);
    internal bool HappySunQuanDying() => _resolve().HappySunQuanDying();
    internal bool HappySunQuanEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HappySunQuanEligible(c, t, w);
    internal bool HappySunQuanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HappySunQuanFirstChild(f, child);
    internal bool HappySunQuanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().HappySunQuanMovement(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? HappySunQuanObserverRoot() => _resolve().HappySunQuanObserverRoot();
    internal bool HappySunQuanRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().HappySunQuanRecovery(f, child);
    internal int HappyXue(int holder) => _resolve().HappyXue(holder);
    internal bool HasHappyLearningPlaySelfOnly(int actor) => _resolve().HasHappyLearningPlaySelfOnly(actor);
    internal bool IsHappyLearningStart(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame p) => _resolve().IsHappyLearningStart(p);
    internal bool IsIssuedHappyLearning(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedHappyLearning(c, w);
    internal void RecordHappySunQuanMovement(long id, long before) => _resolve().RecordHappySunQuanMovement(id, before);
    internal bool RetainHappySunQuanPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHappySunQuanPaid(f);
    internal void ReturnHappyDingliDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyDingliDraw(f, outcome);
    internal bool ReturnHappySunQuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappySunQuanMovement(f);
    internal bool TryBeginHappyLearningActualStart(global::CardGame.Core.CharacterState current) => _resolve().TryBeginHappyLearningActualStart(current);
    internal bool HasHappySunQuanContent { get => _resolve().HasHappySunQuanContent; }
}
