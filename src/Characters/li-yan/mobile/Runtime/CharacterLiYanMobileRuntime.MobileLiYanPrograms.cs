#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiYanMobileRuntime
{
    void IMobileLiYanNextDrawHost.Issue(ProgramSkillFrame frame, int targetSeat, int amount) => IssueMobileLiYanNextDrawBonus(frame, targetSeat, amount);
    internal void IssueMobileLiYanNextDrawBonus(ProgramSkillFrame supplied, int targetSeat, int amount)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.TriggerId is not null || f.SelectedTargetSeats is not [var selected] || selected != targetSeat || f.OwnerSeat == targetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || amount != 1 || f.InstructionIndex < 1 || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.Op != SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileLiYanNextDrawBonusIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == f.InstructionIndex))
            throw new InvalidOperationException("Mobile Duliang lost its original one-time selected-target future Draw instruction.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLiYanNextDrawBonusIssuedEvent(f.Id, f.InstructionIndex, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, targetSeat, amount));
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 77900)]
    internal void AssignMobileLiYanNextDrawBonus(ActualDiscardRecoveryPhaseStartedEvent e)
    {
        if (e.Phase.Kind != ActualDiscardRecoveryPhaseKind.Draw || !_coreMatchParticipantsPlayersAccess._players[e.Phase.ActorSeat].IsAlive || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus))
            return;
        var assigned = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileLiYanNextDrawBonusAssignedEvent>().Select(x => (x.ProgramFrameId, x.InstructionIndex)).ToHashSet();
        foreach (var grant in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileLiYanNextDrawBonusIssuedEvent>().Where(x => x.TargetSeat == e.Phase.ActorSeat && !assigned.Contains((x.ProgramFrameId, x.InstructionIndex))).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileLiYanNextDrawBonusAssignedEvent(grant.ProgramFrameId, grant.InstructionIndex, e.Phase));
    }

    internal int MobileLiYanNextDrawBonus(CharacterState player)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat)
            return 0;
        var assigned = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileLiYanNextDrawBonusAssignedEvent>().Where(x => x.Phase == phase).Select(x => (x.ProgramFrameId, x.InstructionIndex)).ToHashSet();
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileLiYanNextDrawBonusIssuedEvent>().Where(x => x.TargetSeat == player.Seat && assigned.Contains((x.ProgramFrameId, x.InstructionIndex))).Sum(x => x.Amount);
    }
}
