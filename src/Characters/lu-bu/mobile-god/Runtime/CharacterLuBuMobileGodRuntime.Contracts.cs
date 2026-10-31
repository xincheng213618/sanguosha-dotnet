#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterLuBuMobileGodCoreNativeAiDecisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void NotifyAiOfSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target);
}

internal interface ICharacterLuBuMobileGodCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICharacterLuBuMobileGodCoreNativeCardUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long BeginCardUse(global::CardGame.Core.Card card, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, global::System.Collections.Generic.IReadOnlyList<int>? designatedTargetSeats = default, bool isTrueZhangbaSlash = false, global::CardGame.Core.ProgramSelectedActorDuelOrigin? selectedActorDuelOrigin = default, global::CardGame.Core.ProgramDamageTargetDuelOrigin? damageTargetDuelOrigin = default, global::CardGame.Core.ProgramDualColorDuelOrigin? dualColorDuelOrigin = default, global::CardGame.Core.ConditionalDiscardDuelOrigin? conditionalDiscardDuelOrigin = default, global::CardGame.Core.ShiQuanDuelOrigin? shiQuanDuelOrigin = default, global::CardGame.Core.ProgramVirtualOrdinaryTrickOrigin? virtualOrdinaryTrickOrigin = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.ChainedStateBasicPayment? chainedStatePayment = default, global::CardGame.Core.FriendRecastDuelOrigin? friendRecastDuel = default, global::CardGame.Core.ChosenAssaultReturn? chosenAssault = default, global::CardGame.Core.ZhenyingDuelOrigin? zhenyingDuel = default, global::CardGame.Core.HeJinMouzhuUseOrigin? heJinMouzhu = default, global::CardGame.Core.WuAnGuoLiyongDuelOrigin? wuAnGuoLiyong = default, global::CardGame.Core.TianChouShandaoUseOrigin? tianChouShandao = default, global::CardGame.Core.QinLangXianyingUseOrigin? qinLangXianying = default, global::CardGame.Core.AHuiNanJuemanUseOrigin? aHuiNanJueman = default, global::CardGame.Core.LiuYanLimuUseOrigin? liuYanLimu = default, global::CardGame.Core.JueshengDuelOrigin? jueshengDuel = default, global::CardGame.Core.QuHuangNishouUseOrigin? quHuangNishou = default, global::CardGame.Core.WeijiePaidOrigin? weijieOrigin = default, global::CardGame.Core.ZonghuPaidOrigin? zonghuOrigin = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICharacterLuBuMobileGodCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? DragonBoatExpectedNativeTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterLuBuMobileGodCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterLuBuMobileGodCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterLuBuMobileGodCoreShenwuEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenwuRobeTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind kind);
}

internal interface ICharacterLuBuMobileGodContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterLuBuMobileGodCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICharacterLuBuMobileGodCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmorBypass(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterLuBuMobileGodCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PaySlashUseMaterials(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int providerSeat, global::CardGame.Core.CardLocation? sourceLocation);
}

internal interface ICharacterLuBuMobileGodCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
    bool IsDirectProgramFireSlashConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
    global::CardGame.Core.GameEngine.NuzhanModifiers GetNuzhanModifiers(long frameId, global::CardGame.Core.CharacterState source);
}

internal interface ICharacterLuBuMobileGodCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> HellReturnSoulUseTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICharacterLuBuMobileGodCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterLuBuMobileGodCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterLuBuMobileGodCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterBeneficiarySuitShieldActions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> actions);
}

internal interface ICharacterLuBuMobileGodCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterLuBuMobileGodCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }
}

internal interface ICharacterLuBuMobileGodCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterLuBuMobileGodCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
    void BeginNextFangtianHalberdTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending);
}

internal interface ICharacterLuBuMobileGodCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}

internal interface ICharacterLuBuMobileGodCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<int> GetDeclaredCardTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalActionKind actionKind, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterLuBuMobileGodCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICharacterLuBuMobileGodCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICharacterLuBuMobileGodCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICharacterLuBuMobileGodCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterLuBuMobileGodCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICharacterLuBuMobileGodCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterLuBuMobileGodCoreConversionStatesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RegisterTaggedConversionSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICharacterLuBuMobileGodCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> ApplyTurnCardGroupTargetRestrictions(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<int> targets);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterTurnCardUseRestrictions(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.Card? nativeOriginalCard = default);
}

internal interface ICharacterLuBuMobileGodCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SlashUseVariant> GetSlashUseVariants(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::CardGame.Core.CardKind baseEffectiveKind, global::CardGame.Core.CardConversionSource? primaryConversionSource);
}

internal interface ICharacterLuBuMobileGodKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle CreateFangtianHalberdHandle(long resolutionId, int sourceSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveCardKind, bool ignoresArmor, int damageAmount, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, bool usesFangtian, bool countedTowardSlashLimit, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterLuBuMobileGodCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterLuBuMobileGodCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId);
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICharacterLuBuMobileGodCoreBoundCardMovementContinuationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat);
}

internal interface ICharacterLuBuMobileGodCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterLuBuMobileGodCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICharacterLuBuMobileGodResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterLuBuMobileGodRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICharacterLuBuMobileGodObservedSkillGrantRevisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions { get; }
}

internal interface ICharacterLuBuMobileGodCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordSlashUseDebit(long cardUseFrameId, int actorSeat);
}

internal interface ICharacterLuBuMobileGodCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICharacterLuBuMobileGodCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICharacterLuBuMobileGodCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildMobileWumouPrintedTargetActions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICharacterLuBuMobileGodKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterLuBuMobileGodCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind AdvancedEffectiveHandKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICharacterLuBuMobileGodCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
    int RelativeTableOrder(int from, int target);
}

internal interface ICharacterLuBuMobileGodCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterLuBuMobileGodCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterLuBuMobileGodCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? GuanduExpectedNativeTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterLuBuMobileGodCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}
