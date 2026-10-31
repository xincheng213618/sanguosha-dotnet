#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface IKernelAcceptAndRejectOperations : IProgramRuntimeOperations
{
    CommandResult Reject(CommandErrorCode code, string message);
    CommandResult Accept(Action operation);
}

internal interface IKernelAcceptOperations : IProgramRuntimeOperations
{
    CommandResult Accept(Action operation);
}

internal interface IKernelActiveCardMovementBatchIdsAccess : IProgramRuntimeOperations
{
    ref readonly Stack<long> _activeCardMovementBatchIds { get; }
}

internal interface IKernelActivityRuleFlowFactories : IProgramRuntimeOperations
{
    NativeOwners.ActivityBattleRulesHost CreateActivityBattleRulesHost();
    NativeOwners.WenheRuleFlowHost CreateWenheRuleFlowHost();
    NativeOwners.GuanduEventFlowHost CreateGuanduEventFlowHost();
}

internal interface IKernelAdvanceEventRulesAndQueueFactOperations : IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(IGameEvent payload);
}

internal interface IKernelAdvanceRulesAndPublishStateOperations : IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface IKernelAdvanceToHumanBoundaryOperations : IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface IKernelAllegiancePolicyAccess : IProgramRuntimeOperations
{
    ref IMatchModeAllegiancePolicy? _allegiancePolicy { get; }
}

internal interface IKernelBindRuntimeHandlerOperations : IProgramRuntimeOperations
{
    Delegate BindRuntimeHandler(System.Reflection.MethodInfo method, Type delegateType);
}

internal interface IKernelCaoXianContinuation : IProgramRuntimeOperations
{
    bool CaoXianEdge(int i);
    bool CaoXianFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool CaoXianSuffix(int n);
    NativeOwners.CaoXianFacts CreateCaoXianFacts();
}

internal interface IKernelCardAttackAndFactionRequestHandlesFactory : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, FactionCardRequestPurpose purpose, int ownerSeat, IReadOnlyList<int> candidateSeats, string providerFactionId, NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardKind requiredKind = (CardKind)0, NativeOwners.BorrowedSwordHandle? borrowedSword = default, NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface IKernelCardAttackAndFactionRequestHandlesFactoryAndLookup : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId);
    NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, FactionCardRequestPurpose purpose, int ownerSeat, IReadOnlyList<int> candidateSeats, string providerFactionId, NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardKind requiredKind = (CardKind)0, NativeOwners.BorrowedSwordHandle? borrowedSword = default, NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface IKernelCardAttackAndFactionRequestHandlesLookup : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
    NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId);
}

internal interface IKernelCardAttackAndFangtianHandlesFactory : IProgramRuntimeOperations
{
    NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, Card card, CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardConversionSource? conversionSource = default);
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface IKernelCardAttackAndFangtianHandlesLookup : IProgramRuntimeOperations
{
    NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCardAttackHandleFactory : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface IKernelCardAttackHandleLookup : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCardContinuationHandleFactory : IProgramRuntimeOperations
{
    NativeOwners.DuelHandle CreateDuelHandle(long ownerId);
    NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId);
    NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    NativeOwners.StoneAxeHandle CreateStoneAxeHandle(long ownerId);
    NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(long ownerId);
    NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(long ownerId);
    NativeOwners.QinglongFollowupHandle CreateQinglongFollowupHandle(long ownerId);
    NativeOwners.IceSwordHandle CreateIceSwordHandle(long ownerId);
    NativeOwners.QilinBowHandle CreateQilinBowHandle(long ownerId);
    NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(long ownerId);
    NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId);
}

internal interface IKernelCardLegalityAccess : IProgramRuntimeOperations
{
    ref ProgramCardLegality? _cardLegality { get; }
}

internal interface IKernelCardUsePaymentsAccess : IProgramRuntimeOperations
{
    ref ProgramCardUsePayments? _cardUsePayments { get; }
}

internal interface IKernelCardZoneStoreAccess : IProgramRuntimeOperations
{
    ref readonly CardZoneStore _cardZones { get; }
}

internal interface IKernelCommandEventJournalAccess : IProgramRuntimeOperations
{
    ref CommandEventJournal? _commandEventJournal { get; }
}

internal interface IKernelCommandRuntimeAccess : IProgramRuntimeOperations
{
    ICommandSessionRuntime CommandRuntime { get; }
}

