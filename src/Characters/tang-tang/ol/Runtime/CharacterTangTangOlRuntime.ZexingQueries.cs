#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangTangOlRuntime
{
    internal bool ZexingVisited(int owner, int peer) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZexingPeerChosenEvent>().Any(e => e.OwnerSeat == owner && e.Peer == peer && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
    internal bool ZexingShownStillOwned(ProgramSkillFrame f) => f.TangTang?.Shown is { } card && TangTangTransferCards(f.OwnerSeat, f.OwnerSeat).Any(c => c.Id == card.CardId);
    internal IReadOnlyList<TangTangEntity> ZexingTakeCandidates(ProgramSkillFrame f) => f.TangTang is { Shown: { } shown, Peer: >= 0 } r ? TangTangTransferCards(f.OwnerSeat, r.Peer).Where(c => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.Peer], c) != shown.Suit).Select(c => TangTangEntityAt(r.Peer, c.Id)).ToArray() : [];
    internal bool ZexingTakeAllowed(ProgramSkillFrame f) => f.TangTang is { Peer: >= 0 } r && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Peer].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Peer]).Count >= 2 && ZexingTakeCandidates(f).Count >= 2;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 27530, SkillProgramEffectOp.ResolveZexingShownCardChain)]
    internal bool CanActivateZexing(ProgramActivationLegalityContext c) => c.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == c.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && TangTangTransferCards(c.OwnerSeat, c.OwnerSeat).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat && !ZexingVisited(c.OwnerSeat, p.Seat));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27530)]
    internal void CaptureZexingHpLoss(ProgramSkillHpLostEvent e)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { TangTang: { Stage: TangTangStage.HpIssuing, Paid: true } r } f || e.FrameId != f.Id || e.SkillId != f.SkillId || e.TargetSeat != r.Peer || e.Amount != 1)
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { TangTang = r with { Stage = TangTangStage.HpChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZexingHpLossIssuedEvent(f.Id, r.Peer, e.RemainingHp + e.Amount, e.RemainingHp));
    }
}
