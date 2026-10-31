#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKongShuOlEntries
{
    internal bool AdvanceKongShuChildren() => _resolve().AdvanceKongShuChildren();
    internal void AnswerKongShu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerKongShu(choice);
    internal void AssertKongShu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertKongShu(f);
    internal void AssertLeiluanPayments(global::CardGame.Core.CardUseFrame use) => _resolve().AssertLeiluanPayments(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginKongShu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginKongShu(supplied, op);
    internal void BindLeiluanNativeUse(global::CardGame.Core.CardUseFrame supplied) => _resolve().BindLeiluanNativeUse(supplied);
    internal void BindLeiluanResponsePayment(long nativeUseId, global::CardGame.Core.CardActionContext action) => _resolve().BindLeiluanResponsePayment(nativeUseId, action);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildLeiluanPlayActions(global::CardGame.Core.CharacterState owner) => _resolve().BuildLeiluanPlayActions(owner);
    internal void CaptureLeiluanPaymentBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureLeiluanPaymentBatch(batch);
    internal void CollectIssuedLeiluanCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedLeiluanCandidates(action, window, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureLeiluanPaymentBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureLeiluanPaymentBatch(batch, completed);
    internal void ContinueKongShu(long id) => _resolve().ContinueKongShu(id);
    internal bool ContinuePaidKongShu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidKongShu(f);
    internal bool DrainKongShu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainKongShu(f);
    internal void FinishKongShu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishKongShu(f);
    internal bool HasKongShuBeforeDamageObserver(long id) => _resolve().HasKongShuBeforeDamageObserver(id);
    internal bool HasKongShuDamageObserver(long id) => _resolve().HasKongShuDamageObserver(id);
    internal bool IsIssuedLeiluanCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedLeiluanCandidate(candidate, context);
    internal bool IsKongShuDying() => _resolve().IsKongShuDying();
    internal bool IsKongShuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsKongShuMovement(f, e, p);
    internal bool IsKongShuPaidMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsKongShuPaidMovement(f, m);
    internal bool IsKongShuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsKongShuRecoveryParent(f, recovery);
    internal bool IsLeiluanSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().IsLeiluanSelection(selection);
    internal bool IsLeiluanSource(global::CardGame.Core.CardConversionSource? source) => _resolve().IsLeiluanSource(source);
    internal void IssueKongShuDraw(global::CardGame.Core.ProgramSkillFrame f, int payer, int count, global::CardGame.Core.KongShuStage stage) => _resolve().IssueKongShuDraw(f, payer, count, stage);
    internal void IssueKongShuMovement(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids, bool discard) => _resolve().IssueKongShuMovement(f, ids, discard);
    internal global::CardGame.Core.LeiluanNativePayment? IssuedLeiluanPayment(global::CardGame.Core.CardActionContext action) => _resolve().IssuedLeiluanPayment(action);
    internal global::CardGame.Core.PromptChoice KongShuAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().KongShuAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KongShuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KongShuChoices(f);
    internal bool KongShuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().KongShuFirstChild(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<int> KongShuLivingChained() => _resolve().KongShuLivingChained();
    internal global::CardGame.Core.ProgramSkillFrame? KongShuObserverRoot() => _resolve().KongShuObserverRoot();
    internal bool LeiluanActionMatches(global::CardGame.Core.LeiluanNativePayment p, global::CardGame.Core.CardActionContext action) => _resolve().LeiluanActionMatches(p, action);
    internal bool LeiluanAlcoholAllowed(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().LeiluanAlcoholAllowed(owner, cards);
    internal bool LeiluanDisabled(int owner, string state) => _resolve().LeiluanDisabled(owner, state);
    internal bool LeiluanNameUsed(int owner, global::CardGame.Core.CardKind kind) => _resolve().LeiluanNameUsed(owner, kind);
    internal bool LeiluanNativeDirection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool response) => _resolve().LeiluanNativeDirection(owner, kind, response);
    internal global::System.Collections.Generic.IReadOnlyList<int> LeiluanPlayTargets(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().LeiluanPlayTargets(owner, selection);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> LeiluanSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool forResponse, bool ignoreSuitUseProhibition = false) => _resolve().LeiluanSelections(owner, kind, forResponse, ignoreSuitUseProhibition);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> LeiluanTrickCandidates(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LeiluanTrickCandidates(f);
    internal global::CardGame.Core.LeiluanNativePayment PrepareLeiluanPayment(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, bool response, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs) => _resolve().PrepareLeiluanPayment(use, owner, source, kind, response, costs);
    internal void PrepareLeiluanResponsePayment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, long nativeUseId) => _resolve().PrepareLeiluanResponsePayment(owner, selection, nativeUseId);
    internal void PublishKongShu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishKongShu(f);
    internal bool ResumeKongShu(long id) => _resolve().ResumeKongShu(id);
    internal bool ReturnKongShuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnKongShuMovement(f);
    internal bool TryExecuteLeiluanPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction selected, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryExecuteLeiluanPlay(actor, selected, ids, targets);
    internal bool TryFinishLeiluanAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card) => _resolve().TryFinishLeiluanAlcoholCost(use, card);
    internal bool TryResolveLeiluanDyingResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveLeiluanDyingResponse(owner, selection);
    internal bool TrySubmitLeiluanPlay(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitLeiluanPlay(command, out result);
    internal void UpdateLeiluanPayment(long useId, global::CardGame.Core.LeiluanNativePayment payment) => _resolve().UpdateLeiluanPayment(useId, payment);
    internal global::CardGame.Core.CommandError? ValidateLeiluanPlay(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ValidateLeiluanPlay(owner, action, ids, targets);
    internal bool HasKongShuCapability { get => _resolve().HasKongShuCapability; }
    internal bool HasLeiluanCapability { get => _resolve().HasLeiluanCapability; }
    internal int LeiluanCurrentX { get => _resolve().LeiluanCurrentX; }
}
