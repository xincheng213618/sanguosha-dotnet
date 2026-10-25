namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class MobileFenxunHost(GameEngine engine) : IMobileFenxunHost
    { public SkillProgramStepOutcome Grant(ProgramSkillFrame frame) => engine.GrantMobileFenxun(frame); }
    private SkillProgramStepOutcome GrantMobileFenxun(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 2 || f.TriggerId is not null || f.OwnerSeat != _currentSeat || _phase != TurnPhase.Play ||
            f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || f.SelectedCardIds is not [var card] ||
            f.SelectedCardPaymentResult is not { Completed: true, InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected } ||
            f.SelectedCardPayment is not { MovementCommitted: true, InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, CardIds: [var paid] } || paid != card ||
            ProgramEventHistory<MobileFenxunDistanceGrantedEvent>().Any(e => e.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Mobile Fenxun requires the original completed one-card discard receipt.");
        if (_winner != Winner.None || !_players[f.OwnerSeat].IsAlive || !_players[target].IsAlive) return SkillProgramStepOutcome.Continue;
        AdvanceEventRulesAndQueueFact(new MobileFenxunDistanceGrantedEvent(f.Id,
            new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _turnNumber, _currentSeat, target, card));
        return SkillProgramStepOutcome.Continue;
    }
    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 75600)]
    private RuleQueryEvaluation ApplyMobileFenxunDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat || !_contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.GrantMobileFenxunDistance)) return original;
        var history = EventsSinceLastBoundary(e => e is TurnStartedEvent).ToArray();
        if (history.OfType<TurnEndedEvent>().Any(e => e.TurnNumber == _turnNumber && e.ActorSeat == _currentSeat)) return original;
        var grants = history.OfType<MobileFenxunDistanceGrantedEvent>().Where(e => e.ActualTurnNumber == _turnNumber && e.ActualTurnOwnerSeat == _currentSeat &&
            e.Source.OwnerSeat == source.Seat && e.TargetSeat == target.Seat).ToArray();
        if (grants.Length == 0) return original;
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue),
            [new RuleQueryBaseTerm("distance:before-mobile-fenxun", ConvertRuleValue(original))],
            grants.Select(g => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-fenxun:{g.ProgramFrameId}", SkillRuleOperation.Set, 1)).ToArray());
    }
}
