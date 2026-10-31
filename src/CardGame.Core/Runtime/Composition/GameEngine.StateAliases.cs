#nullable enable

namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly List<DirectedTurnCardPolicy> _directedTurnCardPolicies => ref _directedTurnCardPolicies2.ReferenceToDirectedTurnCardPolicies;
    private ref long _directedTurnCardPolicySequence => ref _directedTurnCardPolicies2.ReferenceToDirectedTurnCardPolicySequence;
    private ref readonly Dictionary<(int Owner, string Skill, string Instance, string State), ProgramEndingHandCountHistory> _endingHandCountHistories => ref _endingHandCountPrograms.ReferenceToEndingHandCountHistories;
    // A legal-action read is pure. Share its derived answers only until that read
    // returns; no revision cache can survive a command or a skill/seat mutation.
    private ref NativeOwners.ActionQuery? _actionQuery => ref _actionQueries.ReferenceToActionQuery;
    // Projections and their cache belong to this match. Registered source programs keep their content identity.
    private ref readonly Dictionary<SkillProgram, SkillProgram> _activityOrdinaryPrograms => ref _activityOrdinaryTricks.ReferenceToActivityOrdinaryPrograms;
    // Issued rule contributions, not pending resolution state. Command replay
    // recreates them from the same native use and actual-turn boundary facts.
    private ref readonly List<ActorHandLimitPenalty> _actorHandLimitPenalties => ref _actorHandLimitPenalties2.ReferenceToActorHandLimitPenalties;
    private ref long _actorHandLimitPenaltySequence => ref _actorHandLimitPenalties2.ReferenceToActorHandLimitPenaltySequence;
    private ref readonly HashSet<long> _actualPlayPhaseUseActions => ref _actualPlayPhaseCardUses.ReferenceToActualPlayPhaseUseActions;
    private ref readonly Dictionary<int, IReadOnlyList<CardKind>> _inheritedWeaponAbilities => ref _advancedLifecycle.ReferenceToInheritedWeaponAbilities;
    private ref readonly Dictionary<int, IReadOnlyList<CardKind>> _copiedWeaponAbilities => ref _advancedLifecycle.ReferenceToCopiedWeaponAbilities;
    private ref readonly HashSet<int> _weaponCopyConsumed => ref _advancedLifecycle.ReferenceToWeaponCopyConsumed;
    private ref readonly HashSet<int> _nextUnlimitedCardOwners => ref _advancedPolicies.ReferenceToNextUnlimitedCardOwners;
    private ref readonly HashSet<int> _permanentlyRemovedWeapons => ref _advancedWeapons.ReferenceToPermanentlyRemovedWeapons;
    private ref readonly HashSet<int> _generatedPhysicalCardIds => ref _advancedWeapons.ReferenceToGeneratedPhysicalCardIds;
    private ref readonly HashSet<string> _usedGeneralWeaponSources => ref _advancedWeapons.ReferenceToUsedGeneralWeaponSources;
    private ref readonly Dictionary<int, IReadOnlyList<string>> _copiedWeaponSkills => ref _advancedWeapons.ReferenceToCopiedWeaponSkills;
    private ref readonly List<CurrentTurnOwnSkillSuppression> _currentTurnOwnSkillSuppressions => ref _alcoholSlashSuppressions.ReferenceToCurrentTurnOwnSkillSuppressions;
    private ref readonly Dictionary<(int Turn, int Actor, string Skill, SkillProgramCardCategory Category), long> _firstTurnCategoryUses => ref _alternatingSuitAndCompletedTop.ReferenceToFirstTurnCategoryUses;
    private ref readonly Dictionary<int, (int Turn, HashSet<CardKind> Kinds)> _usedProgramBasicCardNames => ref _alternativeViewAsCosts.ReferenceToUsedProgramBasicCardNames;
    private ref long _cardActionSequence => ref _cardActions.ReferenceToCardActionSequence;
    private ref CardConversionSource? _selectedResponseConversion => ref _cardConversions.ReferenceToSelectedResponseConversion;
    private ref CardConversionSource? _selectedUseConversion => ref _cardConversions.ReferenceToSelectedUseConversion;
    private ref bool _hasSelectedResponseConversionChoice => ref _cardConversions.ReferenceToHasSelectedResponseConversionChoice;
    private ref bool _hasSelectedUseConversionChoice => ref _cardConversions.ReferenceToHasSelectedUseConversionChoice;
    private ref int _cardUseDebitPhaseInstanceId => ref _cardUseDebits2.ReferenceToCardUseDebitPhaseInstanceId;
    private ref readonly Dictionary<long, CardUseDebitIdentity> _cardUseDebits => ref _cardUseDebits2.ReferenceToCardUseDebits;
    private ref readonly HashSet<long> _refundedCardUseDebits => ref _cardUseDebits2.ReferenceToRefundedCardUseDebits;
    private ref readonly List<CharacterStateChangeContext> _pendingCharacterStateChanges => ref _characterStatePrograms.ReferenceToPendingCharacterStateChanges;
    private ref readonly Dictionary<(int Seat, string State), int> _configuredConversionTiers => ref _configuredConversions.ReferenceToConfiguredConversionTiers;
    private ref readonly Dictionary<(int Seat, string State), (int Turn, int Card, CardKind Kind)> _declaredConversionCards => ref _configuredConversions.ReferenceToDeclaredConversionCards;
    private ref readonly List<GiftHandRetentionObligation> _giftHandRetentionObligations => ref _convertingGifts.ReferenceToGiftHandRetentionObligations;
    private ref long _giftHandRetentionSequence => ref _convertingGifts.ReferenceToGiftHandRetentionSequence;
    private ref readonly List<CurrentTurnDirectedHeartSlashBonus> _currentTurnHeartSlashBonuses => ref _currentTurnShownCardBenefits.ReferenceToCurrentTurnHeartSlashBonuses;
    private ref readonly List<CurrentTurnNonLockedSkillSuppression> _currentTurnSkillSuppressions => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnSkillSuppressions;
    private ref long _currentTurnSkillSuppressionRevision => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnSkillSuppressionRevision;
    private ref long _currentTurnQualificationStamp => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnQualificationStamp;
    private ref (long Projection, long Suppression) _currentTurnQualificationDependencies => ref _currentTurnSkillSuppressions2.ReferenceToCurrentTurnQualificationDependencies;
    private ref readonly List<BeneficiarySuitShield> _beneficiarySuitShields => ref _deckEquipmentPrograms.ReferenceToBeneficiarySuitShields;
    private ref long _beneficiarySuitShieldSequence => ref _deckEquipmentPrograms.ReferenceToBeneficiarySuitShieldSequence;
    private ref readonly List<ProgramNextTurnRuleModifier> _programNextTurnRuleModifiers => ref _deferredBenefitsPrograms.ReferenceToProgramNextTurnRuleModifiers;
    private ref readonly List<ProgramFactionRecoveryDebt> _programFactionRecoveryDebts => ref _deferredBenefitsPrograms.ReferenceToProgramFactionRecoveryDebts;
    private ref readonly List<DeferredPublicPileDeposit> _deferredPublicPileDeposits => ref _deferredCards.ReferenceToDeferredPublicPileDeposits;
    private ref readonly List<DeferredHandAlignment> _deferredHandAlignments => ref _deferredHandAlignments2.ReferenceToDeferredHandAlignments;
    private ref long _deferredHandAlignmentSequence => ref _deferredHandAlignments2.ReferenceToDeferredHandAlignmentSequence;
    private ref bool _synchronizingDragonBoatGrants => ref _dragonBoatGenerals.ReferenceToSynchronizingDragonBoatGrants;
    private ref (long Base, long Frame, int Victim, int Turn, int TurnOwner, long SourceRevision, int SourceHp, bool SourceAlive) _dyingQualificationDependencies => ref _dyingWindowSkillPolicies.ReferenceToDyingQualificationDependencies;
    private ref long _dyingQualificationStamp => ref _dyingWindowSkillPolicies.ReferenceToDyingQualificationStamp;
    private ref readonly Dictionary<(int Seat, string Instance), FriendPredictionPolicy> _friendPredictions => ref _friendTrio.ReferenceToFriendPredictions;
    private ref readonly Dictionary<(int Seat, string Instance), int> _friendPredictionCounts => ref _friendTrio.ReferenceToFriendPredictionCounts;
    private ref readonly HashSet<(int Seat, string Instance)> _friendFangqiuSpent => ref _friendTrio.ReferenceToFriendFangqiuSpent;
    private ref readonly HashSet<int> _friendQihuiUnlimited => ref _friendTrio.ReferenceToFriendQihuiUnlimited;
    private ref GuanduBattleConfiguredEvent? _guanduInitialConfiguration => ref _guanduActivities.ReferenceToGuanduInitialConfiguration;
    private ref long _guanduQualificationStamp => ref _guanduActivities.ReferenceToGuanduQualificationStamp;
    private ref (long Underlying, int LivingMask, int FactionHash) _guanduQualificationDependencies => ref _guanduActivities.ReferenceToGuanduQualificationDependencies;
    private ref readonly List<HpChangeContext> _pendingHpChanges => ref _hpChangePrograms.ReferenceToPendingHpChanges;
    private ref bool _synchronizingHpConditionalSkillGrants => ref _hpConditionalSkillGrants.ReferenceToSynchronizingHpConditionalSkillGrants;
    // A source relation remembers its first effective instance through HP exits.
    // This preserves scoped runtime state even when an independent shared grant
    // disappears while the conditional grant is absent. Replay reconstructs it.
    private ref readonly Dictionary<(int OwnerSeat, HpConditionalSkillGrantSource Source), string> _hpConditionalSkillInstances => ref _hpConditionalSkillGrants.ReferenceToHpConditionalSkillInstances;
    // Issued phase facts survive source loss/suppression until this actual phase ends.
    private ref readonly List<IssuedPlayPhaseUseProhibition> _issuedPlayPhaseUseProhibitions => ref _issuedPlayUsePolicies.ReferenceToIssuedPlayPhaseUseProhibitions;
    private ref bool _synchronizingLordProjections => ref _lordSkillProjections.ReferenceToSynchronizingLordProjections;
    private ref ProjectionDependency[] _lordProjectionDependencies => ref _lordSkillProjections.ReferenceToLordProjectionDependencies;
    private ref long _lordProjectionDependencyRevision => ref _lordSkillProjections.ReferenceToLordProjectionDependencyRevision;
    private ref int? _modeInitialActorSeat => ref _matchSetup.ReferenceToModeInitialActorSeat;
    private ref readonly HashSet<int> _roundTurnSeats => ref _actualRoundProgress.ReferenceToRoundTurnSeats;
    private ref int _roundNumber => ref _actualRoundProgress.ReferenceToRoundNumber;
    private ref readonly Dictionary<int, NativeFactionObservation> _nativeFactionObservations => ref _nativeFactionChanges.ReferenceToNativeFactionObservations;
    private ref long _nativeFactionRevision => ref _nativeFactionChanges.ReferenceToNativeFactionRevision;
    private ref long _nativeFactionQualificationStamp => ref _nativeFactionChanges.ReferenceToNativeFactionQualificationStamp;
    private ref (long Base, long Revision) _nativeFactionQualificationDependencies => ref _nativeFactionChanges.ReferenceToNativeFactionQualificationDependencies;
    private ref bool _synchronizingNativeFactions => ref _nativeFactionChanges.ReferenceToSynchronizingNativeFactions;
    private ref readonly Dictionary<int, CardConversionSource> _nextCardTargetAdjustmentOwners => ref _nextCardTargetAdjustments.ReferenceToNextCardTargetAdjustmentOwners;
    private ref IReadOnlyList<int>? _selectedNextCardTargetSeats => ref _nextCardTargetAdjustments.ReferenceToSelectedNextCardTargetSeats;
    private ref readonly List<NamedNextSlashReserve> _namedNextSlashReserves => ref _nextSlashPrograms.ReferenceToNamedNextSlashReserves;
    private ref readonly List<OrderedPrintedSkillLossLease> _orderedPrintedSkillLossLeases => ref _orderedPrintedSkillLoss.ReferenceToOrderedPrintedSkillLossLeases;
    private ref bool _orderedPrintedSkillLossStateStarted => ref _orderedPrintedSkillLoss.ReferenceToOrderedPrintedSkillLossStateStarted;
    // Durable gameplay state, not pending resolution. A departed physical
    // entity never becomes original again on return or source reacquisition.
    private ref readonly Dictionary<OriginalHandStateKey, OriginalHandEntityState> _originalHandEntityStates => ref _originalHandEntities.ReferenceToOriginalHandEntityStates;
    private ref readonly Dictionary<long, OriginalHandPermanentBonus> _originalHandPermanentBonuses => ref _originalHandEntities.ReferenceToOriginalHandPermanentBonuses;
    private ref readonly List<OriginalTargetAdditionGrant> _originalTargetAdditionGrants => ref _originalTargetAdditions.ReferenceToOriginalTargetAdditionGrants;
    private ref bool _gameStartingProgramsResolved => ref _matchSetup.ReferenceToGameStartingProgramsResolved;
    private ref readonly List<PhaseNamePredictionPolicy> _phaseNamePredictions => ref _phaseNamePrediction.ReferenceToPhaseNamePredictions;
    private ref readonly Dictionary<PrivateGeneralLibrarySource, PrivateGeneralLibrary> _privateGeneralLibraries => ref _privateGeneralLibraries2.ReferenceToPrivateGeneralLibraries;
    private ref long _privateGeneralLibraryRevision => ref _privateGeneralLibraries2.ReferenceToPrivateGeneralLibraryRevision;
    private ref bool _synchronizingPrivateGeneralLibraries => ref _privateGeneralLibraries2.ReferenceToSynchronizingPrivateGeneralLibraries;
    private ref long _combinedProjectionStamp => ref _privateGeneralLibraries2.ReferenceToCombinedProjectionStamp;
    private ref (long Lord, long Library) _combinedProjectionDependencies => ref _privateGeneralLibraries2.ReferenceToCombinedProjectionDependencies;
    private ref ProjectionDependency[] _privateLibraryOwnerDependencies => ref _privateGeneralLibraries2.ReferenceToPrivateLibraryOwnerDependencies;
    // Capability latch only: replayed commands rebuild it, it contains no use ID,
    // entity, pending cost or continuation. Ordinary matches never scan its history.
    private ref bool _hasIssuedPrivateWhirl => ref _privateWhirl.ReferenceToHasIssuedPrivateWhirl;
    private ref readonly List<ProgramBooleanStateLease> _programBooleanStateLeases => ref _programBooleanStateExpiration.ReferenceToProgramBooleanStateLeases;
    private ref ProgramPhaseSchedule? _programPhaseSchedule => ref phaseScheduleStorage.Current;
    private ref readonly Dictionary<int, ProgramPlayDamageHandLimit> _programPlayDamageHandLimits => ref _programPersistentCombatPolicies.ReferenceToProgramPlayDamageHandLimits;
    private ref readonly List<ProgramGameFactionRangeTargets> _programGameFactionRangeTargets => ref _programPersistentCombatPolicies.ReferenceToProgramGameFactionRangeTargets;
    private ref readonly Dictionary<ProgramBooleanStateKey, bool> _programBooleanStates => ref _programPersistentState.ReferenceToProgramBooleanStates;
    private ref readonly List<ProgramPhaseColorRestriction> _programPhaseColorRestrictions => ref _programPhaseColorRestrictions2.ReferenceToProgramPhaseColorRestrictions;
    private ref readonly Dictionary<(int Owner, string Skill, string Instance), PublicPersistentPileSource> _publicPersistentPiles => ref _publicPileSources.ReferenceToPublicPersistentPiles;
    private ref readonly Dictionary<(int Turn, int Target, string Skill), long> _firstTurnTargetActions => ref _quotaTopPrograms.ReferenceToFirstTurnTargetActions;
    // Durable gameplay tokens only. Every unfinished choice/payment belongs to its ProgramSkillFrame.
    private ref readonly Dictionary<RecipientCategoryMarkKey, RecipientCategoryMarkToken> _recipientCategoryMarks => ref _recipientCategoryMarks2.ReferenceToRecipientCategoryMarks;
    private ref bool _recipientCategoryMarkLedgerStarted => ref _recipientCategoryMarks2.ReferenceToRecipientCategoryMarkLedgerStarted;
    private ref readonly Dictionary<int, (CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants => ref _redAdditionalTargets.ReferenceToRedAdditionalTargetGrants;
    private ref CardConversionSource? _selectedRedAdditionalTargetSource => ref _redAdditionalTargets.ReferenceToSelectedRedAdditionalTargetSource;
    private ref int _turnRedDiscardCount => ref _redDiscardRecovery.ReferenceToTurnRedDiscardCount;
    private ref int _turnRedDiscardTurn => ref _redDiscardRecovery.ReferenceToTurnRedDiscardTurn;
    private ref RuleProgressionRuntime? _ruleProgression => ref _ruleProgression2.ReferenceToRuleProgression;
    private ref ShenwuDraftSetup? _shenwuDraft => ref _shenwuActivities.ReferenceToShenwuDraft;
    private ref (int Turn, int Seat, TurnPhase Phase, long Grants, bool Alive, bool Revealed)? _shenwuFengdongDependencies => ref _shenwuExpandedPolicies.ReferenceToShenwuFengdongDependencies;
    private ref long _shenwuFengdongStamp => ref _shenwuExpandedPolicies.ReferenceToShenwuFengdongStamp;
    // The journal rebuilds these counters through accepted commands. They are
    // keyed by content identities, never by translated presentation text.
    private ref readonly Dictionary<(int Seat, string Skill, string Activation), int> _programUses => ref _skillPrograms.ReferenceToProgramUses;
    private ref readonly Dictionary<(int Seat, string Skill, string Activation), int> _programPhaseUses => ref _skillPrograms.ReferenceToProgramPhaseUses;
    private ref readonly Dictionary<(int ProviderSeat, int SkillOwnerSeat, string Skill, string Contribution), int> _programContributionUses => ref _skillPrograms.ReferenceToProgramContributionUses;
    private ref readonly List<SourceCurseDeposit> _sourceCurses => ref _sourceCurses2.ReferenceToSourceCurses;
    private ref readonly List<SourceCurseLoss> _sourceCurseLosses => ref _sourceCurses2.ReferenceToSourceCurseLosses;
    private ref readonly List<ProgramSuppression> _programSuppressions => ref _strategicPrograms.ReferenceToProgramSuppressions;
    private ref int _strategicEndPlayTurn => ref _strategicPrograms.ReferenceToStrategicEndPlayTurn;
    private ref readonly Dictionary<int, int> _strategicHandLimitPenaltyTurns => ref _strategicPrograms.ReferenceToStrategicHandLimitPenaltyTurns;
    private ref TableSeatMovedEvent? _cachedTableMovement => ref _tableOrder.ReferenceToCachedTableMovement;
    private ref IReadOnlyList<int>? _cachedTableOrder => ref _tableOrder.ReferenceToCachedTableOrder;
    // This is durable gameplay state. Choices and their frozen pool remain only
    // on ProgramSkillFrame; no pending action is retained in this dictionary.
    private ref readonly Dictionary<TurnDefaultStatKey, TurnDefaultStatState> _turnDefaultStatStates => ref _turnDefaultStats.ReferenceToTurnDefaultStatStates;
    private ref bool _turnDefaultStatLedgerStarted => ref _turnDefaultStats.ReferenceToTurnDefaultStatLedgerStarted;
    private ref int? _winnerSeat => ref _matchOutcome.ReferenceToWinnerSeat;
    private ref IReadOnlyList<NativeDrawInvocationRecordedEvent>? _zhaoZhongDrawInvoiceFacts => ref _zhaoZhongProvenance.ReferenceToZhaoZhongDrawInvoiceFacts;
    private ref Dictionary<(int Turn, int Recipient), NativeDrawInvocationRecordedEvent[]> _zhaoZhongDrawInvoices => ref _zhaoZhongProvenance.ReferenceToZhaoZhongDrawInvoices;
    private ref Dictionary<long, int> _zhaoZhongDrawInvocationCounts => ref _zhaoZhongProvenance.ReferenceToZhaoZhongDrawInvocationCounts;
    // Issued permanent obligations are independent of later source availability;
    // resolution progress itself remains only on the exact draw owning frame.
    private ref readonly Dictionary<(int Target, PlayerMarkerKind Marker), ProgramMarkerExtraDrawPolicy> _markerExtraDrawPolicies => ref _leastHandMarkerDrawPrograms.ReferenceToMarkerExtraDrawPolicies;
    private ref readonly List<Suit> _fullDiscardPhaseSuits => ref _programParticipantPhaseOperations.ReferenceToFullDiscardPhaseSuits;
    private ref int _fullDiscardPhaseSuitTurn => ref _programParticipantPhaseOperations.ReferenceToFullDiscardPhaseSuitTurn;
    // Issued lasting restrictions, independent of the source skill and pending
    // instruction. Normal command replay rebuilds this per-match state.
    private ref readonly List<MouCaoPiControl> _mouCaoPiControls => ref _characterCaoPiMou.ReferenceToMouCaoPiControls;
    private ref IGameEvent? _caoXiHistoryTail => ref _characterCaoXiOl.ReferenceToCaoXiHistoryTail;
    private ref int _caoXiHistoryCount => ref _characterCaoXiOl.ReferenceToCaoXiHistoryCount;
    private ref IReadOnlyList<CaoXiGangshuIncreasedEvent> _caoXiActiveUnits => ref _characterCaoXiOl.ReferenceToCaoXiActiveUnits;
    private ref (long Inner, int Issued, int Ended) _cuiLieQualificationInputs => ref _characterCuiLieAnniversary.ReferenceToCuiLieQualificationInputs;
    private ref long _cuiLieQualificationStamp => ref _characterCuiLieAnniversary.ReferenceToCuiLieQualificationStamp;
    private ref readonly Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> _daQiaoXiaoQiaoQualificationCache => ref _characterDaQiaoXiaoQiaoOl.ReferenceToDaQiaoXiaoQiaoQualificationCache;
    private ref long _daQiaoXiaoQiaoQualificationRevision => ref _characterDaQiaoXiaoQiaoOl.ReferenceToDaQiaoXiaoQiaoQualificationRevision;
    private ref bool _synchronizingDaQiaoXiaoQiaoLuoyan => ref _characterDaQiaoXiaoQiaoOl.ReferenceToSynchronizingDaQiaoXiaoQiaoLuoyan;
    private ref bool _synchronizingAdoptedChildGrants => ref _characterDingYuanOl.ReferenceToSynchronizingAdoptedChildGrants;
    // Only a derived lookup cache. Its complete source is the immutable committed/pending rule facts.
    private ref int _duXiSuitProjectionEventCount => ref _characterDuXiOl.ReferenceToDuXiSuitProjectionEventCount;
    private ref IGameEvent? _duXiSuitProjectionLastFact => ref _characterDuXiOl.ReferenceToDuXiSuitProjectionLastFact;
    private ref IReadOnlyDictionary<int, int> _duXiSuitProjection => ref _characterDuXiOl.ReferenceToDuXiSuitProjection;
    private ref bool _synchronizingEquipmentSuitSkillGrants => ref _characterHeQiOl.ReferenceToSynchronizingEquipmentSuitSkillGrants;
    // A source relation remembers its first effective instance through equipment suit exits.
    // This preserves scoped runtime state even when an independent shared grant
    // disappears while the conditional grant is absent. Replay reconstructs it.
    private ref readonly Dictionary<(int OwnerSeat, EquipmentSuitSkillGrantSource Source), string> _equipmentSuitSkillInstances => ref _characterHeQiOl.ReferenceToEquipmentSuitSkillInstances;
    private ref readonly Dictionary<long, (int Events, long Stamp)> _mouHuCheErQualificationDependencies => ref _characterHuCheErMou.ReferenceToMouHuCheErQualificationDependencies;
    private ref long _mouHuCheErQualificationStamp => ref _characterHuCheErMou.ReferenceToMouHuCheErQualificationStamp;
    // Durable private courses, rather than pending choices. Commands rebuild
    // these immutable accepted orders on replay; public facts reveal no future animal.
    private ref readonly Dictionary<long, MobileWulingCourse> _mobileWulingCourses => ref _characterHuaTuoMobileGod.ReferenceToMobileWulingCourses;
    // 解阵 conversions survive across turns until the owner's next turn start or
    // the converted character's 八卦阵 judgment; replay rebuilds the map while the
    // accepted commands re-execute the converting op.
    private ref readonly Dictionary<int, JiezhenConversionState> _jiezhenConversions => ref _characterHuangChengYanOl.ReferenceToJiezhenConversions;
    private ref NativeOwners.ShiHcLedgerView? _shiHcLedgerCache => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCache;
    private ref int _shiHcLedgerCacheCount => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCacheCount;
    private ref IGameEvent? _shiHcLedgerCacheHead => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCacheHead;
    private ref IGameEvent? _shiHcLedgerCacheTail => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCacheTail;
    private ref bool _hasIssuedLiuQi => ref _characterLiuQiOl.ReferenceToHasIssuedLiuQi;
    // 闭境 marks survive across turns until the owner's next preparation phase
    // recasts them; replay reconstructs the map from the accepted commands.
    private ref readonly Dictionary<int, int> _bijingMarkedCardIds => ref _characterLvKaiOl.ReferenceToBijingMarkedCardIds;
    // Existing match-local derived index; replay rebuilds it in event order.
    // The independent Runtime owns scanning and qualification, not this state.
    private ref int _shiTzLieScanSequence => ref _characterTaiShiCiMobileShi.ReferenceToShiTzLieScanSequence;
    private ref (long Base, int Round, int Count) _miuyanQualificationDependencies => ref _characterWangGuanOl.ReferenceToMiuyanQualificationDependencies;
    private ref long _miuyanQualificationStamp => ref _characterWangGuanOl.ReferenceToMiuyanQualificationStamp;
    private ref (long Inner, int Issued, int Expired) _anniversaryXiahouJieQualificationInputs => ref _characterXiahouJieAnniversary.ReferenceToAnniversaryXiahouJieQualificationInputs;
    private ref long _anniversaryXiahouJieQualificationStamp => ref _characterXiahouJieAnniversary.ReferenceToAnniversaryXiahouJieQualificationStamp;
    // Private 追还 arm choices: arm frame id → secretly chosen seat. The map is
    // runtime trusted state; replay rebuilds it from the accepted choice
    // commands exactly like the 闭境 marked-card map.
    private ref readonly Dictionary<long, int> _zhuihuanArmChoices => ref _characterYangWanOl.ReferenceToZhuihuanArmChoices;
    private ref bool _synchronizingCategoryTalismans => ref _characterZhangLingOl.ReferenceToSynchronizingCategoryTalismans;
    // Derived opt-in only. The command journal reissues this event on recovery;
    // every actual entitlement below still proves the original immutable lease.
    private ref bool _hasIssuedAnniversaryZhangXun => ref _characterZhangXunAnniversary.ReferenceToHasIssuedAnniversaryZhangXun;
    private ref bool _hasIssuedZhugeGuoState => ref _characterZhugeGuoOl.ReferenceToHasIssuedZhugeGuoState;
    private ref readonly Dictionary<int, int> _mouZhitianCounts => ref _characterZhugeLiangMou.ReferenceToMouZhitianCounts;
    private ref readonly HashSet<int> _mouWujingAllowances => ref _characterZhugeLiangMou.ReferenceToMouWujingAllowances;
    private ref YunzhengQualificationSeat[] _yunzhengQualificationSeats => ref _characterZouShiLe.ReferenceToYunzhengQualificationSeats;
    private ref (long Inner, long Movement, int Initial, int Shown) _yunzhengQualificationInputs => ref _characterZouShiLe.ReferenceToYunzhengQualificationInputs;
    private ref long _yunzhengQualificationStamp => ref _characterZouShiLe.ReferenceToYunzhengQualificationStamp;
}
