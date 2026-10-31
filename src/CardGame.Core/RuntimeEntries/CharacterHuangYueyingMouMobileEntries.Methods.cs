#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangYueyingMouMobileEntries
{
    internal bool AdvanceMouHyChildren() => _resolve().AdvanceMouHyChildren();
    internal PromptChoice AiMouHyQicai(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouHyQicai(p, f);
    internal void AnswerMouHyQicai(PromptChoice c) => _resolve().AnswerMouHyQicai(c);
    internal SkillProgramStepOutcome BeginMouHyJizhi(ProgramSkillFrame supplied) => _resolve().BeginMouHyJizhi(supplied);
    internal SkillProgramStepOutcome BeginMouHyQiTransfer(ProgramSkillFrame supplied) => _resolve().BeginMouHyQiTransfer(supplied);
    internal SkillProgramStepOutcome BeginMouHyQicai(ProgramSkillFrame supplied) => _resolve().BeginMouHyQicai(supplied);
    internal bool CanMouHyQicai(ProgramActivationLegalityContext c) => _resolve().CanMouHyQicai(c);
    internal bool CanRunMouHyQiOnlyIssued(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanRunMouHyQiOnlyIssued(candidate, trigger, context);
    internal void CaptureMouHyQiGains(CardMovementBatchContext batch) => _resolve().CaptureMouHyQiGains(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouHyQiGainCandidates(CardMovementBatchContext batch) => _resolve().CollectMouHyQiGainCandidates(batch);
    internal bool CollectMouHyQiOnlyIssued(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().CollectMouHyQiOnlyIssued(candidate, trigger);
    internal void CollectWindow_CollectMouHyQiGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouHyQiGainCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouHyQiGains(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouHyQiGains(batch, completed);
    internal void ContinueMouHy(long id) => _resolve().ContinueMouHy(id);
    internal bool ContinuePaidMouHy(ProgramSkillFrame f) => _resolve().ContinuePaidMouHy(f);
    internal ProgramSkillWindowContext CreateMouHyQiGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c) => _resolve().CreateMouHyQiGainContext(parent, c);
    internal MouHuangYueyingQiGrantedEvent? CurrentMouHyQi(int target) => _resolve().CurrentMouHyQi(target);
    internal bool DrainMouHy(ProgramSkillFrame f) => _resolve().DrainMouHy(f);
    internal void FinishMouHy(ProgramSkillFrame f) => _resolve().FinishMouHy(f);
    internal bool IsIssuedMouHyQiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedMouHyQiCandidate(c, context);
    internal bool IsMouHyJizhiHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsMouHyJizhiHandLimitExempt(owner, cardId);
    internal bool IsMouHyQiGainCandidate(ProgramTriggerCandidate candidate, CardMovementBatchContext batch) => _resolve().IsMouHyQiGainCandidate(candidate, batch);
    internal bool MouHyDying() => _resolve().MouHyDying();
    internal IEnumerable<(Card Card, CardLocation From)> MouHyEquipmentChoices(int owner, int target) => _resolve().MouHyEquipmentChoices(owner, target);
    internal bool MouHyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouHyFirstChild(f, child);
    internal ProgramSkillFrame? MouHyObserverRoot() => _resolve().MouHyObserverRoot();
    internal bool MouHyObserverSuffix(int i) => _resolve().MouHyObserverSuffix(i);
    internal MouHuangYueyingQiGainClaimedEvent? MouHyQiBatchClaim(ProgramTriggerCandidate candidate, CardMovementBatchContext batch) => _resolve().MouHyQiBatchClaim(candidate, batch);
    internal MouHuangYueyingQiGainClaimedEvent? MouHyQiClaim(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().MouHyQiClaim(candidate, context);
    internal int MouHyQiRemaining(MouHuangYueyingQiGrantedEvent grant) => _resolve().MouHyQiRemaining(grant);
    internal IReadOnlyList<PromptChoice> MouHyQicaiChoices(ProgramSkillFrame f) => _resolve().MouHyQicaiChoices(f);
    internal int MouHyQicaiPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().MouHyQicaiPhaseUses(owner, phase);
    internal int MouHyQicaiProjectedUses(int owner, string skill, int fallback) => _resolve().MouHyQicaiProjectedUses(owner, skill, fallback);
    internal bool MouHyRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouHyRecovery(f, recovery);
    internal bool ResumeMouHy(long id) => _resolve().ResumeMouHy(id);
    internal void ReturnMouHyJizhiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMouHyJizhiDraw(f, outcome);
    internal bool ReturnMouHyMovement(ProgramSkillFrame f) => _resolve().ReturnMouHyMovement(f);
    internal void SyncMouHyQiMarker(long frame, int target) => _resolve().SyncMouHyQiMarker(frame, target);
    internal ProgramSkillWindowContext? TryCreateMouHyQiGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouHyQiGainContext(frame, candidate);
    internal bool ValidMouHy(ProgramSkillFrame f) => _resolve().ValidMouHy(f);
    internal bool HasMouHuangYueyingMobile { get => _resolve().HasMouHuangYueyingMobile; }
}
