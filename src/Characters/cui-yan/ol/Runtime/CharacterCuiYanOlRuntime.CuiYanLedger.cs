#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 10)]
    internal bool CuiYanFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasCuiYanCapability;
    internal bool HasCuiYanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShutongFinalizedUse) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveZhijieActualTurnEnd);
    internal bool CuiYanActualTurnOpen => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && !_coreProgramEventHistoryOperations.ProgramEventHistory<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.OwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);

    internal static Suit CuiYanTrueUseSuit(IReadOnlyList<int> ids, Suit? nativeSuit) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cui_yan_ol_CuiYanLedger.CuiYanTrueUseSuit(ids, nativeSuit);
    internal void CaptureCuiYanUseTargets(CardUseFrame use)
    {
        if (!HasCuiYanCapability || !CuiYanActualTurnOpen || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) || use.Action is { Type: not CardActionType.Use })
            return;
        var old = _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanActualUseActorEvent>().LastOrDefault(e => e.UseId == use.Id);
        var next = new CuiYanActualUseActorEvent(use.Id, use.Action?.ActionId ?? 0, old?.ActualTurn ?? _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, old?.TurnOwner ?? _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, use.SourceSeat, use.CardKind);
        if (old != next)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(next);
        var targets = (use.Action?.EffectiveDesignatedTargetSeats ?? use.TargetSeats).Distinct().ToArray();
        if (targets.Length == 0 && !use.TargetsAdjusted && (global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(use.CardKind) == SkillProgramCardCategory.Equipment || use.CardKind is CardKind.DrawTwo or CardKind.Peach or CardKind.Alcohol))
            targets = [use.SourceSeat];
        foreach (var target in targets.Where(_coreTableSeatQueriesOperations.IsValidPlayerSeat))
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanEverTargetedEvent>().Any(e => e.UseId == use.Id && e.TargetSeat == target))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanEverTargetedEvent(use.Id, next.ActualTurn, next.TurnOwner, target));
    }

    internal bool CuiYanTrueResponse(CardActionContext a, out long parentId)
    {
        parentId = 0;
        if (a.Type != CardActionType.Response || a.ActorSeat != a.ProviderSeat || a.RequesterSeat is not null || a.ResponderSeat != a.ActorSeat)
            return false;
        if (a.EffectiveKind == CardKind.Nullification && _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault()is { } n && a.ParentActionId == _coreDelayedNullificationOperations.NullificationParentActionId(n) && a.OpponentSeat == n.SourceSeat)
        {
            parentId = n.Id;
            return true;
        }

        if (a.EffectiveKind == CardKind.Dodge && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(u => u.Action?.ActionId == a.ParentActionId)is { CardAttack: { } attack } use && attack.TargetSeat == a.ActorSeat && a.OpponentSeat == attack.SourceSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? use.CardKind))
        {
            parentId = use.Id;
            return true;
        }

        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27470)]
    internal void ObserveCuiYanCardHistory(IGameEvent payload)
    {
        if (!HasCuiYanCapability || !CuiYanActualTurnOpen || payload is CuiYanActualUseActorEvent or CuiYanEverTargetedEvent or CuiYanRespondedEvent or CuiYanEndQualifiedEvent or ShutongNextUseConsumedEvent)
            return;
        if (payload is CardUseDeclaredEvent d && _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is { } use && use.SourceSeat == d.SourceSeat && use.CardId == d.CardId && use.CardKind == d.CardKind)
            ConsumeShutongForActualUse(use.Id, use.Action, use.SourceSeat, false);
        if (payload is CardActionAcceptedEvent { Action: { } a } && CuiYanTrueResponse(a, out var native))
            ConsumeShutongForActualUse(native, a, a.ActorSeat, true);
        if (payload is CardRespondedEvent response && !_coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanRespondedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.ResponderSeat == response.ResponderSeat))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanRespondedEvent(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, response.ResponderSeat));
        if (payload is TurnEndedEvent)
        {
            foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                if (!_coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanEndQualifiedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.OwnerSeat == owner.Seat))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CuiYanEndQualifiedEvent(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, owner.Seat, CuiYanUsedOrPlayed(owner.Seat), _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanEverTargetedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.TargetSeat == owner.Seat)));
        }
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27470)]
    internal void CaptureCuiYanNativeHistory(IGameEvent payload)
    {
        if (!HasCuiYanCapability || payload is CuiYanActualUseActorEvent or CuiYanEverTargetedEvent or CuiYanRespondedEvent or CuiYanEndQualifiedEvent or ShutongNextUseConsumedEvent)
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

    internal bool CuiYanUsedOrPlayed(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanRespondedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.ResponderSeat == seat) || _coreProgramEventHistoryOperations.ProgramEventHistory<CuiYanActualUseActorEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat).GroupBy(e => e.UseId).Any(g => g.Last().ActorSeat == seat);
}
