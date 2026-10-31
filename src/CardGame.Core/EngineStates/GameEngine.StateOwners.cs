#nullable enable
namespace CardGame.Core;
public sealed partial class GameEngine
{
    private readonly global::CardGame.Core.EngineStates.CoreActualDiscardLedgerState _actualDiscardLedger = new();
    private readonly global::CardGame.Core.EngineStates.CoreActualRoundProgressState _actualRoundProgress = new();
    private readonly global::CardGame.Core.EngineStates.CoreActualTurnProgressState _actualTurnProgress = new();
    private readonly global::CardGame.Core.EngineStates.CoreAiBrainsState _aiBrains2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreAiDiagnosticsState _aiDiagnostics = new();
    private readonly global::CardGame.Core.EngineStates.CoreCardMovementJournalState _cardMovementJournal = new();
    private readonly global::CardGame.Core.EngineStates.CoreCommandJournalStorageState _commandJournalStorage = new();
    private readonly global::CardGame.Core.EngineStates.CoreCurrentTurnStatisticsState _currentTurnStatistics = new();
    private readonly global::CardGame.Core.EngineStates.CoreDelayedTurnEffectsState _delayedTurnEffects = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameLogState _gameLog = new();
    private readonly global::CardGame.Core.EngineStates.CoreJudgmentEffectiveCardsState _judgmentEffectiveCards = new();
    private readonly global::CardGame.Core.EngineStates.CoreMatchDefinitionState _matchDefinition = new();
    private readonly global::CardGame.Core.EngineStates.CoreMatchOutcomeState _matchOutcome = new();
    private readonly global::CardGame.Core.EngineStates.CoreMatchParticipantsState _matchParticipants = new();
    private readonly global::CardGame.Core.EngineStates.CoreMatchSetupState _matchSetup = new();
    private readonly global::CardGame.Core.EngineStates.CorePhysicalCardPopulationState _physicalCardPopulation = new();
    private readonly global::CardGame.Core.EngineStates.CorePlayerDecisionState _playerDecision = new();
    private readonly global::CardGame.Core.EngineStates.CharacterCaoPiMouState _characterCaoPiMou = new();
    private readonly global::CardGame.Core.EngineStates.CharacterCaoXiOlState _characterCaoXiOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterCuiLieAnniversaryState _characterCuiLieAnniversary = new();
    private readonly global::CardGame.Core.EngineStates.CharacterDaQiaoXiaoQiaoOlState _characterDaQiaoXiaoQiaoOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterDingYuanOlState _characterDingYuanOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterDuXiOlState _characterDuXiOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterHeQiOlState _characterHeQiOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterHuCheErMouState _characterHuCheErMou = new();
    private readonly global::CardGame.Core.EngineStates.CharacterHuaTuoMobileGodState _characterHuaTuoMobileGod = new();
    private readonly global::CardGame.Core.EngineStates.CharacterHuangChengYanOlState _characterHuangChengYanOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterHuangChongMobileShiState _characterHuangChongMobileShi = new();
    private readonly global::CardGame.Core.EngineStates.CharacterLiuQiOlState _characterLiuQiOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterLvKaiOlState _characterLvKaiOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterTaiShiCiMobileShiState _characterTaiShiCiMobileShi = new();
    private readonly global::CardGame.Core.EngineStates.CharacterWangGuanOlState _characterWangGuanOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterXiahouJieAnniversaryState _characterXiahouJieAnniversary = new();
    private readonly global::CardGame.Core.EngineStates.CharacterYangWanOlState _characterYangWanOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterZhangLingOlState _characterZhangLingOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterZhangXunAnniversaryState _characterZhangXunAnniversary = new();
    private readonly global::CardGame.Core.EngineStates.CharacterZhugeGuoOlState _characterZhugeGuoOl = new();
    private readonly global::CardGame.Core.EngineStates.CharacterZhugeLiangMouState _characterZhugeLiangMou = new();
    private readonly global::CardGame.Core.EngineStates.CharacterZouShiLeState _characterZouShiLe = new();
    private readonly global::CardGame.Core.EngineStates.CoreDirectedTurnCardPoliciesState _directedTurnCardPolicies2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreEndingHandCountProgramsState _endingHandCountPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineActionQueriesState _actionQueries = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineActivityOrdinaryTricksState _activityOrdinaryTricks = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineActorHandLimitPenaltiesState _actorHandLimitPenalties2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineActualPlayPhaseCardUsesState _actualPlayPhaseCardUses = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineAdvancedLifecycleState _advancedLifecycle = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineAdvancedPoliciesState _advancedPolicies = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineAdvancedWeaponsState _advancedWeapons = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineAlcoholSlashSuppressionsState _alcoholSlashSuppressions = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineAlternatingSuitAndCompletedTopState _alternatingSuitAndCompletedTop = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineAlternativeViewAsCostsState _alternativeViewAsCosts = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineCardActionsState _cardActions = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineCardConversionsState _cardConversions = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineCardUseDebitsState _cardUseDebits2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineCharacterStateProgramsState _characterStatePrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineConfiguredConversionsState _configuredConversions = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineConvertingGiftsState _convertingGifts = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineCurrentTurnShownCardBenefitsState _currentTurnShownCardBenefits = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineCurrentTurnSkillSuppressionsState _currentTurnSkillSuppressions2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineDeckEquipmentProgramsState _deckEquipmentPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineDeferredBenefitsProgramsState _deferredBenefitsPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineDeferredCardsState _deferredCards = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineDeferredHandAlignmentsState _deferredHandAlignments2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineDragonBoatGeneralsState _dragonBoatGenerals = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineDyingWindowSkillPoliciesState _dyingWindowSkillPolicies = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineFriendTrioState _friendTrio = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineGuanduActivitiesState _guanduActivities = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineHpChangeProgramsState _hpChangePrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineHpConditionalSkillGrantsState _hpConditionalSkillGrants = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineIssuedPlayUsePoliciesState _issuedPlayUsePolicies = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineLordSkillProjectionsState _lordSkillProjections = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineNativeFactionChangesState _nativeFactionChanges = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineNextCardTargetAdjustmentsState _nextCardTargetAdjustments = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineNextSlashProgramsState _nextSlashPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineOrderedPrintedSkillLossState _orderedPrintedSkillLoss = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineOriginalHandEntitiesState _originalHandEntities = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineOriginalTargetAdditionsState _originalTargetAdditions = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEnginePhaseNamePredictionState _phaseNamePrediction = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEnginePrivateGeneralLibrariesState _privateGeneralLibraries2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEnginePrivateWhirlState _privateWhirl = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineProgramBooleanStateExpirationState _programBooleanStateExpiration = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineProgramPersistentCombatPoliciesState _programPersistentCombatPolicies = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineProgramPersistentStateState _programPersistentState = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineProgramPhaseColorRestrictionsState _programPhaseColorRestrictions2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEnginePublicPileSourcesState _publicPileSources = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineQuotaTopProgramsState _quotaTopPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineRecipientCategoryMarksState _recipientCategoryMarks2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineRedAdditionalTargetsState _redAdditionalTargets = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineRedDiscardRecoveryState _redDiscardRecovery = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineRuleProgressionState _ruleProgression2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineShenwuActivitiesState _shenwuActivities = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineShenwuExpandedPoliciesState _shenwuExpandedPolicies = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineSkillProgramsState _skillPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineSourceCursesState _sourceCurses2 = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineStrategicProgramsState _strategicPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineTableOrderState _tableOrder = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineTurnDefaultStatsState _turnDefaultStats = new();
    private readonly global::CardGame.Core.EngineStates.CoreGameEngineZhaoZhongProvenanceState _zhaoZhongProvenance = new();
    private readonly global::CardGame.Core.EngineStates.CoreLeastHandMarkerDrawProgramsState _leastHandMarkerDrawPrograms = new();
    private readonly global::CardGame.Core.EngineStates.CoreProgramParticipantPhaseOperationsState _programParticipantPhaseOperations = new();
    private global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramNativeCapabilityBinding> __GeneratedActualStateOwnerBindings() => new global::CardGame.Core.ProgramNativeCapabilityBinding[]
    {
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess), _actualDiscardLedger),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess), _actualDiscardLedger),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess), _actualRoundProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundTurnSeatsAccess), _actualRoundProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess), _actualRoundProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess), _actualTurnProgress),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess), _aiBrains2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess), _aiDiagnostics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess), _aiDiagnostics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess), _aiDiagnostics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess), _cardMovementJournal),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess), _cardMovementJournal),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess), _cardMovementJournal),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess), _commandJournalStorage),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess), _commandJournalStorage),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess), _commandJournalStorage),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess), _commandJournalStorage),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess), _commandJournalStorage),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess), _commandJournalStorage),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess), _currentTurnStatistics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess), _currentTurnStatistics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess), _currentTurnStatistics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess), _currentTurnStatistics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseKillCountByCurrentPlayerAccess), _currentTurnStatistics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess), _currentTurnStatistics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess), _currentTurnStatistics),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess), _delayedTurnEffects),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess), _gameLog),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogAccess), _gameLog),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogSequenceAccess), _gameLog),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess), _judgmentEffectiveCards),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess), _matchDefinition),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess), _matchOutcome),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess), _matchParticipants),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess), _matchSetup),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsAccess), _matchSetup),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess), _matchSetup),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess), _matchSetup),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess), _matchSetup),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess), _matchSetup),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess), _physicalCardPopulation),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionNextPromptIdAccess), _playerDecision),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess), _playerDecision),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterCaoPiMouMouCaoPiControlsAccess), _characterCaoPiMou),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess), _characterCaoXiOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterCuiLieAnniversaryCuiLieQualificationInputsCuiLieQualificationStampAccess), _characterCuiLieAnniversary),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess), _characterDaQiaoXiaoQiaoOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterDingYuanOlSynchronizingAdoptedChildGrantsAccess), _characterDingYuanOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess), _characterDuXiOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess), _characterHeQiOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess), _characterHuCheErMou),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterHuaTuoMobileGodMobileWulingCoursesAccess), _characterHuaTuoMobileGod),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChengYanOlJiezhenConversionsAccess), _characterHuangChengYanOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess), _characterHuangChongMobileShi),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterLiuQiOlHasIssuedLiuQiAccess), _characterLiuQiOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterLvKaiOlBijingMarkedCardIdsAccess), _characterLvKaiOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterTaiShiCiMobileShiShiTzLieScanSequenceAccess), _characterTaiShiCiMobileShi),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterWangGuanOlMiuyanQualificationDependenciesMiuyanQualificationStampAccess), _characterWangGuanOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterXiahouJieAnniversaryAnniversaryXiahouJieQualificationInputsAnniversaryXiahouJieQualificationStampAccess), _characterXiahouJieAnniversary),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess), _characterYangWanOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterZhangLingOlSynchronizingCategoryTalismansAccess), _characterZhangLingOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess), _characterZhangXunAnniversary),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterZhugeGuoOlHasIssuedZhugeGuoStateAccess), _characterZhugeGuoOl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess), _characterZhugeLiangMou),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess), _characterZouShiLe),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess), _directedTurnCardPolicies2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess), _directedTurnCardPolicies2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess), _endingHandCountPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess), _actionQueries),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess), _activityOrdinaryTricks),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess), _actorHandLimitPenalties2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess), _actualPlayPhaseCardUses),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess), _advancedLifecycle),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess), _advancedLifecycle),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess), _advancedPolicies),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess), _advancedWeapons),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess), _advancedWeapons),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess), _advancedWeapons),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess), _alcoholSlashSuppressions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess), _alternatingSuitAndCompletedTop),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess), _alternativeViewAsCosts),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess), _cardActions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess), _cardConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess), _cardConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess), _cardConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess), _cardConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionAccess), _cardConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedUseConversionAccess), _cardConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess), _cardUseDebits2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess), _cardUseDebits2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess), _characterStatePrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess), _configuredConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess), _configuredConversions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess), _convertingGifts),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess), _currentTurnShownCardBenefits),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess), _currentTurnSkillSuppressions2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess), _currentTurnSkillSuppressions2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess), _currentTurnSkillSuppressions2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess), _deckEquipmentPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess), _deckEquipmentPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess), _deferredBenefitsPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess), _deferredBenefitsPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess), _deferredCards),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess), _deferredHandAlignments2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess), _dragonBoatGenerals),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess), _dyingWindowSkillPolicies),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess), _friendTrio),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess), _friendTrio),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess), _guanduActivities),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess), _hpChangePrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess), _hpConditionalSkillGrants),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess), _issuedPlayUsePolicies),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess), _lordSkillProjections),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess), _nativeFactionChanges),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess), _nativeFactionChanges),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess), _nextCardTargetAdjustments),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess), _nextCardTargetAdjustments),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess), _nextSlashPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess), _orderedPrintedSkillLoss),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess), _orderedPrintedSkillLoss),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess), _originalHandEntities),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess), _originalHandEntities),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess), _originalTargetAdditions),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess), _phaseNamePrediction),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess), _privateGeneralLibraries2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess), _privateGeneralLibraries2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess), _privateWhirl),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess), _programBooleanStateExpiration),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess), _programPersistentCombatPolicies),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess), _programPersistentState),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess), _programPhaseColorRestrictions2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess), _publicPileSources),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess), _quotaTopPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess), _recipientCategoryMarks2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess), _redAdditionalTargets),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess), _redAdditionalTargets),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess), _redDiscardRecovery),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess), _ruleProgression2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess), _shenwuExpandedPolicies),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess), _skillPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess), _skillPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess), _skillPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess), _skillPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess), _sourceCurses2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCursesAccess), _sourceCurses2),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsStrategicEndPlayTurnAccess), _strategicPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess), _strategicPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess), _tableOrder),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess), _turnDefaultStats),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess), _zhaoZhongProvenance),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess), _leastHandMarkerDrawPrograms),
        new(typeof(global::CardGame.Core.EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess), _programParticipantPhaseOperations)
    };
}
