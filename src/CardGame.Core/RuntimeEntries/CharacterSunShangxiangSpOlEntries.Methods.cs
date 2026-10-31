#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunShangxiangSpOlEntries
{
    internal void AcceptSpSunShangxiangFanxiang(global::CardGame.Core.ProgramBindingStartedEvent started) => _resolve().AcceptSpSunShangxiangFanxiang(started);
    internal bool AdvanceSpSunShangxiangLiangzhuChildren() => _resolve().AdvanceSpSunShangxiangLiangzhuChildren();
    internal global::CardGame.Core.PromptChoice AiSpSunShangxiangLiangzhu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiSpSunShangxiangLiangzhu(p, f);
    internal void AnswerSpSunShangxiangLiangzhu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerSpSunShangxiangLiangzhu(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSpSunShangxiangLiangzhu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginSpSunShangxiangLiangzhu(supplied);
    internal bool CanOfferSpSunShangxiangFanxiang(int ownerSeat, global::CardGame.Core.SkillProgramTrigger t) => _resolve().CanOfferSpSunShangxiangFanxiang(ownerSeat, t);
    internal bool CanOfferSpSunShangxiangLiangzhu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSpSunShangxiangLiangzhu(c, t, context);
    internal bool CanRunSpSunShangxiangLiangzhu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunSpSunShangxiangLiangzhu(c, context);
    internal void CaptureSpSunShangxiangRecovery(global::CardGame.Core.HpChangeContext change) => _resolve().CaptureSpSunShangxiangRecovery(change);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectSpSunShangxiangRecoveryCandidates(global::CardGame.Core.HpChangeContext change) => _resolve().CollectSpSunShangxiangRecoveryCandidates(change);
    internal global::CardGame.Core.SkillProgramStepOutcome CompleteSpSunShangxiangFanxiang(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().CompleteSpSunShangxiangFanxiang(supplied);
    internal bool ContinueAcceptedSpSunShangxiangFanxiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedSpSunShangxiangFanxiang(f);
    internal bool ContinuePaidSpSunShangxiangLiangzhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidSpSunShangxiangLiangzhu(f);
    internal void ContinueSpSunShangxiangLiangzhu(long id) => _resolve().ContinueSpSunShangxiangLiangzhu(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateSpSunShangxiangRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateSpSunShangxiangRecoveryContext(change, c);
    internal bool ExactSpSunShangxiangFanxiangParent(global::CardGame.Core.ProgramSkillFrame f, long id) => _resolve().ExactSpSunShangxiangFanxiangParent(f, id);
    internal void FinishSpSunShangxiangLiangzhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSpSunShangxiangLiangzhu(f);
    internal bool IsSpSunShangxiangFanxiangTrigger(global::CardGame.Core.SkillProgramTrigger t) => _resolve().IsSpSunShangxiangFanxiangTrigger(t);
    internal bool IsSpSunShangxiangLiangzhuDying() => _resolve().IsSpSunShangxiangLiangzhuDying();
    internal bool IsSpSunShangxiangLiangzhuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsSpSunShangxiangLiangzhuMovement(f, e, p);
    internal bool IsSpSunShangxiangLiangzhuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsSpSunShangxiangLiangzhuRecoveryParent(f, recovery);
    internal bool? ProveSpLiangzhuHealth(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveSpLiangzhuHealth(trigger, candidate, context);
    internal void PublishSpSunShangxiangLiangzhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSpSunShangxiangLiangzhu(f);
    internal bool ResumeSpSunShangxiangLiangzhu(long id) => _resolve().ResumeSpSunShangxiangLiangzhu(id);
    internal void ReturnSpSunShangxiangLiangzhuDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnSpSunShangxiangLiangzhuDraw(f, outcome);
    internal bool ReturnSpSunShangxiangLiangzhuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSpSunShangxiangLiangzhuMovement(f);
    internal int? SpSunShangxiangFanxiangWitness(int owner) => _resolve().SpSunShangxiangFanxiangWitness(owner);
    internal global::System.Collections.Generic.IEnumerable<int> SpSunShangxiangHelpedSeats(int owner) => _resolve().SpSunShangxiangHelpedSeats(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SpSunShangxiangLiangzhuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SpSunShangxiangLiangzhuChoices(f);
    internal bool SpSunShangxiangLiangzhuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SpSunShangxiangLiangzhuFirstChild(f, child);
    internal bool SpSunShangxiangLiangzhuObserverEdge(int n) => _resolve().SpSunShangxiangLiangzhuObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? SpSunShangxiangLiangzhuObserverRoot() => _resolve().SpSunShangxiangLiangzhuObserverRoot();
    internal bool SpSunShangxiangLiangzhuSuffix(int n) => _resolve().SpSunShangxiangLiangzhuSuffix(n);
    internal global::CardGame.Core.SpSunShangxiangRecoveryQualifiedEvent? SpSunShangxiangRecoveryQualification(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.HpChangeContext change) => _resolve().SpSunShangxiangRecoveryQualification(c, change);
    internal bool ValidSpSunShangxiangFanxiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSpSunShangxiangFanxiang(f);
    internal bool ValidSpSunShangxiangLiangzhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSpSunShangxiangLiangzhu(f);
    internal bool HasSpSunShangxiangLiangzhu { get => _resolve().HasSpSunShangxiangLiangzhu; }
}
