#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunShangxiangSpOlEntries
{
    internal void AcceptSpSunShangxiangFanxiang(ProgramBindingStartedEvent started) => _resolve().AcceptSpSunShangxiangFanxiang(started);
    internal bool AdvanceSpSunShangxiangLiangzhuChildren() => _resolve().AdvanceSpSunShangxiangLiangzhuChildren();
    internal PromptChoice AiSpSunShangxiangLiangzhu(PendingDecision p, ProgramSkillFrame f) => _resolve().AiSpSunShangxiangLiangzhu(p, f);
    internal void AnswerSpSunShangxiangLiangzhu(PromptChoice choice) => _resolve().AnswerSpSunShangxiangLiangzhu(choice);
    internal SkillProgramStepOutcome BeginSpSunShangxiangLiangzhu(ProgramSkillFrame supplied) => _resolve().BeginSpSunShangxiangLiangzhu(supplied);
    internal bool CanOfferSpSunShangxiangFanxiang(int ownerSeat, SkillProgramTrigger t) => _resolve().CanOfferSpSunShangxiangFanxiang(ownerSeat, t);
    internal bool CanOfferSpSunShangxiangLiangzhu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferSpSunShangxiangLiangzhu(c, t, context);
    internal bool CanRunSpSunShangxiangLiangzhu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunSpSunShangxiangLiangzhu(c, context);
    internal void CaptureSpSunShangxiangRecovery(HpChangeContext change) => _resolve().CaptureSpSunShangxiangRecovery(change);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectSpSunShangxiangRecoveryCandidates(HpChangeContext change) => _resolve().CollectSpSunShangxiangRecoveryCandidates(change);
    internal SkillProgramStepOutcome CompleteSpSunShangxiangFanxiang(ProgramSkillFrame supplied) => _resolve().CompleteSpSunShangxiangFanxiang(supplied);
    internal bool ContinueAcceptedSpSunShangxiangFanxiang(ProgramSkillFrame f) => _resolve().ContinueAcceptedSpSunShangxiangFanxiang(f);
    internal bool ContinuePaidSpSunShangxiangLiangzhu(ProgramSkillFrame f) => _resolve().ContinuePaidSpSunShangxiangLiangzhu(f);
    internal void ContinueSpSunShangxiangLiangzhu(long id) => _resolve().ContinueSpSunShangxiangLiangzhu(id);
    internal ProgramSkillWindowContext CreateSpSunShangxiangRecoveryContext(HpChangeContext change, ProgramTriggerCandidate c) => _resolve().CreateSpSunShangxiangRecoveryContext(change, c);
    internal bool ExactSpSunShangxiangFanxiangParent(ProgramSkillFrame f, long id) => _resolve().ExactSpSunShangxiangFanxiangParent(f, id);
    internal void FinishSpSunShangxiangLiangzhu(ProgramSkillFrame f) => _resolve().FinishSpSunShangxiangLiangzhu(f);
    internal bool IsSpSunShangxiangFanxiangTrigger(SkillProgramTrigger t) => _resolve().IsSpSunShangxiangFanxiangTrigger(t);
    internal bool IsSpSunShangxiangLiangzhuDying() => _resolve().IsSpSunShangxiangLiangzhuDying();
    internal bool IsSpSunShangxiangLiangzhuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsSpSunShangxiangLiangzhuMovement(f, e, p);
    internal bool IsSpSunShangxiangLiangzhuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsSpSunShangxiangLiangzhuRecoveryParent(f, recovery);
    internal bool? ProveSpLiangzhuHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveSpLiangzhuHealth(trigger, candidate, context);
    internal void PublishSpSunShangxiangLiangzhu(ProgramSkillFrame f) => _resolve().PublishSpSunShangxiangLiangzhu(f);
    internal bool ResumeSpSunShangxiangLiangzhu(long id) => _resolve().ResumeSpSunShangxiangLiangzhu(id);
    internal void ReturnSpSunShangxiangLiangzhuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnSpSunShangxiangLiangzhuDraw(f, outcome);
    internal bool ReturnSpSunShangxiangLiangzhuMovement(ProgramSkillFrame f) => _resolve().ReturnSpSunShangxiangLiangzhuMovement(f);
    internal int? SpSunShangxiangFanxiangWitness(int owner) => _resolve().SpSunShangxiangFanxiangWitness(owner);
    internal IEnumerable<int> SpSunShangxiangHelpedSeats(int owner) => _resolve().SpSunShangxiangHelpedSeats(owner);
    internal IReadOnlyList<PromptChoice> SpSunShangxiangLiangzhuChoices(ProgramSkillFrame f) => _resolve().SpSunShangxiangLiangzhuChoices(f);
    internal bool SpSunShangxiangLiangzhuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SpSunShangxiangLiangzhuFirstChild(f, child);
    internal bool SpSunShangxiangLiangzhuObserverEdge(int n) => _resolve().SpSunShangxiangLiangzhuObserverEdge(n);
    internal ProgramSkillFrame? SpSunShangxiangLiangzhuObserverRoot() => _resolve().SpSunShangxiangLiangzhuObserverRoot();
    internal bool SpSunShangxiangLiangzhuSuffix(int n) => _resolve().SpSunShangxiangLiangzhuSuffix(n);
    internal SpSunShangxiangRecoveryQualifiedEvent? SpSunShangxiangRecoveryQualification(ProgramTriggerCandidate c, HpChangeContext change) => _resolve().SpSunShangxiangRecoveryQualification(c, change);
    internal bool ValidSpSunShangxiangFanxiang(ProgramSkillFrame f) => _resolve().ValidSpSunShangxiangFanxiang(f);
    internal bool ValidSpSunShangxiangLiangzhu(ProgramSkillFrame f) => _resolve().ValidSpSunShangxiangLiangzhu(f);
    internal bool HasSpSunShangxiangLiangzhu { get => _resolve().HasSpSunShangxiangLiangzhu; }
}
