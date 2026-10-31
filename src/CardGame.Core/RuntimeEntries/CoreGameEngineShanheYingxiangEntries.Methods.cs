#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheYingxiangEntries
{
    internal global::CardGame.Core.PromptChoice AiShanheYingxiang(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AiShanheYingxiang(prompt, frame);
    internal void AnswerShanheYingxiang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShanheYingxiang(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheYingxiang(global::CardGame.Core.ProgramSkillFrame supplied, bool used) => _resolve().BeginShanheYingxiang(supplied, used);
    internal bool CanOfferShanheYingxiang(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheYingxiang(candidate, trigger, context);
    internal void CollectShanheYingxiangMovement(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().CollectShanheYingxiangMovement(batch, candidates);
    internal bool ContinuePaidShanheYingxiang(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidShanheYingxiang(frame);
    internal void ContinueShanheYingxiang(long frameId) => _resolve().ContinueShanheYingxiang(frameId);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateShanheYingxiangContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateShanheYingxiangContext(frame, candidate);
    internal void FinishShanheYingxiang(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishShanheYingxiang(frame);
    internal void GainShanheQingjueMissingSuits(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GainShanheQingjueMissingSuits(frame);
    internal bool HasShanheYingxiangNonUseLoss(int owner, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().HasShanheYingxiangNonUseLoss(owner, batch);
    internal bool IsIssuedShanheYingxiangMovement(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShanheYingxiangMovement(candidate, context);
    internal bool IsShanheYingxiangBefore(int owner, int cardId, int before) => _resolve().IsShanheYingxiangBefore(owner, cardId, before);
    internal bool IsShanheYingxiangDrawMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsShanheYingxiangDrawMovement(frame, effect, movement);
    internal bool IsShanheYingxiangMovement(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().IsShanheYingxiangMovement(candidate);
    internal bool IsShanheYingxiangTrueUseLoss(global::CardGame.Core.CardMovementRecord move) => _resolve().IsShanheYingxiangTrueUseLoss(move);
    internal void PayShanheQingjue(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PayShanheQingjue(frame);
    internal void PublishShanheYingxiang(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishShanheYingxiang(frame);
    internal void RecordShanheYingxiangGift(global::CardGame.Core.CardMovedEvent fact) => _resolve().RecordShanheYingxiangGift(fact);
    internal bool ResumeShanheYingxiang(long frameId) => _resolve().ResumeShanheYingxiang(frameId);
    internal void ReturnShanheYingxiangDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShanheYingxiangDraw(supplied, outcome);
    internal global::CardGame.Core.Card[] ShanheQingjueCards(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheQingjueCards(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShanheYingxiangChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheYingxiangChoices(frame);
    internal global::CardGame.Core.CardActionContext? ShanheYingxiangUsedAction(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ShanheYingxiangUsedAction(owner, context);
    internal void TagShanheYingxiang(int owner, global::CardGame.Core.CardMovementRecord move) => _resolve().TagShanheYingxiang(owner, move);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateShanheYingxiangContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateShanheYingxiangContext(frame, candidate);
}
