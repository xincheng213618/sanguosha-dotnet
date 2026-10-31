#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQiaoZhouOlEntries
{
    internal bool AdvanceQiaoZhouSubtree() => _resolve().AdvanceQiaoZhouSubtree();
    internal global::CardGame.Core.PromptChoice AiQiaoZhouForecast(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiQiaoZhouForecast(p, f);
    internal void AnswerQiaoZhouForecast(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerQiaoZhouForecast(choice);
    internal void AnswerQiaoZhouPreparation(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerQiaoZhouPreparation(choice);
    internal void AssertQiaoZhouForecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertQiaoZhouForecast(f);
    internal void AssertQiaoZhouPreparation(global::CardGame.Core.QiaoZhouPreparationFrame f) => _resolve().AssertQiaoZhouPreparation(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginQiaoZhouForecast(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginQiaoZhouForecast(supplied);
    internal void CleanupQiaoZhouForecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CleanupQiaoZhouForecast(f);
    internal bool ContinueIssuedQiaoZhouForecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedQiaoZhouForecast(f);
    internal void ContinueQiaoZhouForecast(long id) => _resolve().ContinueQiaoZhouForecast(id);
    internal void ContinueQiaoZhouPreparation(long frameId) => _resolve().ContinueQiaoZhouPreparation(frameId);
    internal bool DrainQiaoZhouChildren(long id) => _resolve().DrainQiaoZhouChildren(id);
    internal void ExpireQiaoZhouForecasts(global::CardGame.Core.IGameEvent payload) => _resolve().ExpireQiaoZhouForecasts(payload);
    internal bool HasQiaoZhouDamageObserver(long id) => _resolve().HasQiaoZhouDamageObserver(id);
    internal bool HasQiaoZhouPreparationBoundaryFrame() => _resolve().HasQiaoZhouPreparationBoundaryFrame();
    internal bool IsQiaoZhouDying() => _resolve().IsQiaoZhouDying();
    internal bool IsQiaoZhouPreparationMovement(global::CardGame.Core.QiaoZhouPreparationFrame f, global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().IsQiaoZhouPreparationMovement(f, child);
    internal void PayQiaoZhouPreparation(global::CardGame.Core.QiaoZhouPreparationFrame f, int cardId) => _resolve().PayQiaoZhouPreparation(f, cardId);
    internal void PublishQiaoZhouForecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishQiaoZhouForecast(f);
    internal global::CardGame.Core.QiaoZhouForecastActivatedEvent? QiaoZhouActivation(long grantId) => _resolve().QiaoZhouActivation(grantId);
    internal bool QiaoZhouFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().QiaoZhouFirstChild(root, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QiaoZhouForecastChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QiaoZhouForecastChoices(f);
    internal global::CardGame.Core.QiaoZhouForecastGrantedEvent[] QiaoZhouMarks() => _resolve().QiaoZhouMarks();
    internal global::CardGame.Core.ResolutionFrame? QiaoZhouObserverRoot() => _resolve().QiaoZhouObserverRoot();
    internal global::CardGame.Core.Card[] QiaoZhouPreparationCards(global::CardGame.Core.QiaoZhouPreparationFrame f) => _resolve().QiaoZhouPreparationCards(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QiaoZhouPreparationChoices(global::CardGame.Core.QiaoZhouPreparationFrame f) => _resolve().QiaoZhouPreparationChoices(f);
    internal bool QiaoZhouRecipientAvailable(int owner, int recipient) => _resolve().QiaoZhouRecipientAvailable(owner, recipient);
    internal global::CardGame.Core.CardConversionSource QiaoZhouSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QiaoZhouSource(f);
    internal bool ResumeQiaoZhouForecast(long id) => _resolve().ResumeQiaoZhouForecast(id);
    internal bool ReturnQiaoZhouPreparationMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnQiaoZhouPreparationMovement(child);
    internal bool TryBeginQiaoZhouPreparation(global::CardGame.Core.CharacterState current) => _resolve().TryBeginQiaoZhouPreparation(current);
    internal bool TryBeginQiaoZhouPreparationMovement(global::CardGame.Core.QiaoZhouPreparationFrame f) => _resolve().TryBeginQiaoZhouPreparationMovement(f);
    internal bool TrySkipQiaoZhouDiscardPhase(global::CardGame.Core.CharacterState current) => _resolve().TrySkipQiaoZhouDiscardPhase(current);
    internal bool HasQiaoZhouCapability { get => _resolve().HasQiaoZhouCapability; }
}
