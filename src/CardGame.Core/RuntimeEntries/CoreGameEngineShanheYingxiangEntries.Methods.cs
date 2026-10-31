#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheYingxiangEntries
{
    internal PromptChoice AiShanheYingxiang(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().AiShanheYingxiang(prompt, frame);
    internal void AnswerShanheYingxiang(PromptChoice choice) => _resolve().AnswerShanheYingxiang(choice);
    internal SkillProgramStepOutcome BeginShanheYingxiang(ProgramSkillFrame supplied, bool used) => _resolve().BeginShanheYingxiang(supplied, used);
    internal bool CanOfferShanheYingxiang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheYingxiang(candidate, trigger, context);
    internal void CollectShanheYingxiangMovement(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _resolve().CollectShanheYingxiangMovement(batch, candidates);
    internal bool ContinuePaidShanheYingxiang(ProgramSkillFrame frame) => _resolve().ContinuePaidShanheYingxiang(frame);
    internal void ContinueShanheYingxiang(long frameId) => _resolve().ContinueShanheYingxiang(frameId);
    internal ProgramSkillWindowContext CreateShanheYingxiangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateShanheYingxiangContext(frame, candidate);
    internal void FinishShanheYingxiang(ProgramSkillFrame frame) => _resolve().FinishShanheYingxiang(frame);
    internal void GainShanheQingjueMissingSuits(ProgramSkillFrame frame) => _resolve().GainShanheQingjueMissingSuits(frame);
    internal bool HasShanheYingxiangNonUseLoss(int owner, CardMovementBatchContext batch) => _resolve().HasShanheYingxiangNonUseLoss(owner, batch);
    internal bool IsIssuedShanheYingxiangMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedShanheYingxiangMovement(candidate, context);
    internal bool IsShanheYingxiangBefore(int owner, int cardId, int before) => _resolve().IsShanheYingxiangBefore(owner, cardId, before);
    internal bool IsShanheYingxiangMovement(ProgramTriggerCandidate candidate) => _resolve().IsShanheYingxiangMovement(candidate);
    internal bool IsShanheYingxiangTrueUseLoss(CardMovementRecord move) => _resolve().IsShanheYingxiangTrueUseLoss(move);
    internal void PayShanheQingjue(ProgramSkillFrame frame) => _resolve().PayShanheQingjue(frame);
    internal void PublishShanheYingxiang(ProgramSkillFrame frame) => _resolve().PublishShanheYingxiang(frame);
    internal void RecordShanheYingxiangGift(CardMovedEvent fact) => _resolve().RecordShanheYingxiangGift(fact);
    internal bool ResumeShanheYingxiang(long frameId) => _resolve().ResumeShanheYingxiang(frameId);
    internal void ReturnShanheYingxiangDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShanheYingxiangDraw(supplied, outcome);
    internal Card[] ShanheQingjueCards(ProgramSkillFrame frame) => _resolve().ShanheQingjueCards(frame);
    internal IReadOnlyList<PromptChoice> ShanheYingxiangChoices(ProgramSkillFrame frame) => _resolve().ShanheYingxiangChoices(frame);
    internal CardActionContext? ShanheYingxiangUsedAction(int owner, ProgramSkillWindowContext context) => _resolve().ShanheYingxiangUsedAction(owner, context);
    internal void TagShanheYingxiang(int owner, CardMovementRecord move) => _resolve().TagShanheYingxiang(owner, move);
    internal ProgramSkillWindowContext? TryCreateShanheYingxiangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateShanheYingxiangContext(frame, candidate);
}
