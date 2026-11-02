#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoSiOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 470)]
    internal bool CanOfferGuoSi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (!ProgramInstructionResolver.Default.Features(t).HasOperation(SkillProgramEffectOp.OfferSidaoCompletedPairSnatch))
            return true;
        return _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && w.CardUse is { } identity && SidaoTargets(identity, c.OwnerSeat) is { Count: > 0 } commonTargets && SidaoActions(c.OwnerSeat, commonTargets).Count > 0;
    }

    private IReadOnlyList<int> SidaoTargets(ProgramCardUseContext identity, int ownerSeat)
    {
        if (_actualCardUseHistory.FindCompletedUse(identity.ParentCardUseFrameId) is not { Completed: true } current || current.Action.ActionId != identity.CardActionId || current.Action.Type != CardActionType.Use || current.Action.ActorSeat != ownerSeat || current.Phase != TurnPhase.Play || current.TurnOwner != ownerSeat)
            return [];
        var pair = _actualCardUseHistory.AcceptedPairAtCompletionFor(identity.CardActionId);
        if (pair.Count != 2 || pair.Any(use => !use.Finalized || !use.Completed))
            return [];
        return Array.AsReadOnly(pair[0].PrimaryTargets!.Intersect(pair[1].PrimaryTargets!).Where(seat => seat != ownerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray());
    }
}
