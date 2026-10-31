#nullable enable

using CardGame.Core.EngineOwners;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class KernelAcceptAndRejectOperationsAdapter(GameEngine engine) : IKernelAcceptAndRejectOperations
    {
        public CommandResult Reject(CommandErrorCode code, string message) => engine.Reject(code, message);
        public CommandResult Accept(Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class KernelAcceptOperationsAdapter(GameEngine engine) : IKernelAcceptOperations
    {
        public CommandResult Accept(Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class KernelActiveCardMovementBatchIdsAccessAdapter(GameEngine engine) : IKernelActiveCardMovementBatchIdsAccess
    {
        public ref readonly Stack<long> _activeCardMovementBatchIds => ref engine._activeCardMovementBatchIds;
    }

    [ProgramCapability]
    private sealed class KernelActivityRuleFlowFactoriesAdapter(GameEngine engine) : IKernelActivityRuleFlowFactories
    {
        public NativeOwners.ActivityBattleRulesHost CreateActivityBattleRulesHost() => engine.ComposeNativeActivityBattleRulesHost();
        public NativeOwners.WenheRuleFlowHost CreateWenheRuleFlowHost() => engine.ComposeNativeWenheRuleFlowHost();
        public NativeOwners.GuanduEventFlowHost CreateGuanduEventFlowHost() => engine.ComposeNativeGuanduEventFlowHost();
    }

    [ProgramCapability]
    private sealed class KernelAdvanceEventRulesAndQueueFactOperationsAdapter(GameEngine engine) : IKernelAdvanceEventRulesAndQueueFactOperations
    {
        public void AdvanceEventRulesAndQueueFact(IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class KernelAdvanceRulesAndPublishStateOperationsAdapter(GameEngine engine) : IKernelAdvanceRulesAndPublishStateOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
    }

    [ProgramCapability]
    private sealed class KernelAdvanceToHumanBoundaryOperationsAdapter(GameEngine engine) : IKernelAdvanceToHumanBoundaryOperations
    {
        public void AdvanceToHumanBoundary() => engine.AdvanceToHumanBoundary();
    }

    [ProgramCapability]
    private sealed class KernelAllegiancePolicyAccessAdapter(GameEngine engine) : IKernelAllegiancePolicyAccess
    {
        public ref IMatchModeAllegiancePolicy? _allegiancePolicy => ref engine._allegiancePolicy;
    }

    [ProgramCapability]
    private sealed class KernelBindRuntimeHandlerOperationsAdapter(GameEngine engine) : IKernelBindRuntimeHandlerOperations
    {
        public Delegate BindRuntimeHandler(System.Reflection.MethodInfo method, Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class KernelCaoXianContinuationAdapter(GameEngine engine) : IKernelCaoXianContinuation
    {
        public bool CaoXianEdge(int i) => engine.CaoXianOlEntries.Value.CaoXianEdge(i);
        public bool CaoXianFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.CaoXianOlEntries.Value.CaoXianFirstChild(f, child);
        public bool CaoXianSuffix(int n) => engine.CaoXianOlEntries.Value.CaoXianSuffix(n);
        public NativeOwners.CaoXianFacts CreateCaoXianFacts() => engine.ComposeNativeCaoXianFacts();
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFactionRequestHandlesFactoryAdapter(GameEngine engine) : IKernelCardAttackAndFactionRequestHandlesFactory
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, FactionCardRequestPurpose purpose, int ownerSeat, IReadOnlyList<int> candidateSeats, string providerFactionId, NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardKind requiredKind = (CardKind)0, NativeOwners.BorrowedSwordHandle? borrowedSword = default, NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFactionRequestHandlesFactoryAndLookupAdapter(GameEngine engine) : IKernelCardAttackAndFactionRequestHandlesFactoryAndLookup
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
        public NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, FactionCardRequestPurpose purpose, int ownerSeat, IReadOnlyList<int> candidateSeats, string providerFactionId, NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardKind requiredKind = (CardKind)0, NativeOwners.BorrowedSwordHandle? borrowedSword = default, NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFactionRequestHandlesLookupAdapter(GameEngine engine) : IKernelCardAttackAndFactionRequestHandlesLookup
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
        public NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFangtianHandlesFactoryAdapter(GameEngine engine) : IKernelCardAttackAndFangtianHandlesFactory
    {
        public NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, Card card, CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardConversionSource? conversionSource = default) => engine.ComposeNativeFangtianHalberdHandle(resolutionId, sourceSeat, card, effectiveCardKind, ignoresArmor, damageAmount, targetSeats, usesFangtian, countedTowardSlashLimit, conversionSource);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFangtianHandlesLookupAdapter(GameEngine engine) : IKernelCardAttackAndFangtianHandlesLookup
    {
        public NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackHandleFactoryAdapter(GameEngine engine) : IKernelCardAttackHandleFactory
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackHandleLookupAdapter(GameEngine engine) : IKernelCardAttackHandleLookup
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardContinuationHandleFactoryAdapter(GameEngine engine) : IKernelCardContinuationHandleFactory
    {
        public NativeOwners.DuelHandle CreateDuelHandle(long ownerId) => engine.ComposeNativeDuelHandle(ownerId);
        public NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId) => engine.ComposeNativeGroupCardHandle(ownerId);
        public NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId) => engine.ComposeNativeBorrowedSwordHandle(ownerId);
        public NativeOwners.StoneAxeHandle CreateStoneAxeHandle(long ownerId) => engine.ComposeNativeStoneAxeHandle(ownerId);
        public NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(long ownerId) => engine.ComposeNativeCixiongDoubleSwordsHandle(ownerId);
        public NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(long ownerId) => engine.ComposeNativeQinglongCrescentBladeHandle(ownerId);
        public NativeOwners.QinglongFollowupHandle CreateQinglongFollowupHandle(long ownerId) => engine.ComposeNativeQinglongFollowupHandle(ownerId);
        public NativeOwners.IceSwordHandle CreateIceSwordHandle(long ownerId) => engine.ComposeNativeIceSwordHandle(ownerId);
        public NativeOwners.QilinBowHandle CreateQilinBowHandle(long ownerId) => engine.ComposeNativeQilinBowHandle(ownerId);
        public NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(long ownerId) => engine.ComposeNativeFactionDefenseHandle(ownerId);
        public NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardLegalityAccessAdapter(GameEngine engine) : IKernelCardLegalityAccess
    {
        public ref ProgramCardLegality? _cardLegality => ref engine._cardLegality;
    }

    [ProgramCapability]
    private sealed class KernelCardUsePaymentsAccessAdapter(GameEngine engine) : IKernelCardUsePaymentsAccess
    {
        public ref ProgramCardUsePayments? _cardUsePayments => ref engine._cardUsePayments;
    }

    [ProgramCapability]
    private sealed class KernelCardZoneStoreAccessAdapter(GameEngine engine) : IKernelCardZoneStoreAccess
    {
        public ref readonly CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class KernelCommandEventJournalAccessAdapter(GameEngine engine) : IKernelCommandEventJournalAccess
    {
        public ref CommandEventJournal? _commandEventJournal => ref engine._commandEventJournal;
    }

    [ProgramCapability]
    private sealed class KernelCommandRuntimeAccessAdapter(GameEngine engine) : IKernelCommandRuntimeAccess
    {
        public ICommandSessionRuntime CommandRuntime { get => engine.CommandRuntime; }
    }

    [ProgramCapability]
    private sealed class KernelCommandRuntimeAndRevisionAccessAdapter(GameEngine engine) : IKernelCommandRuntimeAndRevisionAccess
    {
        public long _revision { get => engine._revision; }
        public ICommandSessionRuntime CommandRuntime { get => engine.CommandRuntime; }
    }

    [ProgramCapability]
    private sealed class KernelCommandSessionAccessAdapter(GameEngine engine) : IKernelCommandSessionAccess
    {
        public ref readonly CommandSession _commandSession => ref engine._commandSession;
    }

    [ProgramCapability]
    private sealed class KernelCompleteGameOperationsAdapter(GameEngine engine) : IKernelCompleteGameOperations
    {
        public void CompleteGame() => engine.CompleteGame();
    }

    [ProgramCapability]
    private sealed class KernelContentRegistryAccessAdapter(GameEngine engine) : IKernelContentRegistryAccess
    {
        public ref readonly ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class KernelCreateActionQueryScopeOperationsAdapter(GameEngine engine) : IKernelCreateActionQueryScopeOperations
    {
        public NativeOwners.ActionQueryScope CreateActionQueryScope(NativeOwners.ActionQuery? previous = default) => engine.ComposeNativeActionQueryScope(previous);
    }

    [ProgramCapability]
    private sealed class KernelCreateActivePlayerEffectQueriesOperationsAdapter(GameEngine engine) : IKernelCreateActivePlayerEffectQueriesOperations
    {
        public NativeOwners.ActivePlayerEffectQueries CreateActivePlayerEffectQueries() => engine.ComposeNativeActivePlayerEffectQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateBorrowedSwordHandleAndCreateCardAttackHandleOperationsAdapter(GameEngine engine) : IKernelCreateBorrowedSwordHandleAndCreateCardAttackHandleOperations
    {
        public NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId) => engine.ComposeNativeBorrowedSwordHandle(ownerId);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardActionTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateCardActionTriggerWindowHostOperations
    {
        public NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost() => engine.ComposeNativeCardActionTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateCardAttackHandle2AndCreateDuelHandleOperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateCardAttackHandle2AndCreateDuelHandleOperations
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public NativeOwners.DuelHandle CreateDuelHandle(NativeOwners.CardAttackHandle attack) => engine.ComposeNativeDuelHandle(attack);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateCardAttackHandle2OperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateCardAttackHandle2Operations
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperations
    {
        public NativeOwners.DuelHandle CreateDuelHandle(long ownerId) => engine.ComposeNativeDuelHandle(ownerId);
        public NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId) => engine.ComposeNativeGroupCardHandle(ownerId);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateDuelHandleOperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateDuelHandleOperations
    {
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public NativeOwners.DuelHandle CreateDuelHandle(NativeOwners.CardAttackHandle attack) => engine.ComposeNativeDuelHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardsMovedTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateCardsMovedTriggerWindowHostOperations
    {
        public NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost() => engine.ComposeNativeCardsMovedTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateCixiongDoubleSwordsHandleOperationsAdapter(GameEngine engine) : IKernelCreateCixiongDoubleSwordsHandleOperations
    {
        public NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(NativeOwners.CardAttackHandle attack) => engine.ComposeNativeCixiongDoubleSwordsHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateDamageTriggerCapturedAttackOperationsAdapter(GameEngine engine) : IKernelCreateDamageTriggerCapturedAttackOperations
    {
        public NativeOwners.DamageTriggerCapturedAttack CreateDamageTriggerCapturedAttack(IDamageAttempt attack) => engine.ComposeNativeDamageTriggerCapturedAttack(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateDouyuTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateDouyuTriggerWindowHostOperations
    {
        public NativeOwners.DouyuTriggerWindowHost CreateDouyuTriggerWindowHost() => engine.ComposeNativeDouyuTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateFactionCardRequestHandleOperationsAdapter(GameEngine engine) : IKernelCreateFactionCardRequestHandleOperations
    {
        public NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, FactionCardRequestPurpose purpose, int ownerSeat, IReadOnlyList<int> candidateSeats, string providerFactionId, NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardKind requiredKind = (CardKind)0, NativeOwners.BorrowedSwordHandle? borrowedSword = default, NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class KernelCreateFactionDefenseHandleOperationsAdapter(GameEngine engine) : IKernelCreateFactionDefenseHandleOperations
    {
        public NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(NativeOwners.CardAttackHandle attack, int ownerSeat, IReadOnlyList<int> candidateSeats, FactionResponsePolicySource source) => engine.ComposeNativeFactionDefenseHandle(attack, ownerSeat, candidateSeats, source);
    }

    [ProgramCapability]
    private sealed class KernelCreateFangtianHalberdHandleOperationsAdapter(GameEngine engine) : IKernelCreateFangtianHalberdHandleOperations
    {
        public NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, Card card, CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardConversionSource? conversionSource = default) => engine.ComposeNativeFangtianHalberdHandle(resolutionId, sourceSeat, card, effectiveCardKind, ignoresArmor, damageAmount, targetSeats, usesFangtian, countedTowardSlashLimit, conversionSource);
    }

    [ProgramCapability]
    private sealed class KernelCreateHellEncounterHostOperationsAdapter(GameEngine engine) : IKernelCreateHellEncounterHostOperations
    {
        public NativeOwners.HellEncounterHost CreateHellEncounterHost() => engine.ComposeNativeHellEncounterHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateHpChangedTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateHpChangedTriggerWindowHostOperations
    {
        public NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost() => engine.ComposeNativeHpChangedTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateIceSwordHandleOperationsAdapter(GameEngine engine) : IKernelCreateIceSwordHandleOperations
    {
        public NativeOwners.IceSwordHandle CreateIceSwordHandle(NativeOwners.CardAttackHandle attack, int preventedDamageAmount) => engine.ComposeNativeIceSwordHandle(attack, preventedDamageAmount);
    }

    [ProgramCapability]
    private sealed class KernelCreateMoSimaYiFactsOperationsAdapter(GameEngine engine) : IKernelCreateMoSimaYiFactsOperations
    {
        public NativeOwners.MoSimaYiFacts CreateMoSimaYiFacts() => engine.ComposeNativeMoSimaYiFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateModeEncounterHostOperationsAdapter(GameEngine engine) : IKernelCreateModeEncounterHostOperations
    {
        public NativeOwners.ModeEncounterHost CreateModeEncounterHost() => engine.ComposeNativeModeEncounterHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateModeNationalRevealHostOperationsAdapter(GameEngine engine) : IKernelCreateModeNationalRevealHostOperations
    {
        public NativeOwners.ModeNationalRevealHost CreateModeNationalRevealHost() => engine.ComposeNativeModeNationalRevealHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateModeSetupEquipmentHostOperationsAdapter(GameEngine engine) : IKernelCreateModeSetupEquipmentHostOperations
    {
        public NativeOwners.ModeSetupEquipmentHost CreateModeSetupEquipmentHost() => engine.ComposeNativeModeSetupEquipmentHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateMouGuanPingFactsOperationsAdapter(GameEngine engine) : IKernelCreateMouGuanPingFactsOperations
    {
        public NativeOwners.MouGuanPingFacts CreateMouGuanPingFacts() => engine.ComposeNativeMouGuanPingFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateMouHuangGaiFactsAndMouHgFirstChildOperationsAdapter(GameEngine engine) : IKernelCreateMouHuangGaiFactsAndMouHgFirstChildOperations
    {
        public bool MouHgFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.HuangGaiMouEntries.Value.MouHgFirstChild(f, child);
        public NativeOwners.MouHuangGaiFacts CreateMouHuangGaiFacts() => engine.ComposeNativeMouHuangGaiFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateMouSunShangxiangFactsOperationsAdapter(GameEngine engine) : IKernelCreateMouSunShangxiangFactsOperations
    {
        public NativeOwners.MouSunShangxiangFacts CreateMouSunShangxiangFacts() => engine.ComposeNativeMouSunShangxiangFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateMovementArmorOwnerOperationsAdapter(GameEngine engine) : IKernelCreateMovementArmorOwnerOperations
    {
        public NativeOwners.MovementArmorOwner CreateMovementArmorOwner(CharacterState owner) => engine.ComposeNativeMovementArmorOwner(owner);
    }

    [ProgramCapability]
    private sealed class KernelCreateNianChallengeHostOperationsAdapter(GameEngine engine) : IKernelCreateNianChallengeHostOperations
    {
        public NativeOwners.NianChallengeHost CreateNianChallengeHost() => engine.ComposeNativeNianChallengeHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateParticipantSetupContextOperationsAdapter(GameEngine engine) : IKernelCreateParticipantSetupContextOperations
    {
        public NativeOwners.ParticipantSetupContext CreateParticipantSetupContext() => engine.ComposeNativeParticipantSetupContext();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramAttackHandleOperationsAdapter(GameEngine engine) : IKernelCreateProgramAttackHandleOperations
    {
        public NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramExpressionWorldOperationsAdapter(GameEngine engine) : IKernelCreateProgramExpressionWorldOperations
    {
        public NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(ProgramSkillFrame frame) => engine.ComposeNativeProgramExpressionWorld(frame);
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramHealthChildQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramHealthChildQueriesOperations
    {
        public NativeOwners.ProgramHealthChildQueries CreateProgramHealthChildQueries() => engine.ComposeNativeProgramHealthChildQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramLegalityRouterOperationsAdapter(GameEngine engine) : IKernelCreateProgramLegalityRouterOperations
    {
        public ProgramLegalityRouter CreateProgramLegalityRouter(ProgramLegalityCatalog catalog, Func<System.Reflection.MethodInfo, object?>? resolveTarget = default) => new ProgramLegalityRouter(catalog, engine, resolveTarget);
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramSkillHostOperationsAdapter(GameEngine engine) : IKernelCreateProgramSkillHostOperations
    {
        public NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramTriggerActionFactQueriesAndCreateProgramTriggerFactQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramTriggerActionFactQueriesAndCreateProgramTriggerFactQueriesOperations
    {
        public NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries() => engine.ComposeNativeProgramTriggerFactQueries();
        public NativeOwners.ProgramTriggerActionFactQueries CreateProgramTriggerActionFactQueries() => engine.ComposeNativeProgramTriggerActionFactQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramTriggerFactQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramTriggerFactQueriesOperations
    {
        public NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries() => engine.ComposeNativeProgramTriggerFactQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramTriggerQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramTriggerQueriesOperations
    {
        public NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries() => engine.ComposeNativeProgramTriggerQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateQilinBowHandleOperationsAdapter(GameEngine engine) : IKernelCreateQilinBowHandleOperations
    {
        public NativeOwners.QilinBowHandle CreateQilinBowHandle(NativeOwners.CardAttackHandle attack) => engine.ComposeNativeQilinBowHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateQinglongCrescentBladeHandleOperationsAdapter(GameEngine engine) : IKernelCreateQinglongCrescentBladeHandleOperations
    {
        public NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(NativeOwners.CardAttackHandle attack) => engine.ComposeNativeQinglongCrescentBladeHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateRuleProgressionOperationsOperationsAdapter(GameEngine engine) : IKernelCreateRuleProgressionOperationsOperations
    {
        public NativeOwners.RuleProgressionOperations CreateRuleProgressionOperations() => engine.ComposeNativeRuleProgressionOperations();
    }

    [ProgramCapability]
    private sealed class KernelCreateSelectionQueriesOperationsAdapter(GameEngine engine) : IKernelCreateSelectionQueriesOperations
    {
        public NativeOwners.SelectionQueries CreateSelectionQueries() => engine.ComposeNativeSelectionQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateStandardBattleHostOperationsAdapter(GameEngine engine) : IKernelCreateStandardBattleHostOperations
    {
        public NativeOwners.StandardBattleHost CreateStandardBattleHost() => engine.ComposeNativeStandardBattleHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateStoneAxeHandleOperationsAdapter(GameEngine engine) : IKernelCreateStoneAxeHandleOperations
    {
        public NativeOwners.StoneAxeHandle CreateStoneAxeHandle(NativeOwners.CardAttackHandle attack, IReadOnlyList<int> candidateCardIds) => engine.ComposeNativeStoneAxeHandle(attack, candidateCardIds);
    }

    [ProgramCapability]
    private sealed class KernelCreateWuLuKangWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateWuLuKangWindowHostOperations
    {
        public NativeOwners.WuLuKangWindowHost CreateWuLuKangWindowHost() => engine.ComposeNativeWuLuKangWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateYingboGiftHandleOperationsAdapter(GameEngine engine) : IKernelCreateYingboGiftHandleOperations
    {
        public NativeOwners.YingboGiftHandle CreateYingboGiftHandle(long ownerId) => engine.ComposeNativeYingboGiftHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelDamageHooksAccessAdapter(GameEngine engine) : IKernelDamageHooksAccess
    {
        public ref ProgramDamageHooks? _damageHooks => ref engine._damageHooks;
    }

    [ProgramCapability]
    private sealed class KernelDistanceRulesAccessAdapter(GameEngine engine) : IKernelDistanceRulesAccess
    {
        public ref ProgramDistanceRules? _distanceRules => ref engine._distanceRules;
    }

    [ProgramCapability]
    private sealed class KernelDyingObserverPredicateAdapter : IKernelDyingObserverPredicate
    {
        public bool Invoke(Func<int, DyingFrame, bool>? __nativeReceiver, int arg2, DyingFrame arg3) => __nativeReceiver.Invoke(arg2, arg3);
        public KernelDyingObserverPredicateAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class KernelEnsureSessionHealthyOperationsAdapter(GameEngine engine) : IKernelEnsureSessionHealthyOperations
    {
        public void EnsureSessionHealthy() => engine.EnsureSessionHealthy();
    }

    [ProgramCapability]
    private sealed class KernelEquipmentRulesAccessAdapter(GameEngine engine) : IKernelEquipmentRulesAccess
    {
        public ref EquipmentContentRegistry? _equipmentRules => ref engine._equipmentRules;
    }

    [ProgramCapability]
    private sealed class KernelFrameStoreAccessAdapter(GameEngine engine) : IKernelFrameStoreAccess
    {
        public ref readonly FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class KernelFrameStoreQueryAdapter(GameEngine engine) : IKernelFrameStoreQuery
    {
        public ref readonly FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class KernelMatchExecutionLoopAccessAdapter(GameEngine engine) : IKernelMatchExecutionLoopAccess
    {
        public ref readonly MatchExecutionLoop _matchExecutionLoop => ref engine._matchExecutionLoop;
    }

    [ProgramCapability]
    private sealed class KernelMatchModeRulesAccessAdapter(GameEngine engine) : IKernelMatchModeRulesAccess
    {
        public ref readonly ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class KernelMatchModeRuntimeAccessAdapter(GameEngine engine) : IKernelMatchModeRuntimeAccess
    {
        public ref readonly MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class KernelMatchRandomAccessAdapter(GameEngine engine) : IKernelMatchRandomAccess
    {
        public ref readonly DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class KernelMatchesOperationsAdapter(GameEngine engine) : IKernelMatchesOperations
    {
        public bool Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame) => engine.GetRuntimeModule<CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase][index].Matches(engine, frame);
    }

    [ProgramCapability]
    private sealed class KernelMouCaoHongContinuationAdapter(GameEngine engine) : IKernelMouCaoHongContinuation
    {
        public bool MouCaoHongEdge(int i) => engine.CaoHongMouEntries.MouCaoHongEdge(i);
        public bool MouCaoHongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.CaoHongMouEntries.MouCaoHongFirstChild(f, child);
        public bool MouCaoHongSuffix(int n) => engine.CaoHongMouEntries.MouCaoHongSuffix(n);
        public NativeOwners.MouCaoHongFacts CreateMouCaoHongFacts() => engine.ComposeNativeMouCaoHongFacts();
        public bool CurrentMouCaoHongDebt(ProgramSkillFrame f, ProgramSkillWindowContext w) => engine.ComposeNativeMouCaoHongFacts().CurrentMouCaoHongDebt(f, w);
        public MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt2(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => engine.CaoHongMouEntries.CurrentMouCaoHongDebt(c, w);
    }

    [ProgramCapability]
    private sealed class KernelMouCaoShuangAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouCaoShuangAnniversaryContinuation
    {
        public bool MouCaoShuangAnniversaryEdge(int i) => engine.CaoShuangMouEntries.Value.MouCaoShuangAnniversaryEdge(i);
        public bool MouCaoShuangAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.CaoShuangMouEntries.Value.MouCaoShuangAnniversaryFirstChild(f, child);
        public bool MouCaoShuangAnniversarySuffix(int n) => engine.CaoShuangMouEntries.Value.MouCaoShuangAnniversarySuffix(n);
        public NativeOwners.MouCaoShuangAnniversaryFacts CreateMouCaoShuangAnniversaryFacts() => engine.ComposeNativeMouCaoShuangAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouChenLinContinuationAdapter(GameEngine engine) : IKernelMouChenLinContinuation
    {
        public bool MouChenLinEdge(int i) => engine.ChenLinMouEntries.MouChenLinEdge(i);
        public bool MouChenLinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.ChenLinMouEntries.MouChenLinFirstChild(f, child);
        public bool MouChenLinSuffix(int n) => engine.ChenLinMouEntries.MouChenLinSuffix(n);
        public NativeOwners.MouChenLinFacts CreateMouChenLinFacts() => engine.ComposeNativeMouChenLinFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouDianWeiContinuationAdapter(GameEngine engine) : IKernelMouDianWeiContinuation
    {
        public bool MouDianWeiEdge(int i) => engine.DianWeiMouEntries.MouDianWeiEdge(i);
        public bool MouDianWeiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.DianWeiMouEntries.MouDianWeiFirstChild(f, child);
        public bool MouDianWeiSuffix(int n) => engine.DianWeiMouEntries.MouDianWeiSuffix(n);
        public NativeOwners.MouDianWeiFacts CreateMouDianWeiFacts() => engine.ComposeNativeMouDianWeiFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouDongChengContinuationAdapter(GameEngine engine) : IKernelMouDongChengContinuation
    {
        public bool MouDcEdge(int i) => engine.DongChengMouEntries.MouDcEdge(i);
        public bool MouDcFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.DongChengMouEntries.MouDcFirstChild(f, child);
        public bool MouDcSuffix(int i) => engine.DongChengMouEntries.MouDcSuffix(i);
        public NativeOwners.MouDongChengFacts CreateMouDongChengFacts() => engine.ComposeNativeMouDongChengFacts();
        public NativeOwners.MouDcLightningFacts CreateMouDcLightningFacts() => engine.ComposeNativeMouDcLightningFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouDongZhuoOlContinuationAdapter(GameEngine engine) : IKernelMouDongZhuoOlContinuation
    {
        public bool MouDongZhuoOlEdge(int i) => engine.DongZhuoMouOlEntries.MouDongZhuoOlEdge(i);
        public bool MouDongZhuoOlFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.DongZhuoMouOlEntries.MouDongZhuoOlFirstChild(f, child);
        public bool MouDongZhuoOlSuffix(int n) => engine.DongZhuoMouOlEntries.MouDongZhuoOlSuffix(n);
        public NativeOwners.MouDongZhuoOlFacts CreateMouDongZhuoOlFacts() => engine.ComposeNativeMouDongZhuoOlFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouGuoJiaContinuationAdapter(GameEngine engine) : IKernelMouGuoJiaContinuation
    {
        public bool MouGjEdge(int i) => engine.GuoJiaMouEntries.MouGjEdge(i);
        public bool MouGjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.GuoJiaMouEntries.MouGjFirstChild(f, child);
        public bool MouGjSuffix(int n) => engine.GuoJiaMouEntries.MouGjSuffix(n);
        public NativeOwners.MouGuoJiaFacts CreateMouGuoJiaFacts() => engine.ComposeNativeMouGuoJiaFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouHuCheErContinuationAdapter(GameEngine engine) : IKernelMouHuCheErContinuation
    {
        public NativeOwners.MouHuCheErFacts CreateMouHuCheErFacts() => engine.ComposeNativeMouHuCheErFacts();
        public bool MouHuCheErEdge(int i) => engine.HuCheErMouEntries.MouHuCheErEdge(i);
        public bool MouHuCheErFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.HuCheErMouEntries.MouHuCheErFirstChild(f, child);
        public bool MouHuCheErSuffix(int n) => engine.HuCheErMouEntries.MouHuCheErSuffix(n);
    }

    [ProgramCapability]
    private sealed class KernelMouHuLieContinuationAdapter(GameEngine engine) : IKernelMouHuLieContinuation
    {
        public bool MouHuLieEdge(int i) => engine.HuLieMouEntries.MouHuLieEdge(i);
        public bool MouHuLieFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.HuLieMouEntries.MouHuLieFirstChild(f, child);
        public bool MouHuLieSuffix(int n) => engine.HuLieMouEntries.MouHuLieSuffix(n);
        public NativeOwners.MouHuLieFacts CreateMouHuLieFacts() => engine.ComposeNativeMouHuLieFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJiaXuContinuationAdapter(GameEngine engine) : IKernelMouJiaXuContinuation
    {
        public bool MouJxEdge(int i) => engine.JiaXuMouEntries.MouJxEdge(i);
        public bool MouJxFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.JiaXuMouEntries.MouJxFirstChild(f, child);
        public bool MouJxSuffix(int n) => engine.JiaXuMouEntries.MouJxSuffix(n);
        public NativeOwners.MouJiaXuFacts CreateMouJiaXuFacts() => engine.ComposeNativeMouJiaXuFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJiangJiContinuationAdapter(GameEngine engine) : IKernelMouJiangJiContinuation
    {
        public bool MouJjEdge(int i) => engine.JiangJiMouEntries.MouJjEdge(i);
        public bool MouJjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.JiangJiMouEntries.MouJjFirstChild(f, child);
        public bool MouJjSuffix(int n) => engine.JiangJiMouEntries.MouJjSuffix(n);
        public NativeOwners.MouJiangJiFacts CreateMouJiangJiFacts() => engine.ComposeNativeMouJiangJiFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJiangWeiAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouJiangWeiAnniversaryContinuation
    {
        public bool MouJiangWeiAnniversaryEdge(int i) => engine.JiangWeiMouEntries.Value.MouJiangWeiAnniversaryEdge(i);
        public bool MouJiangWeiAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.JiangWeiMouEntries.Value.MouJiangWeiAnniversaryFirstChild(f, child);
        public bool MouJiangWeiAnniversarySuffix(int n) => engine.JiangWeiMouEntries.Value.MouJiangWeiAnniversarySuffix(n);
        public NativeOwners.MouJiangWeiAnniversaryFacts CreateMouJiangWeiAnniversaryFacts() => engine.ComposeNativeMouJiangWeiAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJuShouContinuationAdapter(GameEngine engine) : IKernelMouJuShouContinuation
    {
        public bool MouJsEdge(int i) => engine.JuShouMouEntries.MouJsEdge(i);
        public bool MouJsFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.JuShouMouEntries.MouJsFirstChild(f, child);
        public bool MouJsSuffix(int n) => engine.JuShouMouEntries.MouJsSuffix(n);
        public NativeOwners.MouJuShouFacts CreateMouJuShouFacts() => engine.ComposeNativeMouJuShouFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouLiuXieContinuationAdapter(GameEngine engine) : IKernelMouLiuXieContinuation
    {
        public bool MouLiuXieNativeEdge(int i) => engine.LiuXieMouEntries.Value.MouLiuXieNativeEdge(i);
        public bool MouLiuXieFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => engine.LiuXieMouEntries.Value.MouLiuXieFirstNativeChild(f, child);
        public bool MouLiuXieNativeSuffix(int n) => engine.LiuXieMouEntries.Value.MouLiuXieNativeSuffix(n);
        public NativeOwners.MouLiuXieFacts CreateMouLiuXieFacts() => engine.ComposeNativeMouLiuXieFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouLuSuAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouLuSuAnniversaryContinuation
    {
        public bool MouLuSuEdge(int i) => engine.LuSuMouEntries.Value.MouLuSuEdge(i);
        public bool MouLuSuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.LuSuMouEntries.Value.MouLuSuFirstChild(f, child);
        public bool MouLuSuSuffix(int n) => engine.LuSuMouEntries.Value.MouLuSuSuffix(n);
        public NativeOwners.MouLuSuAnniversaryFacts CreateMouLuSuAnniversaryFacts() => engine.ComposeNativeMouLuSuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouSmContinuationAdapter(GameEngine engine) : IKernelMouSmContinuation
    {
        public bool MouSmEdge(int i) => engine.SimaYiMouEntries.MouSmEdge(i);
        public bool MouSmFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.SimaYiMouEntries.MouSmFirstChild(f, child);
        public bool MouSmSuffix(int n) => engine.SimaYiMouEntries.MouSmSuffix(n);
        public NativeOwners.MouSmFacts CreateMouSmFacts() => engine.ComposeNativeMouSmFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouXuShengContinuationAdapter(GameEngine engine) : IKernelMouXuShengContinuation
    {
        public bool MouXuShengNativeEdge(int i) => engine.XuShengMouEntries.Value.MouXuShengNativeEdge(i);
        public bool MouXuShengFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => engine.XuShengMouEntries.Value.MouXuShengFirstNativeChild(f, child);
        public bool MouXuShengNativeSuffix(int n) => engine.XuShengMouEntries.Value.MouXuShengNativeSuffix(n);
        public NativeOwners.MouXuShengFacts CreateMouXuShengFacts() => engine.ComposeNativeMouXuShengFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouXunYuAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouXunYuAnniversaryContinuation
    {
        public bool MouXyEdge(int i) => engine.XunYuMouEntries.MouXyEdge(i);
        public bool MouXyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.XunYuMouEntries.MouXyFirstChild(f, child);
        public bool MouXySuffix(int n) => engine.XunYuMouEntries.MouXySuffix(n);
        public NativeOwners.MouXunYuAnniversaryFacts CreateMouXunYuAnniversaryFacts() => engine.ComposeNativeMouXunYuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhangHeContinuationAdapter(GameEngine engine) : IKernelMouZhangHeContinuation
    {
        public bool MouQiaobianNativeEdge(int i) => engine.ZhangHeMouMobileEntries.Value.MouQiaobianNativeEdge(i);
        public bool MouQiaobianFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => engine.ZhangHeMouMobileEntries.Value.MouQiaobianFirstNativeChild(f, child);
        public bool MouQiaobianNativeSuffix(int n) => engine.ZhangHeMouMobileEntries.Value.MouQiaobianNativeSuffix(n);
        public NativeOwners.MouZhangHeFacts CreateMouZhangHeFacts() => engine.ComposeNativeMouZhangHeFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhangXiuAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouZhangXiuAnniversaryContinuation
    {
        public bool MouZhangXiuAnniversaryEdge(int i) => engine.ZhangXiuMouEntries.MouZhangXiuAnniversaryEdge(i);
        public bool MouZhangXiuAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.ZhangXiuMouEntries.MouZhangXiuAnniversaryFirstChild(f, child);
        public bool MouZhangXiuAnniversarySuffix(int n) => engine.ZhangXiuMouEntries.MouZhangXiuAnniversarySuffix(n);
        public NativeOwners.MouZhangXiuAnniversaryFacts CreateMouZhangXiuAnniversaryFacts() => engine.ComposeNativeMouZhangXiuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhouYuContinuationAdapter(GameEngine engine) : IKernelMouZhouYuContinuation
    {
        public NativeOwners.MouZhouYuFacts CreateMouZhouYuFacts() => engine.ComposeNativeMouZhouYuFacts();
        public bool MouZyEdge(int i) => engine.ZhouYuMouEntries.MouZyEdge(i);
        public bool MouZyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.ZhouYuMouEntries.MouZyFirstChild(f, child);
        public bool MouZySuffix(int i) => engine.ZhouYuMouEntries.MouZySuffix(i);
    }

    [ProgramCapability]
    private sealed class KernelMouZhuRanContinuationAdapter(GameEngine engine) : IKernelMouZhuRanContinuation
    {
        public bool MouZhuRanNativeEdge(int i) => engine.ZhuRanMouMobileEntries.Value.MouZhuRanNativeEdge(i);
        public bool MouZhuRanFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => engine.ZhuRanMouMobileEntries.Value.MouZhuRanFirstNativeChild(f, child);
        public bool MouZhuRanNativeSuffix(int n) => engine.ZhuRanMouMobileEntries.Value.MouZhuRanNativeSuffix(n);
        public NativeOwners.MouZhuRanFacts CreateMouZhuRanFacts() => engine.ComposeNativeMouZhuRanFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhugeJinContinuationAdapter(GameEngine engine) : IKernelMouZhugeJinContinuation
    {
        public bool MouZhugeJinEdge(int i) => engine.ZhugeJinMouEntries.MouZhugeJinEdge(i);
        public bool MouZhugeJinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => engine.ZhugeJinMouEntries.MouZhugeJinFirstChild(f, child);
        public bool MouZhugeJinSuffix(int n) => engine.ZhugeJinMouEntries.MouZhugeJinSuffix(n);
        public NativeOwners.MouZhugeJinFacts CreateMouZhugeJinFacts() => engine.ComposeNativeMouZhugeJinFacts();
    }

    [ProgramCapability]
    private sealed class KernelNativeReturnsAccessAdapter(GameEngine engine) : IKernelNativeReturnsAccess
    {
        public ref ProgramNativeReturns? _nativeReturns => ref engine._nativeReturns;
    }

    [ProgramCapability]
    private sealed class KernelObservedSkillGrantRevisionsAccessAdapter(GameEngine engine) : IKernelObservedSkillGrantRevisionsAccess
    {
        public ref readonly Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class KernelObserverIndexPredicateAdapter : IKernelObserverIndexPredicate
    {
        public bool Invoke(Func<int, bool> __nativeReceiver, int arg2) => __nativeReceiver.Invoke(arg2);
        public KernelObserverIndexPredicateAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class KernelObserversAccessAdapter(GameEngine engine) : IKernelObserversAccess
    {
        public ObserverDispatcher Observers { get => engine.Observers; }
    }

    [ProgramCapability]
    private sealed class KernelParticipantSetupAccessAdapter(GameEngine engine) : IKernelParticipantSetupAccess
    {
        public ref MatchParticipantSetup? _participantSetup => ref engine._participantSetup;
    }

    [ProgramCapability]
    private sealed class KernelPendingCardsMovedBatchesAccessAdapter(GameEngine engine) : IKernelPendingCardsMovedBatchesAccess
    {
        public ref readonly List<CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class KernelPhaseUsedProjectionsAccessAdapter(GameEngine engine) : IKernelPhaseUsedProjectionsAccess
    {
        public ref ProgramPhaseUsedProjections? _phaseUsedProjections => ref engine._phaseUsedProjections;
    }

    [ProgramCapability]
    private sealed class KernelPlayerViewQueryFactoriesAdapter(GameEngine engine) : IKernelPlayerViewQueryFactories
    {
        public NativeOwners.PlayerViewVisibilityQueries CreatePlayerViewVisibilityQueries() => engine.ComposeNativePlayerViewVisibilityQueries();
        public NativeOwners.PlayerViewCardsQueries CreatePlayerViewCardsQueries() => engine.ComposeNativePlayerViewCardsQueries();
        public NativeOwners.PlayerViewRulesQueries CreatePlayerViewRulesQueries() => engine.ComposeNativePlayerViewRulesQueries();
        public NativeOwners.PlayerViewMatchQueries CreatePlayerViewMatchQueries() => engine.ComposeNativePlayerViewMatchQueries();
    }

    [ProgramCapability]
    private sealed class KernelProgramActionsAccessAdapter(GameEngine engine) : IKernelProgramActionsAccess
    {
        public ref ProgramActionRouter? _programActions => ref engine._programActions;
    }

    [ProgramCapability]
    private sealed class KernelProgramChildPredicateAdapter : IKernelProgramChildPredicate
    {
        public bool Invoke(Func<ProgramSkillFrame, ResolutionFrame, bool> __nativeReceiver, ProgramSkillFrame arg2, ResolutionFrame arg3) => __nativeReceiver.Invoke(arg2, arg3);
        public KernelProgramChildPredicateAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class KernelProgramHistoryAccessAdapter(GameEngine engine) : IKernelProgramHistoryAccess
    {
        public ref ProgramEventHistoryIndex? _programHistory => ref engine._programHistory;
    }

    [ProgramCapability]
    private sealed class KernelProgramInteractionsAccessAdapter(GameEngine engine) : IKernelProgramInteractionsAccess
    {
        public ref ProgramInteractionRouter? _programInteractions => ref engine._programInteractions;
    }

    [ProgramCapability]
    private sealed class KernelProgramIssuedTriggersAccessAdapter(GameEngine engine) : IKernelProgramIssuedTriggersAccess
    {
        public ref ProgramIssuedTriggerRuntime? _programIssuedTriggers => ref engine._programIssuedTriggers;
    }

    [ProgramCapability]
    private sealed class KernelProgramLegalityAccessAdapter(GameEngine engine) : IKernelProgramLegalityAccess
    {
        public ref ProgramLegalityRouter? _programLegality => ref engine._programLegality;
    }

    [ProgramCapability]
    private sealed class KernelProgramPhaseScheduleAccessAdapter(GameEngine engine) : IKernelProgramPhaseScheduleAccess
    {
        public ref ProgramPhaseSchedule? _programPhaseSchedule => ref engine._programPhaseSchedule;
    }

    [ProgramCapability]
    private sealed class KernelProgramPipelineHooksAccessAdapter(GameEngine engine) : IKernelProgramPipelineHooksAccess
    {
        public ref readonly ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class KernelProgramRuleObserversAccessAdapter(GameEngine engine) : IKernelProgramRuleObserversAccess
    {
        public ref readonly ProgramRuleObservers _programRuleObservers => ref engine._programRuleObservers;
    }

    [ProgramCapability]
    private sealed class KernelProgramSelectionQueriesAccessAdapter(GameEngine engine) : IKernelProgramSelectionQueriesAccess
    {
        public ref IProgramSelectionQueries? _programSelectionQueries => ref engine._programSelectionQueries;
    }

    [ProgramCapability]
    private sealed class KernelProgramSelectionValidatorsAccessAdapter(GameEngine engine) : IKernelProgramSelectionValidatorsAccess
    {
        public ref ProgramSelectionValidationRegistry? _programSelectionValidators => ref engine._programSelectionValidators;
    }

    [ProgramCapability]
    private sealed class KernelProgramSelectionsAccessAdapter(GameEngine engine) : IKernelProgramSelectionsAccess
    {
        public ref ProgramSelectionRegistry? _programSelections => ref engine._programSelections;
    }

    [ProgramCapability]
    private sealed class KernelProgramTargetResolversAccessAdapter(GameEngine engine) : IKernelProgramTargetResolversAccess
    {
        public ref ProgramTargetResolverRegistry? _programTargetResolvers => ref engine._programTargetResolvers;
    }

    [ProgramCapability]
    private sealed class KernelProgramTriggerProofsAccessAdapter(GameEngine engine) : IKernelProgramTriggerProofsAccess
    {
        public ref ProgramTriggerProofRuntime? _programTriggerProofs => ref engine._programTriggerProofs;
    }

    [ProgramCapability]
    private sealed class KernelProgramWindowReturnOperationsAdapter(GameEngine engine) : IKernelProgramWindowReturnOperations
    {
        public void Return(ProgramWindowReturnCatalog __nativeReceiver, ProgramSkillFrame frame, bool completed) => __nativeReceiver.Return(engine, frame, completed);
    }

    [ProgramCapability]
    private sealed class KernelPublicHandRevealReturnOperationsAdapter(GameEngine engine) : IKernelPublicHandRevealReturnOperations
    {
        public void Return(PublicHandRevealReturns __nativeReceiver, string key, ResolutionFrame parent) => __nativeReceiver.Return(engine, key, parent);
    }

    [ProgramCapability]
    private sealed class KernelQinglongContinuationHandleFactoryAdapter(GameEngine engine) : IKernelQinglongContinuationHandleFactory
    {
        public NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
        public NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId) => engine.ComposeNativeBorrowedSwordHandle(ownerId);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelRejectOperationsAdapter(GameEngine engine) : IKernelRejectOperations
    {
        public CommandResult Reject(CommandErrorCode code, string message) => engine.Reject(code, message);
    }

    [ProgramCapability]
    private sealed class KernelResolutionSchedulerAccessAdapter(GameEngine engine) : IKernelResolutionSchedulerAccess
    {
        public ref readonly ResolutionScheduler _resolutionScheduler => ref engine._resolutionScheduler;
    }

    [ProgramCapability]
    private sealed class KernelResolveRuntimeHandlerTargetOperationsAdapter(GameEngine engine) : IKernelResolveRuntimeHandlerTargetOperations
    {
        public object? ResolveRuntimeHandlerTarget(System.Reflection.MethodInfo method) => engine.ResolveRuntimeHandlerTarget(method);
    }

    [ProgramCapability]
    private sealed class KernelReturnOperationOperationsAdapter(GameEngine engine) : IKernelReturnOperationOperations
    {
        public void ReturnOperation(SkillProgramEffectOp operation, ProgramSkillFrame frame, NativeDrawOutcome outcome)
        {
            var method = CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value[operation];
            ((Action<ProgramSkillFrame, NativeDrawOutcome>)engine.BindRuntimeHandler(method, typeof(Action<ProgramSkillFrame, NativeDrawOutcome>)))(frame, outcome);
        }
    }

    [ProgramCapability]
    private sealed class KernelRevisionAccessAdapter(GameEngine engine) : IKernelRevisionAccess
    {
        public long _revision { get => engine._revision; }
    }

    [ProgramCapability]
    private sealed class KernelRuleProgressionOperationsAdapter(GameEngine engine) : IKernelRuleProgressionOperations
    {
        public void AdvanceRulesAndPublishState() => engine.AdvanceRulesAndPublishState();
        public void AdvanceEventRulesAndQueueFact(IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class KernelRuleQueryFactFactoriesAdapter(GameEngine engine) : IKernelRuleQueryFactFactories
    {
        public NativeOwners.DistanceQueryFacts CreateDistanceQueryFacts(CharacterState source, CharacterState target) => engine.ComposeNativeDistanceQueryFacts(source, target);
        public NativeOwners.AttackRangeQueryFacts CreateAttackRangeQueryFacts(CharacterState player) => engine.ComposeNativeAttackRangeQueryFacts(player);
        public NativeOwners.DrawCountQueryFacts CreateDrawCountQueryFacts(CharacterState player) => engine.ComposeNativeDrawCountQueryFacts(player);
        public NativeOwners.SlashLimitQueryFacts CreateSlashLimitQueryFacts(CharacterState player) => engine.ComposeNativeSlashLimitQueryFacts(player);
        public NativeOwners.HandLimitQueryFacts CreateHandLimitQueryFacts(CharacterState player) => engine.ComposeNativeHandLimitQueryFacts(player);
        public NativeOwners.CardTargetCountQueryFacts CreateCardTargetCountQueryFacts(CharacterState player, CardKind effectiveCardKind, long? declaredUseFrameId) => engine.ComposeNativeCardTargetCountQueryFacts(player, effectiveCardKind, declaredUseFrameId);
        public NativeOwners.CardUseDistanceQueryFacts CreateCardUseDistanceQueryFacts(CharacterState player, CardKind effectiveCardKind) => engine.ComposeNativeCardUseDistanceQueryFacts(player, effectiveCardKind);
        public NativeOwners.NumericContributionQueryFacts CreateNumericContributionQueryFacts(CharacterState player, SkillRuleQuery ruleQuery) => engine.ComposeNativeNumericContributionQueryFacts(player, ruleQuery);
    }

    [ProgramCapability]
    private sealed class KernelRuntimeHooksAccessAdapter(GameEngine engine) : IKernelRuntimeHooksAccess
    {
        public ref ProgramRuntimeHooks? _runtimeHooks => ref engine._runtimeHooks;
    }

    [ProgramCapability]
    private sealed class KernelShanheMarketAccessAdapter(GameEngine engine) : IKernelShanheMarketAccess
    {
        public ref Campaigns.ShanheCampaignSession? _shanheMarket => ref engine._shanheMarket;
    }

    [ProgramCapability]
    private sealed class KernelSkillBindingIndexAccessAdapter(GameEngine engine) : IKernelSkillBindingIndexAccess
    {
        public ref readonly MatchSkillBindingIndex _skillBindingIndex => ref engine._skillBindingIndex;
    }

    [ProgramCapability]
    private sealed class KernelSkillRuntimeStateAccessAdapter(GameEngine engine) : IKernelSkillRuntimeStateAccess
    {
        public ref readonly SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class KernelSuppressedCardMovedEventBatchIdsAccessAdapter(GameEngine engine) : IKernelSuppressedCardMovedEventBatchIdsAccess
    {
        public ref readonly Stack<long> _suppressedCardMovedEventBatchIds => ref engine._suppressedCardMovedEventBatchIds;
    }

    [ProgramCapability]
    private sealed class KernelTargetRulesAccessAdapter(GameEngine engine) : IKernelTargetRulesAccess
    {
        public ref ProgramTargetRules? _targetRules => ref engine._targetRules;
    }

    [ProgramCapability]
    private sealed class KernelTargetWindowProtocolsAccessAdapter(GameEngine engine) : IKernelTargetWindowProtocolsAccess
    {
        public ref ProgramTargetWindowProtocols? _targetWindowProtocols => ref engine._targetWindowProtocols;
    }

    [ProgramCapability]
    private sealed class KernelTrickEffectHandleFactoryAdapter(GameEngine engine) : IKernelTrickEffectHandleFactory
    {
        public NativeOwners.GroupCardHandle CreateGroupCardHandle(long resolutionId, int sourceSeat, Card card, IReadOnlyList<int> targetSeats, GroupCardEffect effect, CardKind? requiredCardKind, IReadOnlyList<Card>? physicalCards = default) => engine.ComposeNativeGroupCardHandle(resolutionId, sourceSeat, card, targetSeats, effect, requiredCardKind, physicalCards);
        public NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long resolutionId, int sourceSeat, int weaponOwnerSeat, int slashTargetSeat, Card card) => engine.ComposeNativeBorrowedSwordHandle(resolutionId, sourceSeat, weaponOwnerSeat, slashTargetSeat, card);
        public NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, DamageNature? damageNatureOverride = default, IReadOnlyList<Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public NativeOwners.DuelHandle CreateDuelHandle(NativeOwners.CardAttackHandle attack) => engine.ComposeNativeDuelHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelTriggerWindowHostFactoriesAdapter(GameEngine engine) : IKernelTriggerWindowHostFactories
    {
        public NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost() => engine.ComposeNativeCardsMovedTriggerWindowHost();
        public NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost() => engine.ComposeNativeHpChangedTriggerWindowHost();
        public NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost() => engine.ComposeNativeCardActionTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelTryAdvanceOperationsAdapter(GameEngine engine) : IKernelTryAdvanceOperations
    {
        public bool TryAdvance(ResolutionFrame frame) => engine.GetRuntimeModule<CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.TryAdvance(engine, frame);
    }

    [ProgramCapability]
    private sealed class KernelTryResumeOperationsAdapter(GameEngine engine) : IKernelTryResumeOperations
    {
        public bool TryResume(ProgramContinuationCatalog __nativeReceiver, long frameId) => __nativeReceiver.TryResume(engine, frameId);
    }

    [ProgramCapability]
    private sealed class KernelTryReturnMovementOperationsAdapter(GameEngine engine) : IKernelTryReturnMovementOperations
    {
        public bool TryReturnMovement(ProgramContinuationCatalog __nativeReceiver, ProgramSkillFrame frame, bool afterPayment = false) => __nativeReceiver.TryReturnMovement(engine, frame, afterPayment);
    }

    [ProgramCapability]
    private sealed class KernelTurnCardUseEffectsAccessAdapter(GameEngine engine) : IKernelTurnCardUseEffectsAccess
    {
        public ref readonly TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class KernelViewContributorsAccessAdapter(GameEngine engine) : IKernelViewContributorsAccess
    {
        public ref ProgramViewContributors? _viewContributors => ref engine._viewContributors;
    }

}
