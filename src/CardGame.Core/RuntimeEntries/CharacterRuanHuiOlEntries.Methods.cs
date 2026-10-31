#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanHuiOlEntries
{
    internal void AddRuanHuiStep(long id, global::CardGame.Core.RuanHuiStage stage, long before, int count) => _resolve().AddRuanHuiStep(id, stage, before, count);
    internal bool AdvanceRuanHuiSubtree() => _resolve().AdvanceRuanHuiSubtree();
    internal global::CardGame.Core.PromptChoice AiRuanHui(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiRuanHui(p, f);
    internal void AnswerRuanHui(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerRuanHui(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRuanHui(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginRuanHui(supplied, op);
    internal bool CanRunIssuedRuanHuiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunIssuedRuanHuiCandidate(c, context);
    internal void CaptureRuanHuiMovementBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureRuanHuiMovementBatch(batch);
    internal void CleanupRuanHuiReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CleanupRuanHuiReveal(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectRuanHuiClaimCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectRuanHuiClaimCandidates(batch);
    internal void CollectWindow_CollectRuanHuiClaimCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectRuanHuiClaimCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureRuanHuiMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureRuanHuiMovementBatch(batch, completed);
    internal bool ContinuePaidRuanHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidRuanHui(f);
    internal void ContinueRuanHui(long id) => _resolve().ContinueRuanHui(id);
    internal bool? DecideIssuedTrigger560(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger560(candidate, context);
    internal bool DrainRuanHui(long id) => _resolve().DrainRuanHui(id);
    internal void FinishRuanHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishRuanHui(f);
    internal bool IsIssuedRuanHuiCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().IsIssuedRuanHuiCandidate(c);
    internal bool IsMingchaNormalDrawReplacement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsMingchaNormalDrawReplacement(f);
    internal bool IsRuanHuiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsRuanHuiRecoveryParent(f, recovery);
    internal bool JingzhongAbolishedEquipmentUse(global::CardGame.Core.JingzhongUseEntityEvent fact, global::CardGame.Core.CardMovementRecord move) => _resolve().JingzhongAbolishedEquipmentUse(fact, move);
    internal bool JingzhongClaimAlreadyIssued(global::CardGame.Core.JingzhongLeaseEvent lease, long action) => _resolve().JingzhongClaimAlreadyIssued(lease, action);
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? JingzhongDuePhase(global::CardGame.Core.JingzhongLeaseEvent lease) => _resolve().JingzhongDuePhase(lease);
    internal int JingzhongFinalActor(global::CardGame.Core.JingzhongUseEntityEvent e) => _resolve().JingzhongFinalActor(e);
    internal bool JingzhongMaterialStillOwnsDiscard(global::CardGame.Core.JingzhongUseEntityEvent fact, global::CardGame.Core.CardMovementRecord last) => _resolve().JingzhongMaterialStillOwnsDiscard(fact, last);
    internal global::CardGame.Core.JingzhongLeaseEvent[] JingzhongPhaseLeases(long phase) => _resolve().JingzhongPhaseLeases(phase);
    internal int JingzhongUsedCount(long lease) => _resolve().JingzhongUsedCount(lease);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> MingchaRandomCards(int seat) => _resolve().MingchaRandomCards(seat);
    internal void ObserveRuanHuiUse(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveRuanHuiUse(fact);
    internal void PublishRuanHui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishRuanHui(f);
    internal bool QualifyRuanHui(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyRuanHui(c, t, context);
    internal void RecordJingzhongUse(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase, long action, long use, int actor, bool response, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => _resolve().RecordJingzhongUse(phase, action, use, actor, response, costs);
    internal bool ResumeRuanHui(long id) => _resolve().ResumeRuanHui(id);
    internal bool ReturnRuanHuiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnRuanHuiMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RuanHuiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RuanHuiChoices(f);
    internal global::CardGame.Core.JingzhongSettledEntityEvent[] RuanHuiClaimFacts(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().RuanHuiClaimFacts(batch, c);
    internal bool RuanHuiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().RuanHuiFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? RuanHuiObserverRoot() => _resolve().RuanHuiObserverRoot();
    internal bool SameJingzhongLeaseOwner(global::CardGame.Core.JingzhongLeaseEvent a, global::CardGame.Core.JingzhongLeaseEvent b) => _resolve().SameJingzhongLeaseOwner(a, b);
    internal bool HasRuanHuiCapability { get => _resolve().HasRuanHuiCapability; }
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? RuanHuiPlay { get => _resolve().RuanHuiPlay; }
}
