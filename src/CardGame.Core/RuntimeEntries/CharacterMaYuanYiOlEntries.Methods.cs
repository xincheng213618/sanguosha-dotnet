#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaYuanYiOlEntries
{
    internal bool AdvanceMaYuanYiSubtree() => _resolve().AdvanceMaYuanYiSubtree();
    internal void AnswerMaYuanYiTarget(PromptChoice supplied) => _resolve().AnswerMaYuanYiTarget(supplied);
    internal SkillProgramStepOutcome BeginMaYuanYi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMaYuanYi(supplied, op);
    internal bool CanBeginBinghuoJudgment(ProgramSkillFrame f, int targetSeat, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().CanBeginBinghuoJudgment(f, targetSeat, reason, bind, visibility);
    internal bool CanOfferMaYuanYi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferMaYuanYi(c, t, context);
    internal PromptChoice ChooseMaYuanYi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMaYuanYi(p, f);
    internal void ContinueMaYuanYi(long id) => _resolve().ContinueMaYuanYi(id);
    internal bool ContinuePaidMaYuanYi(ProgramSkillFrame f) => _resolve().ContinuePaidMaYuanYi(f);
    internal bool DrainMaYuanYi(ProgramSkillFrame f) => _resolve().DrainMaYuanYi(f);
    internal void FinishMaYuanYi(ProgramSkillFrame f) => _resolve().FinishMaYuanYi(f);
    internal void FinishMaYuanYiMovement(long id, long before, int count) => _resolve().FinishMaYuanYiMovement(id, before, count);
    internal bool HasPaidMoucuanSourceLoss(ProgramSkillFrame f) => _resolve().HasPaidMoucuanSourceLoss(f);
    internal bool IsBinghuoCleanupMovement(MaYuanYiReceipt r, CardMovementRecord m) => _resolve().IsBinghuoCleanupMovement(r, m);
    internal bool IsBinghuoHandoffMovement(MaYuanYiReceipt r, CardMovementRecord m) => _resolve().IsBinghuoHandoffMovement(r, m);
    internal bool IsBinghuoJudgment(JudgmentFrame j, ProgramSkillFrame f) => _resolve().IsBinghuoJudgment(j, f);
    internal IReadOnlyList<Card> JibingPileCards(CharacterState owner) => _resolve().JibingPileCards(owner);
    internal bool MaYuanYiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MaYuanYiFirstChild(f, child);
    internal ProgramSkillFrame? MaYuanYiObserverRoot() => _resolve().MaYuanYiObserverRoot();
    internal IReadOnlyList<PromptChoice> MaYuanYiTargets(ProgramSkillFrame f) => _resolve().MaYuanYiTargets(f);
    internal int MoucuanFactionCount() => _resolve().MoucuanFactionCount();
    internal void PublishMaYuanYiTarget(ProgramSkillFrame f) => _resolve().PublishMaYuanYiTarget(f);
    internal void RecordMaYuanYiBingAction(IGameEvent fact) => _resolve().RecordMaYuanYiBingAction(fact);
    internal bool ResumeMaYuanYi(long id) => _resolve().ResumeMaYuanYi(id);
    internal bool ReturnMaYuanYiMovement(ProgramSkillFrame f) => _resolve().ReturnMaYuanYiMovement(f);
    internal bool TryReturnBinghuoJudgment(JudgmentFrame j) => _resolve().TryReturnBinghuoJudgment(j);
    internal bool ValidBingAction(MaYuanYiBingActionEvent e) => _resolve().ValidBingAction(e);
    internal (MaYuanYiBingActionEvent Fact, int Opponent)? WangjingQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().WangjingQualification(c, context);
    internal bool HasMaYuanYiCapability { get => _resolve().HasMaYuanYiCapability; }
}