internal interface IKernelCommandRuntimeAndRevisionAccess : IProgramRuntimeOperations
{
    long _revision { get; }

    ICommandSessionRuntime CommandRuntime { get; }
}

internal interface IKernelCommandSessionAccess : IProgramRuntimeOperations
{
    ref readonly CommandSession _commandSession { get; }
}

internal interface IKernelCompleteGameOperations : IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface IKernelContentRegistryAccess : IProgramRuntimeOperations
{
    ref readonly ContentRegistry _contentRegistry { get; }
}

internal interface IKernelCreateActionQueryScopeOperations : IProgramRuntimeOperations
{
    NativeOwners.ActionQueryScope CreateActionQueryScope(NativeOwners.ActionQuery? previous = default);
}

internal interface IKernelCreateActivePlayerEffectQueriesOperations : IProgramRuntimeOperations
{
    NativeOwners.ActivePlayerEffectQueries CreateActivePlayerEffectQueries();
}

internal interface IKernelCreateBorrowedSwordHandleAndCreateCardAttackHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCreateCardActionTriggerWindowHostOperations : IProgramRuntimeOperations
{
    NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost();
}

internal interface IKernelCreateCardAttackHandleAndCreateCardAttackHandle2AndCreateDuelHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    NativeOwners.DuelHandle CreateDuelHandle(NativeOwners.CardAttackHandle attack);
    NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId);
}

internal interface IKernelCreateCardAttackHandleAndCreateCardAttackHandle2Operations : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId);
}

internal interface IKernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.DuelHandle CreateDuelHandle(long ownerId);
    NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId);
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCreateCardAttackHandleAndCreateDuelHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    NativeOwners.DuelHandle CreateDuelHandle(NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateCardsMovedTriggerWindowHostOperations : IProgramRuntimeOperations
{
    NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost();
}

internal interface IKernelCreateCixiongDoubleSwordsHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateDamageTriggerCapturedAttackOperations : IProgramRuntimeOperations
{
    NativeOwners.DamageTriggerCapturedAttack CreateDamageTriggerCapturedAttack(GameEngine.IDamageAttempt attack);
}

internal interface IKernelCreateDouyuTriggerWindowHostOperations : IProgramRuntimeOperations
{
    NativeOwners.DouyuTriggerWindowHost CreateDouyuTriggerWindowHost();
}

internal interface IKernelCreateFactionCardRequestHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, FactionCardRequestPurpose purpose, int ownerSeat, IReadOnlyList<int> candidateSeats, string providerFactionId, NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardKind requiredKind = (CardKind)0, NativeOwners.BorrowedSwordHandle? borrowedSword = default, NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface IKernelCreateFactionDefenseHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(NativeOwners.CardAttackHandle attack, int ownerSeat, IReadOnlyList<int> candidateSeats, FactionResponsePolicySource source);
}

internal interface IKernelCreateFangtianHalberdHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, Card card, CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardConversionSource? conversionSource = default);
}

internal interface IKernelCreateHellEncounterHostOperations : IProgramRuntimeOperations
{
    NativeOwners.HellEncounterHost CreateHellEncounterHost();
}

internal interface IKernelCreateHpChangedTriggerWindowHostOperations : IProgramRuntimeOperations
{
    NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost();
}

internal interface IKernelCreateIceSwordHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.IceSwordHandle CreateIceSwordHandle(NativeOwners.CardAttackHandle attack, int preventedDamageAmount);
}

internal interface IKernelCreateMoSimaYiFactsOperations : IProgramRuntimeOperations
{
    NativeOwners.MoSimaYiFacts CreateMoSimaYiFacts();
}

internal interface IKernelCreateModeEncounterHostOperations : IProgramRuntimeOperations
{
    NativeOwners.ModeEncounterHost CreateModeEncounterHost();
}

internal interface IKernelCreateModeNationalRevealHostOperations : IProgramRuntimeOperations
{
    NativeOwners.ModeNationalRevealHost CreateModeNationalRevealHost();
}

internal interface IKernelCreateModeSetupEquipmentHostOperations : IProgramRuntimeOperations
{
    NativeOwners.ModeSetupEquipmentHost CreateModeSetupEquipmentHost();
}

internal interface IKernelCreateMouGuanPingFactsOperations : IProgramRuntimeOperations
{
    NativeOwners.MouGuanPingFacts CreateMouGuanPingFacts();
}

