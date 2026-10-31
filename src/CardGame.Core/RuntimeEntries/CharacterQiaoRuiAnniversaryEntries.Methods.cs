#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQiaoRuiAnniversaryEntries
{
    internal bool AdvanceQiaoRuiChildren() => _resolve().AdvanceQiaoRuiChildren();
    internal void AnswerQiaoRui(PromptChoice choice) => _resolve().AnswerQiaoRui(choice);
    internal SkillProgramStepOutcome BeginQiaoRui(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginQiaoRui(supplied, op);
    internal void BindQiaoRuiSlash(CardUseDeclaredEvent e) => _resolve().BindQiaoRuiSlash(e);
    internal bool CanOfferQiaoRui(ProgramTriggerLegalityContext c) => _resolve().CanOfferQiaoRui(c);
    internal void ChangeQiaoRuiMaximum(ProgramSkillFrame f, int amount, long lossBatch) => _resolve().ChangeQiaoRuiMaximum(f, amount, lossBatch);
    internal void ContinueQiaoRui(long id) => _resolve().ContinueQiaoRui(id);
    internal void FinishQiaoRui(ProgramSkillFrame f) => _resolve().FinishQiaoRui(f);
    internal bool IsAnniversaryQiaoRuiSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => _resolve().IsAnniversaryQiaoRuiSettlementBatch(f, b);
    internal bool IsQiaoRuiNativeUse(CardUseFrame use) => _resolve().IsQiaoRuiNativeUse(use);
    internal void IssueQiaoRuiSlash(ProgramSkillFrame f) => _resolve().IssueQiaoRuiSlash(f);
    internal bool KeepQiaoRuiPaid(ProgramSkillFrame f) => _resolve().KeepQiaoRuiPaid(f);
    internal void ObserveQiaoRuiDamage(DamageAppliedEvent e) => _resolve().ObserveQiaoRuiDamage(e);
    internal void PublishQiaoRui(ProgramSkillFrame f) => _resolve().PublishQiaoRui(f);
    internal PromptChoice QiaoRuiAi(PendingDecision p) => _resolve().QiaoRuiAi(p);
    internal PromptChoice QiaoRuiChoice(ProgramSkillFrame f, string verb, string value, string text, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().QiaoRuiChoice(f, verb, value, text, cards, targets);
    internal IReadOnlyList<PromptChoice> QiaoRuiChoices(ProgramSkillFrame f) => _resolve().QiaoRuiChoices(f);
    internal bool QiaoRuiDying() => _resolve().QiaoRuiDying();
    internal bool QiaoRuiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().QiaoRuiFirstChild(f, child);
    internal bool QiaoRuiLastLoss(int owner, CardMovementBatchContext b) => _resolve().QiaoRuiLastLoss(owner, b);
    internal int[] QiaoRuiMarkedHand(int owner, long cutoff) => _resolve().QiaoRuiMarkedHand(owner, cutoff);
    internal bool QiaoRuiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().QiaoRuiMovement(f, effect, p);
    internal bool QiaoRuiNativeEdge(int child) => _resolve().QiaoRuiNativeEdge(child);
    internal ProgramSkillFrame? QiaoRuiObserverRoot() => _resolve().QiaoRuiObserverRoot();
    internal bool QiaoRuiObserverSuffix(int i) => _resolve().QiaoRuiObserverSuffix(i);
    internal bool QiaoRuiOriginal(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().QiaoRuiOriginal(f, op);
    internal CardUseFrame? QiaoRuiOriginalSlash(ProgramSkillWindowContext? w) => _resolve().QiaoRuiOriginalSlash(w);
    internal IReadOnlyList<AnniversaryQiaoRuiPlan> QiaoRuiPlans(ProgramSkillFrame f) => _resolve().QiaoRuiPlans(f);
    internal IReadOnlyList<AnniversaryQiaoRuiPlan> QiaoRuiPlans2(int ownerSeat, CardConversionSource source, IReadOnlyList<int> originalTargets) => _resolve().QiaoRuiPlans(ownerSeat, source, originalTargets);
    internal bool QiaoRuiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().QiaoRuiRecoveryParent(f, recovery);
    internal bool ResumeQiaoRui(long id) => _resolve().ResumeQiaoRui(id);
    internal bool ReturnQiaoRuiAttack() => _resolve().ReturnQiaoRuiAttack();
    internal void ReturnQiaoRuiDraw(ProgramSkillFrame supplied, NativeDrawOutcome d) => _resolve().ReturnQiaoRuiDraw(supplied, d);
    internal bool ReturnQiaoRuiMovement(ProgramSkillFrame f) => _resolve().ReturnQiaoRuiMovement(f);
    internal void ReturnQiaoRuiUse(CardUseFrame use) => _resolve().ReturnQiaoRuiUse(use);
    internal void SettleQiaoRuiUse(ProgramSkillFrame f) => _resolve().SettleQiaoRuiUse(f);
    internal bool ValidQiaoRui(ProgramSkillFrame f) => _resolve().ValidQiaoRui(f);
    internal void ValidateQiaoRuiNativeReturn(CardUseFrame use) => _resolve().ValidateQiaoRuiNativeReturn(use);
    internal bool WindowSettlement_IsAnniversaryQiaoRuiSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryQiaoRuiSettlementBatch(frame, batch);
    internal bool HasAnniversaryQiaoRuiCapability { get => _resolve().HasAnniversaryQiaoRuiCapability; }
}
