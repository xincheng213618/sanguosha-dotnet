#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunXuAnniversaryEntries
{
    internal bool AdvanceXunXuChildren() => _resolve().AdvanceXunXuChildren();
    internal void AnswerXunXu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerXunXu(choice);
    internal bool BanUnavailableXunXuConversion(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableXunXuConversion(q);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXunXu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginXunXu(supplied, operation);
    internal bool CanOfferXunXu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferXunXu(c, trigger, context);
    internal void CaptureAnniversaryXunXuMovement(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureAnniversaryXunXuMovement(batch);
    internal global::CardGame.Core.PromptChoice ChooseXunXuBoqia(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseXunXuBoqia(p, f);
    internal global::CardGame.Core.PromptChoice ChooseXunXuDiyin(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseXunXuDiyin(p, f);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureAnniversaryXunXuMovement(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureAnniversaryXunXuMovement(batch, completed);
    internal bool ContinueXunXu(long id) => _resolve().ContinueXunXu(id);
    internal bool DrainXunXu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainXunXu(f);
    internal bool ExecuteXunXuPlay(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ExecuteXunXuPlay(owner, action, ids, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterXunXuNativeDuplicates(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> actions) => _resolve().FilterXunXuNativeDuplicates(actor, actions);
    internal void FinishXunXu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishXunXu(f);
    internal bool HasXunXuNativeFinishOwner(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.CardUseFrame? use) => _resolve().HasXunXuNativeFinishOwner(f, out use);
    internal bool IsAnniversaryXunXuBoqiaSource(global::CardGame.Core.CardConversionSource? s) => _resolve().IsAnniversaryXunXuBoqiaSource(s);
    internal bool IsAnniversaryXunXuSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext b) => _resolve().IsAnniversaryXunXuSettlementBatch(f, b);
    internal bool IsXunXuFinishedMaterialBatch(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext b) => _resolve().IsXunXuFinishedMaterialBatch(use, b);
    internal void PublishXunXu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishXunXu(f);
    internal void RecordActualXunXuUse(global::CardGame.Core.IGameEvent payload) => _resolve().RecordActualXunXuUse(payload);
    internal void RecordXunXuMovement(long id, string mode, int target, int count, long before) => _resolve().RecordXunXuMovement(id, mode, target, count, before);
    internal bool RetainXunXu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainXunXu(f);
    internal void ReturnXunXuDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnXunXuDraw(f, outcome);
    internal bool ReturnXunXuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXunXuMovement(f);
    internal int[]? SelectXunXuDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().SelectXunXuDiscardIndexes(batch, c, features);
    internal bool SubmitXunXuPlay(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().SubmitXunXuPlay(command, out result);
    internal bool TryFinishAnniversaryXunXuAcceptedResponse(global::CardGame.Core.CardActionContext action) => _resolve().TryFinishAnniversaryXunXuAcceptedResponse(action);
    internal bool TryFinishAnniversaryXunXuAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card) => _resolve().TryFinishAnniversaryXunXuAlcoholCost(use, card);
    internal bool TryFinishAnniversaryXunXuResponseMaterials(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().TryFinishAnniversaryXunXuResponseMaterials(selection);
    internal bool TryFinishAnniversaryXunXuUseMaterials(global::CardGame.Core.CardUseFrame use) => _resolve().TryFinishAnniversaryXunXuUseMaterials(use);
    internal bool TryResolveAnniversaryXunXuDyingAlcohol(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection s) => _resolve().TryResolveAnniversaryXunXuDyingAlcohol(owner, s);
    internal global::CardGame.Core.CommandError? ValidateXunXuPlay(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ValidateXunXuPlay(owner, action, ids, targets);
    internal bool WindowSettlement_IsAnniversaryXunXuSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryXunXuSettlementBatch(frame, batch);
    internal bool XunXuAllowance(int owner, global::CardGame.Core.CardKind kind) => _resolve().XunXuAllowance(owner, kind);
    internal bool XunXuBeforeDamageObserver(long id) => _resolve().XunXuBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XunXuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XunXuChoices(f);
    internal bool XunXuDamageObserver(long id) => _resolve().XunXuDamageObserver(id);
    internal int[] XunXuDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().XunXuDiscardIndexes(batch, candidate);
    internal bool XunXuDyingObserver() => _resolve().XunXuDyingObserver();
    internal bool XunXuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().XunXuFirstChild(f, child);
    internal bool XunXuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().XunXuMovement(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? XunXuObserverRoot() => _resolve().XunXuObserverRoot();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> XunXuPlayActions(global::CardGame.Core.CharacterState owner) => _resolve().XunXuPlayActions(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.XunXuUsePlan> XunXuPlayPlans(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection s) => _resolve().XunXuPlayPlans(owner, s);
    internal bool XunXuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().XunXuRecoveryParent(f, recovery);
    internal bool XunXuUseDirection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool response, bool dying) => _resolve().XunXuUseDirection(owner, kind, response, dying);
    internal bool HasAnniversaryXunXu { get => _resolve().HasAnniversaryXunXu; }
}
