namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool CanRunMobileGuoHuaiJingce(int ownerSeat) =>
        _phase == TurnPhase.Play && ownerSeat == _currentSeat &&
        _players[ownerSeat] is { IsAlive: true, HasHpValue: true } owner &&
        GetActualPlayPhaseUseCount(ownerSeat) >= owner.Hp;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 75000, SkillProgramEffectOp.PrepareMobileGuoHuaiJingce)]
    private bool QualifyMobileGuoHuaiJingce(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
        context.Window == SkillProgramTriggerWindow.PlayEnding && CanRunMobileGuoHuaiJingce(candidate.OwnerSeat);

    [ProgramCapability]
    private sealed class MobileGuoHuaiHost(GameEngine engine) : IMobileGuoHuaiHost
    {
        public void Prepare(ProgramSkillFrame supplied)
        {
            var frame = engine.GetActiveProgramFrame(supplied.Id);
            if (frame.WindowContext?.Window != SkillProgramTriggerWindow.PlayEnding ||
                !engine.CanRunMobileGuoHuaiJingce(frame.OwnerSeat))
                throw new InvalidOperationException("Jingce requires enough actual uses in its owner's current Play phase.");
        }
    }
}
