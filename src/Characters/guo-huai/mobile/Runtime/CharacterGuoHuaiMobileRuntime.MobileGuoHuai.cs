#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoHuaiMobileRuntime
{
    internal bool CanRunMobileGuoHuaiJingce(int ownerSeat) => _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && ownerSeat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[ownerSeat] is { IsAlive: true, HasHpValue: true } owner && _coreActualPlayPhaseCardUsesOperations.GetActualPlayPhaseUseCount(ownerSeat) >= owner.Hp;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 75000, SkillProgramEffectOp.PrepareMobileGuoHuaiJingce)]
    internal bool QualifyMobileGuoHuaiJingce(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.PlayEnding && CanRunMobileGuoHuaiJingce(candidate.OwnerSeat);
    void IMobileGuoHuaiHost.Prepare(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.PlayEnding || !CanRunMobileGuoHuaiJingce(frame.OwnerSeat))
            throw new InvalidOperationException("Jingce requires enough actual uses in its owner's current Play phase.");
    }
}
