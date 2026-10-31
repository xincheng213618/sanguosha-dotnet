#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQiaoZhouOlEntries
{
    internal bool AdvanceQiaoZhouSubtree() => _resolve().AdvanceQiaoZhouSubtree();
    internal PromptChoice AiQiaoZhouForecast(PendingDecision p, ProgramSkillFrame f) => _resolve().AiQiaoZhouForecast(p, f);
    internal void AnswerQiaoZhouForecast(PromptChoice choice) => _resolve().AnswerQiaoZhouForecast(choice);
    internal void AnswerQiaoZhouPreparation(PromptChoice choice) => _resolve().AnswerQiaoZhouPreparation(choice);
    internal void AssertQiaoZhouPreparation(QiaoZhouPreparationFrame f) => _resolve().AssertQiaoZhouPreparation(f);
    internal SkillProgramStepOutcome BeginQiaoZhouForecast(ProgramSkillFrame supplied) => _resolve().BeginQiaoZhouForecast(supplied);
    internal void CleanupQiaoZhouForecast(ProgramSkillFrame f) => _resolve().CleanupQiaoZhouForecast(f);
    internal bool ContinueIssuedQiaoZhouForecast(ProgramSkillFrame f) => _resolve().ContinueIssuedQiaoZhouForecast(f);
    internal void ContinueQiaoZhouForecast(long id) => _resolve().ContinueQiaoZhouForecast(id);
    internal void ContinueQiaoZhouPreparation(long frameId) => _resolve().ContinueQiaoZhouPreparation(frameId);
    internal bool DrainQiaoZhouChildren(long id) => _resolve().DrainQiaoZhouChildren(id);
    internal void ExpireQiaoZhouForecasts(IGameEvent payload) => _resolve().ExpireQiaoZhouForecasts(payload);
    internal bool IsQiaoZhouPreparationMovement(QiaoZhouPreparationFrame f, CardsMovedTriggerWindowFrame child) => _resolve().IsQiaoZhouPreparationMovement(f, child);
    internal void PayQiaoZhouPreparation(QiaoZhouPreparationFrame f, int cardId) => _resolve().PayQiaoZhouPreparation(f, cardId);
    internal void PublishQiaoZhouForecast(ProgramSkillFrame f) => _resolve().PublishQiaoZhouForecast(f);
    internal QiaoZhouForecastActivatedEvent? QiaoZhouActivation(long grantId) => _resolve().QiaoZhouActivation(grantId);
    internal bool QiaoZhouFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().QiaoZhouFirstChild(root, child);
    internal IReadOnlyList<PromptChoice> QiaoZhouForecastChoices(ProgramSkillFrame f) => _resolve().QiaoZhouForecastChoices(f);
    internal QiaoZhouForecastGrantedEvent[] QiaoZhouMarks() => _resolve().QiaoZhouMarks();
    internal ResolutionFrame? QiaoZhouObserverRoot() => _resolve().QiaoZhouObserverRoot();
    internal Card[] QiaoZhouPreparationCards(QiaoZhouPreparationFrame f) => _resolve().QiaoZhouPreparationCards(f);
    internal IReadOnlyList<PromptChoice> QiaoZhouPreparationChoices(QiaoZhouPreparationFrame f) => _resolve().QiaoZhouPreparationChoices(f);
    internal bool QiaoZhouRecipientAvailable(int owner, int recipient) => _resolve().QiaoZhouRecipientAvailable(owner, recipient);
    internal CardConversionSource QiaoZhouSource(ProgramSkillFrame f) => _resolve().QiaoZhouSource(f);
    internal bool ResumeQiaoZhouForecast(long id) => _resolve().ResumeQiaoZhouForecast(id);
    internal bool ReturnQiaoZhouPreparationMovement(CardsMovedTriggerWindowFrame child) => _resolve().ReturnQiaoZhouPreparationMovement(child);
    internal bool TryBeginQiaoZhouPreparation(CharacterState current) => _resolve().TryBeginQiaoZhouPreparation(current);
    internal bool TryBeginQiaoZhouPreparationMovement(QiaoZhouPreparationFrame f) => _resolve().TryBeginQiaoZhouPreparationMovement(f);
    internal bool TrySkipQiaoZhouDiscardPhase(CharacterState current) => _resolve().TrySkipQiaoZhouDiscardPhase(current);
    internal bool HasQiaoZhouCapability { get => _resolve().HasQiaoZhouCapability; }
}
