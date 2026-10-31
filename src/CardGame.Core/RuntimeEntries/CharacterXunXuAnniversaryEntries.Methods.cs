#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunXuAnniversaryEntries
{
    internal bool AdvanceXunXuChildren() => _resolve().AdvanceXunXuChildren();
    internal void AnswerXunXu(PromptChoice choice) => _resolve().AnswerXunXu(choice);
    internal bool BanUnavailableXunXuConversion(ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableXunXuConversion(q);
    internal SkillProgramStepOutcome BeginXunXu(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginXunXu(supplied, operation);
    internal bool CanOfferXunXu(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferXunXu(c, trigger, context);
    internal void CaptureAnniversaryXunXuMovement(CardMovementBatchContext batch) => _resolve().CaptureAnniversaryXunXuMovement(batch);
    internal PromptChoice ChooseXunXuBoqia(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseXunXuBoqia(p, f);
    internal PromptChoice ChooseXunXuDiyin(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseXunXuDiyin(p, f);
    internal CardMovementBatchContext CompleteMovementBatchCaptureAnniversaryXunXuMovement(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureAnniversaryXunXuMovement(batch, completed);
    internal bool ContinueXunXu(long id) => _resolve().ContinueXunXu(id);
    internal bool DrainXunXu(ProgramSkillFrame f) => _resolve().DrainXunXu(f);
    internal bool ExecuteXunXuPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ExecuteXunXuPlay(owner, action, ids, targets);
    internal IReadOnlyList<LegalAction> FilterXunXuNativeDuplicates(CharacterState actor, IReadOnlyList<LegalAction> actions) => _resolve().FilterXunXuNativeDuplicates(actor, actions);
    internal void FinishXunXu(ProgramSkillFrame f) => _resolve().FinishXunXu(f);
    internal bool HasXunXuNativeFinishOwner(ProgramSkillFrame f, scoped out CardUseFrame? use) => _resolve().HasXunXuNativeFinishOwner(f, out use);
    internal bool IsAnniversaryXunXuBoqiaSource(CardConversionSource? s) => _resolve().IsAnniversaryXunXuBoqiaSource(s);
    internal bool IsAnniversaryXunXuSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => _resolve().IsAnniversaryXunXuSettlementBatch(f, b);
    internal bool IsXunXuFinishedMaterialBatch(CardUseFrame use, CardMovementBatchContext b) => _resolve().IsXunXuFinishedMaterialBatch(use, b);
    internal void PublishXunXu(ProgramSkillFrame f) => _resolve().PublishXunXu(f);
    internal void RecordActualXunXuUse(IGameEvent payload) => _resolve().RecordActualXunXuUse(payload);
    internal void RecordXunXuMovement(long id, string mode, int target, int count, long before) => _resolve().RecordXunXuMovement(id, mode, target, count, before);
    internal bool RetainXunXu(ProgramSkillFrame f) => _resolve().RetainXunXu(f);
    internal void ReturnXunXuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnXunXuDraw(f, outcome);
    internal bool ReturnXunXuMovement(ProgramSkillFrame f) => _resolve().ReturnXunXuMovement(f);
    internal int[]? SelectXunXuDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate c, ProgramInstructionFeatures features) => _resolve().SelectXunXuDiscardIndexes(batch, c, features);
    internal bool SubmitXunXuPlay(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().SubmitXunXuPlay(command, out result);
    internal bool TryFinishAnniversaryXunXuAcceptedResponse(CardActionContext action) => _resolve().TryFinishAnniversaryXunXuAcceptedResponse(action);
    internal bool TryFinishAnniversaryXunXuAlcoholCost(CardUseFrame use, Card card) => _resolve().TryFinishAnniversaryXunXuAlcoholCost(use, card);
    internal bool TryFinishAnniversaryXunXuResponseMaterials(ProgramMultiCardViewAsSelection selection) => _resolve().TryFinishAnniversaryXunXuResponseMaterials(selection);
    internal bool TryFinishAnniversaryXunXuUseMaterials(CardUseFrame use) => _resolve().TryFinishAnniversaryXunXuUseMaterials(use);
    internal bool TryResolveAnniversaryXunXuDyingAlcohol(CharacterState owner, ProgramMultiCardViewAsSelection s) => _resolve().TryResolveAnniversaryXunXuDyingAlcohol(owner, s);
    internal CommandError? ValidateXunXuPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ValidateXunXuPlay(owner, action, ids, targets);
    internal bool WindowSettlement_IsAnniversaryXunXuSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryXunXuSettlementBatch(frame, batch);
    internal bool XunXuAllowance(int owner, CardKind kind) => _resolve().XunXuAllowance(owner, kind);
    internal IReadOnlyList<PromptChoice> XunXuChoices(ProgramSkillFrame f) => _resolve().XunXuChoices(f);
    internal int[] XunXuDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().XunXuDiscardIndexes(batch, candidate);
    internal bool XunXuDyingObserver() => _resolve().XunXuDyingObserver();
    internal bool XunXuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().XunXuFirstChild(f, child);
    internal bool XunXuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().XunXuMovement(f, effect, pending);
    internal ProgramSkillFrame? XunXuObserverRoot() => _resolve().XunXuObserverRoot();
    internal IEnumerable<LegalAction> XunXuPlayActions(CharacterState owner) => _resolve().XunXuPlayActions(owner);
    internal IReadOnlyList<GameEngine.XunXuUsePlan> XunXuPlayPlans(CharacterState owner, ProgramMultiCardViewAsSelection s) => _resolve().XunXuPlayPlans(owner, s);
    internal bool XunXuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().XunXuRecoveryParent(f, recovery);
    internal bool XunXuUseDirection(CharacterState owner, CardKind kind, bool response, bool dying) => _resolve().XunXuUseDirection(owner, kind, response, dying);
    internal bool HasAnniversaryXunXu { get => _resolve().HasAnniversaryXunXu; }
}
