#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoYuOlEntries
{
    internal bool AdvanceCaoYuSubtree() => _resolve().AdvanceCaoYuSubtree();
    internal void AdvanceGongjieDonor(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AdvanceGongjieDonor(f);
    internal void AfterGongjieGifts(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AfterGongjieGifts(f);
    internal global::CardGame.Core.PromptChoice AiCaoYu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiCaoYu(p, f);
    internal void AnswerCaoYu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCaoYu(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaoYu(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginCaoYu(input, op);
    internal bool CaoYuActualTurnOpen(int turn, int owner) => _resolve().CaoYuActualTurnOpen(turn, owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoYuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoYuChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> CaoYuDiscardCards(int owner) => _resolve().CaoYuDiscardCards(owner);
    internal bool CaoYuDyingEntry(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.DyingFrame dying) => _resolve().CaoYuDyingEntry(f, out dying);
    internal bool CaoYuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoYuFirstChild(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> CaoYuGiftCards(int owner, int chooser) => _resolve().CaoYuGiftCards(owner, chooser);
    internal bool CaoYuHistory(int owner, string skill, int recipient) => _resolve().CaoYuHistory(owner, skill, recipient);
    internal global::CardGame.Core.ProgramSkillFrame? CaoYuObserverRoot() => _resolve().CaoYuObserverRoot();
    internal bool CaoYuOpportunityConsumed(long id) => _resolve().CaoYuOpportunityConsumed(id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CaoYuMinimumTransitionEvent> CaoYuOutstanding(int turn) => _resolve().CaoYuOutstanding(turn);
    internal bool CaoYuRoundFirst(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CaoYuRoundFirst(c, context);
    internal global::CardGame.Core.CaoYuMinimumTransitionEvent? CaoYuTransition(global::CardGame.Core.ProgramTriggerCandidate c, long? id = default) => _resolve().CaoYuTransition(c, id);
    internal global::CardGame.Core.GameEngine.CaoYuDeathMinimumCapture? CaptureCaoYuDeathMinimumSources(int victimSeat) => _resolve().CaptureCaoYuDeathMinimumSources(victimSeat);
    internal void CaptureCaoYuMinimumTransitions(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureCaoYuMinimumTransitions(batch);
    internal void CollectXiangxuMinimumCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectXiangxuMinimumCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureCaoYuMinimumTransitions(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureCaoYuMinimumTransitions(batch, completed);
    internal void ContinueCaoYu(long id) => _resolve().ContinueCaoYu(id);
    internal bool ContinueIssuedCaoYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedCaoYu(f);
    internal bool ContinuePaidCaoYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidCaoYu(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateXiangxuAfterTurnEndedContext(global::CardGame.Core.DeferredTurnEndFrame p, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateXiangxuAfterTurnEndedContext(p, c, context);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateXiangxuMinimumContext(global::CardGame.Core.CardsMovedTriggerWindowFrame f, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateXiangxuMinimumContext(f, c);
    internal bool DrainCaoYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainCaoYu(f);
    internal void DrawCaoYu(global::CardGame.Core.ProgramSkillFrame f, int count, global::CardGame.Core.CaoYuStage stage, string reason) => _resolve().DrawCaoYu(f, count, stage, reason);
    internal bool ExcludeOrdinaryMovement_IsXiangxuMinimumTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsXiangxuMinimumTrigger(candidate, trigger);
    internal void FinishCaoYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCaoYu(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeXiangxuAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => _resolve().IncludeXiangxuAfterTurnEndedItems(parent, ordinary);
    internal bool IsCaoYuIssuedMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsCaoYuIssuedMovement(f, m);
    internal bool IsCaoYuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsCaoYuRecoveryParent(f, recovery);
    internal bool IsIssuedXiangxuEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedXiangxuEndingCandidate(c, context);
    internal bool IsXiangxuEndingTrigger(global::CardGame.Core.SkillProgramTrigger t) => _resolve().IsXiangxuEndingTrigger(t);
    internal bool IsXiangxuMinimumCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsXiangxuMinimumCandidate(c, context);
    internal bool IsXiangxuMinimumTrigger(global::CardGame.Core.SkillProgramTrigger t) => _resolve().IsXiangxuMinimumTrigger(t);
    internal void PayCaoYuCards(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids, int? target, global::CardGame.Core.CaoYuStage stage, string reason) => _resolve().PayCaoYuCards(f, ids, target, stage, reason);
    internal bool? ProveXiangxuMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveXiangxuMovement(trigger, candidate, context);
    internal void PublishCaoYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaoYu(f);
    internal bool QualifyCaoYu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyCaoYu(c, t, context);
    internal void RecordCaoYuDeathMinimumTransitions(long deathFrameId, int victimSeat, global::CardGame.Core.GameEngine.CaoYuDeathMinimumCapture? capture) => _resolve().RecordCaoYuDeathMinimumTransitions(deathFrameId, victimSeat, capture);
    internal void RecordCaoYuMovement(long id) => _resolve().RecordCaoYuMovement(id);
    internal void RecoverCaoYu(global::CardGame.Core.ProgramSkillFrame f, int count, global::CardGame.Core.CaoYuStage stage) => _resolve().RecoverCaoYu(f, count, stage);
    internal bool ResumeCaoYu(long id) => _resolve().ResumeCaoYu(id);
    internal bool ReturnCaoYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCaoYu(f);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateXiangxuMinimumContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateXiangxuMinimumContext(frame, candidate);
    internal bool ValidCaoYuTransition(global::CardGame.Core.CaoYuMinimumTransitionEvent e, global::CardGame.Core.CardMovementBatchContext? batch = default) => _resolve().ValidCaoYuTransition(e, batch);
    internal bool XiangzuSpent(int owner) => _resolve().XiangzuSpent(owner);
    internal bool HasCaoYuCapability { get => _resolve().HasCaoYuCapability; }
}
