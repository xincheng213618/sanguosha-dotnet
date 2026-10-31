#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanHuiOlEntries
{
    internal void AddRuanHuiStep(long id, RuanHuiStage stage, long before, int count) => _resolve().AddRuanHuiStep(id, stage, before, count);
    internal bool AdvanceRuanHuiSubtree() => _resolve().AdvanceRuanHuiSubtree();
    internal PromptChoice AiRuanHui(PendingDecision p, ProgramSkillFrame f) => _resolve().AiRuanHui(p, f);
    internal void AnswerRuanHui(PromptChoice choice) => _resolve().AnswerRuanHui(choice);
    internal SkillProgramStepOutcome BeginRuanHui(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginRuanHui(supplied, op);
    internal bool CanRunIssuedRuanHuiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunIssuedRuanHuiCandidate(c, context);
    internal void CaptureRuanHuiMovementBatch(CardMovementBatchContext batch) => _resolve().CaptureRuanHuiMovementBatch(batch);
    internal void CleanupRuanHuiReveal(ProgramSkillFrame f) => _resolve().CleanupRuanHuiReveal(f);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectRuanHuiClaimCandidates(CardMovementBatchContext batch) => _resolve().CollectRuanHuiClaimCandidates(batch);
    internal void CollectWindow_CollectRuanHuiClaimCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectRuanHuiClaimCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureRuanHuiMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureRuanHuiMovementBatch(batch, completed);
    internal bool ContinuePaidRuanHui(ProgramSkillFrame f) => _resolve().ContinuePaidRuanHui(f);
    internal void ContinueRuanHui(long id) => _resolve().ContinueRuanHui(id);
    internal bool? DecideIssuedTrigger560(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger560(candidate, context);
    internal bool DrainRuanHui(long id) => _resolve().DrainRuanHui(id);
    internal void FinishRuanHui(ProgramSkillFrame f) => _resolve().FinishRuanHui(f);
    internal bool IsIssuedRuanHuiCandidate(ProgramTriggerCandidate c) => _resolve().IsIssuedRuanHuiCandidate(c);
    internal bool IsMingchaNormalDrawReplacement(ProgramSkillFrame f) => _resolve().IsMingchaNormalDrawReplacement(f);
    internal bool IsRuanHuiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsRuanHuiRecoveryParent(f, recovery);
    internal bool JingzhongAbolishedEquipmentUse(JingzhongUseEntityEvent fact, CardMovementRecord move) => _resolve().JingzhongAbolishedEquipmentUse(fact, move);
    internal bool JingzhongClaimAlreadyIssued(JingzhongLeaseEvent lease, long action) => _resolve().JingzhongClaimAlreadyIssued(lease, action);
    internal ActualDiscardRecoveryPhaseKey? JingzhongDuePhase(JingzhongLeaseEvent lease) => _resolve().JingzhongDuePhase(lease);
    internal int JingzhongFinalActor(JingzhongUseEntityEvent e) => _resolve().JingzhongFinalActor(e);
    internal bool JingzhongMaterialStillOwnsDiscard(JingzhongUseEntityEvent fact, CardMovementRecord last) => _resolve().JingzhongMaterialStillOwnsDiscard(fact, last);
    internal JingzhongLeaseEvent[] JingzhongPhaseLeases(long phase) => _resolve().JingzhongPhaseLeases(phase);
    internal int JingzhongUsedCount(long lease) => _resolve().JingzhongUsedCount(lease);
    internal IReadOnlyList<Card> MingchaRandomCards(int seat) => _resolve().MingchaRandomCards(seat);
    internal void ObserveRuanHuiUse(IGameEvent fact) => _resolve().ObserveRuanHuiUse(fact);
    internal void PublishRuanHui(ProgramSkillFrame f) => _resolve().PublishRuanHui(f);
    internal bool QualifyRuanHui(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyRuanHui(c, t, context);
    internal void RecordJingzhongUse(ActualDiscardRecoveryPhaseKey phase, long action, long use, int actor, bool response, IReadOnlyList<CardActionCost> costs) => _resolve().RecordJingzhongUse(phase, action, use, actor, response, costs);
    internal bool ResumeRuanHui(long id) => _resolve().ResumeRuanHui(id);
    internal bool ReturnRuanHuiMovement(ProgramSkillFrame f) => _resolve().ReturnRuanHuiMovement(f);
    internal IReadOnlyList<PromptChoice> RuanHuiChoices(ProgramSkillFrame f) => _resolve().RuanHuiChoices(f);
    internal JingzhongSettledEntityEvent[] RuanHuiClaimFacts(CardMovementBatchContext batch, ProgramTriggerCandidate c) => _resolve().RuanHuiClaimFacts(batch, c);
    internal bool RuanHuiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().RuanHuiFirstChild(f, child);
    internal ProgramSkillFrame? RuanHuiObserverRoot() => _resolve().RuanHuiObserverRoot();
    internal bool SameJingzhongLeaseOwner(JingzhongLeaseEvent a, JingzhongLeaseEvent b) => _resolve().SameJingzhongLeaseOwner(a, b);
    internal bool HasRuanHuiCapability { get => _resolve().HasRuanHuiCapability; }
    internal ActualDiscardRecoveryPhaseKey? RuanHuiPlay { get => _resolve().RuanHuiPlay; }
}
