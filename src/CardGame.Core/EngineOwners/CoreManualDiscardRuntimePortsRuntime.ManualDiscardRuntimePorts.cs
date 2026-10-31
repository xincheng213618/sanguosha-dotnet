#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreManualDiscardRuntimePortsRuntime
{
    Winner IManualDiscardFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus IManualDiscardFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    int IManualDiscardFacts.RequiredDiscard(CharacterState owner) => _coreTurnHandCategoryRestrictionsOperations.RequiredSelfHandLimitDiscard(owner);
    int IManualDiscardFacts.QuotaMultiplier(CharacterState owner) => _corePlannedPhaseQuantitiesOperations.Value.GetDiscardQuotaMultiplier(owner);
    IReadOnlyList<Card> IManualDiscardFacts.DiscardableHand(CharacterState owner) => _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableHand(owner);
    int IManualDiscardFacts.HandLimit(CharacterState owner) => _coreNativeRuleQueriesOperations.GetHandLimit(owner);
    void IManualDiscardTransitions.SetPrompt(PendingDecision prompt)
    {
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = prompt;
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.AwaitingHumanDiscard;
    }

    void IManualDiscardTransitions.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IManualDiscardTransitions.BeginDiscardPhaseEnded(CharacterState owner) => _coreProgramLifecycleOperations.TryBeginDiscardPhaseEndedProgramWindow(owner);
    void IManualDiscardTransitions.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void IManualDiscardTransitions.Log(string type, string message, int actorSeat) => _coreMatchLogOperations.AddLog(type, message, actorSeat);
    CommandError? IManualDiscardCommands.ValidatePrompt(int actorSeat, PromptId? promptId) => _coreNativePromptBoundaryOperations.ValidateHumanPrompt(actorSeat, DecisionKind.DiscardCards, promptId, CommandErrorCode.IllegalAction);
    CommandResult IManualDiscardCommands.Accept(Action operation) => _kernelCoreMainOperations.Accept(operation);
    CommandResult IManualDiscardCommands.Reject(CommandErrorCode code, string message) => _kernelCoreMainOperations.Reject(code, message);
}
