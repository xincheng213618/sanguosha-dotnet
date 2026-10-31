#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualTurnsEntries
{
    internal void AdvanceAfterDeferredTurnEnd(CharacterState previous, bool skipped) => _resolve().AdvanceAfterDeferredTurnEnd(previous, skipped);
    internal void BeginDelayedJudgmentOrTurnStart(CharacterState current, int? excludedCardId = default) => _resolve().BeginDelayedJudgmentOrTurnStart(current, excludedCardId);
    internal void BeginTurn() => _resolve().BeginTurn();
    internal void BeginTurnAfterBeforeTurnPrelude(CharacterState current) => _resolve().BeginTurnAfterBeforeTurnPrelude(current);
    internal void BeginTurnStartAfterProgramLifecycle(CharacterState current) => _resolve().BeginTurnStartAfterProgramLifecycle(current);
    internal void CompleteActualTurnCursorAfterRoundEnding(CharacterState previous, bool skipped) => _resolve().CompleteActualTurnCursorAfterRoundEnding(previous, skipped);
    internal void CompleteDrawPhaseAfterProgramWindow(CharacterState current, bool skipPlayPhaseAfterDraw, bool normalDrawReplaced, int normalDrawAdjustment, int? frozenBaseDrawCount = default, bool confirmedActualDrawSubstitution = false) => _resolve().CompleteDrawPhaseAfterProgramWindow(current, skipPlayPhaseAfterDraw, normalDrawReplaced, normalDrawAdjustment, frozenBaseDrawCount, confirmedActualDrawSubstitution);
    internal void CompleteTurnStart(CharacterState current, GameEngine.DelayedTurnEffects delayedEffects, bool skippedActualJudgment = false) => _resolve().CompleteTurnStart(current, delayedEffects, skippedActualJudgment);
    internal void CompleteTurnStartAfterDraw(CharacterState current, GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false) => _resolve().CompleteTurnStartAfterDraw(current, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    internal void ContinueAfterWushuangShopping(CharacterState previous, bool skipped) => _resolve().ContinueAfterWushuangShopping(previous, skipped);
    internal void ContinueTurnAfterActualStart(CharacterState current) => _resolve().ContinueTurnAfterActualStart(current);
    internal void ContinueTurnAfterForeignActualContests(CharacterState current) => _resolve().ContinueTurnAfterForeignActualContests(current);
    internal void ContinueTurnAfterHengye(CharacterState current) => _resolve().ContinueTurnAfterHengye(current);
    internal void ContinueTurnAfterRoundPrograms(CharacterState current) => _resolve().ContinueTurnAfterRoundPrograms(current);
    internal void EndTurn() => _resolve().EndTurn();
    internal void EnterPlayPhase(CharacterState current) => _resolve().EnterPlayPhase(current);
    internal void ExpireActualTurnEndEffects(CharacterState previous) => _resolve().ExpireActualTurnEndEffects(previous);
    internal void FinalizeEndTurn(CharacterState previous) => _resolve().FinalizeEndTurn(previous);
    internal CardKind GetJudgmentEffectiveCardKind(Card card) => _resolve().GetJudgmentEffectiveCardKind(card);
    internal int GetTurnDrawCount(CharacterState current) => _resolve().GetTurnDrawCount(current);
    internal bool HasJudgmentEffectiveCard(CharacterState player, CardKind kind) => _resolve().HasJudgmentEffectiveCard(player, kind);
}
