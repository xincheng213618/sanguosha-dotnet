#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceWeizhuRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24910)]
    internal bool CanOfferShanheSourceWeizhu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != "shanhe:source-weizhu-14034")
            return true;
        return context is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, Amount: > 0, TargetSeat: { } target } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target]) && _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count != 0 && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { Prevented: false } parent && parent.TargetSeat == target && parent.Amount == context.Amount;
    }
}
