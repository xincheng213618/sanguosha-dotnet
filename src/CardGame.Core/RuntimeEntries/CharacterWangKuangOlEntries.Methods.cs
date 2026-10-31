#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangKuangOlEntries
{
    internal bool AdvanceWangKuangSubtree() => _resolve().AdvanceWangKuangSubtree();
    internal void AnswerWangKuang(PromptChoice choice) => _resolve().AnswerWangKuang(choice);
    internal SkillProgramStepOutcome BeginWangKuang(ProgramSkillFrame input, bool ending) => _resolve().BeginWangKuang(input, ending);
    internal WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(CharacterState owner) => _resolve().CaptureWangKuangRenxiaEndingPhase(owner);
    internal PromptChoice ChooseWangKuangAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseWangKuangAi(p, f);
    internal void ChooseWangKuangBranch(ProgramSkillFrame f, WangKuangRenxiaBranch branch) => _resolve().ChooseWangKuangBranch(f, branch);
    internal bool CollectWangKuang(SkillProgramTrigger t) => _resolve().CollectWangKuang(t);
    internal bool ContinuePaidWangKuang(ProgramSkillFrame f) => _resolve().ContinuePaidWangKuang(f);
    internal void ContinueWangKuang(long id) => _resolve().ContinueWangKuang(id);
    internal bool? DecideIssuedTrigger660(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger660(candidate, context);
    internal bool DrainWangKuangChildren(long id) => _resolve().DrainWangKuangChildren(id);
    internal bool IsIssuedWangKuangRenxiaCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedWangKuangRenxiaCandidate(c, context);
    internal bool IsWangKuangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsWangKuangRecoveryParent(f, recovery);
    internal IEnumerable<ProgramTriggerCandidate> IssuedWangKuangRenxiaEndingCandidates(CharacterState owner) => _resolve().IssuedWangKuangRenxiaEndingCandidates(owner);
    internal void PayWangKuangDiscard(ProgramSkillFrame f) => _resolve().PayWangKuangDiscard(f);
    internal void PayWangKuangDraw(ProgramSkillFrame f) => _resolve().PayWangKuangDraw(f);
    internal void PrepareWangKuangDiscard(ProgramSkillFrame f) => _resolve().PrepareWangKuangDiscard(f);
    internal void PublishWangKuang(ProgramSkillFrame f) => _resolve().PublishWangKuang(f);
    internal bool QualifyWangKuang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyWangKuang(c, t, context);
    internal void RecordWangKuangPayment(ProgramSkillFrame f) => _resolve().RecordWangKuangPayment(f);
    internal bool ResumeWangKuang(long id) => _resolve().ResumeWangKuang(id);
    internal bool ValidWangKuang(ProgramSkillFrame f) => _resolve().ValidWangKuang(f);
    internal bool ValidWangKuangObligation(WangKuangRenxiaObligationIssuedEvent p) => _resolve().ValidWangKuangObligation(p);
    internal IReadOnlyList<PromptChoice> WangKuangChoices(ProgramSkillFrame f) => _resolve().WangKuangChoices(f);
    internal WangKuangRenxiaPhaseKey WangKuangCurrentPhase(int owner) => _resolve().WangKuangCurrentPhase(owner);
    internal IReadOnlyList<WangKuangRenxiaCard> WangKuangDiscardable(int owner, string instance) => _resolve().WangKuangDiscardable(owner, instance);
    internal bool WangKuangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WangKuangFirstChild(f, child);
    internal bool WangKuangHandHasDamage(int owner) => _resolve().WangKuangHandHasDamage(owner);
    internal bool WangKuangObligationConsumed(long id) => _resolve().WangKuangObligationConsumed(id);
    internal ProgramSkillFrame? WangKuangObserverRoot() => _resolve().WangKuangObserverRoot();
    internal IEnumerable<WangKuangRenxiaObligationIssuedEvent> WangKuangOutstanding(WangKuangRenxiaPhaseKey phase) => _resolve().WangKuangOutstanding(phase);
    internal bool HasWangKuangCapability { get => _resolve().HasWangKuangCapability; }
}
