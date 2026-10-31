#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawAttemptServices(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries> getCoreGameEngineNativeDrawAttemptsRuntime,global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState,global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime,global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState,global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime,global::System.Func<global::CardGame.Core.ProgramPipelineHooks> getProgramPipelineHooks,global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries> getCoreHappyWeiYanGainDestinationsRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiOlMagicEntries> getCharacterZhangFeiOlMagicRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime) : INativeDrawAttemptQueries, INativeDrawAttemptOperations
{
    public bool HasAttemptCapability => getCoreGameEngineNativeDrawAttemptsRuntime().HasNativeDrawAttemptCapability;
    public bool SetupComplete => matchSetupState.ReferenceToSetupComplete;

    public bool IsAlive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int TurnOwnerSeat => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public ActualDiscardRecoveryPhaseKey? ActualPhase => getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase();
    public long MovementSequence => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().OutsidePhaseMovementSequence;

    public SkillProgramTriggerFacts CaptureTriggerFacts(int seat) => getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(matchParticipantsState.ReferenceToPlayers[seat]);
    public IReadOnlyList<ProgramTriggerCandidate> CollectCandidates(int seat, SkillProgramTriggerFacts facts) => getProgramPipelineHooks().IncludeNativeDrawCandidates(seat, getCoreGameEngineProgramLifecycleRuntime().CollectEligibleProgramTriggerCandidates(matchParticipantsState.ReferenceToPlayers[seat], SkillProgramTriggerWindow.NativeDrawStarting, facts));
    public IEnumerable<CardMovementRecord> Movements()
    {
        foreach (var movement in cardMovementJournalState.ReferenceToCardMovements)
            yield return movement;
    }

    public bool IsRedirectedGain(CardMovementRecord movement, CardLocation expectedTo) => getCoreHappyWeiYanGainDestinationsRuntime().IsHappyWeiYanRedirectedGain(movement, expectedTo);
    public NativeDrawPileSlashProof? DrawPileSlashReplacementProof(ProgramSkillFrame frame, NativeDrawAttemptContext context) => getCharacterZhangFeiOlMagicRuntime().NativeDrawPileSlashReplacementProof(frame, context);
    public bool IsIssuedDrawPileSlashReplacement(NativeDrawAttemptContext context, NativeDrawReplacementResult replacement) => getCharacterZhangFeiOlMagicRuntime().IsIssuedNativeDrawPileSlashReplacement(context, replacement);
    public void CompleteLingkongReplacement(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement) => getCharacterZhouFeiLeRuntime().CompleteLingkongNativeDrawReplacement(frame, attempt, replacement);
}
