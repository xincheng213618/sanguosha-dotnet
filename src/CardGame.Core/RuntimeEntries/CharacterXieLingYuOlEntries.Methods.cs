#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXieLingYuOlEntries
{
    internal bool AdvanceXieLingYuChildren() => _resolve().AdvanceXieLingYuChildren();
    internal void AnswerXieLingYu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerXieLingYu(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXieLingYu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginXieLingYu(supplied, op);
    internal bool CanActivateXinyou(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateXinyou(c);
    internal bool CanContinueIssuedXieLingYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedXieLingYu(f);
    internal void CaptureXieLingYuMovementBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureXieLingYuMovementBatch(batch);
    internal global::CardGame.Core.PromptChoice ChooseXieLingYuAi(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseXieLingYuAi(prompt, f);
    internal void CollectWindow_CollectYuandiCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectYuandiCandidates(batch, result);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectYuandiCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectYuandiCandidates(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureXieLingYuMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureXieLingYuMovementBatch(batch, completed);
    internal void ContinueXieLingYu(long id) => _resolve().ContinueXieLingYu(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateYuandiMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateYuandiMovementContext(frame, candidate);
    internal global::CardGame.Core.YuandiOpportunityEvent? CurrentYuandiOpportunity(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentYuandiOpportunity(c, context);
    internal bool ExcludeOrdinaryMovement_ResolveYuandiDiscardNames(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_ResolveYuandiDiscardNames(candidate, trigger);
    internal void FinishXieLingYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishXieLingYu(f);
    internal bool HasXinyouCardMark(int ownerSeat, int cardId) => _resolve().HasXinyouCardMark(ownerSeat, cardId);
    internal bool IsIssuedYuandiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedYuandiCandidate(c, context);
    internal bool IsXieLingYuPaidMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsXieLingYuPaidMovement(f, m);
    internal bool IsXieLingYuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsXieLingYuRecoveryParent(f, recovery);
    internal bool IsXinyouHandLimitExempt(int ownerSeat, int cardId) => _resolve().IsXinyouHandLimitExempt(ownerSeat, cardId);
    internal bool PermitIssuedXieLingYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitIssuedXieLingYu(f);
    internal void ProjectXinyouCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectXinyouCardSkillMark(cardId, ref projection);
    internal void PublishXieLingYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishXieLingYu(f);
    internal bool ResumeXieLingYu(long id) => _resolve().ResumeXieLingYu(id);
    internal void ReturnXieLingYuDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnXieLingYuDraw(f, outcome);
    internal bool ReturnXieLingYuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXieLingYuMovement(f);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateYuandiMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateYuandiMovementContext(frame, candidate);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XieLingYuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XieLingYuChoices(f);
    internal bool XieLingYuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().XieLingYuFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? XieLingYuObserverRoot() => _resolve().XieLingYuObserverRoot();
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? XinyouActualPlay(int owner) => _resolve().XinyouActualPlay(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> XinyouCostCards(int owner) => _resolve().XinyouCostCards(owner);
    internal bool XinyouPhaseSpent(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().XinyouPhaseSpent(owner, phase);
    internal (long Sequence, bool NonUse)? YuandiFirstGroup(int owner, int turn, int turnOwner, int mask) => _resolve().YuandiFirstGroup(owner, turn, turnOwner, mask);
    internal int YuandiRemainingMask(int owner) => _resolve().YuandiRemainingMask(owner);
    internal bool HasXieLingYuCapability { get => _resolve().HasXieLingYuCapability; }
}
