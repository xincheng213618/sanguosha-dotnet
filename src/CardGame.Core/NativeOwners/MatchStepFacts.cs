#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepFacts(Func<FrameStore> getFrameStore, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreDyingCursorRuntimeRuntime>> getCoreDyingCursorRuntimeRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreDamageCursorRuntimeRuntime>> getCoreDamageCursorRuntimeRuntime, Func<RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, EngineStates.CoreMatchSetupState matchSetupState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreGameEngineStrategicProgramsState gameEngineStrategicProgramsState, EngineStates.CoreMatchDefinitionState matchDefinitionState, EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IMatchStepFacts
{
    public int FrameCount => getFrameStore().Count;
    public DyingFrame? ActiveDying => getCoreDyingCursorRuntimeRuntime().Value.ActiveDying;
    public bool HasDamageTrigger => getCoreDamageCursorRuntimeRuntime().Value.ActiveDamageTrigger is not null;
    public bool HasGroupRecovery => getCoreNativeCardContinuationCursorsRuntime().ActiveGroupCard is { Effect: GroupCardEffect.Recovery };
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public bool GameStartingProgramsResolved => matchSetupState.ReferenceToGameStartingProgramsResolved;
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int StrategicEndPlayTurn => gameEngineStrategicProgramsState.ReferenceToStrategicEndPlayTurn;
    public bool UseInteractiveDiscard => matchDefinitionState.ReferenceToOptions.UseInteractiveDiscard;
    public int Seed => matchDefinitionState.ReferenceToOptions.Seed;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;

    public bool IsHuman(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsHuman;
    public MatchStepPlayer Player(int seat) => new(matchParticipantsState.ReferenceToPlayers[seat].IsAlive, matchParticipantsState.ReferenceToPlayers[seat].IsHuman);
    public string GeneralId(int seat) => matchParticipantsState.ReferenceToPlayers[seat].General.Id;
}
