namespace CardGame.Core;

// Query-scoped views over the original stores and existing projection rules.
// CharacterState is a temporary legacy storage bridge: its original identity and
// field read timing stay intact; no player state is copied into the projector.
internal interface IPlayerViewVisibilityQueries
{
    ModeVisibilityRules Rules { get; }
    bool SupportsGodFactionSelection { get; }
    bool IsWushuangMode { get; }
    bool HasShiLsHandView(int viewerSeat, int ownerSeat);
    bool HasShanheSourceZhaoranPublicHand(int ownerSeat);
    string? GetEffectiveFactionId(CharacterState player);
    string? GetScenarioGodFaction(CharacterState player);
    bool HasNativeEffectiveFactionDeclaration(CharacterState player);
    string? GetPrivateGeneralLibraryFaction(CharacterState player);
    bool IsSkillVisible(int seat, string? skillId, int viewerSeat, bool revealAll);
}

internal interface IPlayerViewCardsQueries
{
    CardSnapshot ProjectFuleHandCard(CharacterState player, Card card, CardSnapshot snapshot);
    PublicPersistentPileSource? SinglePublicPileSource(int seat);
    IReadOnlyList<Card> PublicPileCards(PublicPersistentPileSource source);
    IReadOnlyList<Card> PublicPileDraftCards(ProgramPublicPileDraft draft);
    IReadOnlyList<PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance);
    Card GetFireAttackRevealedCard(CardUseFrame frame);
}

