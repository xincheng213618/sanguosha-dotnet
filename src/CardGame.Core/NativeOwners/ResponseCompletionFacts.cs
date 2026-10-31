#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ResponseCompletionFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext>> getList, global::CardGame.Core.EngineStates.CoreGameEngineHpChangeProgramsState gameEngineHpChangeProgramsState, global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries> getCoreGameEnginePlayCardHistoryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, global::CardGame.Core.NativeOwners.DuelHandleFactory createDuelHandle, global::CardGame.Core.NativeOwners.GroupCardHandleFactory createGroupCardHandle) : IResponseCompletionFacts
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
