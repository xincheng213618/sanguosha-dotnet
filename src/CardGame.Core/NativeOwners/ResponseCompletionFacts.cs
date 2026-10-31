#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ResponseCompletionFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<FrameStore> getFrameStore, Func<List<CardMovementBatchContext>> getList, EngineStates.CoreGameEngineHpChangeProgramsState gameEngineHpChangeProgramsState, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, Func<RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, DuelHandleFactory createDuelHandle, GroupCardHandleFactory createGroupCardHandle) : IResponseCompletionFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public int PendingMovementBatchCount => getList().Count;

    public CardMovementBatchContext PendingMovementBatchAt(int index) => getList()[index];
    public int PendingHpChangeCount => gameEngineHpChangeProgramsState.ReferenceToPendingHpChanges.Count;

    public HpChangeContext PendingHpChangeAt(int index) => gameEngineHpChangeProgramsState.ReferenceToPendingHpChanges[index];
    public bool ContainsMovement(CardMovementRecord movement) => cardMovementJournalState.ReferenceToCardMovements.Contains(movement);
    public CardMovementRecord? LastMovement(Func<CardMovementRecord, bool> predicate) => cardMovementJournalState.ReferenceToCardMovements.LastOrDefault(predicate);
    public bool HasTriggerWindow(SkillProgramTriggerWindow window) => getContentRegistry().ProgramDependencies.HasTriggerWindow(window);
    public bool UsesTriggerCondition(SkillProgramTriggerConditionKind condition) => getContentRegistry().ProgramDependencies.UsesTriggerCondition(condition);
    public bool IsValidPlayerSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool IsAlive(int playerIndex) => matchParticipantsState.ReferenceToPlayers[playerIndex].IsAlive;
    public int PlayerSeat(int playerIndex) => matchParticipantsState.ReferenceToPlayers[playerIndex].Seat;
    public SkillProgramTriggerFacts CaptureProgramTriggerFacts(int playerIndex) => getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(matchParticipantsState.ReferenceToPlayers[playerIndex]);
    public IReadOnlyList<ProgramTriggerCandidate> CollectProgramTriggerCandidates(int playerIndex, SkillProgramTriggerWindow window) => getCoreGameEngineProgramLifecycleRuntime().CollectProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[playerIndex], window);
    public SkillProgramTrigger GetProgramTrigger(ProgramTriggerCandidate candidate) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger2(candidate);
    public SkillProgram? Program(string skillId) => getContentRegistry().Skills.GetValueOrDefault(skillId)?.Program;
    public SkillProgramViewAs? ViewAsRule(CardConversionSource source) => getCoreGameEnginePlayCardHistoryRuntime().ViewAsRule(source);
    public IEnumerable<IGameEvent> CompleteProgramEventHistory() => getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory();
    public NullificationWindowFrame? ActiveNullificationWindow => getCoreNativeCardContinuationCursorsRuntime().ActiveNullificationWindow;
    public long? ActiveDuelOwnerId => getCoreNativeCardContinuationCursorsRuntime().ActiveDuel?.ResolutionId;

    public int DuelOpponentSeat(long ownerId) => createDuelHandle(ownerId).OpponentSeat;
    public int DuelResponderSeat(long ownerId) => createDuelHandle(ownerId).ResponderSeat;
    public long? ActiveGroupOwnerId => getCoreNativeCardContinuationCursorsRuntime().ActiveGroupCard?.ResolutionId;

    public int GroupSourceSeat(long ownerId) => createGroupCardHandle(ownerId).SourceSeat;
    public CardKind? GroupRequiredCardKind(long ownerId) => createGroupCardHandle(ownerId).RequiredCardKind;
    public long? GroupCurrentAttackOwnerId(long ownerId) => createGroupCardHandle(ownerId).CurrentAttack?.ResolutionId;
}
