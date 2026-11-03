#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 10)]
    internal bool CuiYanFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasCuiYanCapability;
    internal bool HasCuiYanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShutongFinalizedUse) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZhijieActualTurnEnd);
    internal bool CuiYanActualTurnOpen => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);

    internal static Suit CuiYanTrueUseSuit(IReadOnlyList<int> ids, Suit? nativeSuit) => RuntimeSkills.RuntimeRules_cui_yan_ol_CuiYanLedger.CuiYanTrueUseSuit(ids, nativeSuit);
    internal void CaptureCuiYanUseTargets(CardUseFrame use)
    {
        if (!HasCuiYanCapability || !CuiYanActualTurnOpen || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) || use.Action is { Type: not CardActionType.Use })
            return;
        var old = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanActualUseActorEvent>().LastOrDefault(e => e.UseId == use.Id);
        var next = new CuiYanActualUseActorEvent(use.Id, use.Action?.ActionId ?? 0, old?.ActualTurn ?? _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, old?.TurnOwner ?? _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, use.SourceSeat, use.CardKind);
        if (old != next)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(next);
        var targets = (use.Action?.EffectiveDesignatedTargetSeats ?? use.TargetSeats).Distinct().ToArray();
        if (targets.Length == 0 && !use.TargetsAdjusted && (CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) == SkillProgramCardCategory.Equipment || use.CardKind is CardKind.DrawTwo or CardKind.Peach or CardKind.Alcohol))
            targets = [use.SourceSeat];
        foreach (var target in targets.Where(_coreTableSeatQueriesOperations.IsValidPlayerSeat))
            if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanEverTargetedEvent>().Any(e => e.UseId == use.Id && e.TargetSeat == target))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanEverTargetedEvent(use.Id, next.ActualTurn, next.TurnOwner, target));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27470)]
    internal void ObserveCuiYanCardHistory(IGameEvent payload)
    {
        if (!HasCuiYanCapability || !CuiYanActualTurnOpen || payload is CuiYanActualUseActorEvent or CuiYanEverTargetedEvent or CuiYanRespondedEvent or CuiYanEndQualifiedEvent)
            return;
        if (payload is CardRespondedEvent response && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanRespondedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.ResponderSeat == response.ResponderSeat))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanRespondedEvent(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, response.ResponderSeat));
        if (payload is TurnEndedEvent)
        {
            foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanEndQualifiedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.OwnerSeat == owner.Seat))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanEndQualifiedEvent(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, owner.Seat, CuiYanUsedOrPlayed(owner.Seat), _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanEverTargetedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.TargetSeat == owner.Seat)));
        }
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27470)]
    internal void CaptureCuiYanNativeHistory(IGameEvent payload)
    {
        if (!HasCuiYanCapability || payload is CuiYanActualUseActorEvent or CuiYanEverTargetedEvent or CuiYanRespondedEvent or CuiYanEndQualifiedEvent)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().ToArray())
            CaptureCuiYanUseTargets(use);
    }

    [NativeUsePreparation(27470)]
    internal bool CaptureCuiYanFinalUse(CardUseFrame use)
    {
        CaptureCuiYanUseTargets(use);
        return false;
    }

    internal bool CuiYanUsedOrPlayed(int seat) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanRespondedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.ResponderSeat == seat) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CuiYanActualUseActorEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat).GroupBy(e => e.UseId).Any(g => g.Last().ActorSeat == seat);
}
