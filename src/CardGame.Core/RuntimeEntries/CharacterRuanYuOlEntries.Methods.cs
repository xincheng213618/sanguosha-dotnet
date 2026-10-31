#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanYuOlEntries
{
    internal bool AdvanceRuanYuChildren() => _resolve().AdvanceRuanYuChildren();
    internal void AnswerRuanYu(global::CardGame.Core.PromptChoice supplied) => _resolve().AnswerRuanYu(supplied);
    internal void AppendRuanYuXingzuoEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendRuanYuXingzuoEndingItems(items);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRuanYu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginRuanYu(supplied, op);
    internal bool CanOfferRuanYu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferRuanYu(c, t, context);
    internal void CaptureMovementBatchStartRuanYuMiaoxianPaymentSources(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartRuanYuMiaoxianPaymentSources(batch);
    internal void CaptureRuanYuHpLoss(global::CardGame.Core.ProgramSkillHpLostEvent paid) => _resolve().CaptureRuanYuHpLoss(paid);
    internal void CaptureRuanYuMiaoxianPaymentBatch(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuanYuMiaoxianPaymentSource>? snapshots) => _resolve().CaptureRuanYuMiaoxianPaymentBatch(batch, snapshots);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuanYuMiaoxianPaymentSource>? CaptureRuanYuMiaoxianPaymentSources(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> locations) => _resolve().CaptureRuanYuMiaoxianPaymentSources(locations);
    internal global::CardGame.Core.PromptChoice ChooseRuanYu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseRuanYu(p, f);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureRuanYuMiaoxianPaymentBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureRuanYuMiaoxianPaymentBatch(batch, completed);
    internal void CompleteRuanYuMovement(long id, int count) => _resolve().CompleteRuanYuMovement(id, count);
    internal bool ContinuePaidRuanYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidRuanYu(f);
    internal void ContinueRuanYu(long id) => _resolve().ContinueRuanYu(id);
    internal global::CardGame.Core.XingzuoEndingIssuedEvent? CurrentRuanYuEndingDebt(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentRuanYuEndingDebt(c, context);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.XingzuoEndingIssuedEvent> CurrentXingzuoDebts() => _resolve().CurrentXingzuoDebts();
    internal bool DrainRuanYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainRuanYu(f);
    internal void FinishRuanYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishRuanYu(f);
    internal bool HasRuanYuRedPayment(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource source, string hash, long parentId) => _resolve().HasRuanYuRedPayment(action, source, hash, parentId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> IncludeRuanYuMiaoxianRedCandidates(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> ordinary, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().IncludeRuanYuMiaoxianRedCandidates(ordinary, action, window);
    internal bool IsIssuedRuanYuMiaoxianRedCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedRuanYuMiaoxianRedCandidate(c, context);
    internal bool IsIssuedRuanYuXingzuoEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedRuanYuXingzuoEndingCandidate(c, context);
    internal bool IsRuanYuMiaoxianBlackAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource source, bool forResponse) => _resolve().IsRuanYuMiaoxianBlackAvailable(owner, card, source, forResponse);
    internal bool IsRuanYuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsRuanYuMovement(f, effect, pending);
    internal void MoveRuanYuExchange(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuanYuEntity> entities, global::System.Collections.Generic.IReadOnlyList<int> desired) => _resolve().MoveRuanYuExchange(f, entities, desired);
    internal void ObserveRuanYuMiaoxianBlack(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveRuanYuMiaoxianBlack(payload);
    internal void PayRuanYuEnding(global::CardGame.Core.ProgramSkillFrame f, int recipient) => _resolve().PayRuanYuEnding(f, recipient);
    internal void PayRuanYuFront(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayRuanYuFront(f);
    internal void PrepareRuanYuMovement(ref global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RuanYuStage stage, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.RuanYuEntity> entities) => _resolve().PrepareRuanYuMovement(ref f, stage, entities);
    internal void PublishRuanYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishRuanYu(f);
    internal void RecordRuanYuMovement(global::CardGame.Core.ProgramSkillFrame f, long batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> records) => _resolve().RecordRuanYuMovement(f, batch, records);
    internal bool ResumeRuanYu(long id) => _resolve().ResumeRuanYu(id);
    internal bool ReturnRuanYuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnRuanYuMovement(f);
    internal bool RuanYuActualMaterial(global::CardGame.Core.MiaoxianPaymentQualifiedEvent paid, global::CardGame.Core.CardActionContext action, long parentId) => _resolve().RuanYuActualMaterial(paid, action, parentId);
    internal bool RuanYuActualUse(global::CardGame.Core.CardActionContext action) => _resolve().RuanYuActualUse(action);
    internal global::CardGame.Core.PromptChoice RuanYuChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? card = default, int? seat = default) => _resolve().RuanYuChoice(f, branch, label, card, seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RuanYuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RuanYuChoices(f);
    internal int[] RuanYuDraftBottom(global::CardGame.Core.RuanYuReceipt r) => _resolve().RuanYuDraftBottom(r);
    internal int[] RuanYuEndingTargets() => _resolve().RuanYuEndingTargets();
    internal bool RuanYuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().RuanYuFirstChild(f, child);
    internal bool RuanYuMiaoxianConversionUnavailable(global::CardGame.Core.ProgramViewAsSourceQuery query) => _resolve().RuanYuMiaoxianConversionUnavailable(query);
    internal global::CardGame.Core.ProgramSkillFrame? RuanYuObserverRoot() => _resolve().RuanYuObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardSnapshot> RuanYuPrivatelyViewedBottomCards(int viewerSeat) => _resolve().RuanYuPrivatelyViewedBottomCards(viewerSeat);
    internal bool ValidRuanYu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidRuanYu(f);
    internal bool HasRuanYuCapability { get => _resolve().HasRuanYuCapability; }
}
