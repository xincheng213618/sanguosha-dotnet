#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardMovementBatchFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore) : ICardMovementBatchFacts
{
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public EngineStatus Status => matchOutcomeState.ReferenceToStatus;
    public ProgramSkillFrame? LastProgram => getFrameStore().OfType<ProgramSkillFrame>().LastOrDefault();
}
