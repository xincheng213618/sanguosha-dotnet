#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepFacts(global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreGameEngineStrategicProgramsState gameEngineStrategicProgramsState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IMatchStepFacts
{
    public int FrameCount => getFrameStore().Count;
    public DyingFrame? ActiveDying => getCoreDyingCursorRuntimeRuntime().ActiveDying;
    public bool HasDamageTrigger => getCoreDamageCursorRuntimeRuntime().ActiveDamageTrigger is not null;
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
