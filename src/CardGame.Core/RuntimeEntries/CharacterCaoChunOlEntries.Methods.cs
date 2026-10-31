#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoChunOlEntries
{
    internal void ArmCaoChunDebt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ArmCaoChunDebt(f);
    internal void AssertCaoChunNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertCaoChunNativeUse(use);
    internal void BeginCaoChunFactionSlash(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().BeginCaoChunFactionSlash(f, target);
    internal void BindCaoChunNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindCaoChunNativeUse(use);
    internal bool CanContinueCaoChun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueCaoChun(f);
    internal bool CanOfferCaoChun(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferCaoChun(c, t, w);
    internal global::CardGame.Core.PromptChoice CaoChunAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoChunAiChoice(f);
    internal global::System.Collections.Generic.Dictionary<string, string> CaoChunArgs(string branch) => _resolve().CaoChunArgs(branch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoChunChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoChunChoices(f);
    internal global::CardGame.Core.ProgramTriggerCandidate? CaoChunDebtCandidate(global::CardGame.Core.CaoChunHandUsePayment p) => _resolve().CaoChunDebtCandidate(p);
    internal global::CardGame.Core.ProgramSkillWindowContext CaoChunDebtContext(global::CardGame.Core.CaoChunHandUsePayment p, long windowId) => _resolve().CaoChunDebtContext(p, windowId);
    internal bool CaoChunFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().CaoChunFinalizedAfterCategoryTargetWindow(query);
    internal bool CaoChunFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoChunFirstChild(root, child);
    internal bool CaoChunInitialAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request) => _resolve().CaoChunInitialAdapterActor(f, actor, target, cursor, parent, request);
    internal global::CardGame.Core.CardLocation CaoChunNativeCostDestination(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => _resolve().CaoChunNativeCostDestination(action, cost);
    internal bool CaoChunNativeRidesOn(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.CardUseFrame use) => _resolve().CaoChunNativeRidesOn(parent, use);
    internal bool CaoChunNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().CaoChunNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.ProgramSkillFrame? CaoChunObserverRoot() => _resolve().CaoChunObserverRoot();
    internal global::CardGame.Core.CaoChunHandUsePayment? CaoChunPaidUseQualification(global::CardGame.Core.CardActionContext action) => _resolve().CaoChunPaidUseQualification(action);
    internal bool CaoChunResponseParentMatches(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind) => _resolve().CaoChunResponseParentMatches(attack, action, kind);
    internal bool CaoChunSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoChunSourceCurrent(f);
    internal global::CardGame.Core.CaoChunHandUsePayment? CaptureCaoChunPaidHandUse(long nativeParentId, global::CardGame.Core.CardActionContext action, long before, long after, global::CardGame.Core.ProgramCompletedResponseKind? responseKind = default) => _resolve().CaptureCaoChunPaidHandUse(nativeParentId, action, before, after, responseKind);
    internal void CaptureCaoChunUsePaymentBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureCaoChunUsePaymentBatch(batch);
    internal void CompleteCaoChunNativePayment(long id) => _resolve().CompleteCaoChunNativePayment(id);
    internal bool CompleteCaoChunSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt receipt) => _resolve().CompleteCaoChunSlash(receipt);
    internal bool CompleteCaoChunUseReturn(global::CardGame.Core.CardUseFrame use) => _resolve().CompleteCaoChunUseReturn(use);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchObserveCaoChunMovement(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchObserveCaoChunMovement(batch, completed);
    internal void DispatchCaoChunRequestedDeckSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().DispatchCaoChunRequestedDeckSlash(f, card, targets);
    internal bool DrainCaoChun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainCaoChun(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteCaoChun(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteCaoChun(supplied, op);
    internal void ExpireCaoChunPlayDebt(int owner, int phaseInstance) => _resolve().ExpireCaoChunPlayDebt(owner, phaseInstance);
    internal void FinishCaoChun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCaoChun(f);
    internal bool FinishCaoChunFactionDeclined(long id, global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => _resolve().FinishCaoChunFactionDeclined(id, pending);
    internal bool HasCaoChunBeforeDamageObserver(long id) => _resolve().HasCaoChunBeforeDamageObserver(id);
    internal bool HasCaoChunDamageObserver(long id) => _resolve().HasCaoChunDamageObserver(id);
    internal bool HasIssuedCaoChunDebtCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedCaoChunDebtCandidate(candidate, context);
    internal void InitializeAcquiredCaoChun(int seat, string skillId) => _resolve().InitializeAcquiredCaoChun(seat, skillId);
    internal bool IsCaoChunInitialRequest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsCaoChunInitialRequest(f);
    internal bool IsCaoChunMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsCaoChunMovement(f, e, p);
    internal bool IsCaoChunNativeEdge(int index) => _resolve().IsCaoChunNativeEdge(index);
    internal void IssueCaoChunMovement(long id, global::CardGame.Core.CaoChunReceipt r) => _resolve().IssueCaoChunMovement(id, r);
    internal void ObserveAcquiredCaoChun(global::CardGame.Core.SkillsAcquiredEvent acquired) => _resolve().ObserveAcquiredCaoChun(acquired);
    internal void ObserveCaoChunMovement(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().ObserveCaoChunMovement(batch);
    internal void ObservePurchasedCaoChun(global::CardGame.Core.WushuangOfferBoughtEvent bought) => _resolve().ObservePurchasedCaoChun(bought);
    internal void PrepareCaoChunChainedSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ChainedStateBasicPayment p) => _resolve().PrepareCaoChunChainedSlash(f, p);
    internal void PrepareCaoChunDrawFundedSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.DrawFundedDistinctBasicPayment p) => _resolve().PrepareCaoChunDrawFundedSlash(f, p);
    internal bool PrepareCaoChunFactionNative(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind) => _resolve().PrepareCaoChunFactionNative(f, pending, provider, cards, kind);
    internal void PublishCaoChun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaoChun(f);
    internal void RequireCaoChun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireCaoChun(f);
    internal void ResolveCaoChunChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveCaoChunChoice(choice);
    internal bool ResumeCaoChun(long id) => _resolve().ResumeCaoChun(id);
    internal bool ReturnCaoChunMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCaoChunMovement(f);
    internal bool TryAdvanceCaoChunSubtree() => _resolve().TryAdvanceCaoChunSubtree();
    internal bool TryBeginCaoChunPaidResponseDebt(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().TryBeginCaoChunPaidResponseDebt(attack, action, continuation);
    internal bool TryBeginCaoChunSimpleDebt(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().TryBeginCaoChunSimpleDebt(id, continuation);
    internal bool ValidCaoChunPaidActivation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCaoChunPaidActivation(f);
    internal bool ValidCaoChunPayment(global::CardGame.Core.CaoChunHandUsePayment p) => _resolve().ValidCaoChunPayment(p);
    internal bool ValidateCaoChunFactionSlashParent(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidateCaoChunFactionSlashParent(pending, f);
    internal bool HasCaoChunCapability { get => _resolve().HasCaoChunCapability; }
    internal bool HasIssuedCaoChunDebt { get => _resolve().HasIssuedCaoChunDebt; }
}
