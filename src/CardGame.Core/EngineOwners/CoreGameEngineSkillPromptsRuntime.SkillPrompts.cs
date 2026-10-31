#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSkillPromptsRuntime
{
    internal CommandResult SubmitPindianPromptAnswer(PromptChoice selected)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.SkillPrompt is null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianFrame)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "There is no current Pindian interaction.");
        return _kernelCoreMainOperations.Accept(() =>
        {
            _corePindianOperations.ResolvePindianChoice(selected);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal bool IsAiPindianPending() => _resolutionStackOperations._resolutionStack.LastOrDefault() is PindianFrame && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { SkillPrompt: not null } decision && !_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].IsHuman;
    internal int CountCardsUsedByCurrentPlayerThisTurn(int ownerSeat)
    {
        var turnStart = _coreCommandJournalStorageEventsPendingEventsAccess._events.FindLastIndex(envelope => envelope.Payload is TurnStartedEvent started && started.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && started.ActorSeat == ownerSeat);
        return _coreCommandJournalStorageEventsPendingEventsAccess._events.Skip(turnStart + 1).Count(envelope => envelope.Payload is CardUseDeclaredEvent declared && declared.SourceSeat == ownerSeat);
    }

    internal int CountCardsUsedOrRespondedByPlayerThisTurn(int ownerSeat)
    {
        // The current turn's start and this command's fresh events may still sit in
        // the pending buffer, so the scan spans both the flushed and pending streams;
        // the boundary is the current turn's start regardless of its owner.
        var turnStart = _coreCommandJournalStorageEventsPendingEventsAccess._events.FindLastIndex(envelope => envelope.Payload is TurnStartedEvent started && started.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
        IEnumerable<IGameEvent> visible = turnStart >= 0 ? _coreCommandJournalStorageEventsPendingEventsAccess._events.Skip(turnStart + 1).Select(envelope => envelope.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents) : _coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents;
        return visible.Count(payload => payload is CardUseDeclaredEvent declared && declared.SourceSeat == ownerSeat || payload is CardActionAcceptedEvent accepted && accepted.Action.Type == CardActionType.Response && accepted.Action.ActorSeat == ownerSeat);
    }
}
