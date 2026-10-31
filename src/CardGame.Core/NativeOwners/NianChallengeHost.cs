#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NianChallengeHost(global::CardGame.Core.NativeOwners.ModeEncounterHostFactory createModeEncounterHost, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.NativeOwners.KernelAcceptCall accept, global::CardGame.Core.NativeOwners.KernelRejectCall reject, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchOutcomesEntries> getCoreMatchOutcomesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, global::System.Func<global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext>> getList, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::CardGame.Core.NativeOwners.KernelCompleteGameCall completeGame, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeLegalActionsEntries> getCoreNativeLegalActionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : INianChallengeHost
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
