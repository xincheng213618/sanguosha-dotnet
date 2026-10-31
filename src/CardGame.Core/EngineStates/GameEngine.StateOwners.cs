#nullable enable
namespace CardGame.Core;
public sealed partial class GameEngine
{
    private readonly EngineStates.CoreActualDiscardLedgerState _actualDiscardLedger = new();
    private readonly EngineStates.CoreActualRoundProgressState _actualRoundProgress = new();
    private readonly EngineStates.CoreActualTurnProgressState _actualTurnProgress = new();
    private readonly EngineStates.CoreAiBrainsState _aiBrains2 = new();
    private readonly EngineStates.CoreAiDiagnosticsState _aiDiagnostics = new();
    private readonly EngineStates.CoreCardMovementJournalState _cardMovementJournal = new();
    private readonly EngineStates.CoreCommandJournalStorageState _commandJournalStorage = new();
    private readonly EngineStates.CoreCurrentTurnStatisticsState _currentTurnStatistics = new();
    private readonly EngineStates.CoreDelayedTurnEffectsState _delayedTurnEffects = new();
    private readonly EngineStates.CoreGameLogState _gameLog = new();
    private readonly EngineStates.CoreJudgmentEffectiveCardsState _judgmentEffectiveCards = new();
    private readonly EngineStates.CoreMatchDefinitionState _matchDefinition = new();
    private readonly EngineStates.CoreMatchOutcomeState _matchOutcome = new();
    private readonly EngineStates.CoreMatchParticipantsState _matchParticipants = new();
    private readonly EngineStates.CoreMatchSetupState _matchSetup = new();
    private readonly EngineStates.CorePhysicalCardPopulationState _physicalCardPopulation = new();
    private readonly EngineStates.CorePlayerDecisionState _playerDecision = new();
    private readonly EngineStates.CharacterCaoPiMouState _characterCaoPiMou = new();
    private readonly EngineStates.CharacterCaoXiOlState _characterCaoXiOl = new();
    private readonly EngineStates.CharacterCuiLieAnniversaryState _characterCuiLieAnniversary = new();
    private readonly EngineStates.CharacterDaQiaoXiaoQiaoOlState _characterDaQiaoXiaoQiaoOl = new();
    private readonly EngineStates.CharacterDingYuanOlState _characterDingYuanOl = new();
    private readonly EngineStates.CharacterDuXiOlState _characterDuXiOl = new();
    private readonly EngineStates.CharacterHeQiOlState _characterHeQiOl = new();
    private readonly EngineStates.CharacterHuCheErMouState _characterHuCheErMou = new();
    private readonly EngineStates.CharacterHuaTuoMobileGodState _characterHuaTuoMobileGod = new();
    private readonly EngineStates.CharacterHuangChengYanOlState _characterHuangChengYanOl = new();
    private readonly EngineStates.CharacterHuangChongMobileShiState _characterHuangChongMobileShi = new();
    private readonly EngineStates.CharacterLiuQiOlState _characterLiuQiOl = new();
    private readonly EngineStates.CharacterLvKaiOlState _characterLvKaiOl = new();
    private readonly EngineStates.CharacterTaiShiCiMobileShiState _characterTaiShiCiMobileShi = new();
    private readonly EngineStates.CharacterWangGuanOlState _characterWangGuanOl = new();
    private readonly EngineStates.CharacterXiahouJieAnniversaryState _characterXiahouJieAnniversary = new();
    private readonly EngineStates.CharacterYangWanOlState _characterYangWanOl = new();
    private readonly EngineStates.CharacterZhangLingOlState _characterZhangLingOl = new();
    private readonly EngineStates.CharacterZhangXunAnniversaryState _characterZhangXunAnniversary = new();
    private readonly EngineStates.CharacterZhugeGuoOlState _characterZhugeGuoOl = new();
    private readonly EngineStates.CharacterZhugeLiangMouState _characterZhugeLiangMou = new();
    private readonly EngineStates.CharacterZouShiLeState _characterZouShiLe = new();
    private readonly EngineStates.CoreDirectedTurnCardPoliciesState _directedTurnCardPolicies2 = new();
    private readonly EngineStates.CoreEndingHandCountProgramsState _endingHandCountPrograms = new();
    private readonly EngineStates.CoreGameEngineActionQueriesState _actionQueries = new();
    private readonly EngineStates.CoreGameEngineActivityOrdinaryTricksState _activityOrdinaryTricks = new();
    private readonly EngineStates.CoreGameEngineActorHandLimitPenaltiesState _actorHandLimitPenalties2 = new();
    private readonly EngineStates.CoreGameEngineActualPlayPhaseCardUsesState _actualPlayPhaseCardUses = new();
    private readonly EngineStates.CoreGameEngineAdvancedLifecycleState _advancedLifecycle = new();
    private readonly EngineStates.CoreGameEngineAdvancedPoliciesState _advancedPolicies = new();
    private readonly EngineStates.CoreGameEngineAdvancedWeaponsState _advancedWeapons = new();
    private readonly EngineStates.CoreGameEngineAlcoholSlashSuppressionsState _alcoholSlashSuppressions = new();
    private readonly EngineStates.CoreGameEngineAlternatingSuitAndCompletedTopState _alternatingSuitAndCompletedTop = new();
    private readonly EngineStates.CoreGameEngineAlternativeViewAsCostsState _alternativeViewAsCosts = new();
    private readonly EngineStates.CoreGameEngineCardActionsState _cardActions = new();
    private readonly EngineStates.CoreGameEngineCardConversionsState _cardConversions = new();
    private readonly EngineStates.CoreGameEngineCardUseDebitsState _cardUseDebits2 = new();
    private readonly EngineStates.CoreGameEngineCharacterStateProgramsState _characterStatePrograms = new();
    private readonly EngineStates.CoreGameEngineConfiguredConversionsState _configuredConversions = new();
    private readonly EngineStates.CoreGameEngineConvertingGiftsState _convertingGifts = new();
    private readonly EngineStates.CoreGameEngineCurrentTurnShownCardBenefitsState _currentTurnShownCardBenefits = new();
    private readonly EngineStates.CoreGameEngineCurrentTurnSkillSuppressionsState _currentTurnSkillSuppressions2 = new();
    private readonly EngineStates.CoreGameEngineDeckEquipmentProgramsState _deckEquipmentPrograms = new();
    private readonly EngineStates.CoreGameEngineDeferredBenefitsProgramsState _deferredBenefitsPrograms = new();
    private readonly EngineStates.CoreGameEngineDeferredCardsState _deferredCards = new();
    private readonly EngineStates.CoreGameEngineDeferredHandAlignmentsState _deferredHandAlignments2 = new();
    private readonly EngineStates.CoreGameEngineDragonBoatGeneralsState _dragonBoatGenerals = new();
    private readonly EngineStates.CoreGameEngineDyingWindowSkillPoliciesState _dyingWindowSkillPolicies = new();
    private readonly EngineStates.CoreGameEngineFriendTrioState _friendTrio = new();
    private readonly EngineStates.CoreGameEngineGuanduActivitiesState _guanduActivities = new();
    private readonly EngineStates.CoreGameEngineHpChangeProgramsState _hpChangePrograms = new();
    private readonly EngineStates.CoreGameEngineHpConditionalSkillGrantsState _hpConditionalSkillGrants = new();
    private readonly EngineStates.CoreGameEngineIssuedPlayUsePoliciesState _issuedPlayUsePolicies = new();
    private readonly EngineStates.CoreGameEngineLordSkillProjectionsState _lordSkillProjections = new();
    private readonly EngineStates.CoreGameEngineNativeFactionChangesState _nativeFactionChanges = new();
    private readonly EngineStates.CoreGameEngineNextCardTargetAdjustmentsState _nextCardTargetAdjustments = new();
    private readonly EngineStates.CoreGameEngineNextSlashProgramsState _nextSlashPrograms = new();
    private readonly EngineStates.CoreGameEngineOrderedPrintedSkillLossState _orderedPrintedSkillLoss = new();
    private readonly EngineStates.CoreGameEngineOriginalHandEntitiesState _originalHandEntities = new();
    private readonly EngineStates.CoreGameEngineOriginalTargetAdditionsState _originalTargetAdditions = new();
    private readonly EngineStates.CoreGameEnginePhaseNamePredictionState _phaseNamePrediction = new();
    private readonly EngineStates.CoreGameEnginePrivateGeneralLibrariesState _privateGeneralLibraries2 = new();
    private readonly EngineStates.CoreGameEnginePrivateWhirlState _privateWhirl = new();
    private readonly EngineStates.CoreGameEngineProgramBooleanStateExpirationState _programBooleanStateExpiration = new();
    private readonly EngineStates.CoreGameEngineProgramPersistentCombatPoliciesState _programPersistentCombatPolicies = new();
    private readonly EngineStates.CoreGameEngineProgramPersistentStateState _programPersistentState = new();
    private readonly EngineStates.CoreGameEngineProgramPhaseColorRestrictionsState _programPhaseColorRestrictions2 = new();
    private readonly EngineStates.CoreGameEnginePublicPileSourcesState _publicPileSources = new();
    private readonly EngineStates.CoreGameEngineQuotaTopProgramsState _quotaTopPrograms = new();
    private readonly EngineStates.CoreGameEngineRecipientCategoryMarksState _recipientCategoryMarks2 = new();
    private readonly EngineStates.CoreGameEngineRedAdditionalTargetsState _redAdditionalTargets = new();
    private readonly EngineStates.CoreGameEngineRedDiscardRecoveryState _redDiscardRecovery = new();
    private readonly EngineStates.CoreGameEngineRuleProgressionState _ruleProgression2 = new();
    private readonly EngineStates.CoreGameEngineShenwuActivitiesState _shenwuActivities = new();
    private readonly EngineStates.CoreGameEngineShenwuExpandedPoliciesState _shenwuExpandedPolicies = new();
    private readonly EngineStates.CoreGameEngineSkillProgramsState _skillPrograms = new();
    private readonly EngineStates.CoreGameEngineSourceCursesState _sourceCurses2 = new();
    private readonly EngineStates.CoreGameEngineStrategicProgramsState _strategicPrograms = new();
    private readonly EngineStates.CoreGameEngineTableOrderState _tableOrder = new();
    private readonly EngineStates.CoreGameEngineTurnDefaultStatsState _turnDefaultStats = new();
    private readonly EngineStates.CoreGameEngineZhaoZhongProvenanceState _zhaoZhongProvenance = new();
    private readonly EngineStates.CoreLeastHandMarkerDrawProgramsState _leastHandMarkerDrawPrograms = new();
    private readonly EngineStates.CoreProgramParticipantPhaseOperationsState _programParticipantPhaseOperations = new();
    private IReadOnlyList<ProgramNativeCapabilityBinding> __GeneratedActualStateOwnerBindings() => new ProgramNativeCapabilityBinding[]
    {
        new(typeof(EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsAccess), _actualDiscardLedger),
        new(typeof(EngineStates.Contracts.ICoreActualDiscardLedgerDiscardPhaseHandDiscardIdsDiscardPhaseHandDiscardTurnNumberAccess), _actualDiscardLedger),
        new(typeof(EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess), _actualRoundProgress),
        new(typeof(EngineStates.Contracts.ICoreActualRoundProgressRoundTurnSeatsAccess), _actualRoundProgress),
        new(typeof(EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess), _actualRoundProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnProgressionAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressStartedAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberTurnProgressionAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseStartedAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressPhaseTurnNumberAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatStartedAccess), _actualTurnProgress),
        new(typeof(EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess), _aiBrains2),
        new(typeof(EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess), _aiDiagnostics),
        new(typeof(EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess), _aiDiagnostics),
        new(typeof(EngineStates.Contracts.ICoreAiDiagnosticsAiGeneralThoughtsAiThoughtsAccess), _aiDiagnostics),
        new(typeof(EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess), _cardMovementJournal),
        new(typeof(EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess), _cardMovementJournal),
        new(typeof(EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess), _cardMovementJournal),
        new(typeof(EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess), _commandJournalStorage),
        new(typeof(EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess), _commandJournalStorage),
        new(typeof(EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess), _commandJournalStorage),
        new(typeof(EngineStates.Contracts.ICoreCommandJournalStorageEventSequencePendingEventsAccess), _commandJournalStorage),
        new(typeof(EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess), _commandJournalStorage),
        new(typeof(EngineStates.Contracts.ICoreCommandJournalStorageEventSequenceEventsPendingEventsAccess), _commandJournalStorage),
        new(typeof(EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerAccess), _currentTurnStatistics),
        new(typeof(EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnAccess), _currentTurnStatistics),
        new(typeof(EngineStates.Contracts.ICoreCurrentTurnStatisticsSlashCountThisTurnWoodenOxUsedThisTurnAccess), _currentTurnStatistics),
        new(typeof(EngineStates.Contracts.ICoreCurrentTurnStatisticsUsedOrPlayedSlashDuringPlayPhaseAccess), _currentTurnStatistics),
        new(typeof(EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseKillCountByCurrentPlayerAccess), _currentTurnStatistics),
        new(typeof(EngineStates.Contracts.ICoreCurrentTurnStatisticsWoodenOxUsedThisTurnAccess), _currentTurnStatistics),
        new(typeof(EngineStates.Contracts.ICoreCurrentTurnStatisticsPlayPhaseDamageDealtByCurrentPlayerPlayPhaseKillCountByCurrentPlayerSlashCountThisTurnUsedOrPlayedSlashDuringPlayPhaseWoodenOxUsedThisTurnAccess), _currentTurnStatistics),
        new(typeof(EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess), _delayedTurnEffects),
        new(typeof(EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess), _gameLog),
        new(typeof(EngineStates.Contracts.ICoreGameLogLogAccess), _gameLog),
        new(typeof(EngineStates.Contracts.ICoreGameLogLogSequenceAccess), _gameLog),
        new(typeof(EngineStates.Contracts.ICoreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess), _judgmentEffectiveCards),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsPlayerCountAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionPlayerCountAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchDefinitionGeneralPoolOptionsAccess), _matchDefinition),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeStatusAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerSeatWinnerTeamIdAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerWinnerFactionIdWinnerSeatWinnerTeamIdAccess), _matchOutcome),
        new(typeof(EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess), _matchParticipants),
        new(typeof(EngineStates.Contracts.ICoreMatchSetupSetupCompleteAccess), _matchSetup),
        new(typeof(EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsAccess), _matchSetup),
        new(typeof(EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedAccess), _matchSetup),
        new(typeof(EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess), _matchSetup),
        new(typeof(EngineStates.Contracts.ICoreMatchSetupAvailableGeneralsSelectionOrderAccess), _matchSetup),
        new(typeof(EngineStates.Contracts.ICoreMatchSetupGameStartingProgramsResolvedSetupCompleteAccess), _matchSetup),
        new(typeof(EngineStates.Contracts.ICorePhysicalCardPopulationInitialCardCountAccess), _physicalCardPopulation),
        new(typeof(EngineStates.Contracts.ICorePlayerDecisionNextPromptIdAccess), _playerDecision),
        new(typeof(EngineStates.Contracts.ICorePlayerDecisionPendingDecisionBackingAccess), _playerDecision),
        new(typeof(EngineStates.Contracts.ICharacterCaoPiMouMouCaoPiControlsAccess), _characterCaoPiMou),
        new(typeof(EngineStates.Contracts.ICharacterCaoXiOlCaoXiActiveUnitsCaoXiHistoryCountCaoXiHistoryTailAccess), _characterCaoXiOl),
        new(typeof(EngineStates.Contracts.ICharacterCuiLieAnniversaryCuiLieQualificationInputsCuiLieQualificationStampAccess), _characterCuiLieAnniversary),
        new(typeof(EngineStates.Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess), _characterDaQiaoXiaoQiaoOl),
        new(typeof(EngineStates.Contracts.ICharacterDingYuanOlSynchronizingAdoptedChildGrantsAccess), _characterDingYuanOl),
        new(typeof(EngineStates.Contracts.ICharacterDuXiOlDuXiSuitProjectionDuXiSuitProjectionEventCountDuXiSuitProjectionLastFactAccess), _characterDuXiOl),
        new(typeof(EngineStates.Contracts.ICharacterHeQiOlEquipmentSuitSkillInstancesSynchronizingEquipmentSuitSkillGrantsAccess), _characterHeQiOl),
        new(typeof(EngineStates.Contracts.ICharacterHuCheErMouMouHuCheErQualificationDependenciesMouHuCheErQualificationStampAccess), _characterHuCheErMou),
        new(typeof(EngineStates.Contracts.ICharacterHuaTuoMobileGodMobileWulingCoursesAccess), _characterHuaTuoMobileGod),
        new(typeof(EngineStates.Contracts.ICharacterHuangChengYanOlJiezhenConversionsAccess), _characterHuangChengYanOl),
        new(typeof(EngineStates.Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess), _characterHuangChongMobileShi),
        new(typeof(EngineStates.Contracts.ICharacterLiuQiOlHasIssuedLiuQiAccess), _characterLiuQiOl),
        new(typeof(EngineStates.Contracts.ICharacterLvKaiOlBijingMarkedCardIdsAccess), _characterLvKaiOl),
        new(typeof(EngineStates.Contracts.ICharacterTaiShiCiMobileShiShiTzLieScanSequenceAccess), _characterTaiShiCiMobileShi),
        new(typeof(EngineStates.Contracts.ICharacterWangGuanOlMiuyanQualificationDependenciesMiuyanQualificationStampAccess), _characterWangGuanOl),
        new(typeof(EngineStates.Contracts.ICharacterXiahouJieAnniversaryAnniversaryXiahouJieQualificationInputsAnniversaryXiahouJieQualificationStampAccess), _characterXiahouJieAnniversary),
        new(typeof(EngineStates.Contracts.ICharacterYangWanOlZhuihuanArmChoicesAccess), _characterYangWanOl),
        new(typeof(EngineStates.Contracts.ICharacterZhangLingOlSynchronizingCategoryTalismansAccess), _characterZhangLingOl),
        new(typeof(EngineStates.Contracts.ICharacterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess), _characterZhangXunAnniversary),
        new(typeof(EngineStates.Contracts.ICharacterZhugeGuoOlHasIssuedZhugeGuoStateAccess), _characterZhugeGuoOl),
        new(typeof(EngineStates.Contracts.ICharacterZhugeLiangMouMouWujingAllowancesMouZhitianCountsAccess), _characterZhugeLiangMou),
        new(typeof(EngineStates.Contracts.ICharacterZouShiLeYunzhengQualificationInputsYunzhengQualificationSeatsYunzhengQualificationStampAccess), _characterZouShiLe),
        new(typeof(EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess), _directedTurnCardPolicies2),
        new(typeof(EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesDirectedTurnCardPolicySequenceAccess), _directedTurnCardPolicies2),
        new(typeof(EngineStates.Contracts.ICoreEndingHandCountProgramsEndingHandCountHistoriesAccess), _endingHandCountPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineActionQueriesActionQueryAccess), _actionQueries),
        new(typeof(EngineStates.Contracts.ICoreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess), _activityOrdinaryTricks),
        new(typeof(EngineStates.Contracts.ICoreGameEngineActorHandLimitPenaltiesActorHandLimitPenaltiesActorHandLimitPenaltySequenceAccess), _actorHandLimitPenalties2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess), _actualPlayPhaseCardUses),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleCopiedWeaponAbilitiesInheritedWeaponAbilitiesWeaponCopyConsumedAccess), _advancedLifecycle),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAdvancedLifecycleInheritedWeaponAbilitiesAccess), _advancedLifecycle),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAdvancedPoliciesNextUnlimitedCardOwnersAccess), _advancedPolicies),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess), _advancedWeapons),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsUsedGeneralWeaponSourcesAccess), _advancedWeapons),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAdvancedWeaponsCopiedWeaponSkillsGeneratedPhysicalCardIdsPermanentlyRemovedWeaponsUsedGeneralWeaponSourcesAccess), _advancedWeapons),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess), _alcoholSlashSuppressions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAlternatingSuitAndCompletedTopFirstTurnCategoryUsesAccess), _alternatingSuitAndCompletedTop),
        new(typeof(EngineStates.Contracts.ICoreGameEngineAlternativeViewAsCostsUsedProgramBasicCardNamesAccess), _alternativeViewAsCosts),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardActionsCardActionSequenceAccess), _cardActions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess), _cardConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess), _cardConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess), _cardConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess), _cardConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedResponseConversionAccess), _cardConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardConversionsSelectedUseConversionAccess), _cardConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess), _cardUseDebits2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdCardUseDebitsRefundedCardUseDebitsAccess), _cardUseDebits2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCharacterStateProgramsPendingCharacterStateChangesAccess), _characterStatePrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersDeclaredConversionCardsAccess), _configuredConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineConfiguredConversionsConfiguredConversionTiersAccess), _configuredConversions),
        new(typeof(EngineStates.Contracts.ICoreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess), _convertingGifts),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCurrentTurnShownCardBenefitsCurrentTurnHeartSlashBonusesAccess), _currentTurnShownCardBenefits),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess), _currentTurnSkillSuppressions2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnQualificationDependenciesCurrentTurnQualificationStampCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess), _currentTurnSkillSuppressions2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionCurrentTurnSkillSuppressionsAccess), _currentTurnSkillSuppressions2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess), _deckEquipmentPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldsAccess), _deckEquipmentPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramNextTurnRuleModifiersAccess), _deferredBenefitsPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess), _deferredBenefitsPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDeferredCardsDeferredPublicPileDepositsAccess), _deferredCards),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDeferredHandAlignmentsDeferredHandAlignmentSequenceDeferredHandAlignmentsAccess), _deferredHandAlignments2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess), _dragonBoatGenerals),
        new(typeof(EngineStates.Contracts.ICoreGameEngineDyingWindowSkillPoliciesDyingQualificationDependenciesDyingQualificationStampAccess), _dyingWindowSkillPolicies),
        new(typeof(EngineStates.Contracts.ICoreGameEngineFriendTrioFriendFangqiuSpentFriendPredictionCountsFriendPredictionsFriendQihuiUnlimitedAccess), _friendTrio),
        new(typeof(EngineStates.Contracts.ICoreGameEngineFriendTrioFriendQihuiUnlimitedAccess), _friendTrio),
        new(typeof(EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess), _guanduActivities),
        new(typeof(EngineStates.Contracts.ICoreGameEngineHpChangeProgramsPendingHpChangesAccess), _hpChangePrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineHpConditionalSkillGrantsHpConditionalSkillInstancesSynchronizingHpConditionalSkillGrantsAccess), _hpConditionalSkillGrants),
        new(typeof(EngineStates.Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess), _issuedPlayUsePolicies),
        new(typeof(EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess), _lordSkillProjections),
        new(typeof(EngineStates.Contracts.ICoreGameEngineNativeFactionChangesNativeFactionObservationsNativeFactionRevisionAccess), _nativeFactionChanges),
        new(typeof(EngineStates.Contracts.ICoreGameEngineNativeFactionChangesFieldsAccess), _nativeFactionChanges),
        new(typeof(EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess), _nextCardTargetAdjustments),
        new(typeof(EngineStates.Contracts.ICoreGameEngineNextCardTargetAdjustmentsNextCardTargetAdjustmentOwnersAccess), _nextCardTargetAdjustments),
        new(typeof(EngineStates.Contracts.ICoreGameEngineNextSlashProgramsNamedNextSlashReservesAccess), _nextSlashPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesAccess), _orderedPrintedSkillLoss),
        new(typeof(EngineStates.Contracts.ICoreGameEngineOrderedPrintedSkillLossOrderedPrintedSkillLossLeasesOrderedPrintedSkillLossStateStartedAccess), _orderedPrintedSkillLoss),
        new(typeof(EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesOriginalHandPermanentBonusesAccess), _originalHandEntities),
        new(typeof(EngineStates.Contracts.ICoreGameEngineOriginalHandEntitiesOriginalHandEntityStatesAccess), _originalHandEntities),
        new(typeof(EngineStates.Contracts.ICoreGameEngineOriginalTargetAdditionsOriginalTargetAdditionGrantsAccess), _originalTargetAdditions),
        new(typeof(EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess), _phaseNamePrediction),
        new(typeof(EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesFieldsAccess), _privateGeneralLibraries2),
        new(typeof(EngineStates.Contracts.ICoreGameEnginePrivateGeneralLibrariesPrivateGeneralLibrariesPrivateGeneralLibraryRevisionAccess), _privateGeneralLibraries2),
        new(typeof(EngineStates.Contracts.ICoreGameEnginePrivateWhirlHasIssuedPrivateWhirlAccess), _privateWhirl),
        new(typeof(EngineStates.Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess), _programBooleanStateExpiration),
        new(typeof(EngineStates.Contracts.ICoreGameEngineProgramPersistentCombatPoliciesProgramGameFactionRangeTargetsProgramPlayDamageHandLimitsAccess), _programPersistentCombatPolicies),
        new(typeof(EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess), _programPersistentState),
        new(typeof(EngineStates.Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess), _programPhaseColorRestrictions2),
        new(typeof(EngineStates.Contracts.ICoreGameEnginePublicPileSourcesPublicPersistentPilesAccess), _publicPileSources),
        new(typeof(EngineStates.Contracts.ICoreGameEngineQuotaTopProgramsFirstTurnTargetActionsAccess), _quotaTopPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess), _recipientCategoryMarks2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsAccess), _redAdditionalTargets),
        new(typeof(EngineStates.Contracts.ICoreGameEngineRedAdditionalTargetsRedAdditionalTargetGrantsSelectedRedAdditionalTargetSourceAccess), _redAdditionalTargets),
        new(typeof(EngineStates.Contracts.ICoreGameEngineRedDiscardRecoveryTurnRedDiscardCountTurnRedDiscardTurnAccess), _redDiscardRecovery),
        new(typeof(EngineStates.Contracts.ICoreGameEngineRuleProgressionRuleProgressionAccess), _ruleProgression2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineShenwuExpandedPoliciesShenwuFengdongDependenciesShenwuFengdongStampAccess), _shenwuExpandedPolicies),
        new(typeof(EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesAccess), _skillPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess), _skillPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesProgramPhaseUsesProgramUsesAccess), _skillPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineSkillProgramsProgramContributionUsesAccess), _skillPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess), _sourceCurses2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineSourceCursesSourceCursesAccess), _sourceCurses2),
        new(typeof(EngineStates.Contracts.ICoreGameEngineStrategicProgramsStrategicEndPlayTurnAccess), _strategicPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineStrategicProgramsProgramSuppressionsStrategicEndPlayTurnStrategicHandLimitPenaltyTurnsAccess), _strategicPrograms),
        new(typeof(EngineStates.Contracts.ICoreGameEngineTableOrderCachedTableMovementCachedTableOrderAccess), _tableOrder),
        new(typeof(EngineStates.Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess), _turnDefaultStats),
        new(typeof(EngineStates.Contracts.ICoreGameEngineZhaoZhongProvenanceZhaoZhongDrawInvocationCountsZhaoZhongDrawInvoiceFactsZhaoZhongDrawInvoicesAccess), _zhaoZhongProvenance),
        new(typeof(EngineStates.Contracts.ICoreLeastHandMarkerDrawProgramsMarkerExtraDrawPoliciesAccess), _leastHandMarkerDrawPrograms),
        new(typeof(EngineStates.Contracts.ICoreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess), _programParticipantPhaseOperations)
    };
}
