#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuYuShengOlRuntime
{
    internal void IssueLuYuShengRunwei(ProgramSkillFrame f, int delta)
    {
        var r = f.LuYuSheng!;
        if (r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } p || p.ActorSeat != r.Payer || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() != p || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) || r.Operation != SkillProgramEffectOp.RunweiDrawOrDiscardForActualPhase || r.Branch != (delta < 0 ? "draw" : "discard") || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LuYuShengRunweiPhaseIssuedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Runwei requires its exact accepted real Discard phase and one committed direction.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengRunweiPhaseIssuedEvent(f.Id, r.Source, p, delta));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.AppendNumericRuleContributions, 135)]
    internal void AddRunweiHandLimitContribution(int seat, SkillRuleQuery query, List<RuleQueryContribution> contributions)
    {
        if (query != SkillRuleQuery.HandLimit || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Discard ||
            _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase ||
            phase.ActorSeat != seat)
            return;

        var adjustment = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<LuYuShengRunweiPhaseIssuedEvent>()
            .Where(grant => grant.Phase == phase)
            .Sum(grant => grant.Delta);
        CoreGameEngineRuleQueriesRuntime.AddFiniteContribution(contributions, $"issued:{seat}:lu-yu-sheng:runwei:hand-limit", adjustment);
    }
}
