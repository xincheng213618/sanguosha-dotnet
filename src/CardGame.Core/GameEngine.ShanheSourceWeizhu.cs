namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24910)]
    private bool CanOfferShanheSourceWeizhu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger,
        ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != "shanhe:source-weizhu-14034") return true;
        return context is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0, TargetSeat: { } target } &&
            IsValidPlayerSeat(target) && _players[target].IsAlive &&
            OtherRevealedPublicTeamAlly(_players[candidate.OwnerSeat], _players[target]) &&
            GetSelfDiscardableOwnedHand(_players[candidate.OwnerSeat]).Count != 0 &&
            _resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId) is
                { Prevented: false } parent && parent.TargetSeat == target && parent.Amount == context.Amount;
    }
}
