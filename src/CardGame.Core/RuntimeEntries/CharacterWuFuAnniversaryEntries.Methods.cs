#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuFuAnniversaryEntries
{
    internal bool AdvanceWuFuChildren() => _resolve().AdvanceWuFuChildren();
    internal void AnswerWuFu(PromptChoice choice) => _resolve().AnswerWuFu(choice);
    internal SkillProgramStepOutcome BeginWuFu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginWuFu(supplied, op);
    internal void BindWuFuSlash(IGameEvent fact) => _resolve().BindWuFuSlash(fact);
    internal bool CanWuFuSlash(int actor, int target) => _resolve().CanWuFuSlash(actor, target);
    internal void CaptureWuFuPhaseGain(IGameEvent fact) => _resolve().CaptureWuFuPhaseGain(fact);
    internal PromptChoice ChooseWuFu(PendingDecision p) => _resolve().ChooseWuFu(p);
    internal void CompleteWuFuItem(ProgramSkillFrame f, bool used) => _resolve().CompleteWuFuItem(f, used);
    internal void CompleteWuFuSlash(long id) => _resolve().CompleteWuFuSlash(id);
    internal void ContinueWuFu(long id) => _resolve().ContinueWuFu(id);
    internal bool DrainWuFu(ProgramSkillFrame f) => _resolve().DrainWuFu(f);
    internal void FinishWuFu(ProgramSkillFrame f) => _resolve().FinishWuFu(f);
    internal bool FinishWuFuAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishWuFuAttack(attack, out paused);
    internal bool IsAnniversaryWuFuHandUseForbidden(int seat, IReadOnlyList<int> ids) => _resolve().IsAnniversaryWuFuHandUseForbidden(seat, ids);
    internal bool IsAnniversaryWuFuSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => _resolve().IsAnniversaryWuFuSettlementBatch(f, b);
    internal bool IsWuFuUse(CardUseFrame use) => _resolve().IsWuFuUse(use);
    internal void IssueWuFuSlash(ProgramSkillFrame f) => _resolve().IssueWuFuSlash(f);
    internal void PublishWuFu(ProgramSkillFrame f) => _resolve().PublishWuFu(f);
    internal bool ResumeWuFu(long id) => _resolve().ResumeWuFu(id);
    internal bool RetainWuFu(ProgramSkillFrame f) => _resolve().RetainWuFu(f);
    internal bool ReturnWuFuAttack(GameEngine.AttackCompletionReceipt receipt) => _resolve().ReturnWuFuAttack(receipt);
    internal void ReturnWuFuDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnWuFuDraw(f, o);
    internal bool ReturnWuFuMovement(ProgramSkillFrame f) => _resolve().ReturnWuFuMovement(f);
    internal void ReturnWuFuUse(CardUseFrame use) => _resolve().ReturnWuFuUse(use);
    internal bool ValidWuFu(ProgramSkillFrame f) => _resolve().ValidWuFu(f);
    internal void ValidateWuFuUse(CardUseFrame use) => _resolve().ValidateWuFuUse(use);
    internal bool WindowSettlement_IsAnniversaryWuFuSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryWuFuSettlementBatch(frame, batch);
    internal IReadOnlyList<PromptChoice> WuFuChoices(ProgramSkillFrame f) => _resolve().WuFuChoices(f);
    internal int WuFuChooser(ProgramSkillFrame f) => _resolve().WuFuChooser(f);
    internal bool WuFuDying() => _resolve().WuFuDying();
    internal bool WuFuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WuFuFirstChild(f, child);
    internal bool WuFuLegalUseBan(ProgramLegalActionQuery q) => _resolve().WuFuLegalUseBan(q);
    internal bool WuFuMaterialBan(ProgramCardMaterialQuery q) => _resolve().WuFuMaterialBan(q);
    internal bool WuFuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().WuFuMovement(f, e, p);
    internal bool WuFuNativeEdge(int i) => _resolve().WuFuNativeEdge(i);
    internal bool WuFuNativeResponseBan(ProgramNativeResponseQuery q) => _resolve().WuFuNativeResponseBan(q);
    internal bool WuFuObserverEdge(int i) => _resolve().WuFuObserverEdge(i);
    internal ProgramSkillFrame? WuFuObserverRoot() => _resolve().WuFuObserverRoot();
    internal bool WuFuPhaseOpen(ActualDiscardRecoveryPhaseKey phase) => _resolve().WuFuPhaseOpen(phase);
    internal bool WuFuResponseIsUse(int actor, CardKind kind, bool dying = false) => _resolve().WuFuResponseIsUse(actor, kind, dying);
    internal void WuFuSubmittedUseBan(ProgramSubmittedUseQuery q) => _resolve().WuFuSubmittedUseBan(q);
    internal AnniversaryWuFuTask WuFuTask(ProgramSkillFrame f) => _resolve().WuFuTask(f);
    internal AnniversaryWuFuTask[] WuFuTasks(ProgramSkillFrame f) => _resolve().WuFuTasks(f);
    internal bool WuFuViewAsBan(ProgramViewAsLegalityQuery q) => _resolve().WuFuViewAsBan(q);
}
