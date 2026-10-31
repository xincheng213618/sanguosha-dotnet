#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreManualDiscardRuntimePortsRuntime
{
    Winner global::CardGame.Core.IManualDiscardFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus global::CardGame.Core.IManualDiscardFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    int global::CardGame.Core.IManualDiscardFacts.RequiredDiscard(CharacterState owner) => _coreTurnHandCategoryRestrictionsOperations.RequiredSelfHandLimitDiscard(owner);
    int global::CardGame.Core.IManualDiscardFacts.QuotaMultiplier(CharacterState owner) => _corePlannedPhaseQuantitiesOperations.GetDiscardQuotaMultiplier(owner);
    IReadOnlyList<Card> global::CardGame.Core.IManualDiscardFacts.DiscardableHand(CharacterState owner) => _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableHand(owner);
    int global::CardGame.Core.IManualDiscardFacts.HandLimit(CharacterState owner) => _coreNativeRuleQueriesOperations.GetHandLimit(owner);
    void global::CardGame.Core.IManualDiscardTransitions.SetPrompt(PendingDecision prompt)
    {
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = prompt;
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.AwaitingHumanDiscard;
    }

    void global::CardGame.Core.IManualDiscardTransitions.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IManualDiscardTransitions.BeginDiscardPhaseEnded(CharacterState owner) => _coreProgramLifecycleOperations.TryBeginDiscardPhaseEndedProgramWindow(owner);
    void global::CardGame.Core.IManualDiscardTransitions.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void global::CardGame.Core.IManualDiscardTransitions.Log(string type, string message, int actorSeat) => _coreMatchLogOperations.AddLog(type, message, actorSeat);
    CommandError? global::CardGame.Core.IManualDiscardCommands.ValidatePrompt(int actorSeat, PromptId? promptId) => _coreNativePromptBoundaryOperations.ValidateHumanPrompt(actorSeat, DecisionKind.DiscardCards, promptId, CommandErrorCode.IllegalAction);
    CommandResult global::CardGame.Core.IManualDiscardCommands.Accept(Action operation) => _kernelCoreMainOperations.Accept(operation);
    CommandResult global::CardGame.Core.IManualDiscardCommands.Reject(CommandErrorCode code, string message) => _kernelCoreMainOperations.Reject(code, message);
}
