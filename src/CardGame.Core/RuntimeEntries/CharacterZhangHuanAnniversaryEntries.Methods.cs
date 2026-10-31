#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHuanAnniversaryEntries
{
    internal bool AdvanceZhangHuanChildren() => _resolve().AdvanceZhangHuanChildren();
    internal Card? AnniversaryZhangHuanQueryOriginalCard(CharacterState actor, int? id) => _resolve().AnniversaryZhangHuanQueryOriginalCard(actor, id);
    internal void AnswerZhangHuan(PromptChoice choice) => _resolve().AnswerZhangHuan(choice);
    internal SkillProgramStepOutcome BeginZhangHuan(ProgramSkillFrame supplied) => _resolve().BeginZhangHuan(supplied);
    internal void BindZhangHuanUse(CardUseDeclaredEvent fact) => _resolve().BindZhangHuanUse(fact);
    internal bool CanOfferZhangHuan(ProgramTriggerLegalityContext q) => _resolve().CanOfferZhangHuan(q);
    internal bool ContinueZhangHuan(long id) => _resolve().ContinueZhangHuan(id);
    internal bool DrainZhangHuan(ProgramSkillFrame f) => _resolve().DrainZhangHuan(f);
    internal void DrawZhangHuan(ProgramSkillFrame f) => _resolve().DrawZhangHuan(f);
    internal void FinishZhangHuan(ProgramSkillFrame f) => _resolve().FinishZhangHuan(f);
    internal bool IsAnniversaryZhangHuanExactCardQuery(CharacterState actor, int? selected) => _resolve().IsAnniversaryZhangHuanExactCardQuery(actor, selected);
    internal bool IsAnniversaryZhangHuanSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => _resolve().IsAnniversaryZhangHuanSettlementBatch(f, b);
    internal bool IsZhangHuanNative(CardUseFrame use) => _resolve().IsZhangHuanNative(use);
    internal void PublishZhangHuan(ProgramSkillFrame f) => _resolve().PublishZhangHuan(f);
    internal bool RetainZhangHuan(ProgramSkillFrame f) => _resolve().RetainZhangHuan(f);
    internal bool ReturnZhangHuanAttack() => _resolve().ReturnZhangHuanAttack();
    internal void ReturnZhangHuanDraw(ProgramSkillFrame supplied, NativeDrawOutcome d) => _resolve().ReturnZhangHuanDraw(supplied, d);
    internal bool ReturnZhangHuanMovement(ProgramSkillFrame f) => _resolve().ReturnZhangHuanMovement(f);
    internal void ReturnZhangHuanUse(CardUseFrame use) => _resolve().ReturnZhangHuanUse(use);
    internal void SettleZhangHuanUse(ProgramSkillFrame f) => _resolve().SettleZhangHuanUse(f);
    internal bool ValidZhangHuan(ProgramSkillFrame f) => _resolve().ValidZhangHuan(f);
    internal void ValidateZhangHuanNativeReturn(CardUseFrame use) => _resolve().ValidateZhangHuanNativeReturn(use);
    internal bool WindowSettlement_IsAnniversaryZhangHuanSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryZhangHuanSettlementBatch(frame, batch);
    internal IReadOnlyList<LegalAction> ZhangHuanActions(ProgramSkillFrame f) => _resolve().ZhangHuanActions(f);
    internal PromptChoice ZhangHuanAi(PendingDecision p) => _resolve().ZhangHuanAi(p);
    internal IReadOnlyList<AnniversaryZhangHuanDiscard> ZhangHuanAvailable(ProgramSkillFrame f) => _resolve().ZhangHuanAvailable(f);
    internal IReadOnlyList<PromptChoice> ZhangHuanChoices(ProgramSkillFrame f) => _resolve().ZhangHuanChoices(f);
    internal IReadOnlyList<AnniversaryZhangHuanDiscard> ZhangHuanDiscards() => _resolve().ZhangHuanDiscards();
    internal bool ZhangHuanDying() => _resolve().ZhangHuanDying();
    internal bool ZhangHuanEndingSlashDebit(long id, int actor) => _resolve().ZhangHuanEndingSlashDebit(id, actor);
    internal IReadOnlyList<int> ZhangHuanExpectedTargets(AnniversaryZhangHuanNative n) => _resolve().ZhangHuanExpectedTargets(n);
    internal bool ZhangHuanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhangHuanFirstChild(f, child);
    internal bool ZhangHuanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().ZhangHuanMovement(f, effect, pending);
    internal ProgramSkillFrame? ZhangHuanObserverRoot() => _resolve().ZhangHuanObserverRoot();
    internal bool ZhangHuanObserverSuffix(int i) => _resolve().ZhangHuanObserverSuffix(i);
    internal bool ZhangHuanParent(ProgramSkillFrame f) => _resolve().ZhangHuanParent(f);
    internal bool ZhangHuanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ZhangHuanRecoveryParent(f, recovery);
    internal bool ZhangHuanTargetWindows(ProgramTargetCardWindowQuery q) => _resolve().ZhangHuanTargetWindows(q);
}