internal interface IKernelCreateMouHuangGaiFactsAndMouHgFirstChildOperations : IProgramRuntimeOperations
{
    bool MouHgFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    NativeOwners.MouHuangGaiFacts CreateMouHuangGaiFacts();
}

internal interface IKernelCreateMouSunShangxiangFactsOperations : IProgramRuntimeOperations
{
    NativeOwners.MouSunShangxiangFacts CreateMouSunShangxiangFacts();
}

internal interface IKernelCreateMovementArmorOwnerOperations : IProgramRuntimeOperations
{
    NativeOwners.MovementArmorOwner CreateMovementArmorOwner(CharacterState owner);
}

internal interface IKernelCreateNianChallengeHostOperations : IProgramRuntimeOperations
{
    NativeOwners.NianChallengeHost CreateNianChallengeHost();
}

internal interface IKernelCreateParticipantSetupContextOperations : IProgramRuntimeOperations
{
    NativeOwners.ParticipantSetupContext CreateParticipantSetupContext();
}

internal interface IKernelCreateProgramAttackHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}

internal interface IKernelCreateProgramExpressionWorldOperations : IProgramRuntimeOperations
{
    NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(ProgramSkillFrame frame);
}

internal interface IKernelCreateProgramHealthChildQueriesOperations : IProgramRuntimeOperations
{
    NativeOwners.ProgramHealthChildQueries CreateProgramHealthChildQueries();
}

internal interface IKernelCreateProgramLegalityRouterOperations : IProgramRuntimeOperations
{
    ProgramLegalityRouter CreateProgramLegalityRouter(ProgramLegalityCatalog catalog, Func<System.Reflection.MethodInfo, object?>? resolveTarget = default);
}

internal interface IKernelCreateProgramSkillHostOperations : IProgramRuntimeOperations
{
    NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface IKernelCreateProgramTriggerActionFactQueriesAndCreateProgramTriggerFactQueriesOperations : IProgramRuntimeOperations
{
    NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries();
    NativeOwners.ProgramTriggerActionFactQueries CreateProgramTriggerActionFactQueries();
}

internal interface IKernelCreateProgramTriggerFactQueriesOperations : IProgramRuntimeOperations
{
    NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries();
}

internal interface IKernelCreateProgramTriggerQueriesOperations : IProgramRuntimeOperations
{
    NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries();
}

internal interface IKernelCreateQilinBowHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.QilinBowHandle CreateQilinBowHandle(NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateQinglongCrescentBladeHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateRuleProgressionOperationsOperations : IProgramRuntimeOperations
{
    NativeOwners.RuleProgressionOperations CreateRuleProgressionOperations();
}

internal interface IKernelCreateSelectionQueriesOperations : IProgramRuntimeOperations
{
    NativeOwners.SelectionQueries CreateSelectionQueries();
}

internal interface IKernelCreateStandardBattleHostOperations : IProgramRuntimeOperations
{
    NativeOwners.StandardBattleHost CreateStandardBattleHost();
}

internal interface IKernelCreateStoneAxeHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.StoneAxeHandle CreateStoneAxeHandle(NativeOwners.CardAttackHandle attack, IReadOnlyList<int> candidateCardIds);
}

internal interface IKernelCreateWuLuKangWindowHostOperations : IProgramRuntimeOperations
{
    NativeOwners.WuLuKangWindowHost CreateWuLuKangWindowHost();
}

internal interface IKernelCreateYingboGiftHandleOperations : IProgramRuntimeOperations
{
    NativeOwners.YingboGiftHandle CreateYingboGiftHandle(long ownerId);
}

internal interface IKernelDamageHooksAccess : IProgramRuntimeOperations
{
    ref ProgramDamageHooks? _damageHooks { get; }
}

internal interface IKernelDistanceRulesAccess : IProgramRuntimeOperations
{
    ref ProgramDistanceRules? _distanceRules { get; }
}

internal interface IKernelDyingObserverPredicate : IProgramRuntimeOperations
{
    bool Invoke(Func<int, DyingFrame, bool>? __nativeReceiver, int arg2, DyingFrame arg3);
}

internal interface IKernelEnsureSessionHealthyOperations : IProgramRuntimeOperations
{
    void EnsureSessionHealthy();
}

internal interface IKernelEquipmentRulesAccess : IProgramRuntimeOperations
{
    ref EquipmentContentRegistry? _equipmentRules { get; }
}

internal interface IKernelFrameStoreAccess : IProgramRuntimeOperations
{
    ref readonly FrameStore _resolutionStack { get; }
}

internal interface IKernelFrameStoreQuery : IProgramRuntimeOperations
{
    ref readonly FrameStore _resolutionStack();
}

internal interface IKernelMatchExecutionLoopAccess : IProgramRuntimeOperations
{
    ref readonly MatchExecutionLoop _matchExecutionLoop { get; }
}

internal interface IKernelMatchModeRulesAccess : IProgramRuntimeOperations
{
    ref readonly ModeRuleSet _modeRules { get; }
}

internal interface IKernelMatchModeRuntimeAccess : IProgramRuntimeOperations
{
    ref readonly MatchModeRuntime _modeRuntime { get; }
}

internal interface IKernelMatchRandomAccess : IProgramRuntimeOperations
{
    ref readonly DeterministicRandom _random { get; }
}

internal interface IKernelMatchesOperations : IProgramRuntimeOperations
{
    bool Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame);
}

