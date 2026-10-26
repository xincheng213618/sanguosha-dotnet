using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed record ShanheSchoolHpAdjustedEvent(long FrameId, int OwnerSeat, int Before, int After, long? ChangeId) : IGameEvent;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheSchoolVitalCapability(GameEngine engine) : IShanheSchoolVitalHost
    {
        public void SetSchoolHpToMaximum(ProgramSkillFrame frame) => engine.SetShanheSchoolHpToMaximum(frame);
    }

    private void SetShanheSchoolHpToMaximum(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheSchoolEffects.FullHpSkillId || frame.TriggerId != "preparation-hp-adjustment" ||
            frame.InstructionIndex != 1 || frame.OwnerSeat != _currentSeat || !HasShanheSchoolEffect(frame.OwnerSeat, 5) ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } context ||
            context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat ||
            _resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId) is not ProgramLifecycleTriggerWindowFrame
                { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, Continuation: ProgramLifecycleContinuation.NormalTurnStart } parent ||
            parent.OwnerSeat != frame.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            !MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) ||
            _resolutionStack.Count < 2 || _resolutionStack[^2].Id != parent.Id ||
            ProgramEventHistory<ShanheSchoolHpAdjustedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("School HP adjustment requires its completed owner's original preparation instruction.");

        var owner = _players[frame.OwnerSeat];
        if (!owner.IsAlive || !owner.HasHpValue || owner.Hp == owner.MaxHp) return;
        var before = owner.Hp;
        owner.Hp = owner.MaxHp;
        // Adjustment emits health-change rules only. It is not a recovery attempt,
        // so recovery replacement and after-recovery rewards cannot change it.
        var change = RecordHpChange(frame.Id, frame.OwnerSeat, frame.OwnerSeat, before, owner.Hp, HpChangeKind.Setting);
        AdvanceEventRulesAndQueueFact(new ShanheSchoolHpAdjustedEvent(frame.Id, frame.OwnerSeat, before, owner.Hp, change?.Id));
    }
}
