namespace CardGame.Core;

public sealed partial class GameEngine
{
    // ActiveDying uses this pure structural predicate: never call ActiveDying here.
    // Only the already dead owner's original token is hidden while its real
    // program attack runs. An inner victim keeps the ordinary rescue/death flow.
    private bool IsOriginalDyingSuspendedByOwnerDeathDamage(DyingFrame dying)
    {
        if (!IsValidPlayerSeat(dying.VictimSeat) || _players[dying.VictimSeat].IsAlive) return false;
        var dyingIndex = _resolutionStack.FindIndex(item => item.Id == dying.Id);
        for (var index = dyingIndex + 1; dyingIndex >= 0 && index < _resolutionStack.Count; index++)
        {
            if (_resolutionStack[index] is not ProgramSkillFrame { AttackAttempt: not null } program ||
                program.OwnerSeat != dying.VictimSeat || program.WindowContext is not
                    { Window: SkillProgramTriggerWindow.OwnerDied } context || context.TargetSeat != dying.VictimSeat ||
                context.OwnerSeat != dying.VictimSeat) continue;
            var windowIndex = _resolutionStack.FindIndex(item => item.Id == context.ParentFrameId);
            if (windowIndex <= dyingIndex || windowIndex >= index ||
                _resolutionStack[windowIndex] is not ProgramDeathTriggerWindowFrame window || window.OwnerSeat != dying.VictimSeat) continue;
            var deathIndex = _resolutionStack.FindIndex(item => item.Id == window.DeathFrameId);
            if (deathIndex > dyingIndex && deathIndex < windowIndex && _resolutionStack[deathIndex] is DeathFrame death &&
                death.ParentFrameId == dying.Id && death.VictimSeat == dying.VictimSeat && !death.OwnerDiedProgramsResolved) return true;
        }
        return false;
    }

    private bool IsOwnerDeathProgramDamage(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context ||
            context.OwnerSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat ||
            !IsValidPlayerSeat(frame.OwnerSeat) || _players[frame.OwnerSeat].IsAlive) return false;
        var window = _resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().LastOrDefault();
        var death = _resolutionStack.OfType<DeathFrame>().LastOrDefault();
        return window is not null && death is not null && context.ParentFrameId == window.Id &&
            window.DeathFrameId == death.Id && window.OwnerSeat == frame.OwnerSeat && death.VictimSeat == frame.OwnerSeat &&
            !death.OwnerDiedProgramsResolved && (ActiveDying is null || death.ParentFrameId == ActiveDying.FrameId);
    }

    private bool IsOwnerDeathSuspendedDying(long programFrameId) => ActiveDying is not null &&
        _resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.Id == programFrameId) is { } program &&
        IsOwnerDeathProgramDamage(program) &&
        _resolutionStack.OfType<DeathFrame>().LastOrDefault()?.ParentFrameId == ActiveDying.FrameId;
}
