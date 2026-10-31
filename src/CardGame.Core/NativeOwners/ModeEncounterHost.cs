#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ModeEncounterHost(global::CardGame.Core.EngineStates.CoreGameLogState gameLogState, global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries> getCoreGameEnginePrivateGeneralLibrariesRuntime, global::System.Func<global::CardGame.Core.SkillRuntimeStateStore> getSkillRuntimeStateStore, global::CardGame.Core.EngineStates.CoreGameEngineSkillProgramsState gameEngineSkillProgramsState, global::CardGame.Core.EngineStates.CoreGameEngineProgramPersistentStateState gameEngineProgramPersistentStateState, global::System.Func<global::System.Collections.Generic.Dictionary<int, long>> getDictionary, global::System.Func<global::CardGame.Core.MatchSkillBindingIndex> getMatchSkillBindingIndex, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineConversionStatesEntries> getCoreGameEngineConversionStatesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheBossCancelledMaterialsEntries> getCoreGameEngineShanheBossCancelledMaterialsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::System.Func<global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext>> getList, global::CardGame.Core.EngineStates.CoreGameEngineHpChangeProgramsState gameEngineHpChangeProgramsState, global::CardGame.Core.EngineStates.CoreGameEngineCharacterStateProgramsState gameEngineCharacterStateProgramsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries> getCoreGameEnginePlayPhaseSkillGrantsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchOutcomesEntries> getCoreMatchOutcomesRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IModeEncounterHost
{
    public int? LastLogSequence => gameLogState.ReferenceToLogSequence;

    public ModeEncounterFacts CaptureFacts() => new(matchSetupState.ReferenceToSetupComplete, matchOutcomeState.ReferenceToWinner != Winner.None, matchDefinitionState.ReferenceToPlayerCount, matchDefinitionState.ReferenceToOptions.HumanSeat, actualTurnProgressState.ReferenceToCurrentSeat, actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToPhase, getFrameStore().Count, getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision?.Kind, Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => new ModeEncounterPlayerFacts(player.Seat, player.Name, player.General.Id, player.IsAlive, player.Hp, player.MaxHp, player.TeamId)).ToArray()));
    public IReadOnlyList<TEvent> History<TEvent>()
        where TEvent : IGameEvent => Array.AsReadOnly(getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().OfType<TEvent>().ToArray());
    public ModeEncounterGeneralFacts General(string generalId)
    {
        var general = matchDefinitionState.ReferenceToGeneralPool.Single(candidate => candidate.Id == generalId);
        return new(general.Id, general.Name, general.BaseHp, general.InitialHp ?? general.BaseHp);
    }

    public int NextRandom(int exclusiveMaximum) => getDeterministicRandom().Next(exclusiveMaximum);
    public void ReplaceCombatant(ModeCombatantReplacement request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(request.Seat) || matchParticipantsState.ReferenceToPlayers[request.Seat].General.Id != request.ExpectedGeneralId)
            throw new InvalidOperationException("An encounter replacement lost its original combatant.");
        if (request.ArmorOverride is < 0 or > 1000)
            throw new InvalidOperationException("An encounter armor override is out of bounds.");
        var previous = matchParticipantsState.ReferenceToPlayers[request.Seat];
        var general = matchDefinitionState.ReferenceToGeneralPool.Single(candidate => candidate.Id == request.GeneralId);
        CharacterState replacement;
        if (request.PreserveCurrentState)
        {
            replacement = previous;
            replacement.General = general;
            replacement.MaxHp = request.MaximumHpOverride ?? general.BaseHp;
            replacement.Hp = request.HpOverride ?? general.InitialHp ?? general.BaseHp;
        }
        else
        {
            replacement = new CharacterState
            {
                Seat = previous.Seat,
                Name = previous.Name,
                IsHuman = false,
                Role = previous.Role,
                TeamId = previous.TeamId,
                TeamRevealed = true,
                RoleRevealed = true,
                General = general,
                GeneralSelected = true,
                GeneralRevealed = true,
                Hp = request.HpOverride ?? general.InitialHp ?? general.BaseHp,
                MaxHp = request.MaximumHpOverride ?? general.BaseHp
            };
            if (getCoreGameEnginePrivateGeneralLibrariesRuntime().HasPrivateGeneralLibraryCapability)
                replacement.GeneralLibraryGenderQuery = getCoreGameEnginePrivateGeneralLibrariesRuntime().GetPrivateGeneralLibraryGender;
            matchParticipantsState.ReferenceToPlayers[replacement.Seat] = replacement;
            getSkillRuntimeStateStore().ResetOwner(replacement.Seat);
            foreach (var key in gameEngineSkillProgramsState.ReferenceToProgramUses.Keys.Where(key => key.Seat == replacement.Seat).ToArray())
                gameEngineSkillProgramsState.ReferenceToProgramUses.Remove(key);
            foreach (var key in gameEngineSkillProgramsState.ReferenceToProgramPhaseUses.Keys.Where(key => key.Seat == replacement.Seat).ToArray())
                gameEngineSkillProgramsState.ReferenceToProgramPhaseUses.Remove(key);
            foreach (var key in gameEngineSkillProgramsState.ReferenceToProgramContributionUses.Keys.Where(key => key.SkillOwnerSeat == replacement.Seat).ToArray())
                gameEngineSkillProgramsState.ReferenceToProgramContributionUses.Remove(key);
            foreach (var key in gameEngineProgramPersistentStateState.ReferenceToProgramBooleanStates.Keys.Where(key => key.OwnerSeat == replacement.Seat).ToArray())
                gameEngineProgramPersistentStateState.ReferenceToProgramBooleanStates.Remove(key);
            getDictionary()[replacement.Seat] = replacement.SkillGrants.Revision;
        }

        if (request.ArmorOverride is { } armor)
            replacement.Armor = armor;
        getMatchSkillBindingIndex().Invalidate(replacement.Seat);
        foreach (var id in getCoreGameEngineNationalSkillsRuntime().EnabledContentSkillIds(replacement))
            getCoreGameEngineConversionStatesRuntime().RegisterTaggedConversionSkill(replacement, id);
    }

    public void ClearDecision(bool resumeRunning = false)
    {
        getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision = null;
        if (resumeRunning)
            matchOutcomeState.ReferenceToStatus = EngineStatus.Running;
    }

    public void CancelResolutionTree(Func<ModeCancelledResolution, IGameEvent> cancellationFact)
    {
        ArgumentNullException.ThrowIfNull(cancellationFact);
        getCoreGameEngineShanheBossCancelledMaterialsRuntime().CaptureShanheBossCancelledMaterials();
        // Retiring an actual-turn subtree has no ordinary child return.
        // Already paid physical cards stay paid until native processing cleanup.
        while (getFrameStore().LastOrDefault()is { } top)
        {
            getFrameStore().CompleteTop(top.Id, top.Kind);
            advanceEventRulesAndQueueFact(cancellationFact(new(top.Id, top.Kind, actualTurnProgressState.ReferenceToTurnNumber)));
        }
    }

    public void ClearTransientBatches()
    {
        getList().Clear();
        gameEngineHpChangeProgramsState.ReferenceToPendingHpChanges.Clear();
        gameEngineCharacterStateProgramsState.ReferenceToPendingCharacterStateChanges.Clear();
    }

    public void DiscardProcessingCards(string reason) => getCorePhysicalMovementFacadesRuntime().MoveAllCards(CardLocation.Processing, CardLocation.DiscardPile, new CardMoveReason(reason));
    public void FinishInterruptedTurn(int expectedSeat, int expectedTurnNumber, string skillGrantEndReason, IGameEvent interruptionFact)
    {
        void AssertOriginalTurn()
        {
            if (!getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(expectedSeat) || actualTurnProgressState.ReferenceToCurrentSeat != expectedSeat || actualTurnProgressState.ReferenceToTurnNumber != expectedTurnNumber)
                throw new InvalidOperationException("Interrupted-turn cleanup lost its original actual turn.");
        }

        AssertOriginalTurn();
        var previous = matchParticipantsState.ReferenceToPlayers[expectedSeat];
        getCoreGameEnginePlayPhaseSkillGrantsRuntime().CloseCurrentPlayPhaseSkillGrants(skillGrantEndReason);
        getCoreGameEngineProgramLifecycleRuntime().CancelScheduledProgramPhase();
        getCoreGameEngineActualDiscardRecoveryPhasesRuntime().ClearActualDiscardRecoveryPhase();
        AssertOriginalTurn();
        getCoreActualTurnsRuntime().ExpireActualTurnEndEffects(previous);
        AssertOriginalTurn();
        actualTurnProgressState.ReferenceToPhase = TurnPhase.Finished;
        advanceEventRulesAndQueueFact(interruptionFact);
        AssertOriginalTurn();
        advanceEventRulesAndQueueFact(new TurnEndedEvent(expectedTurnNumber, previous.Seat));
        AssertOriginalTurn();
    }

    public void BeginNaturalTurnAt(int seat)
    {
        if (!getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat))
            throw new ArgumentOutOfRangeException(nameof(seat));
        actualTurnProgressState.ReferenceToCurrentSeat = seat;
        actualTurnProgressState.ReferenceToTurnProgression = new(ActualTurnKind.Normal, seat, actualTurnProgressState.ReferenceToTurnNumber);
        actualTurnProgressState.ReferenceToPhase = TurnPhase.NotStarted;
    }

    public void EndCurrentTurn() => getCoreActualTurnsRuntime().EndTurn();
    public void DrawInitialCards(int seat, int count) => getCorePhysicalDrawFacadesRuntime().DrawCards(matchParticipantsState.ReferenceToPlayers[seat], count, log: true, CardMoveReasons.InitialDeal);
    public void DeclareTeamWinner(string teamId)
    {
        matchOutcomeState.ReferenceToWinnerTeamId = teamId;
        matchOutcomeState.ReferenceToWinner = getCoreMatchOutcomesRuntime().GetWinnerForTeam(teamId);
        advanceEventRulesAndQueueFact(new WinnerDeterminedEvent(matchOutcomeState.ReferenceToWinner, teamId, null));
    }

    public void PublishFact(IGameEvent fact) => advanceEventRulesAndQueueFact(fact);
    public void PublishState() => advanceRulesAndPublishState();
    public void Log(string kind, string message, int seat) => getCoreMatchLogRuntime().AddLog(kind, message, seat);
}
