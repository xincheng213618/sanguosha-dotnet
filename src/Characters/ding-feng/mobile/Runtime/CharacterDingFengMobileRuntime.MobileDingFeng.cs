#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDingFengMobileRuntime
{
    SkillProgramStepOutcome IMobileFenxunHost.Grant(ProgramSkillFrame frame) => GrantMobileFenxun(frame);
    internal SkillProgramStepOutcome GrantMobileFenxun(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 2 || f.TriggerId is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || f.SelectedCardIds is not [var card] || f.SelectedCardPaymentResult is not { Completed: true, InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected } || f.SelectedCardPayment is not { MovementCommitted: true, InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, CardIds: [var paid] } || paid != card || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileFenxunDistanceGrantedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Mobile Fenxun requires the original completed one-card discard receipt.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileFenxunDistanceGrantedEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, target, card));
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 75600)]
    internal RuleQueryEvaluation ApplyMobileFenxunDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.GrantMobileFenxunDistance))
            return original;
        var history = _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).ToArray();
        if (history.OfType<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat))
            return original;
        var grants = history.OfType<MobileFenxunDistanceGrantedEvent>().Where(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.Source.OwnerSeat == source.Seat && e.TargetSeat == target.Seat).ToArray();
        if (grants.Length == 0)
            return original;
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue), [new RuleQueryBaseTerm("distance:before-mobile-fenxun", CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(original))], grants.Select(g => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-fenxun:{(g.ProgramFrameId)}", SkillRuleOperation.Set, 1)).ToArray());
    }
}
