namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class MobileDianWeiProgramCapability(GameEngine engine) : IMobileDianWeiProgramHost
    { public void ConsumeTarget(ProgramSkillFrame frame) => engine.ConsumeMobileDianWeiTarget(frame); }

    private void ConsumeMobileDianWeiTarget(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileDianWeiQiangxi.Skill || frame.TriggerId is not null || frame.InstructionIndex != 1 ||
            frame.OwnerSeat != _currentSeat || _phase != TurnPhase.Play || frame.SelectedTargetSeats is not [var target] ||
            target == frame.OwnerSeat || !IsValidPlayerSeat(target) || !_players[target].IsAlive || !IsWithinAttackRange(frame.OwnerSeat, target) ||
            !CanActivateTargetPhaseLedger(frame.OwnerSeat, frame.SkillId, MobileDianWeiQiangxi.Ledger, target))
            throw new InvalidOperationException("Mobile Qiangxi lost its original in-range unvisited target in this actual Play phase.");
        var key = $"{MobileDianWeiQiangxi.Ledger}:target:{target}";
        if (!_skillRuntimeState.TryConsumeUsage(frame.OwnerSeat, frame.SkillId, key, SkillUsageScope.Phase, 1))
            throw new InvalidOperationException("Mobile Qiangxi cannot repeat this target in the same Play phase.");
        AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(frame.OwnerSeat, frame.SkillId, key, SkillUsageScope.Phase, 1));
    }
}
