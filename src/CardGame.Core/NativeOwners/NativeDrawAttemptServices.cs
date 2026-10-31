#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawAttemptServices(Func<RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries> getCoreGameEngineNativeDrawAttemptsRuntime,EngineStates.CoreMatchSetupState matchSetupState,EngineStates.CoreMatchParticipantsState matchParticipantsState,Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime,EngineStates.CoreMatchOutcomeState matchOutcomeState,EngineStates.CoreActualTurnProgressState actualTurnProgressState,Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime,Func<RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime,Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime,Func<ProgramPipelineHooks> getProgramPipelineHooks,EngineStates.CoreCardMovementJournalState cardMovementJournalState,Func<RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries> getCoreHappyWeiYanGainDestinationsRuntime,Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangFeiOlMagicRuntime>> getCharacterZhangFeiOlMagicRuntime,Func<RuntimeEntries.CharacterZhouFeiLeEntries> getCharacterZhouFeiLeRuntime) : INativeDrawAttemptQueries, INativeDrawAttemptOperations
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
    public NativeDrawPileSlashProof? DrawPileSlashReplacementProof(ProgramSkillFrame frame, NativeDrawAttemptContext context) => getCharacterZhangFeiOlMagicRuntime().Value.NativeDrawPileSlashReplacementProof(frame, context);
    public bool IsIssuedDrawPileSlashReplacement(NativeDrawAttemptContext context, NativeDrawReplacementResult replacement) => getCharacterZhangFeiOlMagicRuntime().Value.IsIssuedNativeDrawPileSlashReplacement(context, replacement);
    public void CompleteLingkongReplacement(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement) => getCharacterZhouFeiLeRuntime().CompleteLingkongNativeDrawReplacement(frame, attempt, replacement);
}
