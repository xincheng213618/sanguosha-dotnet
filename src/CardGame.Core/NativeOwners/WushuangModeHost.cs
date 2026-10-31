#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangModeHost(EngineStates.CoreMatchDefinitionState matchDefinitionState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreActualRoundProgressState actualRoundProgressState, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, Func<CommandSession> getCommandSession, Func<bool> getBoolean, EngineStates.CoreMatchOutcomeState matchOutcomeState, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, Func<RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, KernelAdvanceToHumanBoundaryCall advanceToHumanBoundary, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime, KernelAcceptCall accept, KernelRejectCall reject, Func<MatchSkillBindingIndex> getMatchSkillBindingIndex, Func<RuntimeEntries.CoreGameEngineConversionStatesEntries> getCoreGameEngineConversionStatesRuntime, Func<RuntimeEntries.CoreGameEngineWushuangShopCardsEntries> getCoreGameEngineWushuangShopCardsRuntime, Func<RuntimeEntries.CoreGameEngineShanheCampaignsEntries> getCoreGameEngineShanheCampaignsRuntime, Func<RuntimeEntries.CoreGameEngineWushuangAcquisitionEntries> getCoreGameEngineWushuangAcquisitionRuntime, EngineStates.CoreAiBrainsState aiBrainsState, Func<RuntimeEntries.CoreGameEngineWushuangAiEntries> getCoreGameEngineWushuangAiRuntime, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, Func<RuntimeEntries.CoreGameEngineModeMatchLimitsEntries> getCoreGameEngineModeMatchLimitsRuntime, KernelCompleteGameCall completeGame) : IWushuangModeHost
{
    public int PlayerCount => matchDefinitionState.ReferenceToPlayerCount;
    public int CurrentSeat => actualTurnProgressState.ReferenceToCurrentSeat;
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public int MaximumTurns => matchDefinitionState.ReferenceToOptions.MaxTurns;
    public int FirstNormalSeat => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MoLuBuOpeningStartedEvent>().FirstOrDefault()?.NormalSeat ?? getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<RoundStartedEvent>().FirstOrDefault()?.ActorSeat ?? actualTurnProgressState.ReferenceToCurrentSeat;
    public long Revision => getCommandSession().Revision;
    public bool ClockRunning => getBoolean();
    public bool HasWinner => matchOutcomeState.ReferenceToWinner != Winner.None;
    public bool IsNormalTurn => actualTurnProgressState.ReferenceToTurnProgression.Kind == ActualTurnKind.Normal;
    public bool AdvanceAfterHumanCommands => matchDefinitionState.ReferenceToOptions.AdvanceAfterHumanCommands;
    public IEnumerable<WushuangPlayerFacts> Players => matchParticipantsState.ReferenceToPlayers.Select(player => Player(player.Seat));

    public WushuangPlayerFacts Player(int seat)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        // Identity/shop queries do not own a sister-health selection. Keep
        // its absence explicit rather than inventing a current HP value.
        var hp = player.HappyQiaoHealth is { } health && health.DaQiaoHp != health.XiaoQiaoHp && player.HappyQiaoPoolQuery?.Invoke(player)is null ? (int? )null : player.Hp;
        return new(player.Seat, player.Name, player.IsHuman, player.IsAlive, hp, player.MaxHp, Array.AsReadOnly(player.SkillGrants.Grants.ToArray()))
        {
            TeamId = player.TeamId
        };
    }

    public IReadOnlyList<string> EnabledSkillIds(int seat) => Array.AsReadOnly(getCoreGameEngineNationalSkillsRuntime().EnabledContentSkillIds(matchParticipantsState.ReferenceToPlayers[seat]).ToArray());
    public int EquipmentSlotCapacity(int seat, EquipmentSlot slot) => matchParticipantsState.ReferenceToPlayers[seat].EquipmentSlotCapacity(slot);
    public EquipmentSlot GetEquipmentSlot(CardKind kind) => getEquipmentContentRegistry().Get(kind).Slot;
    public bool IsEquipment(CardKind kind) => getEquipmentContentRegistry().IsEquipment(kind);
    public IWushuangShopRandom CreateShopRandom(int seed) => new WushuangShopRandom(seed);
    public bool IsSlash(CardKind kind) => EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind);
    public bool HasReachedFiveTokens(int seat) => getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<WushuangTokensChangedEvent>().Any(fact => fact.Seat == seat && fact.Tokens >= 5);
    public IReadOnlyList<ResolutionFrame> Frames => Array.AsReadOnly(getFrameStore().ToArray());
    public PendingDecision? PendingDecision => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision;

    public long NextFrameId() => ++getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence;
    public void PushFrame(ResolutionFrame frame) => getCoreGameEngineRuntimeRuntime().PushRuntimeFrame(frame);
    public void ReplaceFrame(long id, ResolutionFrame frame) => getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(id, frame);
    public void PopFrame(long id, ResolutionFrameKind kind) => getCoreGameEngineRuntimeRuntime().PopResolutionFrame(id, kind);
    public void ClearPrompt() => getCoreNativePromptBoundaryRuntime().ClearPendingDecision();
    public void SetPrompt(PendingDecision prompt)
    {
        getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision = prompt;
        matchOutcomeState.ReferenceToStatus = EngineStatus.AwaitingHumanResponse;
    }

    public PromptId NextPromptId() => getCoreNativePromptBoundaryRuntime().CreatePromptId();
    public void AdvanceState() => advanceRulesAndPublishState();
    public void AdvanceToHumanBoundary() => advanceToHumanBoundary();
    public void PublishFact(IGameEvent fact) => advanceEventRulesAndQueueFact(fact);
    public void LogPurchase(string message, int seat) => getCoreMatchLogRuntime().AddLog("WushuangPurchase", message, seat);
    public CommandResult Accept(Action action) => accept(action);
    public CommandResult Reject(CommandErrorCode code, string message) => reject(code, message);
    public void RemoveSkillGrant(int seat, string grantId) => matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.RemoveGrant(grantId);
    public void GrantSkill(int seat, SkillGrant grant)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        player.SkillGrants.Grant(grant);
        getMatchSkillBindingIndex().Invalidate(seat);
        getCoreGameEngineConversionStatesRuntime().RegisterTaggedConversionSkill(player, grant.SkillId);
    }

    public void PlaceShopCard(int seat, string offerId, WushuangCard definition) => getCoreGameEngineWushuangShopCardsRuntime().PlaceWushuangShopCard(seat, offerId, definition);
    public void PlaceFallbackCard(int seat, CardKind kind) => getCoreGameEngineShanheCampaignsRuntime().PlaceShanhePhysicalCard(new(kind, kind == CardKind.Dodge ? Suit.Spade : Suit.Heart, kind == CardKind.Dodge ? 2 : 7), seat, equipment: false);
    public bool TryBeginAcquisition(long frameId, WushuangShopOffer offer) => getCoreGameEngineWushuangAcquisitionRuntime().TryBeginWushuangAcquisition(frameId, offer);
    public bool TryContinueAiShopping(int seat, WushuangIntermissionFrame frame)
    {
        if (aiBrainsState.ReferenceToAiBrains[seat].PolicyVersion < AiDecisionPolicy.WushuangStrategyVersion)
            return false;
        getCoreGameEngineWushuangAiRuntime().ContinueWushuangAiShopping(matchParticipantsState.ReferenceToPlayers[seat], frame);
        return true;
    }

    public void ContinueAfterShopping(int endedSeat, bool skipped) => getCoreActualTurnsRuntime().ContinueAfterWushuangShopping(matchParticipantsState.ReferenceToPlayers[endedSeat], skipped);
    public void RetirePendingMatch(string? winningTeamId, Func<long, ResolutionFrameKind, IGameEvent> cancellationFact, string processingReason, string judgmentReason) => getCoreGameEngineModeMatchLimitsRuntime().RetirePendingMatchAtModeLimit(winningTeamId, cancellationFact, processingReason, judgmentReason);
    public void Log(string kind, string message) => getCoreMatchLogRuntime().AddLog(kind, message);
    public void PublishCurrentWinner() => advanceEventRulesAndQueueFact(new WinnerDeterminedEvent(matchOutcomeState.ReferenceToWinner, matchOutcomeState.ReferenceToWinnerTeamId, null) { WinnerSeat = matchOutcomeState.ReferenceToWinnerSeat });
    public void CompleteGame() => completeGame();
}
