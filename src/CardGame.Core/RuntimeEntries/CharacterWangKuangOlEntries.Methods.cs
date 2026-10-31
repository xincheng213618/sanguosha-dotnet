#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangKuangOlEntries
{
    internal bool AdvanceWangKuangSubtree() => _resolve().AdvanceWangKuangSubtree();
    internal void AnswerWangKuang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerWangKuang(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWangKuang(global::CardGame.Core.ProgramSkillFrame input, bool ending) => _resolve().BeginWangKuang(input, ending);
    internal global::CardGame.Core.WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(global::CardGame.Core.CharacterState owner) => _resolve().CaptureWangKuangRenxiaEndingPhase(owner);
    internal global::CardGame.Core.PromptChoice ChooseWangKuangAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseWangKuangAi(p, f);
    internal void ChooseWangKuangBranch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WangKuangRenxiaBranch branch) => _resolve().ChooseWangKuangBranch(f, branch);
    internal bool CollectWangKuang(global::CardGame.Core.SkillProgramTrigger t) => _resolve().CollectWangKuang(t);
    internal bool ContinuePaidWangKuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidWangKuang(f);
    internal void ContinueWangKuang(long id) => _resolve().ContinueWangKuang(id);
    internal bool? DecideIssuedTrigger660(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger660(candidate, context);
    internal bool DrainWangKuangChildren(long id) => _resolve().DrainWangKuangChildren(id);
    internal bool IsIssuedWangKuangRenxiaCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedWangKuangRenxiaCandidate(c, context);
    internal bool IsWangKuangRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsWangKuangRecoveryParent(f, recovery);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedWangKuangRenxiaEndingCandidates(global::CardGame.Core.CharacterState owner) => _resolve().IssuedWangKuangRenxiaEndingCandidates(owner);
    internal void PayWangKuangDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayWangKuangDiscard(f);
    internal void PayWangKuangDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayWangKuangDraw(f);
    internal void PrepareWangKuangDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrepareWangKuangDiscard(f);
    internal void PublishWangKuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWangKuang(f);
    internal bool QualifyWangKuang(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyWangKuang(c, t, context);
    internal void RecordWangKuangPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecordWangKuangPayment(f);
    internal bool ResumeWangKuang(long id) => _resolve().ResumeWangKuang(id);
    internal bool ValidWangKuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWangKuang(f);
    internal bool ValidWangKuangObligation(global::CardGame.Core.WangKuangRenxiaObligationIssuedEvent p) => _resolve().ValidWangKuangObligation(p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WangKuangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangKuangChoices(f);
    internal global::CardGame.Core.WangKuangRenxiaPhaseKey WangKuangCurrentPhase(int owner) => _resolve().WangKuangCurrentPhase(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.WangKuangRenxiaCard> WangKuangDiscardable(int owner, string instance) => _resolve().WangKuangDiscardable(owner, instance);
    internal bool WangKuangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WangKuangFirstChild(f, child);
    internal bool WangKuangHandHasDamage(int owner) => _resolve().WangKuangHandHasDamage(owner);
    internal bool WangKuangObligationConsumed(long id) => _resolve().WangKuangObligationConsumed(id);
    internal global::CardGame.Core.ProgramSkillFrame? WangKuangObserverRoot() => _resolve().WangKuangObserverRoot();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.WangKuangRenxiaObligationIssuedEvent> WangKuangOutstanding(global::CardGame.Core.WangKuangRenxiaPhaseKey phase) => _resolve().WangKuangOutstanding(phase);
    internal bool HasWangKuangCapability { get => _resolve().HasWangKuangCapability; }
}
