#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct PlayerViewMatchQueries(global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreGameEngineCardUseDebitsState gameEngineCardUseDebitsState, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::System.Func<global::CardGame.Core.CommandSession> getCommandSession, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime) : IPlayerViewMatchQueries
{
    public ContentModeDefinition ModeDefinition => matchDefinitionState.ReferenceToModeDefinition;
    public int PlayerCount => matchParticipantsState.ReferenceToPlayers.Count;

    public CharacterState PlayerAt(int index) => matchParticipantsState.ReferenceToPlayers[index];
    public int FrameCount => getFrameStore().Count;

    public ResolutionFrame FrameAt(int index) => getFrameStore()[index];
    public ResolutionFrame? LastFrame => getFrameStore().LastOrDefault();
    public IEnumerable<ResolutionFrame> Frames => getFrameStore();
    public GameOptions Options => matchDefinitionState.ReferenceToOptions;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public TurnPhase Phase => actualTurnProgressState.ReferenceToPhase;
    public int CardUseDebitPhaseInstanceId => gameEngineCardUseDebitsState.ReferenceToCardUseDebitPhaseInstanceId;
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public long Revision => getCommandSession().Revision;
    public int? WinnerSeat => matchOutcomeState.ReferenceToWinnerSeat;
    public string? WinnerTeamId => matchOutcomeState.ReferenceToWinnerTeamId;
    public string? WinnerFactionId => matchOutcomeState.ReferenceToWinnerFactionId;

    public IReadOnlyList<int> CurrentTableOrder() => getCoreGameEngineTableOrderRuntime().CurrentTableOrder();
    public CardUseFrame? ActiveFireAttack => getCoreNativeCardContinuationCursorsRuntime().ActiveFireAttack;
    public IPlayerViewGroupCardQueries? ActiveGroupCard => getCoreNativeCardContinuationCursorsRuntime().ActiveGroupCard is { } group ? new global::CardGame.Core.NativeOwners.PlayerViewGroupCardQueries(group) : null;
}
