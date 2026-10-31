#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuShiOlEntries
{
    internal bool AdvanceOlLuShiChildren() => _resolve().AdvanceOlLuShiChildren();
    internal void AnswerOlLuShi(PromptChoice choice) => _resolve().AnswerOlLuShi(choice);
    internal SkillProgramStepOutcome BeginOlLuShi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginOlLuShi(supplied, op);
    internal void BindOlLuShiHealthIntent(ProgramSkillFrame f, HappyQiaoDualHealth before, HappyQiaoPool pool, int targetHp) => _resolve().BindOlLuShiHealthIntent(f, before, pool, targetHp);
    internal bool CanOfferOlLuShi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferOlLuShi(c, t, w);
    internal bool CanOlLuShiLeijieSlash(ProgramSkillFrame f, int targetSeat) => _resolve().CanOlLuShiLeijieSlash(f, targetSeat);
    internal void CaptureOlLuShiEnding(ActualPhaseEndedEvent fact) => _resolve().CaptureOlLuShiEnding(fact);
    internal void CaptureOlLuShiLoss(ProgramSkillHpLostEvent fact) => _resolve().CaptureOlLuShiLoss(fact);
    internal void CaptureOlLuShiOpening(GameStartedEvent fact) => _resolve().CaptureOlLuShiOpening(fact);
    internal void CaptureOlLuShiPreventedLoss(IdentityHpDeductionPreventedEvent fact) => _resolve().CaptureOlLuShiPreventedLoss(fact);
    internal void CaptureOlLuShiSnapshot(CharacterState player, long boundary, bool initial) => _resolve().CaptureOlLuShiSnapshot(player, boundary, initial);
    internal PromptChoice ChooseOlLuShiLeijie(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseOlLuShiLeijie(p, f);
    internal PromptChoice ChooseOlLuShiZhuyan(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseOlLuShiZhuyan(p, f);
    internal bool CompleteOlLuShiLeijieAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteOlLuShiLeijieAttack(attack, out paused);
    internal void CompleteOlLuShiLeijieSlash(long id) => _resolve().CompleteOlLuShiLeijieSlash(id);
    internal bool ContinueOlLuShi(long id) => _resolve().ContinueOlLuShi(id);
    internal bool DrainOlLuShi(ProgramSkillFrame f) => _resolve().DrainOlLuShi(f);
    internal bool HasOlLuShiJudgmentProof(ProgramSkillFrame f) => _resolve().HasOlLuShiJudgmentProof(f);
    internal bool IsOlLuShiLeijieNativeSlash(CardUseFrame use) => _resolve().IsOlLuShiLeijieNativeSlash(use);
    internal void IssueOlLuShiLeijieSlash(ProgramSkillFrame f) => _resolve().IssueOlLuShiLeijieSlash(f);
    internal IReadOnlyList<PromptChoice> OlLuShiChoices(ProgramSkillFrame f) => _resolve().OlLuShiChoices(f);
    internal Card[] OlLuShiDiscardCandidates(ProgramSkillFrame f) => _resolve().OlLuShiDiscardCandidates(f);
    internal bool OlLuShiDying() => _resolve().OlLuShiDying();
    internal bool OlLuShiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OlLuShiFirstChild(f, child);
    internal OlLuShiSnapshotEvent? OlLuShiLastSnapshot(int target) => _resolve().OlLuShiLastSnapshot(target);
    internal int[] OlLuShiLeijieSlashTargets(ProgramSkillFrame f) => _resolve().OlLuShiLeijieSlashTargets(f);
    internal bool OlLuShiMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().OlLuShiMovementProof(f, effect, pending);
    internal ProgramSkillFrame? OlLuShiObserverRoot() => _resolve().OlLuShiObserverRoot();
    internal bool OlLuShiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().OlLuShiRecoveryParent(f, recovery);
    internal bool OlLuShiUnused(int owner, int target, string item) => _resolve().OlLuShiUnused(owner, target, item);
    internal void PublishOlLuShi(ProgramSkillFrame f) => _resolve().PublishOlLuShi(f);
    internal bool RetainOlLuShiIssued(ProgramSkillFrame f) => _resolve().RetainOlLuShiIssued(f);
    internal void ReturnOlLuShiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnOlLuShiDraw(f, outcome);
    internal bool ReturnOlLuShiLeijieAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnOlLuShiLeijieAttack(completion);
    internal void ReturnOlLuShiLeijieDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnOlLuShiLeijieDraw(f, o);
    internal void ReturnOlLuShiLeijieSlash(CardUseFrame use) => _resolve().ReturnOlLuShiLeijieSlash(use);
    internal bool ReturnOlLuShiMovement(ProgramSkillFrame f) => _resolve().ReturnOlLuShiMovement(f);
    internal void ReturnOlLuShiZhuyanDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnOlLuShiZhuyanDraw(f, o);
    internal void ValidateOlLuShiLeijieSlash(CardUseFrame use) => _resolve().ValidateOlLuShiLeijieSlash(use);
    internal bool HasOlLuShi { get => _resolve().HasOlLuShi; }
}
