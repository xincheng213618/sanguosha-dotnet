#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanFuOlEntries
{
    internal bool AdvanceHanFuSubtree() => _resolve().AdvanceHanFuSubtree();
    internal global::CardGame.Core.PromptChoice AiHanFu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHanFu(p, f);
    internal void AnswerHanFu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHanFu(choice);
    internal void AssertHanFu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertHanFu(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHanFu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHanFu(supplied, op);
    internal void ContinueHanFu(long id) => _resolve().ContinueHanFu(id);
    internal bool ContinuePaidHanFu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHanFu(f);
    internal bool DrainHanFuChildren(long id) => _resolve().DrainHanFuChildren(id);
    internal void FinishHanFu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHanFu(f);
    internal void FreezeKuangshouDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FreezeKuangshouDiscard(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HanFuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HanFuChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.HanFuMaterial> HanFuDiscardCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HanFuDiscardCards(f);
    internal bool HanFuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HanFuFirstChild(f, child);
    internal bool HanFuLionProof(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.CardMoveReason? reason) => _resolve().HanFuLionProof(f, seat, reason);
    internal global::CardGame.Core.HanFuMaterial HanFuMaterialFor(int id) => _resolve().HanFuMaterialFor(id);
    internal global::CardGame.Core.ProgramSkillFrame? HanFuObserverRoot() => _resolve().HanFuObserverRoot();
    internal bool HasHanFuBeforeDamageObserver(long id) => _resolve().HasHanFuBeforeDamageObserver(id);
    internal bool HasHanFuDamageObserver(long id) => _resolve().HasHanFuDamageObserver(id);
    internal bool HasPaidShuziInitialSelection(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasPaidShuziInitialSelection(f);
    internal bool IsHanFuDying() => _resolve().IsHanFuDying();
    internal bool IsHanFuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsHanFuMovement(f, e, p);
    internal bool IsHanFuRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsHanFuRecoveryParent(f, recovery);
    internal int[] KuangshouDamagedSeats(int cutoff) => _resolve().KuangshouDamagedSeats(cutoff);
    internal void PayHanFuCards(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.HanFuMaterial> cards, global::CardGame.Core.CardLocation destination, string reason, global::CardGame.Core.HanFuStage stage) => _resolve().PayHanFuCards(f, cards, destination, reason, stage);
    internal void PayShuziBoard(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.HanFuMaterial m) => _resolve().PayShuziBoard(f, m);
    internal void PublishHanFuChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHanFuChoice(f);
    internal void RecordHanFuMovement(long id, global::CardGame.Core.HanFuStage stage, string reason, long before, int actual) => _resolve().RecordHanFuMovement(id, stage, reason, before, actual);
    internal bool ResumeHanFu(long id) => _resolve().ResumeHanFu(id);
    internal bool ReturnHanFuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHanFuMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.HanFuMaterial> ShuziBoardCards(int target) => _resolve().ShuziBoardCards(target);
}
