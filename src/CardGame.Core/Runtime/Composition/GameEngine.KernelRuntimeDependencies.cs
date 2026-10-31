#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Core.EngineOwners;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class KernelAcceptAndRejectOperationsAdapter(GameEngine engine) : IKernelAcceptAndRejectOperations
    {
        public CardGame.Core.CommandResult Reject(CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
        public CardGame.Core.CommandResult Accept(System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class KernelAcceptOperationsAdapter(GameEngine engine) : IKernelAcceptOperations
    {
        public CardGame.Core.CommandResult Accept(System.Action operation) => engine.Accept(operation);
    }

    [ProgramCapability]
    private sealed class KernelActiveCardMovementBatchIdsAccessAdapter(GameEngine engine) : IKernelActiveCardMovementBatchIdsAccess
    {
        public ref readonly System.Collections.Generic.Stack<long> _activeCardMovementBatchIds => ref engine._activeCardMovementBatchIds;
    }

    [ProgramCapability]
    private sealed class KernelActivityRuleFlowFactoriesAdapter(GameEngine engine) : IKernelActivityRuleFlowFactories
    {
        public CardGame.Core.NativeOwners.ActivityBattleRulesHost CreateActivityBattleRulesHost() => engine.ComposeNativeActivityBattleRulesHost();
        public CardGame.Core.NativeOwners.WenheRuleFlowHost CreateWenheRuleFlowHost() => engine.ComposeNativeWenheRuleFlowHost();
        public CardGame.Core.NativeOwners.GuanduEventFlowHost CreateGuanduEventFlowHost() => engine.ComposeNativeGuanduEventFlowHost();
    }

    [ProgramCapability]
    private sealed class KernelAdvanceEventRulesAndQueueFactOperationsAdapter(GameEngine engine) : IKernelAdvanceEventRulesAndQueueFactOperations
    {
        public void AdvanceEventRulesAndQueueFact(CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
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
        public ref CardGame.Core.IMatchModeAllegiancePolicy? _allegiancePolicy => ref engine._allegiancePolicy;
    }

    [ProgramCapability]
    private sealed class KernelBindRuntimeHandlerOperationsAdapter(GameEngine engine) : IKernelBindRuntimeHandlerOperations
    {
        public System.Delegate BindRuntimeHandler(System.Reflection.MethodInfo method, System.Type delegateType) => engine.BindRuntimeHandler(method, delegateType);
    }

    [ProgramCapability]
    private sealed class KernelCaoXianContinuationAdapter(GameEngine engine) : IKernelCaoXianContinuation
    {
        public bool CaoXianEdge(int i) => engine.CaoXianOlEntries.CaoXianEdge(i);
        public bool CaoXianFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.CaoXianOlEntries.CaoXianFirstChild(f, child);
        public bool CaoXianSuffix(int n) => engine.CaoXianOlEntries.CaoXianSuffix(n);
        public CardGame.Core.NativeOwners.CaoXianFacts CreateCaoXianFacts() => engine.ComposeNativeCaoXianFacts();
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFactionRequestHandlesFactoryAdapter(GameEngine engine) : IKernelCardAttackAndFactionRequestHandlesFactory
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardGame.Core.CardKind requiredKind = (CardGame.Core.CardKind)0, CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFactionRequestHandlesFactoryAndLookupAdapter(GameEngine engine) : IKernelCardAttackAndFactionRequestHandlesFactoryAndLookup
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
        public CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardGame.Core.CardKind requiredKind = (CardGame.Core.CardKind)0, CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFactionRequestHandlesLookupAdapter(GameEngine engine) : IKernelCardAttackAndFactionRequestHandlesLookup
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
        public CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFangtianHandlesFactoryAdapter(GameEngine engine) : IKernelCardAttackAndFangtianHandlesFactory
    {
        public CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, CardGame.Core.Card card, CardGame.Core.CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, System.Collections.Generic.IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardGame.Core.CardConversionSource? conversionSource = default) => engine.ComposeNativeFangtianHalberdHandle(resolutionId, sourceSeat, card, effectiveCardKind, ignoresArmor, damageAmount, targetSeats, usesFangtian, countedTowardSlashLimit, conversionSource);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackAndFangtianHandlesLookupAdapter(GameEngine engine) : IKernelCardAttackAndFangtianHandlesLookup
    {
        public CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackHandleFactoryAdapter(GameEngine engine) : IKernelCardAttackHandleFactory
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
    }

    [ProgramCapability]
    private sealed class KernelCardAttackHandleLookupAdapter(GameEngine engine) : IKernelCardAttackHandleLookup
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardContinuationHandleFactoryAdapter(GameEngine engine) : IKernelCardContinuationHandleFactory
    {
        public CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId) => engine.ComposeNativeDuelHandle(ownerId);
        public CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId) => engine.ComposeNativeGroupCardHandle(ownerId);
        public CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId) => engine.ComposeNativeBorrowedSwordHandle(ownerId);
        public CardGame.Core.NativeOwners.StoneAxeHandle CreateStoneAxeHandle(long ownerId) => engine.ComposeNativeStoneAxeHandle(ownerId);
        public CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(long ownerId) => engine.ComposeNativeCixiongDoubleSwordsHandle(ownerId);
        public CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(long ownerId) => engine.ComposeNativeQinglongCrescentBladeHandle(ownerId);
        public CardGame.Core.NativeOwners.QinglongFollowupHandle CreateQinglongFollowupHandle(long ownerId) => engine.ComposeNativeQinglongFollowupHandle(ownerId);
        public CardGame.Core.NativeOwners.IceSwordHandle CreateIceSwordHandle(long ownerId) => engine.ComposeNativeIceSwordHandle(ownerId);
        public CardGame.Core.NativeOwners.QilinBowHandle CreateQilinBowHandle(long ownerId) => engine.ComposeNativeQilinBowHandle(ownerId);
        public CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(long ownerId) => engine.ComposeNativeFactionDefenseHandle(ownerId);
        public CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCardLegalityAccessAdapter(GameEngine engine) : IKernelCardLegalityAccess
    {
        public ref CardGame.Core.ProgramCardLegality? _cardLegality => ref engine._cardLegality;
    }

    [ProgramCapability]
    private sealed class KernelCardUsePaymentsAccessAdapter(GameEngine engine) : IKernelCardUsePaymentsAccess
    {
        public ref CardGame.Core.ProgramCardUsePayments? _cardUsePayments => ref engine._cardUsePayments;
    }

    [ProgramCapability]
    private sealed class KernelCardZoneStoreAccessAdapter(GameEngine engine) : IKernelCardZoneStoreAccess
    {
        public ref readonly CardGame.Core.CardZoneStore _cardZones => ref engine._cardZones;
    }

    [ProgramCapability]
    private sealed class KernelCommandEventJournalAccessAdapter(GameEngine engine) : IKernelCommandEventJournalAccess
    {
        public ref CardGame.Core.CommandEventJournal? _commandEventJournal => ref engine._commandEventJournal;
    }

    [ProgramCapability]
    private sealed class KernelCommandRuntimeAccessAdapter(GameEngine engine) : IKernelCommandRuntimeAccess
    {
        public CardGame.Core.ICommandSessionRuntime CommandRuntime { get => engine.CommandRuntime; }
    }

    [ProgramCapability]
    private sealed class KernelCommandRuntimeAndRevisionAccessAdapter(GameEngine engine) : IKernelCommandRuntimeAndRevisionAccess
    {
        public long _revision { get => engine._revision; }
        public CardGame.Core.ICommandSessionRuntime CommandRuntime { get => engine.CommandRuntime; }
    }

    [ProgramCapability]
    private sealed class KernelCommandSessionAccessAdapter(GameEngine engine) : IKernelCommandSessionAccess
    {
        public ref readonly CardGame.Core.CommandSession _commandSession => ref engine._commandSession;
    }

    [ProgramCapability]
    private sealed class KernelCompleteGameOperationsAdapter(GameEngine engine) : IKernelCompleteGameOperations
    {
        public void CompleteGame() => engine.CompleteGame();
    }

    [ProgramCapability]
    private sealed class KernelContentRegistryAccessAdapter(GameEngine engine) : IKernelContentRegistryAccess
    {
        public ref readonly CardGame.Core.ContentRegistry _contentRegistry => ref engine._contentRegistry;
    }

    [ProgramCapability]
    private sealed class KernelCreateActionQueryScopeOperationsAdapter(GameEngine engine) : IKernelCreateActionQueryScopeOperations
    {
        public CardGame.Core.NativeOwners.ActionQueryScope CreateActionQueryScope(CardGame.Core.NativeOwners.ActionQuery? previous = default) => engine.ComposeNativeActionQueryScope(previous);
    }

    [ProgramCapability]
    private sealed class KernelCreateActivePlayerEffectQueriesOperationsAdapter(GameEngine engine) : IKernelCreateActivePlayerEffectQueriesOperations
    {
        public CardGame.Core.NativeOwners.ActivePlayerEffectQueries CreateActivePlayerEffectQueries() => engine.ComposeNativeActivePlayerEffectQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateBorrowedSwordHandleAndCreateCardAttackHandleOperationsAdapter(GameEngine engine) : IKernelCreateBorrowedSwordHandleAndCreateCardAttackHandleOperations
    {
        public CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId) => engine.ComposeNativeBorrowedSwordHandle(ownerId);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardActionTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateCardActionTriggerWindowHostOperations
    {
        public CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost() => engine.ComposeNativeCardActionTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateCardAttackHandle2AndCreateDuelHandleOperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateCardAttackHandle2AndCreateDuelHandleOperations
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeDuelHandle(attack);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateCardAttackHandle2OperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateCardAttackHandle2Operations
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle2(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateDuelHandleAndCreateGroupCardHandleOperations
    {
        public CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(long ownerId) => engine.ComposeNativeDuelHandle(ownerId);
        public CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long ownerId) => engine.ComposeNativeGroupCardHandle(ownerId);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardAttackHandleAndCreateDuelHandleOperationsAdapter(GameEngine engine) : IKernelCreateCardAttackHandleAndCreateDuelHandleOperations
    {
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeDuelHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateCardsMovedTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateCardsMovedTriggerWindowHostOperations
    {
        public CardGame.Core.NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost() => engine.ComposeNativeCardsMovedTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateCixiongDoubleSwordsHandleOperationsAdapter(GameEngine engine) : IKernelCreateCixiongDoubleSwordsHandleOperations
    {
        public CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle CreateCixiongDoubleSwordsHandle(CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeCixiongDoubleSwordsHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateDamageTriggerCapturedAttackOperationsAdapter(GameEngine engine) : IKernelCreateDamageTriggerCapturedAttackOperations
    {
        public CardGame.Core.NativeOwners.DamageTriggerCapturedAttack CreateDamageTriggerCapturedAttack(CardGame.Core.GameEngine.IDamageAttempt attack) => engine.ComposeNativeDamageTriggerCapturedAttack(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateDouyuTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateDouyuTriggerWindowHostOperations
    {
        public CardGame.Core.NativeOwners.DouyuTriggerWindowHost CreateDouyuTriggerWindowHost() => engine.ComposeNativeDouyuTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateFactionCardRequestHandleOperationsAdapter(GameEngine engine) : IKernelCreateFactionCardRequestHandleOperations
    {
        public CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long resolutionId, CardGame.Core.FactionCardRequestPurpose purpose, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, string providerFactionId, CardGame.Core.NativeOwners.CardAttackHandle? responseAttack = default, int? targetSeat = default, long? programSkillFrameId = default, CardGame.Core.CardKind requiredKind = (CardGame.Core.CardKind)0, CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? qinglongCrescentBlade = default, CardGame.Core.FactionResponsePolicySource? policySource = default, string? assistedResultBind = default) => engine.ComposeNativeFactionCardRequestHandle(resolutionId, purpose, ownerSeat, candidateSeats, providerFactionId, responseAttack, targetSeat, programSkillFrameId, requiredKind, borrowedSword, qinglongCrescentBlade, policySource, assistedResultBind);
    }

    [ProgramCapability]
    private sealed class KernelCreateFactionDefenseHandleOperationsAdapter(GameEngine engine) : IKernelCreateFactionDefenseHandleOperations
    {
        public CardGame.Core.NativeOwners.FactionDefenseHandle CreateFactionDefenseHandle(CardGame.Core.NativeOwners.CardAttackHandle attack, int ownerSeat, System.Collections.Generic.IReadOnlyList<int> candidateSeats, CardGame.Core.FactionResponsePolicySource source) => engine.ComposeNativeFactionDefenseHandle(attack, ownerSeat, candidateSeats, source);
    }

    [ProgramCapability]
    private sealed class KernelCreateFangtianHalberdHandleOperationsAdapter(GameEngine engine) : IKernelCreateFangtianHalberdHandleOperations
    {
        public CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, CardGame.Core.Card card, CardGame.Core.CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, System.Collections.Generic.IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, CardGame.Core.CardConversionSource? conversionSource = default) => engine.ComposeNativeFangtianHalberdHandle(resolutionId, sourceSeat, card, effectiveCardKind, ignoresArmor, damageAmount, targetSeats, usesFangtian, countedTowardSlashLimit, conversionSource);
    }

    [ProgramCapability]
    private sealed class KernelCreateHellEncounterHostOperationsAdapter(GameEngine engine) : IKernelCreateHellEncounterHostOperations
    {
        public CardGame.Core.NativeOwners.HellEncounterHost CreateHellEncounterHost() => engine.ComposeNativeHellEncounterHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateHpChangedTriggerWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateHpChangedTriggerWindowHostOperations
    {
        public CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost() => engine.ComposeNativeHpChangedTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateIceSwordHandleOperationsAdapter(GameEngine engine) : IKernelCreateIceSwordHandleOperations
    {
        public CardGame.Core.NativeOwners.IceSwordHandle CreateIceSwordHandle(CardGame.Core.NativeOwners.CardAttackHandle attack, int preventedDamageAmount) => engine.ComposeNativeIceSwordHandle(attack, preventedDamageAmount);
    }

    [ProgramCapability]
    private sealed class KernelCreateMoSimaYiFactsOperationsAdapter(GameEngine engine) : IKernelCreateMoSimaYiFactsOperations
    {
        public CardGame.Core.NativeOwners.MoSimaYiFacts CreateMoSimaYiFacts() => engine.ComposeNativeMoSimaYiFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateModeEncounterHostOperationsAdapter(GameEngine engine) : IKernelCreateModeEncounterHostOperations
    {
        public CardGame.Core.NativeOwners.ModeEncounterHost CreateModeEncounterHost() => engine.ComposeNativeModeEncounterHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateModeNationalRevealHostOperationsAdapter(GameEngine engine) : IKernelCreateModeNationalRevealHostOperations
    {
        public CardGame.Core.NativeOwners.ModeNationalRevealHost CreateModeNationalRevealHost() => engine.ComposeNativeModeNationalRevealHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateModeSetupEquipmentHostOperationsAdapter(GameEngine engine) : IKernelCreateModeSetupEquipmentHostOperations
    {
        public CardGame.Core.NativeOwners.ModeSetupEquipmentHost CreateModeSetupEquipmentHost() => engine.ComposeNativeModeSetupEquipmentHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateMouGuanPingFactsOperationsAdapter(GameEngine engine) : IKernelCreateMouGuanPingFactsOperations
    {
        public CardGame.Core.NativeOwners.MouGuanPingFacts CreateMouGuanPingFacts() => engine.ComposeNativeMouGuanPingFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateMouHuangGaiFactsAndMouHgFirstChildOperationsAdapter(GameEngine engine) : IKernelCreateMouHuangGaiFactsAndMouHgFirstChildOperations
    {
        public bool MouHgFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.HuangGaiMouEntries.MouHgFirstChild(f, child);
        public CardGame.Core.NativeOwners.MouHuangGaiFacts CreateMouHuangGaiFacts() => engine.ComposeNativeMouHuangGaiFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateMouSunShangxiangFactsOperationsAdapter(GameEngine engine) : IKernelCreateMouSunShangxiangFactsOperations
    {
        public CardGame.Core.NativeOwners.MouSunShangxiangFacts CreateMouSunShangxiangFacts() => engine.ComposeNativeMouSunShangxiangFacts();
    }

    [ProgramCapability]
    private sealed class KernelCreateMovementArmorOwnerOperationsAdapter(GameEngine engine) : IKernelCreateMovementArmorOwnerOperations
    {
        public CardGame.Core.NativeOwners.MovementArmorOwner CreateMovementArmorOwner(CardGame.Core.CharacterState owner) => engine.ComposeNativeMovementArmorOwner(owner);
    }

    [ProgramCapability]
    private sealed class KernelCreateNianChallengeHostOperationsAdapter(GameEngine engine) : IKernelCreateNianChallengeHostOperations
    {
        public CardGame.Core.NativeOwners.NianChallengeHost CreateNianChallengeHost() => engine.ComposeNativeNianChallengeHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateParticipantSetupContextOperationsAdapter(GameEngine engine) : IKernelCreateParticipantSetupContextOperations
    {
        public CardGame.Core.NativeOwners.ParticipantSetupContext CreateParticipantSetupContext() => engine.ComposeNativeParticipantSetupContext();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramAttackHandleOperationsAdapter(GameEngine engine) : IKernelCreateProgramAttackHandleOperations
    {
        public CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId) => engine.ComposeNativeProgramAttackHandle(ownerFrameId);
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramExpressionWorldOperationsAdapter(GameEngine engine) : IKernelCreateProgramExpressionWorldOperations
    {
        public CardGame.Core.NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(CardGame.Core.ProgramSkillFrame frame) => engine.ComposeNativeProgramExpressionWorld(frame);
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramHealthChildQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramHealthChildQueriesOperations
    {
        public CardGame.Core.NativeOwners.ProgramHealthChildQueries CreateProgramHealthChildQueries() => engine.ComposeNativeProgramHealthChildQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramLegalityRouterOperationsAdapter(GameEngine engine) : IKernelCreateProgramLegalityRouterOperations
    {
        public CardGame.Core.ProgramLegalityRouter CreateProgramLegalityRouter(CardGame.Core.ProgramLegalityCatalog catalog, System.Func<System.Reflection.MethodInfo, object?>? resolveTarget = default) => new CardGame.Core.ProgramLegalityRouter(catalog, engine, resolveTarget);
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramSkillHostOperationsAdapter(GameEngine engine) : IKernelCreateProgramSkillHostOperations
    {
        public CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost() => engine.ComposeNativeProgramSkillHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramTriggerActionFactQueriesAndCreateProgramTriggerFactQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramTriggerActionFactQueriesAndCreateProgramTriggerFactQueriesOperations
    {
        public CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries() => engine.ComposeNativeProgramTriggerFactQueries();
        public CardGame.Core.NativeOwners.ProgramTriggerActionFactQueries CreateProgramTriggerActionFactQueries() => engine.ComposeNativeProgramTriggerActionFactQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramTriggerFactQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramTriggerFactQueriesOperations
    {
        public CardGame.Core.NativeOwners.ProgramTriggerFactQueries CreateProgramTriggerFactQueries() => engine.ComposeNativeProgramTriggerFactQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateProgramTriggerQueriesOperationsAdapter(GameEngine engine) : IKernelCreateProgramTriggerQueriesOperations
    {
        public CardGame.Core.NativeOwners.ProgramTriggerQueries CreateProgramTriggerQueries() => engine.ComposeNativeProgramTriggerQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateQilinBowHandleOperationsAdapter(GameEngine engine) : IKernelCreateQilinBowHandleOperations
    {
        public CardGame.Core.NativeOwners.QilinBowHandle CreateQilinBowHandle(CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeQilinBowHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateQinglongCrescentBladeHandleOperationsAdapter(GameEngine engine) : IKernelCreateQinglongCrescentBladeHandleOperations
    {
        public CardGame.Core.NativeOwners.QinglongCrescentBladeHandle CreateQinglongCrescentBladeHandle(CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeQinglongCrescentBladeHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelCreateRuleProgressionOperationsOperationsAdapter(GameEngine engine) : IKernelCreateRuleProgressionOperationsOperations
    {
        public CardGame.Core.NativeOwners.RuleProgressionOperations CreateRuleProgressionOperations() => engine.ComposeNativeRuleProgressionOperations();
    }

    [ProgramCapability]
    private sealed class KernelCreateSelectionQueriesOperationsAdapter(GameEngine engine) : IKernelCreateSelectionQueriesOperations
    {
        public CardGame.Core.NativeOwners.SelectionQueries CreateSelectionQueries() => engine.ComposeNativeSelectionQueries();
    }

    [ProgramCapability]
    private sealed class KernelCreateStandardBattleHostOperationsAdapter(GameEngine engine) : IKernelCreateStandardBattleHostOperations
    {
        public CardGame.Core.NativeOwners.StandardBattleHost CreateStandardBattleHost() => engine.ComposeNativeStandardBattleHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateStoneAxeHandleOperationsAdapter(GameEngine engine) : IKernelCreateStoneAxeHandleOperations
    {
        public CardGame.Core.NativeOwners.StoneAxeHandle CreateStoneAxeHandle(CardGame.Core.NativeOwners.CardAttackHandle attack, System.Collections.Generic.IReadOnlyList<int> candidateCardIds) => engine.ComposeNativeStoneAxeHandle(attack, candidateCardIds);
    }

    [ProgramCapability]
    private sealed class KernelCreateWuLuKangWindowHostOperationsAdapter(GameEngine engine) : IKernelCreateWuLuKangWindowHostOperations
    {
        public CardGame.Core.NativeOwners.WuLuKangWindowHost CreateWuLuKangWindowHost() => engine.ComposeNativeWuLuKangWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelCreateYingboGiftHandleOperationsAdapter(GameEngine engine) : IKernelCreateYingboGiftHandleOperations
    {
        public CardGame.Core.NativeOwners.YingboGiftHandle CreateYingboGiftHandle(long ownerId) => engine.ComposeNativeYingboGiftHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelDamageHooksAccessAdapter(GameEngine engine) : IKernelDamageHooksAccess
    {
        public ref CardGame.Core.ProgramDamageHooks? _damageHooks => ref engine._damageHooks;
    }

    [ProgramCapability]
    private sealed class KernelDistanceRulesAccessAdapter(GameEngine engine) : IKernelDistanceRulesAccess
    {
        public ref CardGame.Core.ProgramDistanceRules? _distanceRules => ref engine._distanceRules;
    }

    [ProgramCapability]
    private sealed class KernelDyingObserverPredicateAdapter : IKernelDyingObserverPredicate
    {
        public bool Invoke(System.Func<int, CardGame.Core.DyingFrame, bool>? __nativeReceiver, int arg2, CardGame.Core.DyingFrame arg3) => __nativeReceiver.Invoke(arg2, arg3);
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
        public ref CardGame.Core.EquipmentContentRegistry? _equipmentRules => ref engine._equipmentRules;
    }

    [ProgramCapability]
    private sealed class KernelFrameStoreAccessAdapter(GameEngine engine) : IKernelFrameStoreAccess
    {
        public ref readonly CardGame.Core.FrameStore _resolutionStack => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class KernelFrameStoreQueryAdapter(GameEngine engine) : IKernelFrameStoreQuery
    {
        public ref readonly CardGame.Core.FrameStore _resolutionStack() => ref engine._resolutionStack;
    }

    [ProgramCapability]
    private sealed class KernelMatchExecutionLoopAccessAdapter(GameEngine engine) : IKernelMatchExecutionLoopAccess
    {
        public ref readonly CardGame.Core.MatchExecutionLoop _matchExecutionLoop => ref engine._matchExecutionLoop;
    }

    [ProgramCapability]
    private sealed class KernelMatchModeRulesAccessAdapter(GameEngine engine) : IKernelMatchModeRulesAccess
    {
        public ref readonly CardGame.Core.ModeRuleSet _modeRules => ref engine._modeRules;
    }

    [ProgramCapability]
    private sealed class KernelMatchModeRuntimeAccessAdapter(GameEngine engine) : IKernelMatchModeRuntimeAccess
    {
        public ref readonly CardGame.Core.MatchModeRuntime _modeRuntime => ref engine._modeRuntime;
    }

    [ProgramCapability]
    private sealed class KernelMatchRandomAccessAdapter(GameEngine engine) : IKernelMatchRandomAccess
    {
        public ref readonly CardGame.Core.DeterministicRandom _random => ref engine._random;
    }

    [ProgramCapability]
    private sealed class KernelMatchesOperationsAdapter(GameEngine engine) : IKernelMatchesOperations
    {
        public bool Matches(CardGame.Core.NativeFrameAutomaticPhase phase, int index, CardGame.Core.ResolutionFrame frame) => engine.GetRuntimeModule<CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase][index].Matches(engine, frame);
    }

    [ProgramCapability]
    private sealed class KernelMouCaoHongContinuationAdapter(GameEngine engine) : IKernelMouCaoHongContinuation
    {
        public bool MouCaoHongEdge(int i) => engine.CaoHongMouEntries.MouCaoHongEdge(i);
        public bool MouCaoHongFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.CaoHongMouEntries.MouCaoHongFirstChild(f, child);
        public bool MouCaoHongSuffix(int n) => engine.CaoHongMouEntries.MouCaoHongSuffix(n);
        public CardGame.Core.NativeOwners.MouCaoHongFacts CreateMouCaoHongFacts() => engine.ComposeNativeMouCaoHongFacts();
        public bool CurrentMouCaoHongDebt(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ProgramSkillWindowContext w) => engine.ComposeNativeMouCaoHongFacts().CurrentMouCaoHongDebt(f, w);
        public CardGame.Core.MouCaoHongXianjuIssuedEvent? CurrentMouCaoHongDebt2(CardGame.Core.ProgramTriggerCandidate c, CardGame.Core.ProgramSkillWindowContext w) => engine.CaoHongMouEntries.CurrentMouCaoHongDebt(c, w);
    }

    [ProgramCapability]
    private sealed class KernelMouCaoShuangAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouCaoShuangAnniversaryContinuation
    {
        public bool MouCaoShuangAnniversaryEdge(int i) => engine.CaoShuangMouEntries.MouCaoShuangAnniversaryEdge(i);
        public bool MouCaoShuangAnniversaryFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.CaoShuangMouEntries.MouCaoShuangAnniversaryFirstChild(f, child);
        public bool MouCaoShuangAnniversarySuffix(int n) => engine.CaoShuangMouEntries.MouCaoShuangAnniversarySuffix(n);
        public CardGame.Core.NativeOwners.MouCaoShuangAnniversaryFacts CreateMouCaoShuangAnniversaryFacts() => engine.ComposeNativeMouCaoShuangAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouChenLinContinuationAdapter(GameEngine engine) : IKernelMouChenLinContinuation
    {
        public bool MouChenLinEdge(int i) => engine.ChenLinMouEntries.MouChenLinEdge(i);
        public bool MouChenLinFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.ChenLinMouEntries.MouChenLinFirstChild(f, child);
        public bool MouChenLinSuffix(int n) => engine.ChenLinMouEntries.MouChenLinSuffix(n);
        public CardGame.Core.NativeOwners.MouChenLinFacts CreateMouChenLinFacts() => engine.ComposeNativeMouChenLinFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouDianWeiContinuationAdapter(GameEngine engine) : IKernelMouDianWeiContinuation
    {
        public bool MouDianWeiEdge(int i) => engine.DianWeiMouEntries.MouDianWeiEdge(i);
        public bool MouDianWeiFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.DianWeiMouEntries.MouDianWeiFirstChild(f, child);
        public bool MouDianWeiSuffix(int n) => engine.DianWeiMouEntries.MouDianWeiSuffix(n);
        public CardGame.Core.NativeOwners.MouDianWeiFacts CreateMouDianWeiFacts() => engine.ComposeNativeMouDianWeiFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouDongChengContinuationAdapter(GameEngine engine) : IKernelMouDongChengContinuation
    {
        public bool MouDcEdge(int i) => engine.DongChengMouEntries.MouDcEdge(i);
        public bool MouDcFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.DongChengMouEntries.MouDcFirstChild(f, child);
        public bool MouDcSuffix(int i) => engine.DongChengMouEntries.MouDcSuffix(i);
        public CardGame.Core.NativeOwners.MouDongChengFacts CreateMouDongChengFacts() => engine.ComposeNativeMouDongChengFacts();
        public CardGame.Core.NativeOwners.MouDcLightningFacts CreateMouDcLightningFacts() => engine.ComposeNativeMouDcLightningFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouDongZhuoOlContinuationAdapter(GameEngine engine) : IKernelMouDongZhuoOlContinuation
    {
        public bool MouDongZhuoOlEdge(int i) => engine.DongZhuoMouOlEntries.MouDongZhuoOlEdge(i);
        public bool MouDongZhuoOlFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.DongZhuoMouOlEntries.MouDongZhuoOlFirstChild(f, child);
        public bool MouDongZhuoOlSuffix(int n) => engine.DongZhuoMouOlEntries.MouDongZhuoOlSuffix(n);
        public CardGame.Core.NativeOwners.MouDongZhuoOlFacts CreateMouDongZhuoOlFacts() => engine.ComposeNativeMouDongZhuoOlFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouGuoJiaContinuationAdapter(GameEngine engine) : IKernelMouGuoJiaContinuation
    {
        public bool MouGjEdge(int i) => engine.GuoJiaMouEntries.MouGjEdge(i);
        public bool MouGjFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.GuoJiaMouEntries.MouGjFirstChild(f, child);
        public bool MouGjSuffix(int n) => engine.GuoJiaMouEntries.MouGjSuffix(n);
        public CardGame.Core.NativeOwners.MouGuoJiaFacts CreateMouGuoJiaFacts() => engine.ComposeNativeMouGuoJiaFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouHuCheErContinuationAdapter(GameEngine engine) : IKernelMouHuCheErContinuation
    {
        public CardGame.Core.NativeOwners.MouHuCheErFacts CreateMouHuCheErFacts() => engine.ComposeNativeMouHuCheErFacts();
        public bool MouHuCheErEdge(int i) => engine.HuCheErMouEntries.MouHuCheErEdge(i);
        public bool MouHuCheErFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.HuCheErMouEntries.MouHuCheErFirstChild(f, child);
        public bool MouHuCheErSuffix(int n) => engine.HuCheErMouEntries.MouHuCheErSuffix(n);
    }

    [ProgramCapability]
    private sealed class KernelMouHuLieContinuationAdapter(GameEngine engine) : IKernelMouHuLieContinuation
    {
        public bool MouHuLieEdge(int i) => engine.HuLieMouEntries.MouHuLieEdge(i);
        public bool MouHuLieFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.HuLieMouEntries.MouHuLieFirstChild(f, child);
        public bool MouHuLieSuffix(int n) => engine.HuLieMouEntries.MouHuLieSuffix(n);
        public CardGame.Core.NativeOwners.MouHuLieFacts CreateMouHuLieFacts() => engine.ComposeNativeMouHuLieFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJiaXuContinuationAdapter(GameEngine engine) : IKernelMouJiaXuContinuation
    {
        public bool MouJxEdge(int i) => engine.JiaXuMouEntries.MouJxEdge(i);
        public bool MouJxFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.JiaXuMouEntries.MouJxFirstChild(f, child);
        public bool MouJxSuffix(int n) => engine.JiaXuMouEntries.MouJxSuffix(n);
        public CardGame.Core.NativeOwners.MouJiaXuFacts CreateMouJiaXuFacts() => engine.ComposeNativeMouJiaXuFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJiangJiContinuationAdapter(GameEngine engine) : IKernelMouJiangJiContinuation
    {
        public bool MouJjEdge(int i) => engine.JiangJiMouEntries.MouJjEdge(i);
        public bool MouJjFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.JiangJiMouEntries.MouJjFirstChild(f, child);
        public bool MouJjSuffix(int n) => engine.JiangJiMouEntries.MouJjSuffix(n);
        public CardGame.Core.NativeOwners.MouJiangJiFacts CreateMouJiangJiFacts() => engine.ComposeNativeMouJiangJiFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJiangWeiAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouJiangWeiAnniversaryContinuation
    {
        public bool MouJiangWeiAnniversaryEdge(int i) => engine.JiangWeiMouEntries.MouJiangWeiAnniversaryEdge(i);
        public bool MouJiangWeiAnniversaryFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.JiangWeiMouEntries.MouJiangWeiAnniversaryFirstChild(f, child);
        public bool MouJiangWeiAnniversarySuffix(int n) => engine.JiangWeiMouEntries.MouJiangWeiAnniversarySuffix(n);
        public CardGame.Core.NativeOwners.MouJiangWeiAnniversaryFacts CreateMouJiangWeiAnniversaryFacts() => engine.ComposeNativeMouJiangWeiAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouJuShouContinuationAdapter(GameEngine engine) : IKernelMouJuShouContinuation
    {
        public bool MouJsEdge(int i) => engine.JuShouMouEntries.MouJsEdge(i);
        public bool MouJsFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.JuShouMouEntries.MouJsFirstChild(f, child);
        public bool MouJsSuffix(int n) => engine.JuShouMouEntries.MouJsSuffix(n);
        public CardGame.Core.NativeOwners.MouJuShouFacts CreateMouJuShouFacts() => engine.ComposeNativeMouJuShouFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouLiuXieContinuationAdapter(GameEngine engine) : IKernelMouLiuXieContinuation
    {
        public bool MouLiuXieNativeEdge(int i) => engine.LiuXieMouEntries.MouLiuXieNativeEdge(i);
        public bool MouLiuXieFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.LiuXieMouEntries.MouLiuXieFirstNativeChild(f, child);
        public bool MouLiuXieNativeSuffix(int n) => engine.LiuXieMouEntries.MouLiuXieNativeSuffix(n);
        public CardGame.Core.NativeOwners.MouLiuXieFacts CreateMouLiuXieFacts() => engine.ComposeNativeMouLiuXieFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouLuSuAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouLuSuAnniversaryContinuation
    {
        public bool MouLuSuEdge(int i) => engine.LuSuMouEntries.MouLuSuEdge(i);
        public bool MouLuSuFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.LuSuMouEntries.MouLuSuFirstChild(f, child);
        public bool MouLuSuSuffix(int n) => engine.LuSuMouEntries.MouLuSuSuffix(n);
        public CardGame.Core.NativeOwners.MouLuSuAnniversaryFacts CreateMouLuSuAnniversaryFacts() => engine.ComposeNativeMouLuSuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouSmContinuationAdapter(GameEngine engine) : IKernelMouSmContinuation
    {
        public bool MouSmEdge(int i) => engine.SimaYiMouEntries.MouSmEdge(i);
        public bool MouSmFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.SimaYiMouEntries.MouSmFirstChild(f, child);
        public bool MouSmSuffix(int n) => engine.SimaYiMouEntries.MouSmSuffix(n);
        public CardGame.Core.NativeOwners.MouSmFacts CreateMouSmFacts() => engine.ComposeNativeMouSmFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouXuShengContinuationAdapter(GameEngine engine) : IKernelMouXuShengContinuation
    {
        public bool MouXuShengNativeEdge(int i) => engine.XuShengMouEntries.MouXuShengNativeEdge(i);
        public bool MouXuShengFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.XuShengMouEntries.MouXuShengFirstNativeChild(f, child);
        public bool MouXuShengNativeSuffix(int n) => engine.XuShengMouEntries.MouXuShengNativeSuffix(n);
        public CardGame.Core.NativeOwners.MouXuShengFacts CreateMouXuShengFacts() => engine.ComposeNativeMouXuShengFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouXunYuAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouXunYuAnniversaryContinuation
    {
        public bool MouXyEdge(int i) => engine.XunYuMouEntries.MouXyEdge(i);
        public bool MouXyFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.XunYuMouEntries.MouXyFirstChild(f, child);
        public bool MouXySuffix(int n) => engine.XunYuMouEntries.MouXySuffix(n);
        public CardGame.Core.NativeOwners.MouXunYuAnniversaryFacts CreateMouXunYuAnniversaryFacts() => engine.ComposeNativeMouXunYuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhangHeContinuationAdapter(GameEngine engine) : IKernelMouZhangHeContinuation
    {
        public bool MouQiaobianNativeEdge(int i) => engine.ZhangHeMouMobileEntries.MouQiaobianNativeEdge(i);
        public bool MouQiaobianFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.ZhangHeMouMobileEntries.MouQiaobianFirstNativeChild(f, child);
        public bool MouQiaobianNativeSuffix(int n) => engine.ZhangHeMouMobileEntries.MouQiaobianNativeSuffix(n);
        public CardGame.Core.NativeOwners.MouZhangHeFacts CreateMouZhangHeFacts() => engine.ComposeNativeMouZhangHeFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhangXiuAnniversaryContinuationAdapter(GameEngine engine) : IKernelMouZhangXiuAnniversaryContinuation
    {
        public bool MouZhangXiuAnniversaryEdge(int i) => engine.ZhangXiuMouEntries.MouZhangXiuAnniversaryEdge(i);
        public bool MouZhangXiuAnniversaryFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.ZhangXiuMouEntries.MouZhangXiuAnniversaryFirstChild(f, child);
        public bool MouZhangXiuAnniversarySuffix(int n) => engine.ZhangXiuMouEntries.MouZhangXiuAnniversarySuffix(n);
        public CardGame.Core.NativeOwners.MouZhangXiuAnniversaryFacts CreateMouZhangXiuAnniversaryFacts() => engine.ComposeNativeMouZhangXiuAnniversaryFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhouYuContinuationAdapter(GameEngine engine) : IKernelMouZhouYuContinuation
    {
        public CardGame.Core.NativeOwners.MouZhouYuFacts CreateMouZhouYuFacts() => engine.ComposeNativeMouZhouYuFacts();
        public bool MouZyEdge(int i) => engine.ZhouYuMouEntries.MouZyEdge(i);
        public bool MouZyFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.ZhouYuMouEntries.MouZyFirstChild(f, child);
        public bool MouZySuffix(int i) => engine.ZhouYuMouEntries.MouZySuffix(i);
    }

    [ProgramCapability]
    private sealed class KernelMouZhuRanContinuationAdapter(GameEngine engine) : IKernelMouZhuRanContinuation
    {
        public bool MouZhuRanNativeEdge(int i) => engine.ZhuRanMouMobileEntries.MouZhuRanNativeEdge(i);
        public bool MouZhuRanFirstNativeChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.ZhuRanMouMobileEntries.MouZhuRanFirstNativeChild(f, child);
        public bool MouZhuRanNativeSuffix(int n) => engine.ZhuRanMouMobileEntries.MouZhuRanNativeSuffix(n);
        public CardGame.Core.NativeOwners.MouZhuRanFacts CreateMouZhuRanFacts() => engine.ComposeNativeMouZhuRanFacts();
    }

    [ProgramCapability]
    private sealed class KernelMouZhugeJinContinuationAdapter(GameEngine engine) : IKernelMouZhugeJinContinuation
    {
        public bool MouZhugeJinEdge(int i) => engine.ZhugeJinMouEntries.MouZhugeJinEdge(i);
        public bool MouZhugeJinFirstChild(CardGame.Core.ProgramSkillFrame f, CardGame.Core.ResolutionFrame child) => engine.ZhugeJinMouEntries.MouZhugeJinFirstChild(f, child);
        public bool MouZhugeJinSuffix(int n) => engine.ZhugeJinMouEntries.MouZhugeJinSuffix(n);
        public CardGame.Core.NativeOwners.MouZhugeJinFacts CreateMouZhugeJinFacts() => engine.ComposeNativeMouZhugeJinFacts();
    }

    [ProgramCapability]
    private sealed class KernelNativeReturnsAccessAdapter(GameEngine engine) : IKernelNativeReturnsAccess
    {
        public ref CardGame.Core.ProgramNativeReturns? _nativeReturns => ref engine._nativeReturns;
    }

    [ProgramCapability]
    private sealed class KernelObservedSkillGrantRevisionsAccessAdapter(GameEngine engine) : IKernelObservedSkillGrantRevisionsAccess
    {
        public ref readonly System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions => ref engine._observedSkillGrantRevisions;
    }

    [ProgramCapability]
    private sealed class KernelObserverIndexPredicateAdapter : IKernelObserverIndexPredicate
    {
        public bool Invoke(System.Func<int, bool> __nativeReceiver, int arg2) => __nativeReceiver.Invoke(arg2);
        public KernelObserverIndexPredicateAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class KernelObserversAccessAdapter(GameEngine engine) : IKernelObserversAccess
    {
        public CardGame.Core.ObserverDispatcher Observers { get => engine.Observers; }
    }

    [ProgramCapability]
    private sealed class KernelParticipantSetupAccessAdapter(GameEngine engine) : IKernelParticipantSetupAccess
    {
        public ref CardGame.Core.MatchParticipantSetup? _participantSetup => ref engine._participantSetup;
    }

    [ProgramCapability]
    private sealed class KernelPendingCardsMovedBatchesAccessAdapter(GameEngine engine) : IKernelPendingCardsMovedBatchesAccess
    {
        public ref readonly System.Collections.Generic.List<CardGame.Core.CardMovementBatchContext> _pendingCardsMovedBatches => ref engine._pendingCardsMovedBatches;
    }

    [ProgramCapability]
    private sealed class KernelPhaseUsedProjectionsAccessAdapter(GameEngine engine) : IKernelPhaseUsedProjectionsAccess
    {
        public ref CardGame.Core.ProgramPhaseUsedProjections? _phaseUsedProjections => ref engine._phaseUsedProjections;
    }

    [ProgramCapability]
    private sealed class KernelPlayerViewQueryFactoriesAdapter(GameEngine engine) : IKernelPlayerViewQueryFactories
    {
        public CardGame.Core.NativeOwners.PlayerViewVisibilityQueries CreatePlayerViewVisibilityQueries() => engine.ComposeNativePlayerViewVisibilityQueries();
        public CardGame.Core.NativeOwners.PlayerViewCardsQueries CreatePlayerViewCardsQueries() => engine.ComposeNativePlayerViewCardsQueries();
        public CardGame.Core.NativeOwners.PlayerViewRulesQueries CreatePlayerViewRulesQueries() => engine.ComposeNativePlayerViewRulesQueries();
        public CardGame.Core.NativeOwners.PlayerViewMatchQueries CreatePlayerViewMatchQueries() => engine.ComposeNativePlayerViewMatchQueries();
    }

    [ProgramCapability]
    private sealed class KernelProgramActionsAccessAdapter(GameEngine engine) : IKernelProgramActionsAccess
    {
        public ref CardGame.Core.ProgramActionRouter? _programActions => ref engine._programActions;
    }

    [ProgramCapability]
    private sealed class KernelProgramChildPredicateAdapter : IKernelProgramChildPredicate
    {
        public bool Invoke(System.Func<CardGame.Core.ProgramSkillFrame, CardGame.Core.ResolutionFrame, bool> __nativeReceiver, CardGame.Core.ProgramSkillFrame arg2, CardGame.Core.ResolutionFrame arg3) => __nativeReceiver.Invoke(arg2, arg3);
        public KernelProgramChildPredicateAdapter(GameEngine engine)
        {
        }
    }

    [ProgramCapability]
    private sealed class KernelProgramHistoryAccessAdapter(GameEngine engine) : IKernelProgramHistoryAccess
    {
        public ref CardGame.Core.ProgramEventHistoryIndex? _programHistory => ref engine._programHistory;
    }

    [ProgramCapability]
    private sealed class KernelProgramInteractionsAccessAdapter(GameEngine engine) : IKernelProgramInteractionsAccess
    {
        public ref CardGame.Core.ProgramInteractionRouter? _programInteractions => ref engine._programInteractions;
    }

    [ProgramCapability]
    private sealed class KernelProgramIssuedTriggersAccessAdapter(GameEngine engine) : IKernelProgramIssuedTriggersAccess
    {
        public ref CardGame.Core.ProgramIssuedTriggerRuntime? _programIssuedTriggers => ref engine._programIssuedTriggers;
    }

    [ProgramCapability]
    private sealed class KernelProgramLegalityAccessAdapter(GameEngine engine) : IKernelProgramLegalityAccess
    {
        public ref CardGame.Core.ProgramLegalityRouter? _programLegality => ref engine._programLegality;
    }

    [ProgramCapability]
    private sealed class KernelProgramPhaseScheduleAccessAdapter(GameEngine engine) : IKernelProgramPhaseScheduleAccess
    {
        public ref CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule => ref engine._programPhaseSchedule;
    }

    [ProgramCapability]
    private sealed class KernelProgramPipelineHooksAccessAdapter(GameEngine engine) : IKernelProgramPipelineHooksAccess
    {
        public ref readonly CardGame.Core.ProgramPipelineHooks _programPipelineHooks => ref engine._programPipelineHooks;
    }

    [ProgramCapability]
    private sealed class KernelProgramRuleObserversAccessAdapter(GameEngine engine) : IKernelProgramRuleObserversAccess
    {
        public ref readonly CardGame.Core.ProgramRuleObservers _programRuleObservers => ref engine._programRuleObservers;
    }

    [ProgramCapability]
    private sealed class KernelProgramSelectionQueriesAccessAdapter(GameEngine engine) : IKernelProgramSelectionQueriesAccess
    {
        public ref CardGame.Core.IProgramSelectionQueries? _programSelectionQueries => ref engine._programSelectionQueries;
    }

    [ProgramCapability]
    private sealed class KernelProgramSelectionValidatorsAccessAdapter(GameEngine engine) : IKernelProgramSelectionValidatorsAccess
    {
        public ref CardGame.Core.ProgramSelectionValidationRegistry? _programSelectionValidators => ref engine._programSelectionValidators;
    }

    [ProgramCapability]
    private sealed class KernelProgramSelectionsAccessAdapter(GameEngine engine) : IKernelProgramSelectionsAccess
    {
        public ref CardGame.Core.ProgramSelectionRegistry? _programSelections => ref engine._programSelections;
    }

    [ProgramCapability]
    private sealed class KernelProgramTargetResolversAccessAdapter(GameEngine engine) : IKernelProgramTargetResolversAccess
    {
        public ref CardGame.Core.ProgramTargetResolverRegistry? _programTargetResolvers => ref engine._programTargetResolvers;
    }

    [ProgramCapability]
    private sealed class KernelProgramTriggerProofsAccessAdapter(GameEngine engine) : IKernelProgramTriggerProofsAccess
    {
        public ref CardGame.Core.ProgramTriggerProofRuntime? _programTriggerProofs => ref engine._programTriggerProofs;
    }

    [ProgramCapability]
    private sealed class KernelProgramWindowReturnOperationsAdapter(GameEngine engine) : IKernelProgramWindowReturnOperations
    {
        public void Return(CardGame.Core.ProgramWindowReturnCatalog __nativeReceiver, CardGame.Core.ProgramSkillFrame frame, bool completed) => __nativeReceiver.Return(engine, frame, completed);
    }

    [ProgramCapability]
    private sealed class KernelPublicHandRevealReturnOperationsAdapter(GameEngine engine) : IKernelPublicHandRevealReturnOperations
    {
        public void Return(CardGame.Core.PublicHandRevealReturns __nativeReceiver, string key, CardGame.Core.ResolutionFrame parent) => __nativeReceiver.Return(engine, key, parent);
    }

    [ProgramCapability]
    private sealed class KernelQinglongContinuationHandleFactoryAdapter(GameEngine engine) : IKernelQinglongContinuationHandleFactory
    {
        public CardGame.Core.NativeOwners.FactionCardRequestHandle CreateFactionCardRequestHandle(long ownerId) => engine.ComposeNativeFactionCardRequestHandle(ownerId);
        public CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long ownerId) => engine.ComposeNativeFangtianHalberdHandle(ownerId);
        public CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long ownerId) => engine.ComposeNativeBorrowedSwordHandle(ownerId);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId) => engine.ComposeNativeCardAttackHandle(ownerId);
    }

    [ProgramCapability]
    private sealed class KernelRejectOperationsAdapter(GameEngine engine) : IKernelRejectOperations
    {
        public CardGame.Core.CommandResult Reject(CardGame.Core.CommandErrorCode code, string message) => engine.Reject(code, message);
    }

    [ProgramCapability]
    private sealed class KernelResolutionSchedulerAccessAdapter(GameEngine engine) : IKernelResolutionSchedulerAccess
    {
        public ref readonly CardGame.Core.ResolutionScheduler _resolutionScheduler => ref engine._resolutionScheduler;
    }

    [ProgramCapability]
    private sealed class KernelResolveRuntimeHandlerTargetOperationsAdapter(GameEngine engine) : IKernelResolveRuntimeHandlerTargetOperations
    {
        public object? ResolveRuntimeHandlerTarget(System.Reflection.MethodInfo method) => engine.ResolveRuntimeHandlerTarget(method);
    }

    [ProgramCapability]
    private sealed class KernelReturnOperationOperationsAdapter(GameEngine engine) : IKernelReturnOperationOperations
    {
        public void ReturnOperation(CardGame.Core.SkillProgramEffectOp operation, CardGame.Core.ProgramSkillFrame frame, CardGame.Core.NativeDrawOutcome outcome)
        {
            var method = CardGame.Core.EngineOwners.CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value[operation];
            ((System.Action<CardGame.Core.ProgramSkillFrame, CardGame.Core.NativeDrawOutcome>)engine.BindRuntimeHandler(method, typeof(System.Action<CardGame.Core.ProgramSkillFrame, CardGame.Core.NativeDrawOutcome>)))(frame, outcome);
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
        public void AdvanceEventRulesAndQueueFact(CardGame.Core.IGameEvent payload) => engine.AdvanceEventRulesAndQueueFact(payload);
    }

    [ProgramCapability]
    private sealed class KernelRuleQueryFactFactoriesAdapter(GameEngine engine) : IKernelRuleQueryFactFactories
    {
        public CardGame.Core.NativeOwners.DistanceQueryFacts CreateDistanceQueryFacts(CardGame.Core.CharacterState source, CardGame.Core.CharacterState target) => engine.ComposeNativeDistanceQueryFacts(source, target);
        public CardGame.Core.NativeOwners.AttackRangeQueryFacts CreateAttackRangeQueryFacts(CardGame.Core.CharacterState player) => engine.ComposeNativeAttackRangeQueryFacts(player);
        public CardGame.Core.NativeOwners.DrawCountQueryFacts CreateDrawCountQueryFacts(CardGame.Core.CharacterState player) => engine.ComposeNativeDrawCountQueryFacts(player);
        public CardGame.Core.NativeOwners.SlashLimitQueryFacts CreateSlashLimitQueryFacts(CardGame.Core.CharacterState player) => engine.ComposeNativeSlashLimitQueryFacts(player);
        public CardGame.Core.NativeOwners.HandLimitQueryFacts CreateHandLimitQueryFacts(CardGame.Core.CharacterState player) => engine.ComposeNativeHandLimitQueryFacts(player);
        public CardGame.Core.NativeOwners.CardTargetCountQueryFacts CreateCardTargetCountQueryFacts(CardGame.Core.CharacterState player, CardGame.Core.CardKind effectiveCardKind, long? declaredUseFrameId) => engine.ComposeNativeCardTargetCountQueryFacts(player, effectiveCardKind, declaredUseFrameId);
        public CardGame.Core.NativeOwners.CardUseDistanceQueryFacts CreateCardUseDistanceQueryFacts(CardGame.Core.CharacterState player, CardGame.Core.CardKind effectiveCardKind) => engine.ComposeNativeCardUseDistanceQueryFacts(player, effectiveCardKind);
        public CardGame.Core.NativeOwners.NumericContributionQueryFacts CreateNumericContributionQueryFacts(CardGame.Core.CharacterState player, CardGame.Core.SkillRuleQuery ruleQuery) => engine.ComposeNativeNumericContributionQueryFacts(player, ruleQuery);
    }

    [ProgramCapability]
    private sealed class KernelRuntimeHooksAccessAdapter(GameEngine engine) : IKernelRuntimeHooksAccess
    {
        public ref CardGame.Core.ProgramRuntimeHooks? _runtimeHooks => ref engine._runtimeHooks;
    }

    [ProgramCapability]
    private sealed class KernelShanheMarketAccessAdapter(GameEngine engine) : IKernelShanheMarketAccess
    {
        public ref CardGame.Core.Campaigns.ShanheCampaignSession? _shanheMarket => ref engine._shanheMarket;
    }

    [ProgramCapability]
    private sealed class KernelSkillBindingIndexAccessAdapter(GameEngine engine) : IKernelSkillBindingIndexAccess
    {
        public ref readonly CardGame.Core.MatchSkillBindingIndex _skillBindingIndex => ref engine._skillBindingIndex;
    }

    [ProgramCapability]
    private sealed class KernelSkillRuntimeStateAccessAdapter(GameEngine engine) : IKernelSkillRuntimeStateAccess
    {
        public ref readonly CardGame.Core.SkillRuntimeStateStore _skillRuntimeState => ref engine._skillRuntimeState;
    }

    [ProgramCapability]
    private sealed class KernelSuppressedCardMovedEventBatchIdsAccessAdapter(GameEngine engine) : IKernelSuppressedCardMovedEventBatchIdsAccess
    {
        public ref readonly System.Collections.Generic.Stack<long> _suppressedCardMovedEventBatchIds => ref engine._suppressedCardMovedEventBatchIds;
    }

    [ProgramCapability]
    private sealed class KernelTargetRulesAccessAdapter(GameEngine engine) : IKernelTargetRulesAccess
    {
        public ref CardGame.Core.ProgramTargetRules? _targetRules => ref engine._targetRules;
    }

    [ProgramCapability]
    private sealed class KernelTargetWindowProtocolsAccessAdapter(GameEngine engine) : IKernelTargetWindowProtocolsAccess
    {
        public ref CardGame.Core.ProgramTargetWindowProtocols? _targetWindowProtocols => ref engine._targetWindowProtocols;
    }

    [ProgramCapability]
    private sealed class KernelTrickEffectHandleFactoryAdapter(GameEngine engine) : IKernelTrickEffectHandleFactory
    {
        public CardGame.Core.NativeOwners.GroupCardHandle CreateGroupCardHandle(long resolutionId, int sourceSeat, CardGame.Core.Card card, System.Collections.Generic.IReadOnlyList<int> targetSeats, CardGame.Core.GroupCardEffect effect, CardGame.Core.CardKind? requiredCardKind, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default) => engine.ComposeNativeGroupCardHandle(resolutionId, sourceSeat, card, targetSeats, effect, requiredCardKind, physicalCards);
        public CardGame.Core.NativeOwners.BorrowedSwordHandle CreateBorrowedSwordHandle(long resolutionId, int sourceSeat, int weaponOwnerSeat, int slashTargetSeat, CardGame.Core.Card card) => engine.ComposeNativeBorrowedSwordHandle(resolutionId, sourceSeat, weaponOwnerSeat, slashTargetSeat, card);
        public CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, CardGame.Core.Card? card, int damageAmount = 1, CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, CardGame.Core.DamageNature? damageNatureOverride = default, System.Collections.Generic.IReadOnlyList<CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default) => engine.ComposeNativeCardAttackHandle(resolutionId, sourceSeat, targetSeat, card, damageAmount, playedCardKind, ignoresArmor, isDelayedJudgmentDamage, delayedJudgmentSeat, damageNatureOverride, physicalCards, programJudgmentFrameId, programSkillFrameId, conversionSource, programSkillCardUseFrameId);
        public CardGame.Core.NativeOwners.DuelHandle CreateDuelHandle(CardGame.Core.NativeOwners.CardAttackHandle attack) => engine.ComposeNativeDuelHandle(attack);
    }

    [ProgramCapability]
    private sealed class KernelTriggerWindowHostFactoriesAdapter(GameEngine engine) : IKernelTriggerWindowHostFactories
    {
        public CardGame.Core.NativeOwners.CardsMovedTriggerWindowHost CreateCardsMovedTriggerWindowHost() => engine.ComposeNativeCardsMovedTriggerWindowHost();
        public CardGame.Core.NativeOwners.HpChangedTriggerWindowHost CreateHpChangedTriggerWindowHost() => engine.ComposeNativeHpChangedTriggerWindowHost();
        public CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost() => engine.ComposeNativeCardActionTriggerWindowHost();
    }

    [ProgramCapability]
    private sealed class KernelTryAdvanceOperationsAdapter(GameEngine engine) : IKernelTryAdvanceOperations
    {
        public bool TryAdvance(CardGame.Core.ResolutionFrame frame) => engine.GetRuntimeModule<CardGame.Core.EngineOwners.CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.TryAdvance(engine, frame);
    }

    [ProgramCapability]
    private sealed class KernelTryResumeOperationsAdapter(GameEngine engine) : IKernelTryResumeOperations
    {
        public bool TryResume(CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, long frameId) => __nativeReceiver.TryResume(engine, frameId);
    }

    [ProgramCapability]
    private sealed class KernelTryReturnMovementOperationsAdapter(GameEngine engine) : IKernelTryReturnMovementOperations
    {
        public bool TryReturnMovement(CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, CardGame.Core.ProgramSkillFrame frame, bool afterPayment = false) => __nativeReceiver.TryReturnMovement(engine, frame, afterPayment);
    }

    [ProgramCapability]
    private sealed class KernelTurnCardUseEffectsAccessAdapter(GameEngine engine) : IKernelTurnCardUseEffectsAccess
    {
        public ref readonly CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects => ref engine._turnCardUseEffects;
    }

    [ProgramCapability]
    private sealed class KernelViewContributorsAccessAdapter(GameEngine engine) : IKernelViewContributorsAccess
    {
        public ref CardGame.Core.ProgramViewContributors? _viewContributors => ref engine._viewContributors;
    }

}
