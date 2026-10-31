#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheMarketCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreShanheMarketCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDying(global::CardGame.Core.DyingFrame dying, bool survived);
    void RequestHumanDyingResponse(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> peaches, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> alcohols);
    global::CardGame.Core.Card[] GetDyingPeaches(global::CardGame.Core.CharacterState responder);
    global::CardGame.Core.Card[] GetDyingAlcohols(global::CardGame.Core.CharacterState responder, int victimSeat);
}

internal interface ICoreShanheMarketCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreShanheMarketCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreShanheMarketResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheMarketCoreTargetHandVisibilityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RefreshVisibleTargetHandSelectionPrompt();
}

internal interface ICoreShanheMarketCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreShanheMarketCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    global::CardGame.Core.PromptId CreatePromptId();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateResponseChoices(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> responseCards, global::CardGame.Core.CardKind requiredCardKind, global::CardGame.Core.CardKind? incomingCard = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardKind>? effectiveCardKindSelector = default, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, global::CardGame.Core.CharacterState? responder = default);
    void RequestHumanPlay();
}

internal interface ICoreShanheMarketCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreShanheMarketContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShanheMarketCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreShanheMarketCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueNullificationWindow(global::CardGame.Core.NullificationWindowFrame pending);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetNullificationCards(global::CardGame.Core.CharacterState responder);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CreateNullificationChoices(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreShanheMarketShanheMarketOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket { get; }
}

internal interface ICoreShanheMarketKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreShanheMarketCoreShanheAlliedTricksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RefreshShanheAlliedHarvestSelection();
}

internal interface ICoreShanheMarketCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreShanheMarketCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreShanheMarketCoreRevealedFireAttackPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RefreshRevealedFireAttackPaymentPrompt();
}

internal interface ICoreShanheMarketKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreShanheMarketCoreProgramHandLimitKindExemptionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ShanheMarketDiscardQuote? CaptureShanheMarketDiscardQuote();
    bool RefreshShanheMarketDiscardPrompt(global::CardGame.Core.ShanheMarketDiscardQuote? quote);
}

internal interface ICoreShanheMarketCoreShanheTrickResponseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShanheTrickNullificationBanned(int responderSeat);
}
