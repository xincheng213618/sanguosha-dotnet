#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengGongYingOlEntries
{
    internal bool AdvanceChengGongYingSubtree() => _resolve().AdvanceChengGongYingSubtree();
    internal PromptChoice AiChengGongYing(PendingDecision p, ProgramSkillFrame f) => _resolve().AiChengGongYing(p, f);
    internal void AnswerChengGongYing(PromptChoice choice) => _resolve().AnswerChengGongYing(choice);
    internal SkillProgramStepOutcome BeginChengGongYing(ProgramSkillFrame input) => _resolve().BeginChengGongYing(input);
    internal void BindKuangxiangBlackUse(IGameEvent payload) => _resolve().BindKuangxiangBlackUse(payload);
    internal bool CanOfferChengGongYing(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferChengGongYing(candidate, trigger, c);
    internal bool ChengGongYingCanDiscard(int payer, int id) => _resolve().ChengGongYingCanDiscard(payer, id);
    internal IReadOnlyList<PromptChoice> ChengGongYingChoices(ProgramSkillFrame f) => _resolve().ChengGongYingChoices(f);
    internal int ChengGongYingChooser(ProgramSkillFrame f) => _resolve().ChengGongYingChooser(f);
    internal int ChengGongYingDrawPenalty(int actorSeat) => _resolve().ChengGongYingDrawPenalty(actorSeat);
    internal bool ChengGongYingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ChengGongYingFirstChild(f, child);
    internal int ChengGongYingKuangxiangDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ChengGongYingKuangxiangDamageBonus(attack);
    internal ProgramSkillFrame? ChengGongYingObserverRoot() => _resolve().ChengGongYingObserverRoot();
    internal bool ChengGongYingParent(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().ChengGongYingParent(candidate, c);
    internal void ConsumeChengGongYingDraw(int actorSeat) => _resolve().ConsumeChengGongYingDraw(actorSeat);
    internal void ContinueChengGongYing(long id) => _resolve().ContinueChengGongYing(id);
    internal bool ContinuePaidChengGongYing(ProgramSkillFrame f) => _resolve().ContinuePaidChengGongYing(f);
    internal void FinishChengGongYing(ProgramSkillFrame f) => _resolve().FinishChengGongYing(f);
    internal bool IsChengGongYingIssuedMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsChengGongYingIssuedMovement(f, m);
    internal void IssueChengGongYingDiscard(ProgramSkillFrame f) => _resolve().IssueChengGongYingDiscard(f);
    internal void IssueChengGongYingDraw(ProgramSkillFrame f) => _resolve().IssueChengGongYingDraw(f);
    internal bool KuangxiangDrawGrantConsumed(long id) => _resolve().KuangxiangDrawGrantConsumed(id);
    internal void PublishChengGongYing(ProgramSkillFrame f) => _resolve().PublishChengGongYing(f);
    internal void RecordChengGongYingMovement(long id) => _resolve().RecordChengGongYingMovement(id);
    internal bool ResumeChengGongYing(long id) => _resolve().ResumeChengGongYing(id);
    internal bool ReturnChengGongYingMovement(ProgramSkillFrame f) => _resolve().ReturnChengGongYingMovement(f);
    internal bool TryReturnChengGongYingPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _resolve().TryReturnChengGongYingPaidPreparation(frame);
    internal bool HasChengGongYingCapability { get => _resolve().HasChengGongYingCapability; }
}
