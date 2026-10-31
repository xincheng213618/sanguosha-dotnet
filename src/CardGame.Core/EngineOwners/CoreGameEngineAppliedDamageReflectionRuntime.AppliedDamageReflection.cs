#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAppliedDamageReflectionRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IAppliedDamageReflectionHost.ReflectAppliedDamage(ProgramSkillFrame frame) => BeginAppliedDamageReflection(frame);
    internal SkillProgramStepOutcome BeginAppliedDamageReflection(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == active.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame window || active.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || context.ParentFrameId != window.Id || context.DamageFrameId != window.ParentFrameId || context.OwnerSeat != active.OwnerSeat || context.TargetSeat != active.OwnerSeat || window.TargetSeat != active.OwnerSeat || window.TriggerWindow != context.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(active, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Applied-damage reflection lost its exact owner window and current candidate.");
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        var original = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(item => item.Id == window.ParentFrameId);
        if (original is null || original.ParentFrameId != attack.ResolutionId || original.SourceSeat != attack.SourceSeat || original.TargetSeat != attack.TargetSeat || original.Amount != attack.DamageAmount || original.Nature != _coreNativeCardClassificationsOperations.GetDamageNature(attack) || !attack.DamageWasApplied || original.Amount <= 0 || context.Amount != original.Amount || context.SourceSeat != (attack.IsSourceLess ? (int? )null : attack.SourceSeat))
            throw new InvalidOperationException("Applied-damage reflection requires the unchanged positive native damage segment.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId) || context.SourceSeat is not { } source || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive)
            return SkillProgramStepOutcome.Continue;
        // Copy the applied segment, including damage absorbed by numerical armor.
        // A fresh cardless skill attack inherits normal native modifiers and children.
        return _coreSkillProgramsOperations.BeginProgramSkillDamage(active, source, original.Amount, nature: original.Nature);
    }
}
