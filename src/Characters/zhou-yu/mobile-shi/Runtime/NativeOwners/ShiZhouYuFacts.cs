#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhouYuFacts(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<RuntimeEntries.CoreNativeCardClassificationsEntries> getCoreNativeCardClassificationsRuntime) : IShiZhouYuFacts
{
    public ShiZhouYuPlayer Player(int seat) => new(seat, matchParticipantsState.ReferenceToPlayers[seat].Name, matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman, matchParticipantsState.ReferenceToPlayers[seat].IsChained);
    public IReadOnlyList<ShiZhouYuPlayer> Players => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => Player(player.Seat)).ToArray());

    public bool ValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public IReadOnlyList<Card> Equipment(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]);
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int Phase => (int)actualTurnProgressState.ReferenceToPhase;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public bool HasTriggerOperation(SkillProgramEffectOp operation) => getContentRegistry().ProgramDependencies.HasTriggerOperation(operation);
    public ContentSkillDefinition Skill(string skillId) => getContentRegistry().GetSkill(skillId);
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public CardConversionSource Source(ProgramSkillFrame frame) => new(frame.SkillId, EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    public CardLocation Location(int cardId) => getCardZoneStore().GetLocation(cardId);
    public IReadOnlyList<Card> CardsAt(CardLocation location) => getCardZoneStore().CardsAt(location);
    public bool IsRedCard(int owner, Card card) => EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[owner], card));
    public bool Allies(int owner, int target) => EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(matchParticipantsState.ReferenceToPlayers[owner], matchParticipantsState.ReferenceToPlayers[target]);
    public int KeepValue(int owner, Card card) => EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(card, matchParticipantsState.ReferenceToPlayers[owner]);
    public bool ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right) => EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(left, right);
    public ShiZhouYuDamage? Damage => getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt is { } attack ? new(attack.ResolutionId, attack.TargetSeat, getCoreNativeCardClassificationsRuntime().GetDamageNature(attack)) : null;
}
