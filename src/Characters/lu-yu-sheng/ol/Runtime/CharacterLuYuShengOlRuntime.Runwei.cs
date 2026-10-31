#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuYuShengOlRuntime
{
    internal void IssueLuYuShengRunwei(ProgramSkillFrame f, int delta)
    {
        var r = f.LuYuSheng!;
        if (r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } p || p.ActorSeat != r.Payer || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase() != p || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) || r.Operation != SkillProgramEffectOp.RunweiDrawOrDiscardForActualPhase || r.Branch != (delta < 0 ? "draw" : "discard") || _coreProgramEventHistoryOperations.ProgramEventHistory<LuYuShengRunweiPhaseIssuedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Runwei requires its exact accepted real Discard phase and one committed direction.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengRunweiPhaseIssuedEvent(f.Id, r.Source, p, delta));
    }

    internal int LuYuShengRunweiHandLimitBonus(int actorSeat) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Discard && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Discard } p && p.ActorSeat == actorSeat ? _coreProgramEventHistoryOperations.ProgramEventHistory<LuYuShengRunweiPhaseIssuedEvent>().Where(e => e.Phase == p).Sum(e => e.Delta) : 0;
}
