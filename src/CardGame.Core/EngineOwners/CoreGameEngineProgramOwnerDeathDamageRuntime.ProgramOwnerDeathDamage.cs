#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramOwnerDeathDamageRuntime
{
    internal bool IsOriginalDyingSuspendedByOwnerDeathDamage(DyingFrame dying)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(dying.VictimSeat) || _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive)
            return false;
        var dyingIndex = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == dying.Id);
        for (var index = dyingIndex + 1; dyingIndex >= 0 && index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame { AttackAttempt: not null } program || program.OwnerSeat != dying.VictimSeat || program.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context || context.TargetSeat != dying.VictimSeat || context.OwnerSeat != dying.VictimSeat)
                continue;
            var windowIndex = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == context.ParentFrameId);
            if (windowIndex <= dyingIndex || windowIndex >= index || _resolutionStackOperations._resolutionStack[windowIndex] is not ProgramDeathTriggerWindowFrame window || window.OwnerSeat != dying.VictimSeat)
                continue;
            var deathIndex = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == window.DeathFrameId);
            if (deathIndex > dyingIndex && deathIndex < windowIndex && _resolutionStackOperations._resolutionStack[deathIndex] is DeathFrame death && death.ParentFrameId == dying.Id && death.VictimSeat == dying.VictimSeat && !death.OwnerDiedProgramsResolved)
                return true;
        }

        return false;
    }

    internal bool IsOwnerDeathProgramDamage(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context || context.OwnerSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(frame.OwnerSeat) || _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return false;
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramDeathTriggerWindowFrame>().LastOrDefault();
        var death = _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().LastOrDefault();
        return window is not null && death is not null && context.ParentFrameId == window.Id && window.DeathFrameId == death.Id && window.OwnerSeat == frame.OwnerSeat && death.VictimSeat == frame.OwnerSeat && !death.OwnerDiedProgramsResolved && (_coreDyingCursorRuntimeOperations.ActiveDying is null || death.ParentFrameId == _coreDyingCursorRuntimeOperations.ActiveDying.FrameId);
    }

    internal bool IsOwnerDeathSuspendedDying(long programFrameId) => _coreDyingCursorRuntimeOperations.ActiveDying is not null && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.Id == programFrameId)is { } program && IsOwnerDeathProgramDamage(program) && _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().LastOrDefault()?.ParentFrameId == _coreDyingCursorRuntimeOperations.ActiveDying.FrameId;
}
