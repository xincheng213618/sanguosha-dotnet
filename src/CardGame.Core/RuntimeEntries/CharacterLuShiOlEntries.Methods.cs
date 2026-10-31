#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuShiOlEntries
{
    internal bool AdvanceOlLuShiChildren() => _resolve().AdvanceOlLuShiChildren();
    internal void AnswerOlLuShi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerOlLuShi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOlLuShi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginOlLuShi(supplied, op);
    internal void BindOlLuShiHealthIntent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.HappyQiaoDualHealth before, global::CardGame.Core.HappyQiaoPool pool, int targetHp) => _resolve().BindOlLuShiHealthIntent(f, before, pool, targetHp);
    internal bool CanOfferOlLuShi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferOlLuShi(c, t, w);
    internal bool CanOlLuShiLeijieSlash(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().CanOlLuShiLeijieSlash(f, targetSeat);
    internal void CaptureOlLuShiEnding(global::CardGame.Core.ActualPhaseEndedEvent fact) => _resolve().CaptureOlLuShiEnding(fact);
    internal void CaptureOlLuShiLoss(global::CardGame.Core.ProgramSkillHpLostEvent fact) => _resolve().CaptureOlLuShiLoss(fact);
    internal void CaptureOlLuShiOpening(global::CardGame.Core.GameStartedEvent fact) => _resolve().CaptureOlLuShiOpening(fact);
    internal void CaptureOlLuShiPreventedLoss(global::CardGame.Core.IdentityHpDeductionPreventedEvent fact) => _resolve().CaptureOlLuShiPreventedLoss(fact);
    internal void CaptureOlLuShiSnapshot(global::CardGame.Core.CharacterState player, long boundary, bool initial) => _resolve().CaptureOlLuShiSnapshot(player, boundary, initial);
    internal global::CardGame.Core.PromptChoice ChooseOlLuShiLeijie(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseOlLuShiLeijie(p, f);
    internal global::CardGame.Core.PromptChoice ChooseOlLuShiZhuyan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseOlLuShiZhuyan(p, f);
    internal bool CompleteOlLuShiLeijieAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteOlLuShiLeijieAttack(attack, out paused);
    internal void CompleteOlLuShiLeijieSlash(long id) => _resolve().CompleteOlLuShiLeijieSlash(id);
    internal bool ContinueOlLuShi(long id) => _resolve().ContinueOlLuShi(id);
    internal bool DrainOlLuShi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainOlLuShi(f);
    internal bool HasOlLuShiJudgmentProof(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasOlLuShiJudgmentProof(f);
    internal bool IsOlLuShiLeijieNativeSlash(global::CardGame.Core.CardUseFrame use) => _resolve().IsOlLuShiLeijieNativeSlash(use);
    internal void IssueOlLuShiLeijieSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueOlLuShiLeijieSlash(f);
    internal bool OlLuShiBeforeDamage(long id) => _resolve().OlLuShiBeforeDamage(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OlLuShiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OlLuShiChoices(f);
    internal bool OlLuShiDamageObserver(long id) => _resolve().OlLuShiDamageObserver(id);
    internal global::CardGame.Core.Card[] OlLuShiDiscardCandidates(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OlLuShiDiscardCandidates(f);
    internal bool OlLuShiDying() => _resolve().OlLuShiDying();
    internal bool OlLuShiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OlLuShiFirstChild(f, child);
    internal global::CardGame.Core.OlLuShiSnapshotEvent? OlLuShiLastSnapshot(int target) => _resolve().OlLuShiLastSnapshot(target);
    internal int[] OlLuShiLeijieSlashTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OlLuShiLeijieSlashTargets(f);
    internal bool OlLuShiMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().OlLuShiMovementProof(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? OlLuShiObserverRoot() => _resolve().OlLuShiObserverRoot();
    internal bool OlLuShiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().OlLuShiRecoveryParent(f, recovery);
    internal bool OlLuShiUnused(int owner, int target, string item) => _resolve().OlLuShiUnused(owner, target, item);
    internal void PublishOlLuShi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishOlLuShi(f);
    internal bool RetainOlLuShiIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainOlLuShiIssued(f);
    internal void ReturnOlLuShiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnOlLuShiDraw(f, outcome);
    internal bool ReturnOlLuShiLeijieAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnOlLuShiLeijieAttack(completion);
    internal void ReturnOlLuShiLeijieDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnOlLuShiLeijieDraw(f, o);
    internal void ReturnOlLuShiLeijieSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnOlLuShiLeijieSlash(use);
    internal bool ReturnOlLuShiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOlLuShiMovement(f);
    internal void ReturnOlLuShiZhuyanDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnOlLuShiZhuyanDraw(f, o);
    internal void ValidateOlLuShiLeijieSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateOlLuShiLeijieSlash(use);
    internal bool HasOlLuShi { get => _resolve().HasOlLuShi; }
}
