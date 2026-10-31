#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardDeclarationsCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash);
}

internal interface ICoreCardDeclarationsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreCardDeclarationsCoreNativeFactionDefenseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteFactionDefenseResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card? selectedDodge, bool usedBagua, global::CardGame.Core.CardConversionSource? virtualResponseSource = default);
    void AdvanceFactionDefenseCandidate(bool keepCurrentRequest = false);
}

internal interface ICoreCardDeclarationsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreCardDeclarationsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveNullificationChoice(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedCard, global::CardGame.Core.CardConversionSource? selectedConversionSource = default);
}

internal interface ICoreCardDeclarationsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreCardDeclarationsCharacterSunHuanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnAnniversarySunHuanDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success);
}

internal interface ICoreCardDeclarationsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? JuewuForeignHandRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::CardGame.Core.DeclaredCardPayment CaptureWuyouDeclarationPayment(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent);
}

internal interface ICoreCardDeclarationsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreCardDeclarationsCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard);
}

internal interface ICoreCardDeclarationsCharacterGuoYouZhiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnMouGyzDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success);
}

internal interface ICoreCardDeclarationsCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureMoQuanyuDeclarationQuota(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets);
    void RecordMoQuanyuDeclarationPayment(long declarationId);
}

internal interface ICoreCardDeclarationsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardDeclarationsCoreActionDiscardColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card);
}

internal interface ICoreCardDeclarationsObservedSkillGrantRevisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions { get; }
}

internal interface ICoreCardDeclarationsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureWuLuKangDixianDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent);
}

internal interface ICoreCardDeclarationsCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveRecoveryCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, string cardName, global::CardGame.Core.CardKind? playedCardKind = default, int recoveryAmount = 1, global::System.Collections.Generic.IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, global::CardGame.Core.DrawFundedDistinctBasicPayment? drawFundedPayment = default, global::CardGame.Core.HuanpeiPaidOrigin? huanpeiOrigin = default);
}

internal interface ICoreCardDeclarationsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCardDeclarationsCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryApplyQigongDeclaration(global::CardGame.Core.CardDeclarationFrame declaration);
    bool TryReturnQigongDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success);
}

internal interface ICoreCardDeclarationsCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureMouZhaJiangDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent);
}

internal interface ICoreCardDeclarationsCharacterJuShouMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnMouJsDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded);
}

internal interface ICoreCardDeclarationsCoreCharacterHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<string> AcquireRuntimeSkills(global::CardGame.Core.CharacterState player, string sourceSkillId, global::System.Collections.Generic.IReadOnlyList<string> skillIds);
}

internal interface ICoreCardDeclarationsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreCardDeclarationsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreCardDeclarationsCoreNativeMultiTargetActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0);
    void AddFangtianHalberdSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreCardDeclarationsCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureShanheClaimedDeclarationUse(global::CardGame.Core.DeclaredCardPayment paid);
}

internal interface ICoreCardDeclarationsCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDyingCardResponse(global::CardGame.Core.DyingResponseEvent response);
}

internal interface ICoreCardDeclarationsCoreJianjiDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnJianjiDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool success);
}

internal interface ICoreCardDeclarationsCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PayQizhengSingleResponse(long id, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    void ResolveQizhengAfterResponse(long id);
}

internal interface ICoreCardDeclarationsCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureMouSmDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent);
    global::System.Collections.Generic.IReadOnlyList<int> MouSmDeclarationTargets(global::CardGame.Core.CardDeclarationFrame frame, int viewer, bool revealAll);
    bool ReturnMouSmPlayedDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded);
}

internal interface ICoreCardDeclarationsCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICoreCardDeclarationsCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnHappyQiaoShutuDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool succeeded);
}

internal interface ICoreCardDeclarationsCharacterGuoYuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnShiGyDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success);
}

internal interface ICoreCardDeclarationsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreCardDeclarationsCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int EffectiveOwnedCardRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardDeclarationsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }
}

internal interface ICoreCardDeclarationsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreCardDeclarationsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
    void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon);
}

internal interface ICoreCardDeclarationsCoreNativeActionDispatchOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default);
}

internal interface ICoreCardDeclarationsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreCardDeclarationsCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteFactionSlashResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card selectedSlash);
    void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false);
    void BeginProvidedFactionSlashSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind effectiveKind, bool usesZhuqueFan, global::CardGame.Core.CardConversionSource? conversionSource = default);
    void BeginBorrowedSwordFactionSlashSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind effectiveKind, bool usesZhuqueFan, global::CardGame.Core.CardConversionSource? conversionSource = default);
    void BeginQinglongCrescentBladeFactionSlashSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards, global::CardGame.Core.CardKind effectiveKind, bool usesZhuqueFan, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreCardDeclarationsSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreCardDeclarationsCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreCardDeclarationsCoreNativeGroupResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse);
}

internal interface ICoreCardDeclarationsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardDeclarationsCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureBianFuRenClaimedDeclarationPayment(long nativeUseId);
}

internal interface ICoreCardDeclarationsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardDeclarationsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICoreCardDeclarationsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreCardDeclarationsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeFrame(long frameId);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreCardDeclarationsCoreNativeUsedDiscardOriginsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureClaimedDeclarationTrueUse(global::CardGame.Core.DeclaredCardPayment paid);
}

internal interface ICoreCardDeclarationsCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreCardDeclarationsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICoreCardDeclarationsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCardDeclarationsCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureBeifenDeclarationPayment(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent);
}

internal interface ICoreCardDeclarationsCharacterXiaoQiaoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureWeiwanDeclarationPayment(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent);
}

internal interface ICoreCardDeclarationsCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ReturnMouLuSuPlayedDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded);
}

internal interface ICoreCardDeclarationsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardDeclarationsCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnKeBiNengPingduanDeclaration(global::CardGame.Core.CardDeclarationFrame declaration, bool success);
}

internal interface ICoreCardDeclarationsCharacterZhouYuOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureMouGuquDeclaration(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent);
}

internal interface ICoreCardDeclarationsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment CaptureDiouDeclaration(global::CardGame.Core.DeclaredCardPayment paid, global::CardGame.Core.CardDeclarationParentReturn parent);
}

internal interface ICoreCardDeclarationsCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
}

internal interface ICoreCardDeclarationsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description);
}