internal interface IKernelMouCaoHongContinuation : IProgramRuntimeOperations
{
    bool MouCaoHongEdge(int i);
    bool MouCaoHongFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouCaoHongSuffix(int n);
    NativeOwners.MouCaoHongFacts CreateMouCaoHongFacts();
    bool CurrentMouCaoHongDebt(ProgramSkillFrame f, ProgramSkillWindowContext w);
    MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt2(ProgramTriggerCandidate c, ProgramSkillWindowContext w);
}

internal interface IKernelMouCaoShuangAnniversaryContinuation : IProgramRuntimeOperations
{
    bool MouCaoShuangAnniversaryEdge(int i);
    bool MouCaoShuangAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouCaoShuangAnniversarySuffix(int n);
    NativeOwners.MouCaoShuangAnniversaryFacts CreateMouCaoShuangAnniversaryFacts();
}

internal interface IKernelMouChenLinContinuation : IProgramRuntimeOperations
{
    bool MouChenLinEdge(int i);
    bool MouChenLinFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouChenLinSuffix(int n);
    NativeOwners.MouChenLinFacts CreateMouChenLinFacts();
}

internal interface IKernelMouDianWeiContinuation : IProgramRuntimeOperations
{
    bool MouDianWeiEdge(int i);
    bool MouDianWeiFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouDianWeiSuffix(int n);
    NativeOwners.MouDianWeiFacts CreateMouDianWeiFacts();
}

internal interface IKernelMouDongChengContinuation : IProgramRuntimeOperations
{
    bool MouDcEdge(int i);
    bool MouDcFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouDcSuffix(int i);
    NativeOwners.MouDongChengFacts CreateMouDongChengFacts();
    NativeOwners.MouDcLightningFacts CreateMouDcLightningFacts();
}

internal interface IKernelMouDongZhuoOlContinuation : IProgramRuntimeOperations
{
    bool MouDongZhuoOlEdge(int i);
    bool MouDongZhuoOlFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouDongZhuoOlSuffix(int n);
    NativeOwners.MouDongZhuoOlFacts CreateMouDongZhuoOlFacts();
}

internal interface IKernelMouGuoJiaContinuation : IProgramRuntimeOperations
{
    bool MouGjEdge(int i);
    bool MouGjFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouGjSuffix(int n);
    NativeOwners.MouGuoJiaFacts CreateMouGuoJiaFacts();
}

internal interface IKernelMouHuCheErContinuation : IProgramRuntimeOperations
{
    NativeOwners.MouHuCheErFacts CreateMouHuCheErFacts();
    bool MouHuCheErEdge(int i);
    bool MouHuCheErFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouHuCheErSuffix(int n);
}

internal interface IKernelMouHuLieContinuation : IProgramRuntimeOperations
{
    bool MouHuLieEdge(int i);
    bool MouHuLieFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouHuLieSuffix(int n);
    NativeOwners.MouHuLieFacts CreateMouHuLieFacts();
}

internal interface IKernelMouJiaXuContinuation : IProgramRuntimeOperations
{
    bool MouJxEdge(int i);
    bool MouJxFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouJxSuffix(int n);
    NativeOwners.MouJiaXuFacts CreateMouJiaXuFacts();
}

