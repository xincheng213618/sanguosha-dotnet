#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHuanAnniversaryEntries
{
    internal bool AdvanceZhangHuanChildren() => _resolve().AdvanceZhangHuanChildren();
    internal global::CardGame.Core.Card? AnniversaryZhangHuanQueryOriginalCard(global::CardGame.Core.CharacterState actor, int? id) => _resolve().AnniversaryZhangHuanQueryOriginalCard(actor, id);
    internal void AnswerZhangHuan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerZhangHuan(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhangHuan(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginZhangHuan(supplied);
    internal void BindZhangHuanUse(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().BindZhangHuanUse(fact);
    internal bool CanOfferZhangHuan(global::CardGame.Core.ProgramTriggerLegalityContext q) => _resolve().CanOfferZhangHuan(q);
    internal bool ContinueZhangHuan(long id) => _resolve().ContinueZhangHuan(id);
    internal bool DrainZhangHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainZhangHuan(f);
    internal void DrawZhangHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrawZhangHuan(f);
    internal void FinishZhangHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishZhangHuan(f);
    internal bool IsAnniversaryZhangHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => _resolve().IsAnniversaryZhangHuanExactCardQuery(actor, selected);
    internal bool IsAnniversaryZhangHuanSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext b) => _resolve().IsAnniversaryZhangHuanSettlementBatch(f, b);
    internal bool IsZhangHuanNative(global::CardGame.Core.CardUseFrame use) => _resolve().IsZhangHuanNative(use);
    internal void PublishZhangHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhangHuan(f);
    internal bool RetainZhangHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainZhangHuan(f);
    internal bool ReturnZhangHuanAttack() => _resolve().ReturnZhangHuanAttack();
    internal void ReturnZhangHuanDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome d) => _resolve().ReturnZhangHuanDraw(supplied, d);
    internal bool ReturnZhangHuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhangHuanMovement(f);
    internal void ReturnZhangHuanUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnZhangHuanUse(use);
    internal void SettleZhangHuanUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleZhangHuanUse(f);
    internal bool ValidZhangHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhangHuan(f);
    internal void ValidateZhangHuanNativeReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateZhangHuanNativeReturn(use);
    internal bool WindowSettlement_IsAnniversaryZhangHuanSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryZhangHuanSettlementBatch(frame, batch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> ZhangHuanActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangHuanActions(f);
    internal global::CardGame.Core.PromptChoice ZhangHuanAi(global::CardGame.Core.PendingDecision p) => _resolve().ZhangHuanAi(p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.AnniversaryZhangHuanDiscard> ZhangHuanAvailable(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangHuanAvailable(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhangHuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangHuanChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.AnniversaryZhangHuanDiscard> ZhangHuanDiscards() => _resolve().ZhangHuanDiscards();
    internal bool ZhangHuanDying() => _resolve().ZhangHuanDying();
    internal bool ZhangHuanEndingSlashDebit(long id, int actor) => _resolve().ZhangHuanEndingSlashDebit(id, actor);
    internal global::System.Collections.Generic.IReadOnlyList<int> ZhangHuanExpectedTargets(global::CardGame.Core.AnniversaryZhangHuanNative n) => _resolve().ZhangHuanExpectedTargets(n);
    internal bool ZhangHuanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhangHuanFirstChild(f, child);
    internal bool ZhangHuanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().ZhangHuanMovement(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? ZhangHuanObserverRoot() => _resolve().ZhangHuanObserverRoot();
    internal bool ZhangHuanObserverSuffix(int i) => _resolve().ZhangHuanObserverSuffix(i);
    internal bool ZhangHuanParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangHuanParent(f);
    internal bool ZhangHuanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ZhangHuanRecoveryParent(f, recovery);
    internal bool ZhangHuanTargetWindows(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().ZhangHuanTargetWindows(q);
}
