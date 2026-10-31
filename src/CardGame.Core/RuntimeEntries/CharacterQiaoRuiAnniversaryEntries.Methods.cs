#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQiaoRuiAnniversaryEntries
{
    internal bool AdvanceQiaoRuiChildren() => _resolve().AdvanceQiaoRuiChildren();
    internal void AnswerQiaoRui(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerQiaoRui(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginQiaoRui(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginQiaoRui(supplied, op);
    internal void BindQiaoRuiSlash(global::CardGame.Core.CardUseDeclaredEvent e) => _resolve().BindQiaoRuiSlash(e);
    internal bool CanOfferQiaoRui(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferQiaoRui(c);
    internal void ChangeQiaoRuiMaximum(global::CardGame.Core.ProgramSkillFrame f, int amount, long lossBatch) => _resolve().ChangeQiaoRuiMaximum(f, amount, lossBatch);
    internal void ContinueQiaoRui(long id) => _resolve().ContinueQiaoRui(id);
    internal void FinishQiaoRui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishQiaoRui(f);
    internal bool IsAnniversaryQiaoRuiSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext b) => _resolve().IsAnniversaryQiaoRuiSettlementBatch(f, b);
    internal bool IsQiaoRuiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsQiaoRuiNativeUse(use);
    internal void IssueQiaoRuiSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueQiaoRuiSlash(f);
    internal bool KeepQiaoRuiPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepQiaoRuiPaid(f);
    internal void ObserveQiaoRuiDamage(global::CardGame.Core.DamageAppliedEvent e) => _resolve().ObserveQiaoRuiDamage(e);
    internal void PublishQiaoRui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishQiaoRui(f);
    internal global::CardGame.Core.PromptChoice QiaoRuiAi(global::CardGame.Core.PendingDecision p) => _resolve().QiaoRuiAi(p);
    internal bool QiaoRuiBeforeDamage(long id) => _resolve().QiaoRuiBeforeDamage(id);
    internal global::CardGame.Core.PromptChoice QiaoRuiChoice(global::CardGame.Core.ProgramSkillFrame f, string verb, string value, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().QiaoRuiChoice(f, verb, value, text, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QiaoRuiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QiaoRuiChoices(f);
    internal bool QiaoRuiDamageObserver(long id) => _resolve().QiaoRuiDamageObserver(id);
    internal bool QiaoRuiDying() => _resolve().QiaoRuiDying();
    internal bool QiaoRuiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().QiaoRuiFirstChild(f, child);
    internal bool QiaoRuiLastLoss(int owner, global::CardGame.Core.CardMovementBatchContext b) => _resolve().QiaoRuiLastLoss(owner, b);
    internal int[] QiaoRuiMarkedHand(int owner, long cutoff) => _resolve().QiaoRuiMarkedHand(owner, cutoff);
    internal bool QiaoRuiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().QiaoRuiMovement(f, effect, p);
    internal bool QiaoRuiNativeEdge(int child) => _resolve().QiaoRuiNativeEdge(child);
    internal global::CardGame.Core.ProgramSkillFrame? QiaoRuiObserverRoot() => _resolve().QiaoRuiObserverRoot();
    internal bool QiaoRuiObserverSuffix(int i) => _resolve().QiaoRuiObserverSuffix(i);
    internal bool QiaoRuiOriginal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().QiaoRuiOriginal(f, op);
    internal global::CardGame.Core.CardUseFrame? QiaoRuiOriginalSlash(global::CardGame.Core.ProgramSkillWindowContext? w) => _resolve().QiaoRuiOriginalSlash(w);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.AnniversaryQiaoRuiPlan> QiaoRuiPlans(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QiaoRuiPlans(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.AnniversaryQiaoRuiPlan> QiaoRuiPlans2(int ownerSeat, global::CardGame.Core.CardConversionSource source, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => _resolve().QiaoRuiPlans(ownerSeat, source, originalTargets);
    internal bool QiaoRuiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().QiaoRuiRecoveryParent(f, recovery);
    internal bool ResumeQiaoRui(long id) => _resolve().ResumeQiaoRui(id);
    internal bool ReturnQiaoRuiAttack() => _resolve().ReturnQiaoRuiAttack();
    internal void ReturnQiaoRuiDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome d) => _resolve().ReturnQiaoRuiDraw(supplied, d);
    internal bool ReturnQiaoRuiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnQiaoRuiMovement(f);
    internal void ReturnQiaoRuiUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnQiaoRuiUse(use);
    internal void SettleQiaoRuiUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleQiaoRuiUse(f);
    internal bool ValidQiaoRui(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidQiaoRui(f);
    internal void ValidateQiaoRuiNativeReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateQiaoRuiNativeReturn(use);
    internal bool WindowSettlement_IsAnniversaryQiaoRuiSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryQiaoRuiSettlementBatch(frame, batch);
    internal bool HasAnniversaryQiaoRuiCapability { get => _resolve().HasAnniversaryQiaoRuiCapability; }
}
