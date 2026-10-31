#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActivityBattleRulesHost(global::CardGame.Core.EngineStates.CoreMatchSetupState matchSetupState, global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState, global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::System.Func<global::System.Collections.Generic.List<global::CardGame.Core.CardMovementBatchContext>> getList, global::CardGame.Core.EngineStates.CoreGameEngineHpChangeProgramsState gameEngineHpChangeProgramsState, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries> getCoreGameEngineActualDiscardRecoveryPhasesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, global::CardGame.Core.EngineStates.CoreGameEngineGuanduActivitiesState gameEngineGuanduActivitiesState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries> getCoreGameEngineCardUseLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTeamActivitiesEntries> getCoreGameEngineTeamActivitiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduGeneralsEntries> getCoreGameEngineGuanduGeneralsRuntime, global::System.Func<global::CardGame.Core.DeterministicRandom> getDeterministicRandom, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchOutcomesEntries> getCoreMatchOutcomesRuntime, global::CardGame.Core.NativeOwners.KernelCompleteGameCall completeGame, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IModeBattleRulesHost
{
    public ModeBattleRulesFacts CaptureFacts() => new(matchSetupState.ReferenceToSetupComplete, matchSetupState.ReferenceToGameStartingProgramsResolved, matchOutcomeState.ReferenceToWinner != Winner.None, matchDefinitionState.ReferenceToOptions.HumanSeat, actualTurnProgressState.ReferenceToCurrentSeat, actualTurnProgressState.ReferenceToTurnNumber, actualRoundProgressState.ReferenceToRoundNumber, actualTurnProgressState.ReferenceToPhase, getFrameStore().Count, getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision?.Kind, getList().Count, gameEngineHpChangeProgramsState.ReferenceToPendingHpChanges.Count, getCardZoneStore().Count(CardLocation.DrawPile), getFrameStore().OfType<DrawPhaseObligationFrame>().LastOrDefault()?.IsExtra == true, getCoreGameEngineActualDiscardRecoveryPhasesRuntime().CurrentActualDiscardRecoveryPhase(), matchParticipantsState.ReferenceToPlayers.Select(player => new ModeBattlePlayerFacts(player.Seat, player.Name, player.IsAlive, player.Hp, player.MaxHp, player.TeamId, getCorePhysicalOwnedCardQueriesRuntime().GetHand(player).Count, getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(player).Count)).ToArray());
    public IReadOnlyList<TEvent> History<TEvent>()
        where TEvent : IGameEvent => Array.AsReadOnly(getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().OfType<TEvent>().ToArray());
    public GuanduBattleConfiguredEvent? InitialGuanduConfiguration => gameEngineGuanduActivitiesState.ReferenceToGuanduInitialConfiguration;

    private static ModeBattleNativeUse? UseFacts(CardUseFrame? use) => use?.Action is { } action ? new(use.Id, new(action.ActionId, action.Type, action.ActorSeat, action.EffectiveKind), use.ExtraCardEffect is not null) : null;
    public ModeBattleNativeUse? FindUse(long frameId) => UseFacts(getCoreGameEngineCardUseLifecycleRuntime().LifecycleCardUse(frameId));
    public ModeBattleNativeUse? FindUseByAction(long actionId) => UseFacts(getFrameStore().OfType<CardUseFrame>().SingleOrDefault(frame => frame.Action?.ActionId == actionId));
    public ModeBattleDraftFacts Draft(int seat)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        return new(seat, player.GeneralSelected, player.General.Id, player.GeneralDraftCandidates?.Select(general => general.Id).ToArray() ?? []);
    }

    public bool HasActivitySkill(int seat, string skillId) => getCoreGameEngineTeamActivitiesRuntime().HasActivitySkill(matchParticipantsState.ReferenceToPlayers[seat], skillId);
    public bool HasIndependentSkillGrant(int seat, string skillId) => getCoreGameEngineNationalSkillsRuntime().EnabledRuntimeSkillGrants(matchParticipantsState.ReferenceToPlayers[seat]).Any(grant => grant.SkillId == skillId && !grant.IsModeRule);
    public bool IsSlash(CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind);
    public string FactionName(string factionId) => global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(factionId);
    public IEnumerable<RuleQueryContribution> NativeGeneralContributions(int seat, SkillRuleQuery query) => getCoreGameEngineGuanduGeneralsRuntime().GuanduGeneralRuleContributions(matchParticipantsState.ReferenceToPlayers[seat], query);
    public int NextRandom(int exclusiveMaximum) => getDeterministicRandom().Next(exclusiveMaximum);
    public void PublishFact(IGameEvent fact) => advanceEventRulesAndQueueFact(fact);
    public void PublishState() => advanceRulesAndPublishState();
    public void ClearDecision() => getCoreNativePromptBoundaryRuntime().ClearPendingDecision();
    public void SetWinner(Winner winner, string? teamId = null, int? seat = null)
    {
        matchOutcomeState.ReferenceToWinner = winner;
        matchOutcomeState.ReferenceToWinnerTeamId = teamId;
        matchOutcomeState.ReferenceToWinnerSeat = seat;
    }

    public Winner WinnerForTeam(string teamId) => getCoreMatchOutcomesRuntime().GetWinnerForTeam(teamId);
    public void CompleteGame() => completeGame();
    public void TransferAllHandCards(int fromSeat, int toSeat, string reason) => getCorePhysicalMovementFacadesRuntime().MoveCards(getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[fromSeat]).ToArray(), CardLocation.Hand(fromSeat), CardLocation.Hand(toSeat), new(reason));
    public void DiscardAllHandAndEquipment(int seat, string reason)
    {
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveLogicalDiscardCards(seat, seat, getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).ToArray(), CardLocation.Hand(seat), new(reason));
        getCoreGameEngineNativeLogicalDiscardsRuntime().MoveLogicalDiscardCards(seat, seat, getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]).ToArray(), CardLocation.Equipment(seat), new(reason));
    }

    public void Log(string kind, string message, int sourceSeat, int? targetSeat = null)
    {
        if (targetSeat is { } target)
            getCoreMatchLogRuntime().AddLog(kind, message, sourceSeat, target);
        else
            getCoreMatchLogRuntime().AddLog(kind, message, sourceSeat);
    }
}
