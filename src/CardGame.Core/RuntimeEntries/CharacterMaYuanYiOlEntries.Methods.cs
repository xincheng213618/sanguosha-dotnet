#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaYuanYiOlEntries
{
    internal bool AdvanceMaYuanYiSubtree() => _resolve().AdvanceMaYuanYiSubtree();
    internal void AnswerMaYuanYiTarget(global::CardGame.Core.PromptChoice supplied) => _resolve().AnswerMaYuanYiTarget(supplied);
    internal void AssertMaYuanYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMaYuanYi(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMaYuanYi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMaYuanYi(supplied, op);
    internal bool CanBeginBinghuoJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().CanBeginBinghuoJudgment(f, targetSeat, reason, bind, visibility);
    internal bool CanOfferMaYuanYi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMaYuanYi(c, t, context);
    internal global::CardGame.Core.PromptChoice ChooseMaYuanYi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMaYuanYi(p, f);
    internal void ContinueMaYuanYi(long id) => _resolve().ContinueMaYuanYi(id);
    internal bool ContinuePaidMaYuanYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidMaYuanYi(f);
    internal bool DrainMaYuanYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMaYuanYi(f);
    internal void FinishMaYuanYi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMaYuanYi(f);
    internal void FinishMaYuanYiMovement(long id, long before, int count) => _resolve().FinishMaYuanYiMovement(id, before, count);
    internal bool HasMaYuanYiBeforeDamageObserver(long id) => _resolve().HasMaYuanYiBeforeDamageObserver(id);
    internal bool HasMaYuanYiDamageObserver(long id) => _resolve().HasMaYuanYiDamageObserver(id);
    internal bool HasPaidMoucuanSourceLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasPaidMoucuanSourceLoss(f);
    internal bool IsBinghuoCleanupMovement(global::CardGame.Core.MaYuanYiReceipt r, global::CardGame.Core.CardMovementRecord m) => _resolve().IsBinghuoCleanupMovement(r, m);
    internal bool IsBinghuoHandoffMovement(global::CardGame.Core.MaYuanYiReceipt r, global::CardGame.Core.CardMovementRecord m) => _resolve().IsBinghuoHandoffMovement(r, m);
    internal bool IsBinghuoJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsBinghuoJudgment(j, f);
    internal bool IsExactBinghuoFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame? judgment, int frameIndex) => _resolve().IsExactBinghuoFinalizedChildSubtree(frame, judgment, frameIndex);
    internal bool IsMaYuanYiDying() => _resolve().IsMaYuanYiDying();
    internal bool IsMaYuanYiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMaYuanYiMovement(f, e, p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> JibingPileCards(global::CardGame.Core.CharacterState owner) => _resolve().JibingPileCards(owner);
    internal bool MaYuanYiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MaYuanYiFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MaYuanYiObserverRoot() => _resolve().MaYuanYiObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MaYuanYiTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MaYuanYiTargets(f);
    internal int MoucuanFactionCount() => _resolve().MoucuanFactionCount();
    internal void PublishMaYuanYiTarget(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMaYuanYiTarget(f);
    internal void RecordMaYuanYiBingAction(global::CardGame.Core.IGameEvent fact) => _resolve().RecordMaYuanYiBingAction(fact);
    internal bool ResumeMaYuanYi(long id) => _resolve().ResumeMaYuanYi(id);
    internal bool ReturnMaYuanYiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMaYuanYiMovement(f);
    internal bool TryReturnBinghuoJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().TryReturnBinghuoJudgment(j);
    internal bool ValidBingAction(global::CardGame.Core.MaYuanYiBingActionEvent e) => _resolve().ValidBingAction(e);
    internal (global::CardGame.Core.MaYuanYiBingActionEvent Fact, int Opponent)? WangjingQualification(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().WangjingQualification(c, context);
    internal bool HasMaYuanYiCapability { get => _resolve().HasMaYuanYiCapability; }
}
