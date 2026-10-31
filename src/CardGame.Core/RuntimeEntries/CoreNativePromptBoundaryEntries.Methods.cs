#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativePromptBoundaryEntries
{
    internal void ClearPendingDecision() => _resolve().ClearPendingDecision();
    internal PromptId CreatePromptId() => _resolve().CreatePromptId();
    internal IReadOnlyList<PromptChoice> CreateResponseChoices(IReadOnlyList<Card> responseCards, CardKind requiredCardKind, CardKind? incomingCard = default, Func<Card, CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, CharacterState? responder = default) => _resolve().CreateResponseChoices(responseCards, requiredCardKind, incomingCard, effectiveCardKindSelector, includeBagua, includeFactionDefense, includeFactionSlash, responder);
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
    internal void RequireHumanDecision(DecisionKind expected) => _resolve().RequireHumanDecision(expected);
    internal CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId, CommandErrorCode inactiveCode) => _resolve().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
}
