#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface IKernelAcceptAndRejectOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.CommandResult Reject(CardGame.Core.CommandErrorCode code, string message);
    CardGame.Core.CommandResult Accept(System.Action operation);
}

internal interface IKernelAcceptOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.CommandResult Accept(System.Action operation);
}

internal interface IKernelActiveCardMovementBatchIdsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly System.Collections.Generic.Stack<long> _activeCardMovementBatchIds { get; }
}

internal interface IKernelActivityRuleFlowFactories : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ActivityBattleRulesHost CreateActivityBattleRulesHost();
    CardGame.Core.NativeOwners.WenheRuleFlowHost CreateWenheRuleFlowHost();
    CardGame.Core.NativeOwners.GuanduEventFlowHost CreateGuanduEventFlowHost();
}

internal interface IKernelAdvanceEventRulesAndQueueFactOperations : CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(CardGame.Core.IGameEvent payload);
}

internal interface IKernelAdvanceRulesAndPublishStateOperations : CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface IKernelAdvanceToHumanBoundaryOperations : CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface IKernelAllegiancePolicyAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.IMatchModeAllegiancePolicy? _allegiancePolicy { get; }
}

internal interface IKernelBindRuntimeHandlerOperations : CardGame.Core.IProgramRuntimeOperations
{
    System.Delegate BindRuntimeHandler(System.Reflection.MethodInfo method, System.Type delegateType);
}

internal interface IKernelCaoXianContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool CaoXianEdge(int i);
    bool CaoXianFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool CaoXianSuffix(int n);
    CardGame.Core.NativeOwners.CaoXianFacts CreateCaoXianFacts();
}

internal interface IKernelCardAttackAndFactionRequestHandlesFactory : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardGame.Core.CardKind requiredKind = (CardGame.Core.CardKind)0, CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface IKernelCardAttackAndFactionRequestHandlesFactoryAndLookup : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId);
    CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardGame.Core.CardKind requiredKind = (CardGame.Core.CardKind)0, CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface IKernelCardAttackAndFactionRequestHandlesLookup : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
    CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId);
}

internal interface IKernelCardAttackAndFangtianHandlesFactory : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, CardGame.Core.Card card, CardGame.Core.CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, System.Collections.Generic.IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardGame.Core.CardConversionSource? conversionSource = default);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface IKernelCardAttackAndFangtianHandlesLookup : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCardAttackHandleFactory : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface IKernelCardAttackHandleLookup : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCardContinuationHandleFactory : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId);
    CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId);
    CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    CardGame.Core.NativeOwners.StoneAxeHandle CreateStoneAxeHandle(long ownerId);
    CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(long ownerId);
    CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(long ownerId);
    CardGame.Core.NativeOwners.QinglongFollowupHandle CreateQinglongFollowupHandle(long ownerId);
    CardGame.Core.NativeOwners.IceSwordHandle CreateIceSwordHandle(long ownerId);
    CardGame.Core.NativeOwners.QilinBowHandle CreateQilinBowHandle(long ownerId);
    CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(long ownerId);
    CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId);
}

internal interface IKernelCardLegalityAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramCardLegality? _cardLegality { get; }
}

internal interface IKernelCardUsePaymentsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramCardUsePayments? _cardUsePayments { get; }
}

internal interface IKernelCardZoneStoreAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface IKernelCommandEventJournalAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.CommandEventJournal? _commandEventJournal { get; }
}

internal interface IKernelCommandRuntimeAccess : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.ICommandSessionRuntime CommandRuntime { get; }
}

internal interface IKernelCommandRuntimeAndRevisionAccess : CardGame.Core.IProgramRuntimeOperations
{
    long _revision { get; }

    CardGame.Core.ICommandSessionRuntime CommandRuntime { get; }
}

internal interface IKernelCommandSessionAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.CommandSession _commandSession { get; }
}

internal interface IKernelCompleteGameOperations : CardGame.Core.IProgramRuntimeOperations
{
    void CompleteGame();
}

internal interface IKernelContentRegistryAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface IKernelCreateActionQueryScopeOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ActionQueryScope CreateActionQueryScope(CardGame.Core.NativeOwners.ActionQuery? previous = default);
}