internal interface IPlayerViewRulesQueries
{
    IEnumerable<GeneralSkillDefinition> OwnedRuntimeSkills(CharacterState player, GeneralDefinition general, bool includeAcquired);
    SkillBindingShard GetSkillBindingShard(CharacterState player);
    int? GetIncomingFixedDamageCap(CharacterState player, IReadOnlySet<string>? visibleSkillIds);
    int GetHandLimit(CharacterState player);
    IReadOnlyList<OriginalHandEntitySnapshot>? ProjectOriginalHandEntities(int ownerSeat, int viewerSeat, bool revealAll);
    IReadOnlyDictionary<string, int>? GetConfiguredConversionTierSnapshot(CharacterState player);
    IReadOnlyList<ProgramAlternatingChoiceStateSnapshot>? GetAlternatingChoiceStateSnapshot(CharacterState player);
    IReadOnlyList<BeneficiarySuitShield> BeneficiarySuitShields { get; }
    IReadOnlyList<DeferredHandAlignment> DeferredHandAlignments { get; }
    IReadOnlyList<TurnHandCategoryRestriction>? GetTurnHandCategoryRestrictionSnapshot(int seat);
    IReadOnlyList<MouCaoPiControl>? MouCaoPiControlsSnapshot(int seat);
    IReadOnlyList<TurnHandLimitCardKindExemption>? GetTurnHandLimitCardKindExemptionsSnapshot(int seat);
    IReadOnlyList<TurnSlashSuitAllowance>? GetTurnSlashSuitAllowancesSnapshot(int seat);
    IReadOnlyList<FirstRoundGameUsageRefund>? GetFirstRoundGameUsageRefundsSnapshot(int seat);
    JudgedRankSlashThresholdSnapshot? JudgedRankSlashSnapshot(int seat);
    bool TracksActualPlayPhaseCardUses { get; }
    int GetActualPlayPhaseUseCount(int seat);
    IReadOnlyList<IssuedPlayPhaseUseProhibition> IssuedPlayPhaseUseProhibitions { get; }
    bool HasIssuedPlayPhaseUseBan(int seat);
    IReadOnlyList<IssuedPlayPhaseSuitUseAllowance>? GetIssuedPlayPhaseSuitUseAllowances(int seat);
    bool HasProgramSkill(CharacterState player, string skillId);
    IReadOnlyList<SourceCurseSnapshot>? CreateSourceCurseSnapshots(int seat);
    IReadOnlyList<PublicPersistentPileSnapshot>? CreatePublicPersistentPileSnapshots(int seat);
    IReadOnlyList<PrivateGeneralLibrarySnapshot>? ProjectPrivateGeneralLibraries(CharacterState player, int viewerSeat, bool revealAll);
    IReadOnlyList<PrivateTurnHoldSnapshot>? ProjectPrivateTurnHolds(int seat, int viewerSeat, bool revealAll);
    IReadOnlyList<NamedAmbushPileSnapshot>? ProjectNamedAmbushPiles(int seat, int viewerSeat, bool revealAll);
    IReadOnlyList<PrivateWhirlPileSnapshot>? ProjectPrivateWhirlPiles(int seat, int viewerSeat, bool revealAll);
    IReadOnlyList<DeferredPublicPileDeposit> DeferredPublicPileDeposits { get; }
    bool HasProgramPersistentZone(CharacterState player, CardZoneKind zone);
    IReadOnlyList<PlayerMarkerSnapshot>? AppendOriginalUseLightningMarker(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers);
    IReadOnlyList<PlayerMarkerSnapshot>? AppendMouGuoHuaiJingce(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers);
    IReadOnlyList<PlayerMarkerSnapshot>? AppendMouGanNingWei(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers);
    IReadOnlyList<PlayerMarkerSnapshot>? AppendMouHuaXiongWei(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers);
    IReadOnlyList<PlayerMarkerSnapshot>? AppendMouLixianProgressMarkers(CharacterState player, bool canSeeGeneral, IReadOnlyList<PlayerMarkerSnapshot>? markers);
    IReadOnlyList<PlayerMarkerSnapshot>? AppendMoQuanyuPublicMarkers(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers);
    IReadOnlyList<PlayerMarkerSnapshot>? AppendOlMoRumoMarker(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers);
    SkillRuntimeStateSnapshot CreateProgramAwareSkillStateSnapshot(CharacterState player, string skillId);
    ProgramViewContributors ViewContributors { get; }
    PlayerSnapshot ProjectPlayer(PlayerSnapshot snapshot, int viewerSeat, bool revealAll);
    GameSnapshot ProjectGame(GameSnapshot snapshot, int viewerSeat, bool revealAll);
    PendingDecision? GetPendingDecisionForActor(int viewerSeat);
    PendingDecision ProjectVisibleTargetHandDecision(PendingDecision decision, int viewerSeat);
    PendingDecision ProjectCardPaymentCount(PendingDecision decision);
    PendingDecision ProjectNativeCardSelections(PendingDecision decision);
    IReadOnlyList<CardDeclarationSnapshot>? GetCardDeclarationSnapshots(int viewerSeat, bool revealAll);
    IReadOnlyList<FriendPredictionView>? GetFriendPredictionViews(int viewerSeat);
    IReadOnlyList<BoundarySidiPredictionView>? GetBoundarySidiPredictionViews(int viewerSeat);
    IReadOnlyList<ProgramResponseExchangeStateSnapshot>? GetResponseExchangeStateSnapshots();
    IReadOnlyList<TurnProhibitedPhysicalCardsSnapshot>? GetResponseEntityRestrictionSnapshots();
}

internal interface IPlayerViewMatchQueries
{
    ContentModeDefinition ModeDefinition { get; }
    int PlayerCount { get; }
    CharacterState PlayerAt(int index);
    int FrameCount { get; }
    ResolutionFrame FrameAt(int index);
    ResolutionFrame? LastFrame { get; }
    IEnumerable<ResolutionFrame> Frames { get; }
    GameOptions Options { get; }
    EngineStatus Status { get; }
    Winner Winner { get; }
    int TurnNumber { get; }
    int CurrentSeat { get; }
    TurnPhase Phase { get; }
    int CardUseDebitPhaseInstanceId { get; }
    bool SetupComplete { get; }
    long Revision { get; }
    int? WinnerSeat { get; }
    string? WinnerTeamId { get; }
    string? WinnerFactionId { get; }
    IReadOnlyList<int> CurrentTableOrder();
    CardUseFrame? ActiveFireAttack { get; }
    IPlayerViewGroupCardQueries? ActiveGroupCard { get; }
}

// This cursor keeps the original continuation owner and reads each field only
// when projection reaches it, just as the legacy GroupCardHandle did.
internal interface IPlayerViewGroupCardQueries
{
    GroupCardEffect Effect { get; }
    int TargetIndex { get; }
    IReadOnlyList<int> TargetSeats { get; }
    IReadOnlyList<int> RevealedCardIds { get; }
}
