#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXieLingYuOlEntries
{
    internal bool AdvanceXieLingYuChildren() => _resolve().AdvanceXieLingYuChildren();
    internal void AnswerXieLingYu(PromptChoice choice) => _resolve().AnswerXieLingYu(choice);
    internal SkillProgramStepOutcome BeginXieLingYu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginXieLingYu(supplied, op);
    internal bool CanActivateXinyou(ProgramActivationLegalityContext c) => _resolve().CanActivateXinyou(c);
    internal bool CanContinueIssuedXieLingYu(ProgramSkillFrame f) => _resolve().CanContinueIssuedXieLingYu(f);
    internal void CaptureXieLingYuMovementBatch(CardMovementBatchContext batch) => _resolve().CaptureXieLingYuMovementBatch(batch);
    internal PromptChoice ChooseXieLingYuAi(PendingDecision prompt, ProgramSkillFrame f) => _resolve().ChooseXieLingYuAi(prompt, f);
    internal void CollectWindow_CollectYuandiCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectYuandiCandidates(batch, result);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectYuandiCandidates(CardMovementBatchContext batch) => _resolve().CollectYuandiCandidates(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureXieLingYuMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureXieLingYuMovementBatch(batch, completed);
    internal void ContinueXieLingYu(long id) => _resolve().ContinueXieLingYu(id);
    internal ProgramSkillWindowContext CreateYuandiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateYuandiMovementContext(frame, candidate);
    internal YuandiOpportunityEvent? CurrentYuandiOpportunity(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentYuandiOpportunity(c, context);
    internal bool ExcludeOrdinaryMovement_ResolveYuandiDiscardNames(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_ResolveYuandiDiscardNames(candidate, trigger);
    internal void FinishXieLingYu(ProgramSkillFrame f) => _resolve().FinishXieLingYu(f);
    internal bool HasXinyouCardMark(int ownerSeat, int cardId) => _resolve().HasXinyouCardMark(ownerSeat, cardId);
    internal bool IsIssuedYuandiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedYuandiCandidate(c, context);
    internal bool IsXieLingYuPaidMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsXieLingYuPaidMovement(f, m);
    internal bool IsXieLingYuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsXieLingYuRecoveryParent(f, recovery);
    internal bool IsXinyouHandLimitExempt(int ownerSeat, int cardId) => _resolve().IsXinyouHandLimitExempt(ownerSeat, cardId);
    internal bool PermitIssuedXieLingYu(ProgramSkillFrame f) => _resolve().PermitIssuedXieLingYu(f);
    internal void ProjectXinyouCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectXinyouCardSkillMark(cardId, ref projection);
    internal void PublishXieLingYu(ProgramSkillFrame f) => _resolve().PublishXieLingYu(f);
    internal bool ResumeXieLingYu(long id) => _resolve().ResumeXieLingYu(id);
    internal void ReturnXieLingYuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnXieLingYuDraw(f, outcome);
    internal bool ReturnXieLingYuMovement(ProgramSkillFrame f) => _resolve().ReturnXieLingYuMovement(f);
    internal ProgramSkillWindowContext? TryCreateYuandiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateYuandiMovementContext(frame, candidate);
    internal IReadOnlyList<PromptChoice> XieLingYuChoices(ProgramSkillFrame f) => _resolve().XieLingYuChoices(f);
    internal bool XieLingYuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().XieLingYuFirstChild(f, child);
    internal ProgramSkillFrame? XieLingYuObserverRoot() => _resolve().XieLingYuObserverRoot();
    internal ActualDiscardRecoveryPhaseKey? XinyouActualPlay(int owner) => _resolve().XinyouActualPlay(owner);
    internal IReadOnlyList<Card> XinyouCostCards(int owner) => _resolve().XinyouCostCards(owner);
    internal bool XinyouPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().XinyouPhaseSpent(owner, phase);
    internal (long Sequence, bool NonUse)? YuandiFirstGroup(int owner, int turn, int turnOwner, int mask) => _resolve().YuandiFirstGroup(owner, turn, turnOwner, mask);
    internal int YuandiRemainingMask(int owner) => _resolve().YuandiRemainingMask(owner);
    internal bool HasXieLingYuCapability { get => _resolve().HasXieLingYuCapability; }
}
