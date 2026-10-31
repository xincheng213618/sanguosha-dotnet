#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengGongYingOlEntries
{
    internal bool AdvanceChengGongYingSubtree() => _resolve().AdvanceChengGongYingSubtree();
    internal global::CardGame.Core.PromptChoice AiChengGongYing(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiChengGongYing(p, f);
    internal void AnswerChengGongYing(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerChengGongYing(choice);
    internal void AssertChengGongYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertChengGongYing(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginChengGongYing(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginChengGongYing(input);
    internal void BindKuangxiangBlackUse(global::CardGame.Core.IGameEvent payload) => _resolve().BindKuangxiangBlackUse(payload);
    internal bool CanOfferChengGongYing(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferChengGongYing(candidate, trigger, c);
    internal bool ChengGongYingCanDiscard(int payer, int id) => _resolve().ChengGongYingCanDiscard(payer, id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ChengGongYingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChengGongYingChoices(f);
    internal int ChengGongYingChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChengGongYingChooser(f);
    internal int ChengGongYingDrawPenalty(int actorSeat) => _resolve().ChengGongYingDrawPenalty(actorSeat);
    internal bool ChengGongYingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ChengGongYingFirstChild(f, child);
    internal int ChengGongYingKuangxiangDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ChengGongYingKuangxiangDamageBonus(attack);
    internal global::CardGame.Core.ProgramSkillFrame? ChengGongYingObserverRoot() => _resolve().ChengGongYingObserverRoot();
    internal bool ChengGongYingParent(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().ChengGongYingParent(candidate, c);
    internal void ConsumeChengGongYingDraw(int actorSeat) => _resolve().ConsumeChengGongYingDraw(actorSeat);
    internal void ContinueChengGongYing(long id) => _resolve().ContinueChengGongYing(id);
    internal bool ContinuePaidChengGongYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidChengGongYing(f);
    internal void FinishChengGongYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishChengGongYing(f);
    internal bool HasChengGongYingBeforeDamageObserver(long id) => _resolve().HasChengGongYingBeforeDamageObserver(id);
    internal bool HasChengGongYingDamageObserver(long id) => _resolve().HasChengGongYingDamageObserver(id);
    internal bool IsChengGongYingDying() => _resolve().IsChengGongYingDying();
    internal bool IsChengGongYingIssuedMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsChengGongYingIssuedMovement(f, m);
    internal bool IsChengGongYingMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsChengGongYingMovement(f, effect, movement);
    internal void IssueChengGongYingDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueChengGongYingDiscard(f);
    internal void IssueChengGongYingDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueChengGongYingDraw(f);
    internal bool KuangxiangDrawGrantConsumed(long id) => _resolve().KuangxiangDrawGrantConsumed(id);
    internal void PublishChengGongYing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishChengGongYing(f);
    internal void RecordChengGongYingMovement(long id) => _resolve().RecordChengGongYingMovement(id);
    internal bool ResumeChengGongYing(long id) => _resolve().ResumeChengGongYing(id);
    internal bool ReturnChengGongYingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnChengGongYingMovement(f);
    internal bool TryReturnChengGongYingPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().TryReturnChengGongYingPaidPreparation(frame);
    internal bool HasChengGongYingCapability { get => _resolve().HasChengGongYingCapability; }
}
