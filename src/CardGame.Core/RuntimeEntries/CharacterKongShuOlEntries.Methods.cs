#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKongShuOlEntries
{
    internal bool AdvanceKongShuChildren() => _resolve().AdvanceKongShuChildren();
    internal void AnswerKongShu(PromptChoice choice) => _resolve().AnswerKongShu(choice);
    internal void AssertLeiluanPayments(CardUseFrame use) => _resolve().AssertLeiluanPayments(use);
    internal SkillProgramStepOutcome BeginKongShu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginKongShu(supplied, op);
    internal void BindLeiluanNativeUse(CardUseFrame supplied) => _resolve().BindLeiluanNativeUse(supplied);
    internal void BindLeiluanResponsePayment(long nativeUseId, CardActionContext action) => _resolve().BindLeiluanResponsePayment(nativeUseId, action);
    internal IEnumerable<LegalAction> BuildLeiluanPlayActions(CharacterState owner) => _resolve().BuildLeiluanPlayActions(owner);
    internal void CaptureLeiluanPaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureLeiluanPaymentBatch(batch);
    internal void CollectIssuedLeiluanCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedLeiluanCandidates(action, window, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureLeiluanPaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureLeiluanPaymentBatch(batch, completed);
    internal void ContinueKongShu(long id) => _resolve().ContinueKongShu(id);
    internal bool ContinuePaidKongShu(ProgramSkillFrame f) => _resolve().ContinuePaidKongShu(f);
    internal bool DrainKongShu(ProgramSkillFrame f) => _resolve().DrainKongShu(f);
    internal void FinishKongShu(ProgramSkillFrame f) => _resolve().FinishKongShu(f);
    internal bool IsIssuedLeiluanCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedLeiluanCandidate(candidate, context);
    internal bool IsKongShuPaidMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsKongShuPaidMovement(f, m);
    internal bool IsKongShuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsKongShuRecoveryParent(f, recovery);
    internal bool IsLeiluanSelection(ProgramMultiCardViewAsSelection selection) => _resolve().IsLeiluanSelection(selection);
    internal bool IsLeiluanSource(CardConversionSource? source) => _resolve().IsLeiluanSource(source);
    internal void IssueKongShuDraw(ProgramSkillFrame f, int payer, int count, KongShuStage stage) => _resolve().IssueKongShuDraw(f, payer, count, stage);
    internal void IssueKongShuMovement(ProgramSkillFrame f, IReadOnlyList<int> ids, bool discard) => _resolve().IssueKongShuMovement(f, ids, discard);
    internal LeiluanNativePayment? IssuedLeiluanPayment(CardActionContext action) => _resolve().IssuedLeiluanPayment(action);
    internal PromptChoice KongShuAi(PendingDecision p, ProgramSkillFrame f) => _resolve().KongShuAi(p, f);
    internal IReadOnlyList<PromptChoice> KongShuChoices(ProgramSkillFrame f) => _resolve().KongShuChoices(f);
    internal bool KongShuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().KongShuFirstChild(f, child);
    internal IReadOnlyList<int> KongShuLivingChained() => _resolve().KongShuLivingChained();
    internal ProgramSkillFrame? KongShuObserverRoot() => _resolve().KongShuObserverRoot();
    internal bool LeiluanActionMatches(LeiluanNativePayment p, CardActionContext action) => _resolve().LeiluanActionMatches(p, action);
    internal bool LeiluanAlcoholAllowed(CharacterState owner, IReadOnlyList<Card> cards) => _resolve().LeiluanAlcoholAllowed(owner, cards);
    internal bool LeiluanDisabled(int owner, string state) => _resolve().LeiluanDisabled(owner, state);
    internal bool LeiluanNameUsed(int owner, CardKind kind) => _resolve().LeiluanNameUsed(owner, kind);
    internal bool LeiluanNativeDirection(CharacterState owner, CardKind kind, bool response) => _resolve().LeiluanNativeDirection(owner, kind, response);
    internal IReadOnlyList<int> LeiluanPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().LeiluanPlayTargets(owner, selection);
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> LeiluanSelections(CharacterState owner, CardKind kind, bool forResponse, bool ignoreSuitUseProhibition = false) => _resolve().LeiluanSelections(owner, kind, forResponse, ignoreSuitUseProhibition);
    internal IReadOnlyList<Card> LeiluanTrickCandidates(ProgramSkillFrame f) => _resolve().LeiluanTrickCandidates(f);
    internal LeiluanNativePayment PrepareLeiluanPayment(CardUseFrame use, CharacterState owner, CardConversionSource source, CardKind kind, bool response, IReadOnlyList<CardActionCost> costs) => _resolve().PrepareLeiluanPayment(use, owner, source, kind, response, costs);
    internal void PrepareLeiluanResponsePayment(CharacterState owner, ProgramMultiCardViewAsSelection selection, long nativeUseId) => _resolve().PrepareLeiluanResponsePayment(owner, selection, nativeUseId);
    internal void PublishKongShu(ProgramSkillFrame f) => _resolve().PublishKongShu(f);
    internal bool ResumeKongShu(long id) => _resolve().ResumeKongShu(id);
    internal bool ReturnKongShuMovement(ProgramSkillFrame f) => _resolve().ReturnKongShuMovement(f);
    internal bool TryExecuteLeiluanPlay(CharacterState actor, LegalAction selected, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().TryExecuteLeiluanPlay(actor, selected, ids, targets);
    internal bool TryFinishLeiluanAlcoholCost(CardUseFrame use, Card card) => _resolve().TryFinishLeiluanAlcoholCost(use, card);
    internal bool TryResolveLeiluanDyingResponse(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveLeiluanDyingResponse(owner, selection);
    internal bool TrySubmitLeiluanPlay(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().TrySubmitLeiluanPlay(command, out result);
    internal void UpdateLeiluanPayment(long useId, LeiluanNativePayment payment) => _resolve().UpdateLeiluanPayment(useId, payment);
    internal CommandError? ValidateLeiluanPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ValidateLeiluanPlay(owner, action, ids, targets);
    internal bool HasKongShuCapability { get => _resolve().HasKongShuCapability; }
    internal bool HasLeiluanCapability { get => _resolve().HasLeiluanCapability; }
    internal int LeiluanCurrentX { get => _resolve().LeiluanCurrentX; }
}
