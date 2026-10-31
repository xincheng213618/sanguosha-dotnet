#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDianWeiBoundaryMobileRuntime
{
    void IMobileDianWeiProgramHost.ConsumeTarget(ProgramSkillFrame frame) => ConsumeMobileDianWeiTarget(frame);
    internal void ConsumeMobileDianWeiTarget(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileDianWeiQiangxi.Skill || frame.TriggerId is not null || frame.InstructionIndex != 1 || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || frame.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreProgramDodgeResponsesOperations.IsWithinAttackRange(frame.OwnerSeat, target) || !_coreProgramParticipantPhaseOperationsOperations.CanActivateTargetPhaseLedger(frame.OwnerSeat, frame.SkillId, MobileDianWeiQiangxi.Ledger, target))
            throw new InvalidOperationException("Mobile Qiangxi lost its original in-range unvisited target in this actual Play phase.");
        var key = $"{(MobileDianWeiQiangxi.Ledger)}:target:{(target)}";
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, key, SkillUsageScope.Phase, 1))
            throw new InvalidOperationException("Mobile Qiangxi cannot repeat this target in the same Play phase.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(frame.OwnerSeat, frame.SkillId, key, SkillUsageScope.Phase, 1));
    }
}
