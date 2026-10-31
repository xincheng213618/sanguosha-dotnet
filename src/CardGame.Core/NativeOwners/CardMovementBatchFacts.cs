#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardMovementBatchFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchSetupState matchSetupState, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<FrameStore> getFrameStore) : ICardMovementBatchFacts
{
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;
    public ProgramSkillFrame? LastProgram => getFrameStore().OfType<ProgramSkillFrame>().LastOrDefault();
}
