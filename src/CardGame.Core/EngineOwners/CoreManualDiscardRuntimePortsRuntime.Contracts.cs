#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreManualDiscardRuntimePortsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreManualDiscardRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginDiscardPhaseEndedProgramWindow(global::CardGame.Core.CharacterState phaseOwner);
}

internal interface ICoreManualDiscardRuntimePortsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
}

internal interface ICoreManualDiscardRuntimePortsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetHandLimit(global::CardGame.Core.CharacterState player);
}

internal interface ICoreManualDiscardRuntimePortsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode);
}

internal interface ICoreManualDiscardRuntimePortsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreManualDiscardRuntimePortsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreManualDiscardRuntimePortsCorePlannedPhaseQuantitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetDiscardQuotaMultiplier(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreManualDiscardRuntimePortsCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RequiredSelfHandLimitDiscard(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableHand(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreManualDiscardRuntimePortsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void EndTurn();
}
