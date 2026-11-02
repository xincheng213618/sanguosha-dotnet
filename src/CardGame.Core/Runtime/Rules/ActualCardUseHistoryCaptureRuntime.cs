namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class ActualCardUseHistoryCaptureRuntime(IProgramContentQueries content,
    IActualCardUseHistoryQueries history, IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    RuntimeEntries.CoreNativeCardContinuationCursorsEntries cursors,
    RuntimeEntries.CoreActiveNativeAttackQueriesEntries attacks,
    EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess turn,
    EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess phase)
    : IActualCardUseHistoryCapture
{
    public bool IsRequired => content.RequiresFact(ProgramTrackedFact.ActualCardUseHistory);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 499)]
    internal void RecordAccepted(CardActionAcceptedEvent accepted)
    {
        if (!IsRequired || turn._turnNumber < 1 || history.FindAction(accepted.Action.ActionId) is not null)
            return;
        events.AdvanceFact(new ActualCardActionRecordedEvent(accepted.Action, turn._turnNumber,
            turn._currentSeat, turn._phase, phase._cardUseDebitPhaseInstanceId,
            IsSelfNativeResponseUse(accepted.Action)));
    }

    public void RecordFinalized(CardUseFrame use, CardActionContext action)
    {
        if (!IsRequired || action.Type != CardActionType.Use ||
            history.FindAction(action.ActionId) is null || history.FindFinalizedUse(use.Id) is not null)
            return;
        events.AdvanceFact(new ActualCardUseTargetsFinalizedEvent(use.Id, action,
            ActualCardUseTargets.PrincipalTargets(action), turn._turnNumber, turn._turnProgression.OwnerSeat));
    }

    public void RecordCompleted(CardUseFrame use, CardActionContext action)
    {
        if (!IsRequired || action.Type != CardActionType.Use ||
            history.FindAction(action.ActionId) is null || history.FindCompletedUse(use.Id) is not null)
            return;
        events.AdvanceFact(new ActualCardUseTargetsCompletedEvent(use.Id, action,
            ActualCardUseTargets.PrincipalTargets(action), turn._turnNumber, turn._turnProgression.OwnerSeat));
    }

    public void RecordUpdated(CardUseFrame use, CardActionContext action)
    {
        if (!IsRequired || action.Type != CardActionType.Use || history.FindAction(action.ActionId) is not { } accepted ||
            accepted.ActualTurn == turn._turnNumber && accepted.TurnOwner == turn._turnProgression.OwnerSeat &&
            ReferenceEquals(history.FindLatestAction(action.ActionId), action))
            return;
        events.AdvanceFact(new ActualCardUseActionUpdatedEvent(use.Id, action,
            turn._turnNumber, turn._turnProgression.OwnerSeat));
    }

    private bool IsSelfNativeResponseUse(CardActionContext action)
    {
        if (action.Type != CardActionType.Response || action.ActorSeat != action.ProviderSeat ||
            action.RequesterSeat is not null || action.ResponderSeat != action.ActorSeat)
            return false;
        if (action.EffectiveKind == CardKind.Nullification)
            return cursors.ActiveNullificationWindow is { } window && action.OpponentSeat == window.SourceSeat &&
                action.ParentActionId == (window.DelayedReturn is null
                    ? (frames.Find(window.ParentFrameId) as CardUseFrame)?.Action?.ActionId : null);
        return action.EffectiveKind == CardKind.Dodge && attacks.ActiveCardAttack is { } attack &&
            attack.EffectiveCardKind is { } incoming && RuntimeSkills.RuntimeRules_Core_CommonRules.IsSlashCard(incoming) &&
            attack.TargetSeat == action.ActorSeat && action.OpponentSeat == attack.SourceSeat &&
            (frames.Find(attack.ResolutionId) as CardUseFrame)?.Action?.ActionId == action.ParentActionId;
    }
}
