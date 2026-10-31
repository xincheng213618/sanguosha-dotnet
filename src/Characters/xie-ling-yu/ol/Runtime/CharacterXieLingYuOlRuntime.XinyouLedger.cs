#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXieLingYuOlRuntime
{
    internal ActualDiscardRecoveryPhaseKey? XinyouActualPlay(int owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == owner && p.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) ? p : null;
    internal bool XinyouPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<XinyouPaidEvent>().Any(e => e.OwnerSeat == owner && e.PhaseToken == phase.Token);
    internal IReadOnlyList<Card> XinyouCostCards(int owner) => _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c)) == SkillProgramCardCategory.Basic).ToArray();
    internal bool IsXinyouHandLimitExempt(int ownerSeat, int cardId) => HasXieLingYuCapability && _coreProgramEventHistoryOperations.ProgramEventHistory<XinyouGainedEntityEvent>().Any(e => e.OwnerSeat == ownerSeat && e.CardId == cardId && e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Hand(ownerSeat);
    // Registration only; the query still belongs to legacy GameEngine.Xinyou.Ledger.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 240)]
    internal void ProjectXinyouCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(_coreMatchParticipantsPlayersAccess._players.Any(player => HasXinyouCardMark(player.Seat, cardId)) ? "心幽" : null);
    }

    internal bool HasXinyouCardMark(int ownerSeat, int cardId) => IsXinyouHandLimitExempt(ownerSeat, cardId);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 27582, SkillProgramEffectOp.ResolveXinyouBasicCycle)]
    internal bool CanActivateXinyou(ProgramActivationLegalityContext c) => c.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && XinyouActualPlay(c.OwnerSeat)is { } phase && !XinyouPhaseSpent(c.OwnerSeat, phase) && XinyouCostCards(c.OwnerSeat).Count > 0;
}
