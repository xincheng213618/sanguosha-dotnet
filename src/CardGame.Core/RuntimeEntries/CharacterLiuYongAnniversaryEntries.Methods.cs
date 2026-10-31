#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYongAnniversaryEntries
{
    internal bool AdvanceAnniversaryLiuYongChildren() => _resolve().AdvanceAnniversaryLiuYongChildren();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnniversaryLiuYongChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryLiuYongChoices(f);
    internal bool AnniversaryLiuYongDyingObserver() => _resolve().AnniversaryLiuYongDyingObserver();
    internal bool AnniversaryLiuYongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AnniversaryLiuYongFirstChild(f, child);
    internal int? AnniversaryLiuYongLeader(int owner, global::CardGame.Core.CardMovementBatchContext? beforeBatch = default) => _resolve().AnniversaryLiuYongLeader(owner, beforeBatch);
    internal int AnniversaryLiuYongLimit(int owner) => _resolve().AnniversaryLiuYongLimit(owner);
    internal bool AnniversaryLiuYongMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().AnniversaryLiuYongMovementProof(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryLiuYongObserverRoot() => _resolve().AnniversaryLiuYongObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.AnniversaryLiuYongPlan> AnniversaryLiuYongPlans(int owner) => _resolve().AnniversaryLiuYongPlans(owner);
    internal bool AnniversaryLiuYongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().AnniversaryLiuYongRecoveryParent(f, recovery);
    internal string? AnniversaryLiuYongXiLabel(int cardId) => _resolve().AnniversaryLiuYongXiLabel(cardId);
    internal void AnswerAnniversaryLiuYong(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAnniversaryLiuYong(choice);
    internal void AppendAnniversaryLiuYongMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().AppendAnniversaryLiuYongMovementCandidates(batch, candidates);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryLiuYong(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginAnniversaryLiuYong(supplied, operation);
    internal bool CanActivateAnniversaryLiuYong(global::CardGame.Core.CharacterState owner) => _resolve().CanActivateAnniversaryLiuYong(owner);
    internal bool CanOfferAnniversaryLiuYongFengxiang(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryLiuYongFengxiang(candidate, trigger, context);
    internal void CaptureAnniversaryLiuYongDamage(global::CardGame.Core.DamageAppliedEvent fact) => _resolve().CaptureAnniversaryLiuYongDamage(fact);
    internal void CaptureAnniversaryLiuYongMovement(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureAnniversaryLiuYongMovement(batch);
    internal global::CardGame.Core.PromptChoice ChooseAnniversaryLiuYong(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAnniversaryLiuYong(p, f);
    internal bool CompleteAnniversaryLiuYongSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteAnniversaryLiuYongSlash(attack, out paused);
    internal void CompleteAnniversaryLiuYongUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteAnniversaryLiuYongUse(f);
    internal bool ContinueAnniversaryLiuYong(long id) => _resolve().ContinueAnniversaryLiuYong(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateAnniversaryLiuYongMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateAnniversaryLiuYongMovementContext(frame, candidate);
    internal bool DrainAnniversaryLiuYong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainAnniversaryLiuYong(f);
    internal bool IsAnniversaryLiuYongMovementCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsAnniversaryLiuYongMovementCandidate(candidate, context);
    internal bool IsAnniversaryLiuYongSlashUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsAnniversaryLiuYongSlashUse(use);
    internal bool IsAnniversaryLiuYongVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsAnniversaryLiuYongVirtualUse(use);
    internal void IssueAnniversaryLiuYongSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueAnniversaryLiuYongSlash(f);
    internal void IssueAnniversaryLiuYongUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueAnniversaryLiuYongUse(f);
    internal bool? ProveLiuYongMovement(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveLiuYongMovement(candidate, context);
    internal void PublishAnniversaryLiuYong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAnniversaryLiuYong(f);
    internal bool RetainAnniversaryLiuYong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainAnniversaryLiuYong(f);
    internal void ReturnAnniversaryLiuYongDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnAnniversaryLiuYongDraw(f, outcome);
    internal bool ReturnAnniversaryLiuYongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryLiuYongMovement(f);
    internal void ReturnAnniversaryLiuYongNative(global::CardGame.Core.CardUseFrame use, long parentId) => _resolve().ReturnAnniversaryLiuYongNative(use, parentId);
    internal void ReturnAnniversaryLiuYongSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnAnniversaryLiuYongSlash(use);
    internal bool ReturnAnniversaryLiuYongSlashAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnAnniversaryLiuYongSlashAttack(completion);
    internal bool ReturnAnniversaryLiuYongVirtualAttack() => _resolve().ReturnAnniversaryLiuYongVirtualAttack();
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateAnniversaryLiuYongMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateAnniversaryLiuYongMovementContext(frame, candidate);
    internal bool TryReturnAnniversaryLiuYongVirtualUse(global::CardGame.Core.CardUseFrame use) => _resolve().TryReturnAnniversaryLiuYongVirtualUse(use);
    internal void ValidateAnniversaryLiuYongSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateAnniversaryLiuYongSlash(use);
    internal bool HasAnniversaryLiuYong { get => _resolve().HasAnniversaryLiuYong; }
}
