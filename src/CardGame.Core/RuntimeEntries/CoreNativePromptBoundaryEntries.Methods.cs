#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativePromptBoundaryEntries
{
    internal void ClearPendingDecision() => _resolve().ClearPendingDecision();
    internal global::CardGame.Core.PromptId CreatePromptId() => _resolve().CreatePromptId();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateResponseChoices(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, global::CardGame.Core.CardKind requiredCardKind, global::CardGame.Core.CardKind? incomingCard = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, global::CardGame.Core.CharacterState? responder = default) => _resolve().CreateResponseChoices(responseCards, requiredCardKind, incomingCard, effectiveCardKindSelector, includeBagua, includeFactionDefense, includeFactionSlash, responder);
    internal void EnsureStarted() => _resolve().EnsureStarted();
    internal bool IsAiDyingResponsePending() => _resolve().IsAiDyingResponsePending();
    internal bool IsAiFireAttackPending() => _resolve().IsAiFireAttackPending();
    internal bool IsAiHarvestPending() => _resolve().IsAiHarvestPending();
    internal bool IsAiNullificationPending() => _resolve().IsAiNullificationPending();
    internal bool IsAiResponsePending() => _resolve().IsAiResponsePending();
    internal bool IsAiSkipDiscardPolicyPending() => _resolve().IsAiSkipDiscardPolicyPending();
    internal bool IsAiTargetCardSelectionPending() => _resolve().IsAiTargetCardSelectionPending();
    internal bool IsHumanDecisionPending() => _resolve().IsHumanDecisionPending();
    internal void RefreshNationalResponseDecision() => _resolve().RefreshNationalResponseDecision();
    internal void RequestHumanPlay() => _resolve().RequestHumanPlay();
    internal void RequireHumanDecision(global::CardGame.Core.DecisionKind expected) => _resolve().RequireHumanDecision(expected);
    internal global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode) => _resolve().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
}
