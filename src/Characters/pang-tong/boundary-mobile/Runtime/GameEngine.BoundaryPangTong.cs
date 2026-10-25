namespace CardGame.Core;

public sealed partial class GameEngine
{
    // The proactive activation and dying binding consume the same native game
    // usage key. The accepted limited effect survives source loss in its children.
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 65400)]
    private bool KeepBoundaryPangTongIssuedNiepan(ProgramSkillFrame f) => f.SkillId == "boundary:niepan-mobile" && f.InstructionIndex is >= 1 and <= 5 &&
        _skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, "named-group:rebirth", SkillUsageScope.Game) == 1 &&
        (f.TriggerId is null && f.ActivationId == "rebirth" && ProgramEventHistory<ProgramSkillStartedEvent>().Count(e =>
            e.FrameId == f.Id && e.SkillId == f.SkillId && e.OwnerSeat == f.OwnerSeat && e.ActivationId == f.ActivationId) == 1 ||
         f.TriggerId == "dying-rebirth" && f.WindowContext is { Window: SkillProgramTriggerWindow.SelfDyingResponse, TargetSeat: var target } && target == f.OwnerSeat &&
            ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId &&
                e.OwnerSeat == f.OwnerSeat && e.BindingId == f.TriggerId && e.Window == SkillProgramTriggerWindow.SelfDyingResponse) == 1);
}
