#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NianChallengeHost(ModeEncounterHostFactory createModeEncounterHost, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchParticipantsState matchParticipantsState, KernelAcceptCall accept, KernelRejectCall reject, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreMatchOutcomesEntries> getCoreMatchOutcomesRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<List<CardMovementBatchContext>> getList, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, KernelCompleteGameCall completeGame, Func<RuntimeEntries.CoreNativeLegalActionsEntries> getCoreNativeLegalActionsRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : INianChallengeHost
{
    private readonly IModeEncounterHost _encounter = createModeEncounterHost();
    public IModeEncounterHost Encounter => _encounter;
    public bool Started => actualTurnProgressState.ReferenceToStarted;
    public IReadOnlyList<NianParticipantFacts> Participants => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => new NianParticipantFacts(player.Seat, player.IsHuman, player.GeneralSelected, player.HasHpValue)).ToArray());

    public CommandResult Accept(Action action) => accept(action);
    public CommandResult Reject(CommandErrorCode code, string message) => reject(code, message);
    public IReadOnlyList<ModeRevealedJudgmentCard> CaptureRevealedJudgments() => Array.AsReadOnly(getFrameStore().OfType<JudgmentFrame>().Where(frame => frame.CardId is not null).Select(frame => new ModeRevealedJudgmentCard(frame.CardId!.Value, frame.TargetSeat)).Distinct().ToArray());
    public void CancelDeferredPhases()
    {
        getCoreGameEngineProgramLifecycleRuntime().CancelScheduledProgramPhase();
        getCoreGameEngineActualDiscardRecoveryPhasesRuntime().ClearActualDiscardRecoveryPhase();
    }

    public void SetWinnerForCleanup(string teamId)
    {
        matchOutcomeState.ReferenceToWinnerTeamId = teamId;
        matchOutcomeState.ReferenceToWinner = getCoreMatchOutcomesRuntime().GetWinnerForTeam(teamId);
    }

    public void RetireRevealedJudgment(ModeRevealedJudgmentCard judgment, string reason)
    {
        if (getCardZoneStore().GetLocation(judgment.CardId) == CardLocation.Judgment(judgment.TargetSeat))
            getCorePhysicalMovementFacadesRuntime().MoveCard(getCoreGameEngineCardAttackStateRuntime().GetAttackCard(judgment.CardId), CardLocation.Judgment(judgment.TargetSeat), CardLocation.DiscardPile, new CardMoveReason(reason));
    }

    public void ClearPendingCardMovements() => getList().Clear();
    public void PublishCurrentWinner() => advanceEventRulesAndQueueFact(new WinnerDeterminedEvent(matchOutcomeState.ReferenceToWinner, matchOutcomeState.ReferenceToWinnerTeamId, null));
    public void CompleteGame() => completeGame();
    public bool TryExecuteProgramAction(int seat, string skillId, string activationId, Func<int> selectTarget)
    {
        var action = getCoreNativeLegalActionsRuntime().BuildLegalActions(matchParticipantsState.ReferenceToPlayers[seat]).SingleOrDefault(action => action.Kind == LegalActionKind.UseProgramSkill && action.ProgramSkillId == skillId && action.ProgramActivationId == activationId);
        if (action is null)
            return false;
        getCoreGameEngineSkillProgramsRuntime().ExecuteProgramSkill(matchParticipantsState.ReferenceToPlayers[seat], action, [], [selectTarget()]);
        return true;
    }
}