internal interface IKernelCreateActivePlayerEffectQueriesOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ActivePlayerEffectQueries CreateActivePlayerEffectQueries();
}

internal interface IKernelCreateBorrowedSwordHandleAndCreateCardAttackHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCreateCardActionTriggerWindowHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost();
}

internal interface IKernelCreateCardAttackHandleAndCreateCardAttackHandle2AndCreateDuelHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(CardGame.Core.NativeOwners.CardAttackHandle attack);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId);
}

internal interface IKernelCreateCardAttackHandleAndCreateCardAttackHandle2Operations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId);
}

internal interface IKernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId);
    CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelCreateCardAttackHandleAndCreateDuelHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateCardsMovedTriggerWindowHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost();
}

internal interface IKernelCreateCixiongDoubleSwordsHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateDamageTriggerCapturedAttackOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.DamageTriggerCapturedAttack CreateDamageTriggerCapturedAttack(CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface IKernelCreateDouyuTriggerWindowHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.DouyuTriggerWindowHost CreateDouyuTriggerWindowHost();
}

internal interface IKernelCreateFactionCardRequestHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardGame.Core.CardKind requiredKind = (CardGame.Core.CardKind)0, CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default);
}

internal interface IKernelCreateFactionDefenseHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(CardGame.Core.NativeOwners.CardAttackHandle attack, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, CardGame.Core.FactionResponsePolicySource source);
}

internal interface IKernelCreateFangtianHalberdHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, CardGame.Core.Card card, CardGame.Core.CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, System.Collections.Generic.IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface IKernelCreateHellEncounterHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.HellEncounterHost CreateHellEncounterHost();
}

internal interface IKernelCreateHpChangedTriggerWindowHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost();
}

internal interface IKernelCreateIceSwordHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.IceSwordHandle CreateIceSwordHandle(CardGame.Core.NativeOwners.CardAttackHandle attack, int preventedDamageAmount);
}

internal interface IKernelCreateMoSimaYiFactsOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.MoSimaYiFacts CreateMoSimaYiFacts();
}

internal interface IKernelCreateModeEncounterHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ModeEncounterHost CreateModeEncounterHost();
}

internal interface IKernelCreateModeNationalRevealHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ModeNationalRevealHost CreateModeNationalRevealHost();
}

internal interface IKernelCreateModeSetupEquipmentHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ModeSetupEquipmentHost CreateModeSetupEquipmentHost();
}

internal interface IKernelCreateMouGuanPingFactsOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.MouGuanPingFacts CreateMouGuanPingFacts();
}

internal interface IKernelCreateMouHuangGaiFactsAndMouHgFirstChildOperations : CardGame.Core.IProgramRuntimeOperations
{
    bool MouHgFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    CardGame.Core.NativeOwners.MouHuangGaiFacts CreateMouHuangGaiFacts();
}

internal interface IKernelCreateMouSunShangxiangFactsOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.MouSunShangxiangFacts CreateMouSunShangxiangFacts();
}

internal interface IKernelCreateMovementArmorOwnerOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.MovementArmorOwner CreateMovementArmorOwner(CardGame.Core.CharacterState owner);
}

internal interface IKernelCreateNianChallengeHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.NianChallengeHost CreateNianChallengeHost();
}

internal interface IKernelCreateParticipantSetupContextOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ParticipantSetupContext CreateParticipantSetupContext();
}

internal interface IKernelCreateProgramAttackHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}

internal interface IKernelCreateProgramExpressionWorldOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(CardGame.Core.ProgramSkillFrame frame);
}

internal interface IKernelCreateProgramHealthChildQueriesOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ProgramHealthChildQueries CreateProgramHealthChildQueries();
}

internal interface IKernelCreateProgramLegalityRouterOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.ProgramLegalityRouter CreateProgramLegalityRouter(CardGame.Core.ProgramLegalityCatalog catalog, System.Func<System.Reflection.MethodInfo, object?>? resolveTarget = default);
}

internal interface IKernelCreateProgramSkillHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface IKernelCreateProgramTriggerActionFactQueriesAndCreateProgramTriggerFactQueriesOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries();
    CardGame.Core.NativeOwners.ProgramTriggerActionFactQueries CreateProgramTriggerActionFactQueries();
}

internal interface IKernelCreateProgramTriggerFactQueriesOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries();
}

