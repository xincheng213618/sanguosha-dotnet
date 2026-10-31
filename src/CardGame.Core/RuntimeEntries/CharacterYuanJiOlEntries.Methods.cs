#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanJiOlEntries
{
    internal global::CardGame.Core.PromptChoice AiYuanJi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiYuanJi(p, f);
    internal void AnswerYuanJiOrder(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerYuanJiOrder(choice);
    internal void AssertYuanJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertYuanJi(f);
    internal void BeginYuanJiDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginYuanJiDraw(f);
    internal void CaptureYuanJiAtomicLoss(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureYuanJiAtomicLoss(batch);
    internal void CollectJieyanCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectJieyanCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureYuanJiAtomicLoss(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureYuanJiAtomicLoss(batch, completed);
    internal bool ContinueIssuedYuanJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedYuanJi(f);
    internal void ContinueYuanJi(long id) => _resolve().ContinueYuanJi(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateJieyanContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateJieyanContext(frame, c);
    internal bool DrainYuanJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainYuanJi(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> DrawYuanJiEnd(int seat, bool top, global::CardGame.Core.CardConversionSource source) => _resolve().DrawYuanJiEnd(seat, top, source);
    internal bool ExcludeOrdinaryMovement_ResolveJieyanOppositeEndDrawPair(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_ResolveJieyanOppositeEndDrawPair(candidate, trigger);
    internal void FinishYuanJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishYuanJi(f);
    internal bool HasYuanJiBeforeDamageObserver(long id) => _resolve().HasYuanJiBeforeDamageObserver(id);
    internal bool HasYuanJiDamageObserver(long id) => _resolve().HasYuanJiDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeYuanJiIssuedReturns(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeYuanJiIssuedReturns(owner, window, ordinary);
    internal bool IsIssuedJieyanCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedJieyanCandidate(c, context);
    internal bool IsIssuedYuanJiReturn(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedYuanJiReturn(c, context);
    internal bool IsYuanJiDying() => _resolve().IsYuanJiDying();
    internal bool IsYuanJiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsYuanJiMovement(f, e, p);
    internal bool IsYuanJiOwnMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsYuanJiOwnMovement(f, m);
    internal bool IsYuanJiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsYuanJiRecoveryParent(f, recovery);
    internal bool JieyanDisabled(int owner) => _resolve().JieyanDisabled(owner);
    internal global::CardGame.Core.YuanJiLossOpportunityEvent? JieyanOpportunity(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().JieyanOpportunity(c);
    internal void PayYuanJiReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayYuanJiReturn(f);
    internal void ProjectYuanJiCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectYuanJiCardSkillMark(cardId, ref projection);
    internal bool? ProveJieyanMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveJieyanMovement(trigger, candidate, context);
    internal void PublishYuanJiOrder(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishYuanJiOrder(f);
    internal bool QualifyYuanJi(global::CardGame.Core.ProgramTriggerLegalityContext q) => _resolve().QualifyYuanJi(q);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveYuanJi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveYuanJi(supplied, op);
    internal bool ResumeYuanJi(long id) => _resolve().ResumeYuanJi(id);
    internal bool ReturnYuanJiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYuanJiMovement(f);
    internal void ReturnYuanJiPublicHand(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnYuanJiPublicHand(f);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateJieyanContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateJieyanContext(frame, candidate);
    internal bool ValidJieyanOpportunity(global::CardGame.Core.YuanJiLossOpportunityEvent e, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().ValidJieyanOpportunity(e, batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.YuanJiMarkClaim> YuanJiActiveClaims(int owner, string skill) => _resolve().YuanJiActiveClaims(owner, skill);
    internal string? YuanJiCardMarkLabel(int ownerSeat, int cardId) => _resolve().YuanJiCardMarkLabel(ownerSeat, cardId);
    internal bool YuanJiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().YuanJiFirstChild(f, child);
    internal bool YuanJiObserverEdge(int n) => _resolve().YuanJiObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? YuanJiObserverRoot() => _resolve().YuanJiObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> YuanJiOrderChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YuanJiOrderChoices(f);
    internal bool YuanJiParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().YuanJiParent(f);
    internal bool YuanJiWindowCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.ResolutionFrame parent) => _resolve().YuanJiWindowCandidate(candidate, context, out parent);
    internal bool HasYuanJiCapability { get => _resolve().HasYuanJiCapability; }
}
