#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualPlayPhaseCardUsesRuntime
{
    internal bool TracksActualPlayPhaseCardUses => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesActualPlayPhaseUseLedger;

    internal int GetActualPlayPhaseUseCount(int actorSeat) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && actorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat ? _coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions.Count : 0;
    internal void RecordActualPlayPhaseUse(CardActionContext action)
    {
        _corePhaseNamePredictionOperations.ObservePhaseNamePredictionUse(action, qualifiedActualUse: true);
        _coreEndingHistoricalUsesOperations.RecordOwnPlayEligibleUse(action);
        _coreAlternatingSuitAndCompletedTopOperations.RecordFirstTurnCategoryUse(action);
        if (!TracksActualPlayPhaseCardUses || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return;
        if (_coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions.Add(action.ActionId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualPlayPhaseCardUseRecordedEvent(new(action.ActorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, _coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess._actualPlayPhaseUseActions.Count), action.ActionId) { EffectiveCardKind = _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.StopUsingAfterTrickOrHpCount) ? action.EffectiveKind : null });
    }
}
