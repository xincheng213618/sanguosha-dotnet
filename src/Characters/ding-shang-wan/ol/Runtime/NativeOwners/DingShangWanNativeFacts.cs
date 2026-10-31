#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DingShangWanNativeFacts(global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState,global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime,global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries> getCoreGameEngineAfterTurnEndedProgramsRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries> getCoreGameEngineResponseCompletionRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime) : IDingShangWanNativeFacts
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
