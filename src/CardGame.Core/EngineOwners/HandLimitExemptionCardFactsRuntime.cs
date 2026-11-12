namespace CardGame.Core.EngineOwners;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class HandLimitExemptionCardFactsRuntime(
    IMatchParticipantStateAccess _participants, IProgramEventHistoryQueries _history,
    RuntimeEntries.CoreNativeCardAppearanceEntries _appearance,
    RuntimeEntries.CoreGameEngineAlcoholIdentityPoliciesEntries _identities,
    IKernelTurnCardUseEffectsAccess _turnEffects,
    EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess _setup,
    EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess _turn)
    : IHandLimitExemptionCardFacts, IHandLimitExemptionSequences
{
    public CardKind LegacyHandKind(int holderSeat, Card card) => card.Kind == CardKind.Alcohol &&
        _identities.HasAlcoholKingIdentityPolicy(_participants.At(holderSeat)) ? CardKind.Slash : card.Kind;

    public Suit? EffectiveSuit(int holderSeat, Card card) =>
        _appearance.EffectiveSuit(_participants.At(holderSeat), card);

    public HandLimitTurnStatus TurnStatus => new(_setup._setupComplete,
        _turn._turnProgression.TurnNumber == _turn._turnNumber,
        _turn._turnProgression.OwnerSeat == _turn._currentSeat,
        _history.Read<TurnEndedEvent>().Any(fact => fact.TurnNumber == _turn._turnNumber &&
            fact.ActorSeat == _turn._turnProgression.OwnerSeat),
        _history.Read<ActualTurnCursorReturnedEvent>().Any(fact => fact.ActualTurn == _turn._turnNumber &&
            fact.OwnerSeat == _turn._turnProgression.OwnerSeat));

    public long Next() => _turnEffects._turnCardUseEffects.AllocateHandLimitGrantSequence();
}
