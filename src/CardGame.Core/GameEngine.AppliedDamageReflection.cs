namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class AppliedDamageReflectionCapability(GameEngine engine) : IAppliedDamageReflectionHost
    {
        public SkillProgramStepOutcome ReflectAppliedDamage(ProgramSkillFrame frame) =>
            engine.BeginAppliedDamageReflection(frame);
    }

    private SkillProgramStepOutcome BeginAppliedDamageReflection(ProgramSkillFrame frame)
    {
        var active = GetActiveProgramFrame(frame.Id);
        var index = _resolutionStack.FindIndex(item => item.Id == active.Id);
        if (index < 1 || _resolutionStack[index - 1] is not DamageTriggerWindowFrame window ||
            active.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context ||
            context.ParentFrameId != window.Id || context.DamageFrameId != window.ParentFrameId ||
            context.OwnerSeat != active.OwnerSeat || context.TargetSeat != active.OwnerSeat ||
            window.TargetSeat != active.OwnerSeat || window.TriggerWindow != context.Window ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(active, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Applied-damage reflection lost its exact owner window and current candidate.");

        var attack = GetDamageTriggerAttack(window);
        var original = _resolutionStack.OfType<DamageFrame>().SingleOrDefault(item => item.Id == window.ParentFrameId);
        if (original is null || original.ParentFrameId != attack.ResolutionId ||
            original.SourceSeat != attack.SourceSeat || original.TargetSeat != attack.TargetSeat ||
            original.Amount != attack.DamageAmount || original.Nature != GetDamageNature(attack) ||
            !attack.DamageWasApplied || original.Amount <= 0 || context.Amount != original.Amount ||
            context.SourceSeat != (attack.IsSourceLess ? (int?)null : attack.SourceSeat))
            throw new InvalidOperationException("Applied-damage reflection requires the unchanged positive native damage segment.");

        if (_winner != Winner.None || !_players[active.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[active.OwnerSeat], active.SkillId, active.SkillInstanceId) ||
            context.SourceSeat is not { } source || !IsValidPlayerSeat(source) || !_players[source].IsAlive)
            return SkillProgramStepOutcome.Continue;

        // Copy the applied segment, including damage absorbed by numerical armor.
        // A fresh cardless skill attack inherits normal native modifiers and children.
        return BeginProgramSkillDamage(active, source, original.Amount, nature: original.Nature);
    }
}
