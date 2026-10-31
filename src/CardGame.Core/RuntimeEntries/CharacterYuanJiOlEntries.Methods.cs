#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanJiOlEntries
{
    internal PromptChoice AiYuanJi(PendingDecision p, ProgramSkillFrame f) => _resolve().AiYuanJi(p, f);
    internal void AnswerYuanJiOrder(PromptChoice choice) => _resolve().AnswerYuanJiOrder(choice);
    internal void BeginYuanJiDraw(ProgramSkillFrame f) => _resolve().BeginYuanJiDraw(f);
    internal void CaptureYuanJiAtomicLoss(CardMovementBatchContext batch) => _resolve().CaptureYuanJiAtomicLoss(batch);
    internal void CollectJieyanCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectJieyanCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureYuanJiAtomicLoss(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureYuanJiAtomicLoss(batch, completed);
    internal bool ContinueIssuedYuanJi(ProgramSkillFrame f) => _resolve().ContinueIssuedYuanJi(f);
    internal void ContinueYuanJi(long id) => _resolve().ContinueYuanJi(id);
    internal ProgramSkillWindowContext CreateJieyanContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => _resolve().CreateJieyanContext(frame, c);
    internal bool DrainYuanJi(ProgramSkillFrame f) => _resolve().DrainYuanJi(f);
    internal IReadOnlyList<int> DrawYuanJiEnd(int seat, bool top, CardConversionSource source) => _resolve().DrawYuanJiEnd(seat, top, source);
    internal bool ExcludeOrdinaryMovement_ResolveJieyanOppositeEndDrawPair(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_ResolveJieyanOppositeEndDrawPair(candidate, trigger);
    internal void FinishYuanJi(ProgramSkillFrame f) => _resolve().FinishYuanJi(f);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeYuanJiIssuedReturns(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeYuanJiIssuedReturns(owner, window, ordinary);
    internal bool IsIssuedJieyanCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedJieyanCandidate(c, context);
    internal bool IsIssuedYuanJiReturn(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedYuanJiReturn(c, context);
    internal bool IsYuanJiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsYuanJiRecoveryParent(f, recovery);
    internal bool JieyanDisabled(int owner) => _resolve().JieyanDisabled(owner);
    internal YuanJiLossOpportunityEvent? JieyanOpportunity(ProgramTriggerCandidate c) => _resolve().JieyanOpportunity(c);
    internal void PayYuanJiReturn(ProgramSkillFrame f) => _resolve().PayYuanJiReturn(f);
    internal void ProjectYuanJiCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectYuanJiCardSkillMark(cardId, ref projection);
    internal bool? ProveJieyanMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveJieyanMovement(trigger, candidate, context);
    internal void PublishYuanJiOrder(ProgramSkillFrame f) => _resolve().PublishYuanJiOrder(f);
    internal bool QualifyYuanJi(ProgramTriggerLegalityContext q) => _resolve().QualifyYuanJi(q);
    internal SkillProgramStepOutcome ResolveYuanJi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ResolveYuanJi(supplied, op);
    internal bool ResumeYuanJi(long id) => _resolve().ResumeYuanJi(id);
    internal bool ReturnYuanJiMovement(ProgramSkillFrame f) => _resolve().ReturnYuanJiMovement(f);
    internal void ReturnYuanJiPublicHand(ProgramSkillFrame f) => _resolve().ReturnYuanJiPublicHand(f);
    internal ProgramSkillWindowContext? TryCreateJieyanContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateJieyanContext(frame, candidate);
    internal bool ValidJieyanOpportunity(YuanJiLossOpportunityEvent e, CardMovementBatchContext batch) => _resolve().ValidJieyanOpportunity(e, batch);
    internal IEnumerable<YuanJiMarkClaim> YuanJiActiveClaims(int owner, string skill) => _resolve().YuanJiActiveClaims(owner, skill);
    internal string? YuanJiCardMarkLabel(int ownerSeat, int cardId) => _resolve().YuanJiCardMarkLabel(ownerSeat, cardId);
    internal IReadOnlyList<PromptChoice> YuanJiOrderChoices(ProgramSkillFrame f) => _resolve().YuanJiOrderChoices(f);
    internal bool YuanJiWindowCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, scoped out ResolutionFrame parent) => _resolve().YuanJiWindowCandidate(candidate, context, out parent);
    internal bool HasYuanJiCapability { get => _resolve().HasYuanJiCapability; }
}
