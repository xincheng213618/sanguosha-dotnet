#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualTurnsEntries
{
    internal void AdvanceAfterDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped) => _resolve().AdvanceAfterDeferredTurnEnd(previous, skipped);
    internal void BeginDelayedJudgmentOrTurnStart(global::CardGame.Core.CharacterState current, int? excludedCardId = default) => _resolve().BeginDelayedJudgmentOrTurnStart(current, excludedCardId);
    internal void BeginTurn() => _resolve().BeginTurn();
    internal void BeginTurnAfterBeforeTurnPrelude(global::CardGame.Core.CharacterState current) => _resolve().BeginTurnAfterBeforeTurnPrelude(current);
    internal void BeginTurnStartAfterProgramLifecycle(global::CardGame.Core.CharacterState current) => _resolve().BeginTurnStartAfterProgramLifecycle(current);
    internal void CompleteActualTurnCursorAfterRoundEnding(global::CardGame.Core.CharacterState previous, bool skipped) => _resolve().CompleteActualTurnCursorAfterRoundEnding(previous, skipped);
    internal void CompleteDrawPhaseAfterProgramWindow(global::CardGame.Core.CharacterState current, bool skipPlayPhaseAfterDraw, bool normalDrawReplaced, int normalDrawAdjustment, int? frozenBaseDrawCount = default, bool confirmedActualDrawSubstitution = false) => _resolve().CompleteDrawPhaseAfterProgramWindow(current, skipPlayPhaseAfterDraw, normalDrawReplaced, normalDrawAdjustment, frozenBaseDrawCount, confirmedActualDrawSubstitution);
    internal void CompleteTurnStart(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool skippedActualJudgment = false) => _resolve().CompleteTurnStart(current, delayedEffects, skippedActualJudgment);
    internal void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false) => _resolve().CompleteTurnStartAfterDraw(current, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    internal void ContinueAfterWushuangShopping(global::CardGame.Core.CharacterState previous, bool skipped) => _resolve().ContinueAfterWushuangShopping(previous, skipped);
    internal void ContinueTurnAfterActualStart(global::CardGame.Core.CharacterState current) => _resolve().ContinueTurnAfterActualStart(current);
    internal void ContinueTurnAfterForeignActualContests(global::CardGame.Core.CharacterState current) => _resolve().ContinueTurnAfterForeignActualContests(current);
    internal void ContinueTurnAfterHengye(global::CardGame.Core.CharacterState current) => _resolve().ContinueTurnAfterHengye(current);
    internal void ContinueTurnAfterRoundPrograms(global::CardGame.Core.CharacterState current) => _resolve().ContinueTurnAfterRoundPrograms(current);
    internal void EndTurn() => _resolve().EndTurn();
    internal void EnterPlayPhase(global::CardGame.Core.CharacterState current) => _resolve().EnterPlayPhase(current);
    internal void ExpireActualTurnEndEffects(global::CardGame.Core.CharacterState previous) => _resolve().ExpireActualTurnEndEffects(previous);
    internal void FinalizeEndTurn(global::CardGame.Core.CharacterState previous) => _resolve().FinalizeEndTurn(previous);
    internal global::CardGame.Core.CardKind GetJudgmentEffectiveCardKind(global::CardGame.Core.Card card) => _resolve().GetJudgmentEffectiveCardKind(card);
    internal int GetTurnDrawCount(global::CardGame.Core.CharacterState current) => _resolve().GetTurnDrawCount(current);
    internal bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => _resolve().HasJudgmentEffectiveCard(player, kind);
}
