namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 58000, SkillProgramEffectOp.Draw)]
    private bool CanRunBoundaryZhouTaiFenji(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != "boundary:fenji-mobile") return true;
        return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.SourceSeat is { } ended &&
            context.TargetSeat == ended && IsValidPlayerSeat(ended) && _players[ended].IsAlive && GetHand(_players[ended]).Count == 0 &&
            _resolutionStack.OfType<DeferredTurnEndFrame>().Any(parent => parent.Id == context.ParentFrameId && parent.OwnerSeat == ended && IsActualAfterTurnEndedParent(parent));
    }
}