internal interface IKernelMouJiangJiContinuation : IProgramRuntimeOperations
{
    bool MouJjEdge(int i);
    bool MouJjFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouJjSuffix(int n);
    NativeOwners.MouJiangJiFacts CreateMouJiangJiFacts();
}

internal interface IKernelMouJiangWeiAnniversaryContinuation : IProgramRuntimeOperations
{
    bool MouJiangWeiAnniversaryEdge(int i);
    bool MouJiangWeiAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouJiangWeiAnniversarySuffix(int n);
    NativeOwners.MouJiangWeiAnniversaryFacts CreateMouJiangWeiAnniversaryFacts();
}

internal interface IKernelMouJuShouContinuation : IProgramRuntimeOperations
{
    bool MouJsEdge(int i);
    bool MouJsFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouJsSuffix(int n);
    NativeOwners.MouJuShouFacts CreateMouJuShouFacts();
}

internal interface IKernelMouLiuXieContinuation : IProgramRuntimeOperations
{
    bool MouLiuXieNativeEdge(int i);
    bool MouLiuXieFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouLiuXieNativeSuffix(int n);
    NativeOwners.MouLiuXieFacts CreateMouLiuXieFacts();
}

internal interface IKernelMouLuSuAnniversaryContinuation : IProgramRuntimeOperations
{
    bool MouLuSuEdge(int i);
    bool MouLuSuFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouLuSuSuffix(int n);
    NativeOwners.MouLuSuAnniversaryFacts CreateMouLuSuAnniversaryFacts();
}

internal interface IKernelMouSmContinuation : IProgramRuntimeOperations
{
    bool MouSmEdge(int i);
    bool MouSmFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouSmSuffix(int n);
    NativeOwners.MouSmFacts CreateMouSmFacts();
}

internal interface IKernelMouXuShengContinuation : IProgramRuntimeOperations
{
    bool MouXuShengNativeEdge(int i);
    bool MouXuShengFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouXuShengNativeSuffix(int n);
    NativeOwners.MouXuShengFacts CreateMouXuShengFacts();
}

internal interface IKernelMouXunYuAnniversaryContinuation : IProgramRuntimeOperations
{
    bool MouXyEdge(int i);
    bool MouXyFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouXySuffix(int n);
    NativeOwners.MouXunYuAnniversaryFacts CreateMouXunYuAnniversaryFacts();
}

internal interface IKernelMouZhangHeContinuation : IProgramRuntimeOperations
{
    bool MouQiaobianNativeEdge(int i);
    bool MouQiaobianFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouQiaobianNativeSuffix(int n);
    NativeOwners.MouZhangHeFacts CreateMouZhangHeFacts();
}

internal interface IKernelMouZhangXiuAnniversaryContinuation : IProgramRuntimeOperations
{
    bool MouZhangXiuAnniversaryEdge(int i);
    bool MouZhangXiuAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouZhangXiuAnniversarySuffix(int n);
    NativeOwners.MouZhangXiuAnniversaryFacts CreateMouZhangXiuAnniversaryFacts();
}

internal interface IKernelMouZhouYuContinuation : IProgramRuntimeOperations
{
    NativeOwners.MouZhouYuFacts CreateMouZhouYuFacts();
    bool MouZyEdge(int i);
    bool MouZyFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouZySuffix(int i);
}

internal interface IKernelMouZhuRanContinuation : IProgramRuntimeOperations
{
    bool MouZhuRanNativeEdge(int i);
    bool MouZhuRanFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouZhuRanNativeSuffix(int n);
    NativeOwners.MouZhuRanFacts CreateMouZhuRanFacts();
}

internal interface IKernelMouZhugeJinContinuation : IProgramRuntimeOperations
{
    bool MouZhugeJinEdge(int i);
    bool MouZhugeJinFirstChild(ProgramSkillFrame f, ResolutionFrame child);
    bool MouZhugeJinSuffix(int n);
    NativeOwners.MouZhugeJinFacts CreateMouZhugeJinFacts();
}

internal interface IKernelNativeReturnsAccess : IProgramRuntimeOperations
{
    ref ProgramNativeReturns? _nativeReturns { get; }
}

internal interface IKernelObservedSkillGrantRevisionsAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, long> _observedSkillGrantRevisions { get; }
}

internal interface IKernelObserverIndexPredicate : IProgramRuntimeOperations
{
    bool Invoke(Func<int, bool> __nativeReceiver, int arg2);
}

