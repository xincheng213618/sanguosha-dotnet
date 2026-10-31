#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DingShangWanNativeFacts(EngineStates.CoreMatchOutcomeState matchOutcomeState,EngineStates.CoreCardMovementJournalState cardMovementJournalState,Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime,Func<RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime,Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime,Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime,EngineStates.CoreMatchParticipantsState matchParticipantsState,Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime,Func<RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries> getCoreGameEngineAfterTurnEndedProgramsRuntime,Func<RuntimeEntries.CoreGameEngineResponseCompletionEntries> getCoreGameEngineResponseCompletionRuntime,Func<RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime,Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime,Func<RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime) : IDingShangWanNativeFacts
{
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public long MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public bool CurrentDamageHasSource => getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt is { IsSourceLess: false };
    public DyingFrame? ActiveDying => getCoreDyingCursorRuntimeRuntime().ActiveDying;

    public bool ValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool HasSkillInstance(int seat, string skill, string instance) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skill, instance);
    public SkillProgramTrigger Trigger(ProgramSkillFrame frame) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger(frame);
    public bool IsActualAfterTurnEndedParent(DeferredTurnEndFrame frame) => getCoreGameEngineAfterTurnEndedProgramsRuntime().IsActualAfterTurnEndedParent(frame);
    public ProgramTriggerCandidate AfterTurnEndedCandidate(DeferredTurnEndFrame frame) => getCoreGameEngineAfterTurnEndedProgramsRuntime().AfterTurnEndedCandidate(frame);
    public bool ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame frame) => getCoreGameEngineResponseCompletionRuntime().ValidResponseCompletionWindow(frame);
    public bool RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(child, parent);
    public bool MovementObserverEdge(int child) => getCoreGameEngineObserverSubtreesRuntime().IsMovementObserverEdge(child);
    public bool DyingExitEdge(int child) => getCharacterYanPuOlRuntime().IsYanPuDyingExitEdge(child);
    public bool SuspendedOrPaidRescueSuffix(int child, DyingFrame dying) => getCoreGameEngineObserverSubtreesRuntime().IsSuspendedOrPaidRescueObserverSuffix(child, dying);
}
