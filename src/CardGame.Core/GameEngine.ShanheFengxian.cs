namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27981)]
    private bool CanOfferShanheFengxianXiuluo(ProgramTriggerCandidate candidate,
        SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        candidate.SkillId != "shanhe:fengxian-xiuluo" ||
        GetJudgment(_players[candidate.OwnerSeat]).Count > 0;
}
