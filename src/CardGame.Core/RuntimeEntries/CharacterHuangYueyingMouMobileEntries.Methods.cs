#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangYueyingMouMobileEntries
{
    internal bool AdvanceMouHyChildren() => _resolve().AdvanceMouHyChildren();
    internal global::CardGame.Core.PromptChoice AiMouHyQicai(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouHyQicai(p, f);
    internal void AnswerMouHyQicai(global::CardGame.Core.PromptChoice c) => _resolve().AnswerMouHyQicai(c);
    internal void AssertMouHy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouHy(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouHyJizhi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouHyJizhi(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouHyQiTransfer(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouHyQiTransfer(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouHyQicai(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouHyQicai(supplied);
    internal bool CanMouHyQicai(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanMouHyQicai(c);
    internal bool CanRunMouHyQiOnlyIssued(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunMouHyQiOnlyIssued(candidate, trigger, context);
    internal void CaptureMouHyQiGains(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouHyQiGains(batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectMouHyQiGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectMouHyQiGainCandidates(batch);
    internal bool CollectMouHyQiOnlyIssued(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CollectMouHyQiOnlyIssued(candidate, trigger);
    internal void CollectWindow_CollectMouHyQiGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouHyQiGainCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouHyQiGains(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouHyQiGains(batch, completed);
    internal void ContinueMouHy(long id) => _resolve().ContinueMouHy(id);
    internal bool ContinuePaidMouHy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMouHy(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateMouHyQiGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateMouHyQiGainContext(parent, c);
    internal global::CardGame.Core.MouHuangYueyingQiGrantedEvent? CurrentMouHyQi(int target) => _resolve().CurrentMouHyQi(target);
    internal bool DrainMouHy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouHy(f);
    internal void FinishMouHy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouHy(f);
    internal bool IsIssuedMouHyQiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedMouHyQiCandidate(c, context);
    internal bool IsMouHyJizhiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().IsMouHyJizhiHandLimitExempt(owner, cardId);
    internal bool IsMouHyQiGainCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsMouHyQiGainCandidate(candidate, batch);
    internal bool MouHyDamageObserver(long id) => _resolve().MouHyDamageObserver(id);
    internal bool MouHyDying() => _resolve().MouHyDying();
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From)> MouHyEquipmentChoices(int owner, int target) => _resolve().MouHyEquipmentChoices(owner, target);
    internal bool MouHyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouHyFirstChild(f, child);
    internal bool MouHyMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation m) => _resolve().MouHyMovementProof(f, e, m);
    internal global::CardGame.Core.ProgramSkillFrame? MouHyObserverRoot() => _resolve().MouHyObserverRoot();
    internal bool MouHyObserverSuffix(int i) => _resolve().MouHyObserverSuffix(i);
    internal global::CardGame.Core.MouHuangYueyingQiGainClaimedEvent? MouHyQiBatchClaim(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().MouHyQiBatchClaim(candidate, batch);
    internal global::CardGame.Core.MouHuangYueyingQiGainClaimedEvent? MouHyQiClaim(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MouHyQiClaim(candidate, context);
    internal int MouHyQiRemaining(global::CardGame.Core.MouHuangYueyingQiGrantedEvent grant) => _resolve().MouHyQiRemaining(grant);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouHyQicaiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouHyQicaiChoices(f);
    internal int MouHyQicaiPhaseUses(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().MouHyQicaiPhaseUses(owner, phase);
    internal int MouHyQicaiProjectedUses(int owner, string skill, int fallback) => _resolve().MouHyQicaiProjectedUses(owner, skill, fallback);
    internal bool MouHyRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouHyRecovery(f, recovery);
    internal bool ResumeMouHy(long id) => _resolve().ResumeMouHy(id);
    internal void ReturnMouHyJizhiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMouHyJizhiDraw(f, outcome);
    internal bool ReturnMouHyMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouHyMovement(f);
    internal void SyncMouHyQiMarker(long frame, int target) => _resolve().SyncMouHyQiMarker(frame, target);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateMouHyQiGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateMouHyQiGainContext(frame, candidate);
    internal bool ValidMouHy(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouHy(f);
    internal bool HasMouHuangYueyingMobile { get => _resolve().HasMouHuangYueyingMobile; }
}
