#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPhaseColorRestrictionsRuntime
{
    internal void GrantProgramPlayPhaseColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(active, sourceBind);
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || targetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || binding.CardIds.Count != 1 || binding.FrozenRevealedSuit is not { } suit || suit == Suit.None)
            throw new InvalidOperationException("A phase color restriction requires a frozen paid color and the current Play phase.");
        var restriction = new ProgramPhaseColorRestriction(active.SkillId, active.SkillInstanceId, active.OwnerSeat, targetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(suit));
        _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions.Add(restriction);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPhaseColorRestrictionGrantedEvent(restriction));
    }

    internal bool IsPlayPhasePhysicalCardRestricted(CharacterState player, Card card)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || card.Suit == Suit.None)
            return false;
        foreach (var restriction in _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions)
            if (restriction.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && restriction.TargetSeat == player.Seat && restriction.IsRed == global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(player, card)))
                return true;
        return false;
    }

    internal IReadOnlyList<string>? UnfulfilledPlayPhaseColorRestrictions(int ownerSeat)
    {
        var instances = _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions.Where(item => item.OwnerSeat == ownerSeat && item.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && item.TargetSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && !item.UsedSlash).Select(item => item.SkillInstanceId).Distinct(StringComparer.Ordinal).ToArray();
        return instances.Length == 0 ? null : instances;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 230)]
    internal void ObservePlayPhaseColorRestriction(IGameEvent payload)
    {
        if (payload is TurnStartedEvent)
            _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions.Clear();
        if (payload is not CardUseDeclaredEvent use || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            return;
        for (var index = 0; index < _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions.Count; index++)
            if (_coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions[index].TargetSeat == use.SourceSeat && _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions[index].TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
                _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions[index] = _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions[index] with
                {
                    UsedSlash = true
                };
    }
}
