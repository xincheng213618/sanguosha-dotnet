#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouWangLingFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, global::CardGame.Core.EngineStates.CoreAiBrainsState aiBrainsState, global::CardGame.Core.NativeOwners.KernelCreateSnapshotCall createSnapshot, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWangLingMouEntries> getCharacterWangLingMouRuntime) : IMouWangLingFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool GameOver => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool Human(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public int MaximumHp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].MaxHp;
    public string Name(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Name;
    public SkillProgram Program(string id) => getContentRegistry().GetSkill(id).Program!;
    public CardLocation Location(int id) => getCardZoneStore().GetLocation(id);
    public IReadOnlyList<Card> Hand(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]);
    public CardKind HandKind(int seat, int id) => getCoreGameEngineAdvancedLifecycleRuntime().AdvancedEffectiveHandKind(matchParticipantsState.ReferenceToPlayers[seat], getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id));
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public IReadOnlyList<CardMovementRecord> Movements(long before, long after) => Array.AsReadOnly(cardMovementJournalState.ReferenceToCardMovements.Where(m => m.Sequence > before && m.Sequence <= after).ToArray());
    public double KeepValue(int owner, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), matchParticipantsState.ReferenceToPlayers[owner]);
    public double TargetValue(int owner, int target) => aiBrainsState.ReferenceToAiBrains[owner].ScoreProgramTarget(createSnapshot(owner), target, new(0, 0, 0, 0, 1, 0, true, false));
    public bool ParentMatches(ProgramSkillFrame f) => getCharacterWangLingMouRuntime().MouWangLingParentMatches(f);
}