internal interface IKernelCreateProgramTriggerQueriesOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries();
}

internal interface IKernelCreateQilinBowHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.QilinBowHandle CreateQilinBowHandle(CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateQinglongCrescentBladeHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface IKernelCreateRuleProgressionOperationsOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.RuleProgressionOperations CreateRuleProgressionOperations();
}

internal interface IKernelCreateSelectionQueriesOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.SelectionQueries CreateSelectionQueries();
}

internal interface IKernelCreateStandardBattleHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.StandardBattleHost CreateStandardBattleHost();
}

internal interface IKernelCreateStoneAxeHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.StoneAxeHandle CreateStoneAxeHandle(CardGame.Core.NativeOwners.CardAttackHandle attack, System.Collections.Generic.IReadOnlyList<int> candidateCardIds);
}

internal interface IKernelCreateWuLuKangWindowHostOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.WuLuKangWindowHost CreateWuLuKangWindowHost();
}

internal interface IKernelCreateYingboGiftHandleOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.YingboGiftHandle CreateYingboGiftHandle(long ownerId);
}

internal interface IKernelDamageHooksAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramDamageHooks? _damageHooks { get; }
}

internal interface IKernelDistanceRulesAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramDistanceRules? _distanceRules { get; }
}

internal interface IKernelDyingObserverPredicate : CardGame.Core.IProgramRuntimeOperations
{
    bool Invoke(System.Func<int, CardGame.Core.DyingFrame, bool>? __nativeReceiver, int arg2, CardGame.Core.DyingFrame arg3);
}

internal interface IKernelEnsureSessionHealthyOperations : CardGame.Core.IProgramRuntimeOperations
{
    void EnsureSessionHealthy();
}

internal interface IKernelEquipmentRulesAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.EquipmentContentRegistry? _equipmentRules { get; }
}

internal interface IKernelFrameStoreAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface IKernelFrameStoreQuery : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.FrameStore _resolutionStack();
}

internal interface IKernelMatchExecutionLoopAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.MatchExecutionLoop _matchExecutionLoop { get; }
}

internal interface IKernelMatchModeRulesAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface IKernelMatchModeRuntimeAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface IKernelMatchRandomAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.DeterministicRandom _random { get; }
}

internal interface IKernelMatchesOperations : CardGame.Core.IProgramRuntimeOperations
{
    bool Matches(CardGame.Core.NativeFrameAutomaticPhase phase, int index, CardGame.Core.ResolutionFrame frame);
}

internal interface IKernelMouCaoHongContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouCaoHongEdge(int i);
    bool MouCaoHongFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouCaoHongSuffix(int n);
    CardGame.Core.NativeOwners.MouCaoHongFacts CreateMouCaoHongFacts();
    bool CurrentMouCaoHongDebt(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ProgramSkillWindowContext w);
    CardGame.Core.MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt2(CardGame.Core.ProgramTriggerCandidate c, CardGame.Core.ProgramSkillWindowContext w);
}

internal interface IKernelMouCaoShuangAnniversaryContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouCaoShuangAnniversaryEdge(int i);
    bool MouCaoShuangAnniversaryFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouCaoShuangAnniversarySuffix(int n);
    CardGame.Core.NativeOwners.MouCaoShuangAnniversaryFacts CreateMouCaoShuangAnniversaryFacts();
}

internal interface IKernelMouChenLinContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouChenLinEdge(int i);
    bool MouChenLinFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouChenLinSuffix(int n);
    CardGame.Core.NativeOwners.MouChenLinFacts CreateMouChenLinFacts();
}

internal interface IKernelMouDianWeiContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouDianWeiEdge(int i);
    bool MouDianWeiFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouDianWeiSuffix(int n);
    CardGame.Core.NativeOwners.MouDianWeiFacts CreateMouDianWeiFacts();
}

internal interface IKernelMouDongChengContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouDcEdge(int i);
    bool MouDcFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouDcSuffix(int i);
    CardGame.Core.NativeOwners.MouDongChengFacts CreateMouDongChengFacts();
    CardGame.Core.NativeOwners.MouDcLightningFacts CreateMouDcLightningFacts();
}

internal interface IKernelMouDongZhuoOlContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouDongZhuoOlEdge(int i);
    bool MouDongZhuoOlFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouDongZhuoOlSuffix(int n);
    CardGame.Core.NativeOwners.MouDongZhuoOlFacts CreateMouDongZhuoOlFacts();
}

internal interface IKernelMouGuoJiaContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouGjEdge(int i);
    bool MouGjFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouGjSuffix(int n);
    CardGame.Core.NativeOwners.MouGuoJiaFacts CreateMouGuoJiaFacts();
}

internal interface IKernelMouHuCheErContinuation : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.MouHuCheErFacts CreateMouHuCheErFacts();
    bool MouHuCheErEdge(int i);
    bool MouHuCheErFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouHuCheErSuffix(int n);
}

internal interface IKernelMouHuLieContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouHuLieEdge(int i);
    bool MouHuLieFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouHuLieSuffix(int n);
    CardGame.Core.NativeOwners.MouHuLieFacts CreateMouHuLieFacts();
}

internal interface IKernelMouJiaXuContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouJxEdge(int i);
    bool MouJxFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouJxSuffix(int n);
    CardGame.Core.NativeOwners.MouJiaXuFacts CreateMouJiaXuFacts();
}

internal interface IKernelMouJiangJiContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouJjEdge(int i);
    bool MouJjFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouJjSuffix(int n);
    CardGame.Core.NativeOwners.MouJiangJiFacts CreateMouJiangJiFacts();
}

internal interface IKernelMouJiangWeiAnniversaryContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouJiangWeiAnniversaryEdge(int i);
    bool MouJiangWeiAnniversaryFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouJiangWeiAnniversarySuffix(int n);
    CardGame.Core.NativeOwners.MouJiangWeiAnniversaryFacts CreateMouJiangWeiAnniversaryFacts();
}

internal interface IKernelMouJuShouContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouJsEdge(int i);
    bool MouJsFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouJsSuffix(int n);
    CardGame.Core.NativeOwners.MouJuShouFacts CreateMouJuShouFacts();
}

internal interface IKernelMouLiuXieContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouLiuXieNativeEdge(int i);
    bool MouLiuXieFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouLiuXieNativeSuffix(int n);
    CardGame.Core.NativeOwners.MouLiuXieFacts CreateMouLiuXieFacts();
}

internal interface IKernelMouLuSuAnniversaryContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouLuSuEdge(int i);
    bool MouLuSuFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouLuSuSuffix(int n);
    CardGame.Core.NativeOwners.MouLuSuAnniversaryFacts CreateMouLuSuAnniversaryFacts();
}

internal interface IKernelMouSmContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouSmEdge(int i);
    bool MouSmFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouSmSuffix(int n);
    CardGame.Core.NativeOwners.MouSmFacts CreateMouSmFacts();
}

internal interface IKernelMouXuShengContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouXuShengNativeEdge(int i);
    bool MouXuShengFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouXuShengNativeSuffix(int n);
    CardGame.Core.NativeOwners.MouXuShengFacts CreateMouXuShengFacts();
}

internal interface IKernelMouXunYuAnniversaryContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouXyEdge(int i);
    bool MouXyFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouXySuffix(int n);
    CardGame.Core.NativeOwners.MouXunYuAnniversaryFacts CreateMouXunYuAnniversaryFacts();
}

internal interface IKernelMouZhangHeContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouQiaobianNativeEdge(int i);
    bool MouQiaobianFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouQiaobianNativeSuffix(int n);
    CardGame.Core.NativeOwners.MouZhangHeFacts CreateMouZhangHeFacts();
}

internal interface IKernelMouZhangXiuAnniversaryContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouZhangXiuAnniversaryEdge(int i);
    bool MouZhangXiuAnniversaryFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouZhangXiuAnniversarySuffix(int n);
    CardGame.Core.NativeOwners.MouZhangXiuAnniversaryFacts CreateMouZhangXiuAnniversaryFacts();
}

internal interface IKernelMouZhouYuContinuation : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.MouZhouYuFacts CreateMouZhouYuFacts();
    bool MouZyEdge(int i);
    bool MouZyFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouZySuffix(int i);
}