internal interface IKernelObserversAccess : IProgramRuntimeOperations
{
    ObserverDispatcher Observers { get; }
}

internal interface IKernelParticipantSetupAccess : IProgramRuntimeOperations
{
    ref MatchParticipantSetup? _participantSetup { get; }
}

internal interface IKernelPendingCardsMovedBatchesAccess : IProgramRuntimeOperations
{
    ref readonly List<CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface IKernelPhaseUsedProjectionsAccess : IProgramRuntimeOperations
{
    ref ProgramPhaseUsedProjections? _phaseUsedProjections { get; }
}

internal interface IKernelPlayerViewQueryFactories : IProgramRuntimeOperations
{
    NativeOwners.PlayerViewVisibilityQueries CreatePlayerViewVisibilityQueries();
    NativeOwners.PlayerViewCardsQueries CreatePlayerViewCardsQueries();
    NativeOwners.PlayerViewRulesQueries CreatePlayerViewRulesQueries();
    NativeOwners.PlayerViewMatchQueries CreatePlayerViewMatchQueries();
}

internal interface IKernelProgramActionsAccess : IProgramRuntimeOperations
{
    ref ProgramActionRouter? _programActions { get; }
}

internal interface IKernelProgramChildPredicate : IProgramRuntimeOperations
{
    bool Invoke(Func<ProgramSkillFrame, ResolutionFrame, bool> __nativeReceiver, ProgramSkillFrame arg2, ResolutionFrame arg3);
}

internal interface IKernelProgramHistoryAccess : IProgramRuntimeOperations
{
    ref ProgramEventHistoryIndex? _programHistory { get; }
}

internal interface IKernelProgramInteractionsAccess : IProgramRuntimeOperations
{
    ref ProgramInteractionRouter? _programInteractions { get; }
}

internal interface IKernelProgramIssuedTriggersAccess : IProgramRuntimeOperations
{
    ref ProgramIssuedTriggerRuntime? _programIssuedTriggers { get; }
}

internal interface IKernelProgramLegalityAccess : IProgramRuntimeOperations
{
    ref ProgramLegalityRouter? _programLegality { get; }
}

internal interface IKernelProgramPhaseScheduleAccess : IProgramRuntimeOperations
{
    ref ProgramPhaseSchedule? _programPhaseSchedule { get; }
}

internal interface IKernelProgramPipelineHooksAccess : IProgramRuntimeOperations
{
    ref readonly ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface IKernelProgramRuleObserversAccess : IProgramRuntimeOperations
{
    ref readonly ProgramRuleObservers _programRuleObservers { get; }
}

internal interface IKernelProgramSelectionQueriesAccess : IProgramRuntimeOperations
{
    ref IProgramSelectionQueries? _programSelectionQueries { get; }
}

internal interface IKernelProgramSelectionValidatorsAccess : IProgramRuntimeOperations
{
    ref ProgramSelectionValidationRegistry? _programSelectionValidators { get; }
}

internal interface IKernelProgramSelectionsAccess : IProgramRuntimeOperations
{
    ref ProgramSelectionRegistry? _programSelections { get; }
}

internal interface IKernelProgramTargetResolversAccess : IProgramRuntimeOperations
{
    ref ProgramTargetResolverRegistry? _programTargetResolvers { get; }
}

internal interface IKernelProgramTriggerProofsAccess : IProgramRuntimeOperations
{
    ref ProgramTriggerProofRuntime? _programTriggerProofs { get; }
}

internal interface IKernelProgramWindowReturnOperations : IProgramRuntimeOperations
{
    void Return(ProgramWindowReturnCatalog __nativeReceiver, ProgramSkillFrame frame, bool completed);
}

internal interface IKernelPublicHandRevealReturnOperations : IProgramRuntimeOperations
{
    void Return(PublicHandRevealReturns __nativeReceiver, string key, ResolutionFrame parent);
}

internal interface IKernelQinglongContinuationHandleFactory : IProgramRuntimeOperations
{
    NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId);
    NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelRejectOperations : IProgramRuntimeOperations
{
    CommandResult Reject(CommandErrorCode code, string message);
}

internal interface IKernelResolutionSchedulerAccess : IProgramRuntimeOperations
{
    ref readonly ResolutionScheduler _resolutionScheduler { get; }
}

internal interface IKernelResolveRuntimeHandlerTargetOperations : IProgramRuntimeOperations
{
    object? ResolveRuntimeHandlerTarget(System.Reflection.MethodInfo method);
}

internal interface IKernelReturnOperationOperations : IProgramRuntimeOperations
{
    void ReturnOperation(SkillProgramEffectOp operation, ProgramSkillFrame frame, NativeDrawOutcome outcome);
}

internal interface IKernelRevisionAccess : IProgramRuntimeOperations
{
    long _revision { get; }
}

internal interface IKernelRuleProgressionOperations : IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(IGameEvent payload);
}

internal interface IKernelRuleQueryFactFactories : IProgramRuntimeOperations
{
    NativeOwners.DistanceQueryFacts CreateDistanceQueryFacts(CharacterState source, CharacterState target);
    NativeOwners.AttackRangeQueryFacts CreateAttackRangeQueryFacts(CharacterState player);
    NativeOwners.DrawCountQueryFacts CreateDrawCountQueryFacts(CharacterState player);
    NativeOwners.SlashLimitQueryFacts CreateSlashLimitQueryFacts(CharacterState player);
    NativeOwners.HandLimitQueryFacts CreateHandLimitQueryFacts(CharacterState player);
    NativeOwners.CardTargetCountQueryFacts CreateCardTargetCountQueryFacts(CharacterState player, CardKind effectiveCardKind, long? declaredUseFrameId);
    NativeOwners.CardUseDistanceQueryFacts CreateCardUseDistanceQueryFacts(CharacterState player, CardKind effectiveCardKind);
    NativeOwners.NumericContributionQueryFacts CreateNumericContributionQueryFacts(CharacterState player, SkillRuleQuery ruleQuery);
}

internal interface IKernelRuntimeHooksAccess : IProgramRuntimeOperations
{
    ref ProgramRuntimeHooks? _runtimeHooks { get; }
}

internal interface IKernelShanheMarketAccess : IProgramRuntimeOperations
{
    ref Campaigns.ShanheCampaignSession? _shanheMarket { get; }
}

internal interface IKernelSkillBindingIndexAccess : IProgramRuntimeOperations
{
    ref readonly MatchSkillBindingIndex _skillBindingIndex { get; }
}

internal interface IKernelSkillRuntimeStateAccess : IProgramRuntimeOperations
{
    ref readonly SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface IKernelSuppressedCardMovedEventBatchIdsAccess : IProgramRuntimeOperations
{
    ref readonly Stack<long> _suppressedCardMovedEventBatchIds { get; }
}

internal interface IKernelTargetRulesAccess : IProgramRuntimeOperations
{
    ref ProgramTargetRules? _targetRules { get; }
}

internal interface IKernelTargetWindowProtocolsAccess : IProgramRuntimeOperations
{
    ref ProgramTargetWindowProtocols? _targetWindowProtocols { get; }
}

internal interface IKernelTrickEffectHandleFactory : IProgramRuntimeOperations
{
    NativeOwners.GroupCardHandle CreateGroupCardHandle(long resolutionId, int sourceSeat, Card card, IReadOnlyList<int> targetSeats, GroupCardEffect effect, CardKind? requiredCardKind, IReadOnlyList<Card>? physicalCards = default);
    NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long resolutionId, int sourceSeat, int weaponOwnerSeat, int slashTargetSeat, Card card);
    NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    NativeOwners.DuelHandle CreateDuelHandle(NativeOwners.CardAttackHandle attack);
}

internal interface IKernelTriggerWindowHostFactories : IProgramRuntimeOperations
{
    NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost();
    NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost();
    NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost();
}

internal interface IKernelTryAdvanceOperations : IProgramRuntimeOperations
{
    bool TryAdvance(ResolutionFrame frame);
}

internal interface IKernelTryResumeOperations : IProgramRuntimeOperations
{
    bool TryResume(GameEngine.ProgramContinuationCatalog __nativeReceiver, long frameId);
}

internal interface IKernelTryReturnMovementOperations : IProgramRuntimeOperations
{
    bool TryReturnMovement(GameEngine.ProgramContinuationCatalog __nativeReceiver, ProgramSkillFrame frame, bool afterPayment = false);
}

internal interface IKernelTurnCardUseEffectsAccess : IProgramRuntimeOperations
{
    ref readonly TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface IKernelViewContributorsAccess : IProgramRuntimeOperations
{
    ref ProgramViewContributors? _viewContributors { get; }
}
