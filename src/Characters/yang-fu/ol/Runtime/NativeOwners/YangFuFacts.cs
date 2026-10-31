#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangFuFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramEventHistoryRuntime>> getCoreGameEngineProgramEventHistoryRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEnginePrivateOfferReturnsEntries> getCoreGameEnginePrivateOfferReturnsRuntime, Func<RuntimeEntries.CoreGameEngineCardUseDebitsEntries> getCoreGameEngineCardUseDebitsRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry) : IYangFuFacts
{
    public YangFuPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].Name, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman, matchParticipantsState.ReferenceToPlayers[seat].IsChained);
    public IReadOnlyList<YangFuPlayer> Players => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => Player(player.Seat)).ToArray());

    public bool ValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public IReadOnlyList<CardMovementRecord> Movements => cardMovementJournalState.ReferenceToCardMovements.AsReadOnly();

    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().Value.ProgramEventHistory<T>();
    public CardLocation Location(int cardId) => getCardZoneStore().GetLocation(cardId);
    public CardUseFrame? CardUse(long useId) => getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(useId);
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public SkillProgramEffect? PausedEffect(ProgramSkillFrame frame) => getCoreGameEnginePrivateOfferReturnsRuntime().GetPausedPrivateOfferEffect(frame);
    public bool HasTriggerOperation(SkillProgramEffectOp operation) => getContentRegistry().ProgramDependencies.HasTriggerOperation(operation);
    public bool ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right) => EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(left, right);
    public bool Allies(int owner, int target) => EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(matchParticipantsState.ReferenceToPlayers[owner], matchParticipantsState.ReferenceToPlayers[target]);
    public bool HasUseDebit(long actionId) => getCoreGameEngineCardUseDebitsRuntime().GetCardUseDebit(actionId)is not null;
    public bool IsEquipment(CardKind kind) => getEquipmentContentRegistry().IsEquipment(kind);
}
