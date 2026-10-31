#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanYuOlEntries
{
    internal bool AdvanceRuanYuChildren() => _resolve().AdvanceRuanYuChildren();
    internal void AnswerRuanYu(PromptChoice supplied) => _resolve().AnswerRuanYu(supplied);
    internal void AppendRuanYuXingzuoEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendRuanYuXingzuoEndingItems(items);
    internal SkillProgramStepOutcome BeginRuanYu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginRuanYu(supplied, op);
    internal bool CanOfferRuanYu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferRuanYu(c, t, context);
    internal void CaptureMovementBatchStartRuanYuMiaoxianPaymentSources(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartRuanYuMiaoxianPaymentSources(batch);
    internal void CaptureRuanYuHpLoss(ProgramSkillHpLostEvent paid) => _resolve().CaptureRuanYuHpLoss(paid);
    internal void CaptureRuanYuMiaoxianPaymentBatch(CardMovementBatchContext batch, IReadOnlyList<RuanYuMiaoxianPaymentSource>? snapshots) => _resolve().CaptureRuanYuMiaoxianPaymentBatch(batch, snapshots);
    internal IReadOnlyList<RuanYuMiaoxianPaymentSource>? CaptureRuanYuMiaoxianPaymentSources(IEnumerable<CardLocation> locations) => _resolve().CaptureRuanYuMiaoxianPaymentSources(locations);
    internal PromptChoice ChooseRuanYu(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseRuanYu(p, f);
    internal CardMovementBatchContext CompleteMovementBatchCaptureRuanYuMiaoxianPaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureRuanYuMiaoxianPaymentBatch(batch, completed);
    internal void CompleteRuanYuMovement(long id, int count) => _resolve().CompleteRuanYuMovement(id, count);
    internal bool ContinuePaidRuanYu(ProgramSkillFrame f) => _resolve().ContinuePaidRuanYu(f);
    internal void ContinueRuanYu(long id) => _resolve().ContinueRuanYu(id);
    internal XingzuoEndingIssuedEvent? CurrentRuanYuEndingDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentRuanYuEndingDebt(c, context);
    internal IEnumerable<XingzuoEndingIssuedEvent> CurrentXingzuoDebts() => _resolve().CurrentXingzuoDebts();
    internal bool DrainRuanYu(ProgramSkillFrame f) => _resolve().DrainRuanYu(f);
    internal void FinishRuanYu(ProgramSkillFrame f) => _resolve().FinishRuanYu(f);
    internal bool HasRuanYuRedPayment(CardActionContext action, CardConversionSource source, string hash, long parentId) => _resolve().HasRuanYuRedPayment(action, source, hash, parentId);
    internal IReadOnlyList<ProgramCardTriggerCandidate> IncludeRuanYuMiaoxianRedCandidates(IReadOnlyList<ProgramCardTriggerCandidate> ordinary, CardActionContext action, SkillProgramTriggerWindow window) => _resolve().IncludeRuanYuMiaoxianRedCandidates(ordinary, action, window);
    internal bool IsIssuedRuanYuMiaoxianRedCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedRuanYuMiaoxianRedCandidate(c, context);
    internal bool IsIssuedRuanYuXingzuoEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedRuanYuXingzuoEndingCandidate(c, context);
    internal bool IsRuanYuMiaoxianBlackAvailable(CharacterState owner, Card card, CardConversionSource source, bool forResponse) => _resolve().IsRuanYuMiaoxianBlackAvailable(owner, card, source, forResponse);
    internal bool IsRuanYuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsRuanYuMovement(f, effect, pending);
    internal void MoveRuanYuExchange(ProgramSkillFrame f, IReadOnlyList<RuanYuEntity> entities, IReadOnlyList<int> desired) => _resolve().MoveRuanYuExchange(f, entities, desired);
    internal void ObserveRuanYuMiaoxianBlack(IGameEvent payload) => _resolve().ObserveRuanYuMiaoxianBlack(payload);
    internal void PayRuanYuEnding(ProgramSkillFrame f, int recipient) => _resolve().PayRuanYuEnding(f, recipient);
    internal void PayRuanYuFront(ProgramSkillFrame f) => _resolve().PayRuanYuFront(f);
    internal void PrepareRuanYuMovement(ref ProgramSkillFrame f, RuanYuStage stage, IReadOnlyList<RuanYuEntity> entities) => _resolve().PrepareRuanYuMovement(ref f, stage, entities);
    internal void PublishRuanYu(ProgramSkillFrame f) => _resolve().PublishRuanYu(f);
    internal void RecordRuanYuMovement(ProgramSkillFrame f, long batch, IReadOnlyList<CardMovementRecord> records) => _resolve().RecordRuanYuMovement(f, batch, records);
    internal bool ResumeRuanYu(long id) => _resolve().ResumeRuanYu(id);
    internal bool ReturnRuanYuMovement(ProgramSkillFrame f) => _resolve().ReturnRuanYuMovement(f);
    internal bool RuanYuActualMaterial(MiaoxianPaymentQualifiedEvent paid, CardActionContext action, long parentId) => _resolve().RuanYuActualMaterial(paid, action, parentId);
    internal bool RuanYuActualUse(CardActionContext action) => _resolve().RuanYuActualUse(action);
    internal PromptChoice RuanYuChoice(ProgramSkillFrame f, string branch, string label, int? card = default, int? seat = default) => _resolve().RuanYuChoice(f, branch, label, card, seat);
    internal IReadOnlyList<PromptChoice> RuanYuChoices(ProgramSkillFrame f) => _resolve().RuanYuChoices(f);
    internal int[] RuanYuDraftBottom(RuanYuReceipt r) => _resolve().RuanYuDraftBottom(r);
    internal int[] RuanYuEndingTargets() => _resolve().RuanYuEndingTargets();
    internal bool RuanYuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().RuanYuFirstChild(f, child);
    internal bool RuanYuMiaoxianConversionUnavailable(ProgramViewAsSourceQuery query) => _resolve().RuanYuMiaoxianConversionUnavailable(query);
    internal ProgramSkillFrame? RuanYuObserverRoot() => _resolve().RuanYuObserverRoot();
    internal IReadOnlyList<CardSnapshot> RuanYuPrivatelyViewedBottomCards(int viewerSeat) => _resolve().RuanYuPrivatelyViewedBottomCards(viewerSeat);
    internal bool ValidRuanYu(ProgramSkillFrame f) => _resolve().ValidRuanYu(f);
    internal bool HasRuanYuCapability { get => _resolve().HasRuanYuCapability; }
}
