#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoChunOlEntries
{
    internal void ArmCaoChunDebt(ProgramSkillFrame f) => _resolve().ArmCaoChunDebt(f);
    internal void AssertCaoChunNativeUse(CardUseFrame use) => _resolve().AssertCaoChunNativeUse(use);
    internal void BeginCaoChunFactionSlash(ProgramSkillFrame f, int target) => _resolve().BeginCaoChunFactionSlash(f, target);
    internal void BindCaoChunNativeUse(CardUseFrame use) => _resolve().BindCaoChunNativeUse(use);
    internal bool CanContinueCaoChun(ProgramSkillFrame f) => _resolve().CanContinueCaoChun(f);
    internal bool CanOfferCaoChun(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferCaoChun(c, t, w);
    internal PromptChoice CaoChunAiChoice(ProgramSkillFrame f) => _resolve().CaoChunAiChoice(f);
    internal Dictionary<string, string> CaoChunArgs(string branch) => _resolve().CaoChunArgs(branch);
    internal IReadOnlyList<PromptChoice> CaoChunChoices(ProgramSkillFrame f) => _resolve().CaoChunChoices(f);
    internal ProgramTriggerCandidate? CaoChunDebtCandidate(CaoChunHandUsePayment p) => _resolve().CaoChunDebtCandidate(p);
    internal ProgramSkillWindowContext CaoChunDebtContext(CaoChunHandUsePayment p, long windowId) => _resolve().CaoChunDebtContext(p, windowId);
    internal bool CaoChunFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().CaoChunFinalizedAfterCategoryTargetWindow(query);
    internal bool CaoChunFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().CaoChunFirstChild(root, child);
    internal bool CaoChunInitialAdapterActor(ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request) => _resolve().CaoChunInitialAdapterActor(f, actor, target, cursor, parent, request);
    internal CardLocation CaoChunNativeCostDestination(CardActionContext action, CardActionCost cost) => _resolve().CaoChunNativeCostDestination(action, cost);
    internal bool CaoChunNativeRidesOn(ProgramSkillFrame parent, CardUseFrame use) => _resolve().CaoChunNativeRidesOn(parent, use);
    internal bool CaoChunNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().CaoChunNormalizeImplicitDrawTwoTargetWindow(query);
    internal ProgramSkillFrame? CaoChunObserverRoot() => _resolve().CaoChunObserverRoot();
    internal CaoChunHandUsePayment? CaoChunPaidUseQualification(CardActionContext action) => _resolve().CaoChunPaidUseQualification(action);
    internal bool CaoChunResponseParentMatches(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCompletedResponseKind kind) => _resolve().CaoChunResponseParentMatches(attack, action, kind);
    internal bool CaoChunSourceCurrent(ProgramSkillFrame f) => _resolve().CaoChunSourceCurrent(f);
    internal CaoChunHandUsePayment? CaptureCaoChunPaidHandUse(long nativeParentId, CardActionContext action, long before, long after, ProgramCompletedResponseKind? responseKind = default) => _resolve().CaptureCaoChunPaidHandUse(nativeParentId, action, before, after, responseKind);
    internal void CaptureCaoChunUsePaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureCaoChunUsePaymentBatch(batch);
    internal void CompleteCaoChunNativePayment(long id) => _resolve().CompleteCaoChunNativePayment(id);
    internal bool CompleteCaoChunSlash(GameEngine.AttackCompletionReceipt receipt) => _resolve().CompleteCaoChunSlash(receipt);
    internal bool CompleteCaoChunUseReturn(CardUseFrame use) => _resolve().CompleteCaoChunUseReturn(use);
    internal CardMovementBatchContext CompleteMovementBatchObserveCaoChunMovement(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchObserveCaoChunMovement(batch, completed);
    internal void DispatchCaoChunRequestedDeckSlash(ProgramSkillFrame f, Card card, IReadOnlyList<int> targets) => _resolve().DispatchCaoChunRequestedDeckSlash(f, card, targets);
    internal bool DrainCaoChun(ProgramSkillFrame f) => _resolve().DrainCaoChun(f);
    internal SkillProgramStepOutcome ExecuteCaoChun(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteCaoChun(supplied, op);
    internal void ExpireCaoChunPlayDebt(int owner, int phaseInstance) => _resolve().ExpireCaoChunPlayDebt(owner, phaseInstance);
    internal void FinishCaoChun(ProgramSkillFrame f) => _resolve().FinishCaoChun(f);
    internal bool FinishCaoChunFactionDeclined(long id, NativeOwners.FactionCardRequestHandle pending) => _resolve().FinishCaoChunFactionDeclined(id, pending);
    internal bool HasIssuedCaoChunDebtCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasIssuedCaoChunDebtCandidate(candidate, context);
    internal void InitializeAcquiredCaoChun(int seat, string skillId) => _resolve().InitializeAcquiredCaoChun(seat, skillId);
    internal bool IsCaoChunInitialRequest(ProgramSkillFrame f) => _resolve().IsCaoChunInitialRequest(f);
    internal bool IsCaoChunNativeEdge(int index) => _resolve().IsCaoChunNativeEdge(index);
    internal void IssueCaoChunMovement(long id, CaoChunReceipt r) => _resolve().IssueCaoChunMovement(id, r);
    internal void ObserveAcquiredCaoChun(SkillsAcquiredEvent acquired) => _resolve().ObserveAcquiredCaoChun(acquired);
    internal void ObserveCaoChunMovement(CardMovementBatchContext batch) => _resolve().ObserveCaoChunMovement(batch);
    internal void ObservePurchasedCaoChun(WushuangOfferBoughtEvent bought) => _resolve().ObservePurchasedCaoChun(bought);
    internal void PrepareCaoChunChainedSlash(ProgramSkillFrame f, ChainedStateBasicPayment p) => _resolve().PrepareCaoChunChainedSlash(f, p);
    internal void PrepareCaoChunDrawFundedSlash(ProgramSkillFrame f, DrawFundedDistinctBasicPayment p) => _resolve().PrepareCaoChunDrawFundedSlash(f, p);
    internal bool PrepareCaoChunFactionNative(ProgramSkillFrame f, NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> cards, CardKind kind) => _resolve().PrepareCaoChunFactionNative(f, pending, provider, cards, kind);
    internal void PublishCaoChun(ProgramSkillFrame f) => _resolve().PublishCaoChun(f);
    internal void RequireCaoChun(ProgramSkillFrame f) => _resolve().RequireCaoChun(f);
    internal void ResolveCaoChunChoice(PromptChoice choice) => _resolve().ResolveCaoChunChoice(choice);
    internal bool ResumeCaoChun(long id) => _resolve().ResumeCaoChun(id);
    internal bool ReturnCaoChunMovement(ProgramSkillFrame f) => _resolve().ReturnCaoChunMovement(f);
    internal bool TryAdvanceCaoChunSubtree() => _resolve().TryAdvanceCaoChunSubtree();
    internal bool TryBeginCaoChunPaidResponseDebt(NativeOwners.CardAttackHandle? attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().TryBeginCaoChunPaidResponseDebt(attack, action, continuation);
    internal bool TryBeginCaoChunSimpleDebt(long id, ProgramSimpleCardContinuation continuation) => _resolve().TryBeginCaoChunSimpleDebt(id, continuation);
    internal bool ValidCaoChunPaidActivation(ProgramSkillFrame f) => _resolve().ValidCaoChunPaidActivation(f);
    internal bool ValidCaoChunPayment(CaoChunHandUsePayment p) => _resolve().ValidCaoChunPayment(p);
    internal bool ValidateCaoChunFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f) => _resolve().ValidateCaoChunFactionSlashParent(pending, f);
    internal bool HasCaoChunCapability { get => _resolve().HasCaoChunCapability; }
    internal bool HasIssuedCaoChunDebt { get => _resolve().HasIssuedCaoChunDebt; }
}
