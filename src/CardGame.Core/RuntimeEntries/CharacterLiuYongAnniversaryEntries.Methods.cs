#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYongAnniversaryEntries
{
    internal bool AdvanceAnniversaryLiuYongChildren() => _resolve().AdvanceAnniversaryLiuYongChildren();
    internal IReadOnlyList<PromptChoice> AnniversaryLiuYongChoices(ProgramSkillFrame f) => _resolve().AnniversaryLiuYongChoices(f);
    internal bool AnniversaryLiuYongDyingObserver() => _resolve().AnniversaryLiuYongDyingObserver();
    internal bool AnniversaryLiuYongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AnniversaryLiuYongFirstChild(f, child);
    internal int? AnniversaryLiuYongLeader(int owner, CardMovementBatchContext? beforeBatch = default) => _resolve().AnniversaryLiuYongLeader(owner, beforeBatch);
    internal int AnniversaryLiuYongLimit(int owner) => _resolve().AnniversaryLiuYongLimit(owner);
    internal bool AnniversaryLiuYongMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().AnniversaryLiuYongMovementProof(f, effect, pending);
    internal ProgramSkillFrame? AnniversaryLiuYongObserverRoot() => _resolve().AnniversaryLiuYongObserverRoot();
    internal IReadOnlyList<AnniversaryLiuYongPlan> AnniversaryLiuYongPlans(int owner) => _resolve().AnniversaryLiuYongPlans(owner);
    internal bool AnniversaryLiuYongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().AnniversaryLiuYongRecoveryParent(f, recovery);
    internal string? AnniversaryLiuYongXiLabel(int cardId) => _resolve().AnniversaryLiuYongXiLabel(cardId);
    internal void AnswerAnniversaryLiuYong(PromptChoice choice) => _resolve().AnswerAnniversaryLiuYong(choice);
    internal void AppendAnniversaryLiuYongMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _resolve().AppendAnniversaryLiuYongMovementCandidates(batch, candidates);
    internal SkillProgramStepOutcome BeginAnniversaryLiuYong(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginAnniversaryLiuYong(supplied, operation);
    internal bool CanActivateAnniversaryLiuYong(CharacterState owner) => _resolve().CanActivateAnniversaryLiuYong(owner);
    internal bool CanOfferAnniversaryLiuYongFengxiang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryLiuYongFengxiang(candidate, trigger, context);
    internal void CaptureAnniversaryLiuYongDamage(DamageAppliedEvent fact) => _resolve().CaptureAnniversaryLiuYongDamage(fact);
    internal void CaptureAnniversaryLiuYongMovement(CardMovementBatchContext batch) => _resolve().CaptureAnniversaryLiuYongMovement(batch);
    internal PromptChoice ChooseAnniversaryLiuYong(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAnniversaryLiuYong(p, f);
    internal bool CompleteAnniversaryLiuYongSlash(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteAnniversaryLiuYongSlash(attack, out paused);
    internal void CompleteAnniversaryLiuYongUse(ProgramSkillFrame f) => _resolve().CompleteAnniversaryLiuYongUse(f);
    internal bool ContinueAnniversaryLiuYong(long id) => _resolve().ContinueAnniversaryLiuYong(id);
    internal ProgramSkillWindowContext CreateAnniversaryLiuYongMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateAnniversaryLiuYongMovementContext(frame, candidate);
    internal bool DrainAnniversaryLiuYong(ProgramSkillFrame f) => _resolve().DrainAnniversaryLiuYong(f);
    internal bool IsAnniversaryLiuYongMovementCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsAnniversaryLiuYongMovementCandidate(candidate, context);
    internal bool IsAnniversaryLiuYongSlashUse(CardUseFrame use) => _resolve().IsAnniversaryLiuYongSlashUse(use);
    internal bool IsAnniversaryLiuYongVirtualUse(CardUseFrame use) => _resolve().IsAnniversaryLiuYongVirtualUse(use);
    internal void IssueAnniversaryLiuYongSlash(ProgramSkillFrame f) => _resolve().IssueAnniversaryLiuYongSlash(f);
    internal void IssueAnniversaryLiuYongUse(ProgramSkillFrame f) => _resolve().IssueAnniversaryLiuYongUse(f);
    internal bool? ProveLiuYongMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveLiuYongMovement(candidate, context);
    internal void PublishAnniversaryLiuYong(ProgramSkillFrame f) => _resolve().PublishAnniversaryLiuYong(f);
    internal bool RetainAnniversaryLiuYong(ProgramSkillFrame f) => _resolve().RetainAnniversaryLiuYong(f);
    internal void ReturnAnniversaryLiuYongDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnAnniversaryLiuYongDraw(f, outcome);
    internal bool ReturnAnniversaryLiuYongMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryLiuYongMovement(f);
    internal void ReturnAnniversaryLiuYongNative(CardUseFrame use, long parentId) => _resolve().ReturnAnniversaryLiuYongNative(use, parentId);
    internal void ReturnAnniversaryLiuYongSlash(CardUseFrame use) => _resolve().ReturnAnniversaryLiuYongSlash(use);
    internal bool ReturnAnniversaryLiuYongSlashAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnAnniversaryLiuYongSlashAttack(completion);
    internal bool ReturnAnniversaryLiuYongVirtualAttack() => _resolve().ReturnAnniversaryLiuYongVirtualAttack();
    internal ProgramSkillWindowContext? TryCreateAnniversaryLiuYongMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateAnniversaryLiuYongMovementContext(frame, candidate);
    internal bool TryReturnAnniversaryLiuYongVirtualUse(CardUseFrame use) => _resolve().TryReturnAnniversaryLiuYongVirtualUse(use);
    internal void ValidateAnniversaryLiuYongSlash(CardUseFrame use) => _resolve().ValidateAnniversaryLiuYongSlash(use);
    internal bool HasAnniversaryLiuYong { get => _resolve().HasAnniversaryLiuYong; }
}
