#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouGuoJiaFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CharacterGuoJiaMouEntries> getCharacterGuoJiaMouRuntime) : IMouGuoJiaFacts
{
    public int Turn => actualTurnProgressState.ReferenceToTurnNumber;
    public long Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool Finished => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool Alive(int s) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(s) && matchParticipantsState.ReferenceToPlayers[s].IsAlive;
    public bool Human(int s) => matchParticipantsState.ReferenceToPlayers[s].IsHuman;
    public string Name(int s) => matchParticipantsState.ReferenceToPlayers[s].Name;
    public SkillProgram? Program(string id) => getContentRegistry().Skills.GetValueOrDefault(id)?.Program;
    public IEnumerable<T> History<T>()
        where T : IGameEvent => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<T>();
    public bool Yin(int s, string instance) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(s, MouGuoJiaRuntime.Xianmou, instance, "yin");
    public IReadOnlyList<int> Living => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());

    public IReadOnlyList<MouGjEntity> Hand(int s) => Array.AsReadOnly(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[s]).Select(c => new MouGjEntity(c.Id, c.Kind, CardLocation.Hand(s), c.DisplayName)).ToArray());
    public bool At(MouGjEntity c) => getCardZoneStore().GetLocation(c.Id) == c.From;
    public bool Discardable(int s, int id) => getCardZoneStore().GetLocation(id) == CardLocation.Hand(s) && !getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(s, getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), CardLocation.Hand(s), OwnedCardMoveIntent.Discard);
    public int LossCount(int s) => cardMovementJournalState.ReferenceToCardMovements.Count(m => m.TurnNumber == actualTurnProgressState.ReferenceToTurnNumber && m.From.OwnerSeat == s && m.From != m.To && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment);
    public IReadOnlyList<(int Seat, string Instance)> XianmouOwners => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && getCoreGameEngineSkillProgramsRuntime().EnabledSkillPrograms(p).Any(s => s.Id == MouGuoJiaRuntime.Xianmou)).Select(p => (p.Seat, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(p, MouGuoJiaRuntime.Xianmou))).ToArray());

    public bool Parent(ProgramSkillFrame f) => getCharacterGuoJiaMouRuntime().MouGjParent(f);
}
