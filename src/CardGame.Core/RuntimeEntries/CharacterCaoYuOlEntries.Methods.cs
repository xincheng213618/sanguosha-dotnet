#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoYuOlEntries
{
    internal bool AdvanceCaoYuSubtree() => _resolve().AdvanceCaoYuSubtree();
    internal void AdvanceGongjieDonor(ProgramSkillFrame f) => _resolve().AdvanceGongjieDonor(f);
    internal void AfterGongjieGifts(ProgramSkillFrame f) => _resolve().AfterGongjieGifts(f);
    internal PromptChoice AiCaoYu(PendingDecision p, ProgramSkillFrame f) => _resolve().AiCaoYu(p, f);
    internal void AnswerCaoYu(PromptChoice choice) => _resolve().AnswerCaoYu(choice);
    internal SkillProgramStepOutcome BeginCaoYu(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginCaoYu(input, op);
    internal bool CaoYuActualTurnOpen(int turn, int owner) => _resolve().CaoYuActualTurnOpen(turn, owner);
    internal IReadOnlyList<PromptChoice> CaoYuChoices(ProgramSkillFrame f) => _resolve().CaoYuChoices(f);
    internal IReadOnlyList<Card> CaoYuDiscardCards(int owner) => _resolve().CaoYuDiscardCards(owner);
    internal bool CaoYuDyingEntry(ProgramSkillFrame f, scoped out DyingFrame dying) => _resolve().CaoYuDyingEntry(f, out dying);
    internal bool CaoYuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CaoYuFirstChild(f, child);
    internal IReadOnlyList<Card> CaoYuGiftCards(int owner, int chooser) => _resolve().CaoYuGiftCards(owner, chooser);
    internal bool CaoYuHistory(int owner, string skill, int recipient) => _resolve().CaoYuHistory(owner, skill, recipient);
    internal ProgramSkillFrame? CaoYuObserverRoot() => _resolve().CaoYuObserverRoot();
    internal bool CaoYuOpportunityConsumed(long id) => _resolve().CaoYuOpportunityConsumed(id);
    internal IEnumerable<CaoYuMinimumTransitionEvent> CaoYuOutstanding(int turn) => _resolve().CaoYuOutstanding(turn);
    internal bool CaoYuRoundFirst(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CaoYuRoundFirst(c, context);
    internal CaoYuMinimumTransitionEvent? CaoYuTransition(ProgramTriggerCandidate c, long? id = default) => _resolve().CaoYuTransition(c, id);
    internal GameEngine.CaoYuDeathMinimumCapture? CaptureCaoYuDeathMinimumSources(int victimSeat) => _resolve().CaptureCaoYuDeathMinimumSources(victimSeat);
    internal void CaptureCaoYuMinimumTransitions(CardMovementBatchContext batch) => _resolve().CaptureCaoYuMinimumTransitions(batch);
    internal void CollectXiangxuMinimumCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectXiangxuMinimumCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureCaoYuMinimumTransitions(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureCaoYuMinimumTransitions(batch, completed);
    internal void ContinueCaoYu(long id) => _resolve().ContinueCaoYu(id);
    internal bool ContinueIssuedCaoYu(ProgramSkillFrame f) => _resolve().ContinueIssuedCaoYu(f);
    internal bool ContinuePaidCaoYu(ProgramSkillFrame f) => _resolve().ContinuePaidCaoYu(f);
    internal ProgramSkillWindowContext CreateXiangxuAfterTurnEndedContext(DeferredTurnEndFrame p, ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CreateXiangxuAfterTurnEndedContext(p, c, context);
    internal ProgramSkillWindowContext CreateXiangxuMinimumContext(CardsMovedTriggerWindowFrame f, ProgramTriggerCandidate c) => _resolve().CreateXiangxuMinimumContext(f, c);
    internal bool DrainCaoYu(ProgramSkillFrame f) => _resolve().DrainCaoYu(f);
    internal void DrawCaoYu(ProgramSkillFrame f, int count, CaoYuStage stage, string reason) => _resolve().DrawCaoYu(f, count, stage, reason);
    internal bool ExcludeOrdinaryMovement_IsXiangxuMinimumTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsXiangxuMinimumTrigger(candidate, trigger);
    internal void FinishCaoYu(ProgramSkillFrame f) => _resolve().FinishCaoYu(f);
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeXiangxuAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary) => _resolve().IncludeXiangxuAfterTurnEndedItems(parent, ordinary);
    internal bool IsCaoYuIssuedMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsCaoYuIssuedMovement(f, m);
    internal bool IsCaoYuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsCaoYuRecoveryParent(f, recovery);
    internal bool IsIssuedXiangxuEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedXiangxuEndingCandidate(c, context);
    internal bool IsXiangxuEndingTrigger(SkillProgramTrigger t) => _resolve().IsXiangxuEndingTrigger(t);
    internal bool IsXiangxuMinimumCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsXiangxuMinimumCandidate(c, context);
    internal bool IsXiangxuMinimumTrigger(SkillProgramTrigger t) => _resolve().IsXiangxuMinimumTrigger(t);
    internal void PayCaoYuCards(ProgramSkillFrame f, IReadOnlyList<int> ids, int? target, CaoYuStage stage, string reason) => _resolve().PayCaoYuCards(f, ids, target, stage, reason);
    internal bool? ProveXiangxuMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveXiangxuMovement(trigger, candidate, context);
    internal void PublishCaoYu(ProgramSkillFrame f) => _resolve().PublishCaoYu(f);
    internal bool QualifyCaoYu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyCaoYu(c, t, context);
    internal void RecordCaoYuDeathMinimumTransitions(long deathFrameId, int victimSeat, GameEngine.CaoYuDeathMinimumCapture? capture) => _resolve().RecordCaoYuDeathMinimumTransitions(deathFrameId, victimSeat, capture);
    internal void RecordCaoYuMovement(long id) => _resolve().RecordCaoYuMovement(id);
    internal void RecoverCaoYu(ProgramSkillFrame f, int count, CaoYuStage stage) => _resolve().RecoverCaoYu(f, count, stage);
    internal bool ResumeCaoYu(long id) => _resolve().ResumeCaoYu(id);
    internal bool ReturnCaoYu(ProgramSkillFrame f) => _resolve().ReturnCaoYu(f);
    internal ProgramSkillWindowContext? TryCreateXiangxuMinimumContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateXiangxuMinimumContext(frame, candidate);
    internal bool ValidCaoYuTransition(CaoYuMinimumTransitionEvent e, CardMovementBatchContext? batch = default) => _resolve().ValidCaoYuTransition(e, batch);
    internal bool XiangzuSpent(int owner) => _resolve().XiangzuSpent(owner);
    internal bool HasCaoYuCapability { get => _resolve().HasCaoYuCapability; }
}