internal interface IKernelMouZhuRanContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouZhuRanNativeEdge(int i);
    bool MouZhuRanFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouZhuRanNativeSuffix(int n);
    CardGame.Core.NativeOwners.MouZhuRanFacts CreateMouZhuRanFacts();
}

internal interface IKernelMouZhugeJinContinuation : CardGame.Core.IProgramRuntimeOperations
{
    bool MouZhugeJinEdge(int i);
    bool MouZhugeJinFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child);
    bool MouZhugeJinSuffix(int n);
    CardGame.Core.NativeOwners.MouZhugeJinFacts CreateMouZhugeJinFacts();
}

internal interface IKernelNativeReturnsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramNativeReturns? _nativeReturns { get; }
}

internal interface IKernelObservedSkillGrantRevisionsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions { get; }
}

internal interface IKernelObserverIndexPredicate : CardGame.Core.IProgramRuntimeOperations
{
    bool Invoke(System.Func<int, bool> __nativeReceiver, int arg2);
}

internal interface IKernelObserversAccess : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.ObserverDispatcher Observers { get; }
}

internal interface IKernelParticipantSetupAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.MatchParticipantSetup? _participantSetup { get; }
}

internal interface IKernelPendingCardsMovedBatchesAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly System.Collections.Generic.List<CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches { get; }
}

internal interface IKernelPhaseUsedProjectionsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramPhaseUsedProjections? _phaseUsedProjections { get; }
}

internal interface IKernelPlayerViewQueryFactories : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.PlayerViewVisibilityQueries CreatePlayerViewVisibilityQueries();
    CardGame.Core.NativeOwners.PlayerViewCardsQueries CreatePlayerViewCardsQueries();
    CardGame.Core.NativeOwners.PlayerViewRulesQueries CreatePlayerViewRulesQueries();
    CardGame.Core.NativeOwners.PlayerViewMatchQueries CreatePlayerViewMatchQueries();
}

internal interface IKernelProgramActionsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramActionRouter? _programActions { get; }
}

internal interface IKernelProgramChildPredicate : CardGame.Core.IProgramRuntimeOperations
{
    bool Invoke(System.Func<CardGame.Core.ProgramSkillFrame, CardGame.Core.ResolutionFrame, bool> __nativeReceiver, CardGame.Core.ProgramSkillFrame arg2, CardGame.Core.ResolutionFrame arg3);
}

internal interface IKernelProgramHistoryAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramEventHistoryIndex? _programHistory { get; }
}

internal interface IKernelProgramInteractionsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramInteractionRouter? _programInteractions { get; }
}

internal interface IKernelProgramIssuedTriggersAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramIssuedTriggerRuntime? _programIssuedTriggers { get; }
}

internal interface IKernelProgramLegalityAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramLegalityRouter? _programLegality { get; }
}

internal interface IKernelProgramPhaseScheduleAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule { get; }
}

internal interface IKernelProgramPipelineHooksAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface IKernelProgramRuleObserversAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.ProgramRuleObservers _programRuleObservers { get; }
}

internal interface IKernelProgramSelectionQueriesAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.IProgramSelectionQueries? _programSelectionQueries { get; }
}

internal interface IKernelProgramSelectionValidatorsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramSelectionValidationRegistry? _programSelectionValidators { get; }
}

internal interface IKernelProgramSelectionsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramSelectionRegistry? _programSelections { get; }
}

internal interface IKernelProgramTargetResolversAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramTargetResolverRegistry? _programTargetResolvers { get; }
}

internal interface IKernelProgramTriggerProofsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramTriggerProofRuntime? _programTriggerProofs { get; }
}

internal interface IKernelProgramWindowReturnOperations : CardGame.Core.IProgramRuntimeOperations
{
    void Return(CardGame.Core.ProgramWindowReturnCatalog __nativeReceiver, CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface IKernelPublicHandRevealReturnOperations : CardGame.Core.IProgramRuntimeOperations
{
    void Return(CardGame.Core.PublicHandRevealReturns __nativeReceiver, string key, CardGame.Core.ResolutionFrame parent);
}

internal interface IKernelQinglongContinuationHandleFactory : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId);
    CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId);
    CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface IKernelRejectOperations : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.CommandResult Reject(CardGame.Core.CommandErrorCode code, string message);
}

internal interface IKernelResolutionSchedulerAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.ResolutionScheduler _resolutionScheduler { get; }
}

internal interface IKernelResolveRuntimeHandlerTargetOperations : CardGame.Core.IProgramRuntimeOperations
{
    object? ResolveRuntimeHandlerTarget(System.Reflection.MethodInfo method);
}

internal interface IKernelReturnOperationOperations : CardGame.Core.IProgramRuntimeOperations
{
    void ReturnOperation(CardGame.Core.SkillProgramEffectOp operation, CardGame.Core.ProgramSkillFrame frame, CardGame.Core.NativeDrawOutcome outcome);
}

internal interface IKernelRevisionAccess : CardGame.Core.IProgramRuntimeOperations
{
    long _revision { get; }
}

internal interface IKernelRuleProgressionOperations : CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(CardGame.Core.IGameEvent payload);
}

internal interface IKernelRuleQueryFactFactories : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.DistanceQueryFacts CreateDistanceQueryFacts(CardGame.Core.CharacterState source, CardGame.Core.CharacterState target);
    CardGame.Core.NativeOwners.AttackRangeQueryFacts CreateAttackRangeQueryFacts(CardGame.Core.CharacterState player);
    CardGame.Core.NativeOwners.DrawCountQueryFacts CreateDrawCountQueryFacts(CardGame.Core.CharacterState player);
    CardGame.Core.NativeOwners.SlashLimitQueryFacts CreateSlashLimitQueryFacts(CardGame.Core.CharacterState player);
    CardGame.Core.NativeOwners.HandLimitQueryFacts CreateHandLimitQueryFacts(CardGame.Core.CharacterState player);
    CardGame.Core.NativeOwners.CardTargetCountQueryFacts CreateCardTargetCountQueryFacts(CardGame.Core.CharacterState player, CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId);
    CardGame.Core.NativeOwners.CardUseDistanceQueryFacts CreateCardUseDistanceQueryFacts(CardGame.Core.CharacterState player, CardGame.Core.CardKind effectiveCardKind);
    CardGame.Core.NativeOwners.NumericContributionQueryFacts CreateNumericContributionQueryFacts(CardGame.Core.CharacterState player, CardGame.Core.SkillRuleQuery ruleQuery);
}

internal interface IKernelRuntimeHooksAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramRuntimeHooks? _runtimeHooks { get; }
}

internal interface IKernelShanheMarketAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket { get; }
}

internal interface IKernelSkillBindingIndexAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.MatchSkillBindingIndex _skillBindingIndex { get; }
}

internal interface IKernelSkillRuntimeStateAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface IKernelSuppressedCardMovedEventBatchIdsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly System.Collections.Generic.Stack<long> _suppressedCardMovedEventBatchIds { get; }
}

internal interface IKernelTargetRulesAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramTargetRules? _targetRules { get; }
}

internal interface IKernelTargetWindowProtocolsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramTargetWindowProtocols? _targetWindowProtocols { get; }
}

internal interface IKernelTrickEffectHandleFactory : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long resolutionId, int sourceSeat, CardGame.Core.Card card, System.Collections.Generic.IReadOnlyList<int> targetSeats, CardGame.Core.GroupCardEffect effect, CardGame.Core.CardKind? requiredCardKind, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default);
    CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long resolutionId, int sourceSeat, int weaponOwnerSeat, int slashTargetSeat, CardGame.Core.Card card);
    CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
    CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface IKernelTriggerWindowHostFactories : CardGame.Core.IProgramRuntimeOperations
{
    CardGame.Core.NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost();
    CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost();
    CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost();
}

internal interface IKernelTryAdvanceOperations : CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvance(CardGame.Core.ResolutionFrame frame);
}

internal interface IKernelTryResumeOperations : CardGame.Core.IProgramRuntimeOperations
{
    bool TryResume(CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, long frameId);
}

internal interface IKernelTryReturnMovementOperations : CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnMovement(CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, CardGame.Core.ProgramSkillFrame frame, bool afterPayment = false);
}

internal interface IKernelTurnCardUseEffectsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref readonly CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface IKernelViewContributorsAccess : CardGame.Core.IProgramRuntimeOperations
{
    ref CardGame.Core.ProgramViewContributors? _viewContributors { get; }
}
