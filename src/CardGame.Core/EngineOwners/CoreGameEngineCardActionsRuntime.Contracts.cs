#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardActionsCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void TryIssueTieredRoundResponse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuJingLiubing { get; }

    bool HasWuJingHeji { get; }

    global::CardGame.Core.Suit? WuJingLiubingUsedSuit(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? conversions, global::CardGame.Core.Suit? ordinary);
    bool IsWuJingHejiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreWuJingQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuJingCompletedUseObserver(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginNextFangtianHalberdTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending);
    void ContinueSlashAfterFinalizedTargets(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ContinueSlashAfterProgramTargetEffects(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ContinueSlashAfterRedirectPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ContinueSlashAfterResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardActionsCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHuCheErUse(long id);
}

internal interface ICoreCardActionsCoreCurrentTurnUseKindsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksCurrentTurnUseKinds { get; }
}

internal interface ICoreCardActionsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSiruiCapability { get; }

    int? SiruiResponseRank(global::CardGame.Core.CardConversionSource? source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs);
    void BindSiruiResponseAction(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardActionContext action);
    bool TryGetSiruiDestroyedResponseCost(global::CardGame.Core.CardActionContext action, int cardId, out global::CardGame.Core.CardMovementRecord movement);
    bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreProgramCardUseNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramUseNullification(global::CardGame.Core.CardUseFrame use);
    bool IsExactSimpleBeforeTargetWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCharacterCaiYongLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureLeCaiYongFeibaiQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardActionsCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBianFuRenCapability { get; }
}

internal interface ICoreCardActionsCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLuSuPlayedWindow(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    global::CardGame.Core.MouLuSuAnniversaryPlayedReturn? MouLuSuPlayedWindowReturn(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    bool TryReturnMouLuSuPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool ValidMouLuSuPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool IsMouLuSuPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreCardActionsCoreForeignSelectedCardSlashReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelectedForeignCardSlashRemovedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.JizhiResolution pending);
}

internal interface ICoreCardActionsCharacterChengYuMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouDanchiSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCoreGuanduEquipmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsGuanduDestroyedChariotMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}

internal interface ICoreCardActionsCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyZuoCiDestroyedResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish);
}

internal interface ICoreCardActionsCoreNativeActionDispatchOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default);
}

internal interface ICoreCardActionsCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuiyunCompletedUseObserver(long useId);
    bool IsHuiyunOutsideGeneralWeaponMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? CaptureAlcoholActionRank(int providerSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> ids);
    int? CaptureAlcoholResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs);
}

internal interface ICoreCardActionsCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRoundOnceCardlessBasicUse(long id);
}

internal interface ICoreCardActionsCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardResponseCompletedObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    bool TryDrainCardResponseCompletionCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied);
    void ContinueCardResponseAfterCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool ValidResponseCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool ResponseCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
    bool IsResponseCompletionCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreCardActionsCharacterYuanTanYuanShangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNeifaCapability { get; }

    void CaptureNeifaPaidEquipmentUse(long useId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCoreDragonBoatActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDragonBoatActivity { get; }
}

internal interface ICoreCardActionsCoreTargetedOrdinaryTrickOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetedOrdinaryTrickAdjustment { get; }
}

internal interface ICoreCardActionsCoreOverflowTargetCancellationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteOverflowCanceledUse(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICoreCardActionsCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedPolicyForAction(long actionId);
}

internal interface ICoreCardActionsCoreActionDiscardColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card);
    bool? CaptureActionColor(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICoreCardActionsCharacterGuoYuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShiGyCompletedUseObserver(long id);
}

internal interface ICoreCardActionsCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role);
}

internal interface ICoreCardActionsCharacterSunQuanOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMoQuanyuSingleton(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.SkillProgramTriggerWindow kind);
}

internal interface ICoreCardActionsCharacterTaishiCiMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteShenTaishiCiCancelledSlash(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCharacterZhenJiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHappyFeifuClaimedResponseEntity(global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreCardActionsCharacterRuanYuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuanYuCapability { get; }

    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> IncludeRuanYuMiaoxianRedCandidates(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> ordinary, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardActionsProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreCardActionsCharacterZhugeGuoLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryStartLeChengyanEffect(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCoreActorHandLimitTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActorHandLimitTargetAnnouncements(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    bool ShouldCollectActorHandLimitPenalty(global::CardGame.Core.CardActionContext action, int ownerSeat, global::CardGame.Core.SkillProgramTriggerWindow window);
    global::CardGame.Core.ProgramCardTriggerWindowFrame AppendActorHandLimitTargetCandidates(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreCardActionsCoreCardSupplyCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryDrainCardSupplyCompletionCosts(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied);
    void ContinueCardSupplyAfterCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool ValidCardSupplyCompletionWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool CardSupplyCompletionFirstChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child);
    bool IsCardSupplyCompletionCostPaid(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreCardActionsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardActionsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreCardActionsCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreCardActionsCorePhysicalDrawFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default);
}

internal interface ICoreCardActionsKernelCoreGameEngineProgramTriggerWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardActionTriggerWindowHost CreateCardActionTriggerWindowHost();
}

internal interface ICoreCardActionsCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasConvertedSkillInvocations(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    bool TryBeginConvertedSkillInvocations(global::CardGame.Core.ProgramCardTriggerWindowFrame f);
    bool IsCardWindowWaitingForSkillInvocation(global::CardGame.Core.ProgramCardTriggerWindowFrame window, int index);
}

internal interface ICoreCardActionsCoreSoleTargetRedirectOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSoleTargetRedirect(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
    bool TryReturnSoleTargetRedirect(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
    bool IsExactSoleTargetRedirectReturn(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreCardActionsCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICoreCardActionsCoreDyingWindowSkillPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBlackTrickTargetPolicy { get; }
}

internal interface ICoreCardActionsCoreEndingHistoricalUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksOwnPlayHistory { get; }
}

internal interface ICoreCardActionsCoreForeignContestAidObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameTypeAidTargetObserver(long useId);
}

internal interface ICoreCardActionsCharacterGuoSiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGuoSiCapability { get; }

    void CaptureGuoSiFinalizedUse(long id, global::CardGame.Core.CardActionContext a);
    void CaptureGuoSiCompletedUse(long id, global::CardGame.Core.CardActionContext a);
    bool IsSuspendedGuoSiCompletedParent(global::CardGame.Core.CardUseFrame original);
    bool GuoSiNativeRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCardActionsCoreFirstSlashDuelLegacyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardTriggerWindowFrame DeferFirstSlashDuelCandidates(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreCardActionsCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShanheStartingResourceResponseFinish(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord finish);
}

internal interface ICoreCardActionsCoreVirtualOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesVirtualOrdinaryTrickUseAction(long frameId, global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFirstCategoryUse(int owner, string skill, global::CardGame.Core.CardActionContext action);
    int[] AvailableCompletedTopCosts(global::CardGame.Core.CardActionContext a, long parentFrameId);
}

internal interface ICoreCardActionsCoreFirstSlashDuelBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanCollectFirstSlashDuelCandidate(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCharacterDengAiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenDaSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame w);
}

internal interface ICoreCardActionsCharacterZhouCangBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBoundaryZhouCangCompletedUseObserver(long useId);
}

internal interface ICoreCardActionsCoreCompletedGiftQinglongRidesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsUnselectedCompletedGiftQinglongRide(global::CardGame.Core.ProgramSkillFrame frame);
    bool TryGetCompletedGiftRecipientSlashRide(global::CardGame.Core.ProgramCardTriggerWindowFrame window, out global::CardGame.Core.CardUseFrame child);
}

internal interface ICoreCardActionsCoreCurrentUseTargetCandidateRefreshOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame NormalizeImplicitDrawTwoUseTarget(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCardActionsCoreDelayedPlacementTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueFinalizedDelayedPlacement(global::CardGame.Core.ProgramCardTriggerWindowFrame completedWindow);
    bool IsExactFinalizedDelayedPlacementWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCharacterLiuChengMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMouLiuChengCompletion(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    void CaptureMouLiuChengAccrual(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    global::CardGame.Core.ProgramSkillWindowContext? AttachMouLiuChengAccrual(global::CardGame.Core.CardActionContext action, string skill, string instance, global::CardGame.Core.ProgramSkillWindowContext context);
    global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouLiuChengAccrual(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
    bool MatchesMouLiuChengNativeAction(long id, global::CardGame.Core.CardActionContext action, int cardId);
    bool IsMouLiuChengSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
}

internal interface ICoreCardActionsCharacterPangTongMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouPtLateTargets(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreCardActionsCharacterSuFeiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSuFeiCapability { get; }

    void CaptureSuFeiFinalizedUse(long nativeUseId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCharacterXuHuangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesMouXuHuangNativeAction(long id, global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsCharacterDongChengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesMouDcNativeAction(long id, global::CardGame.Core.CardActionContext a, int card);
}

internal interface ICoreCardActionsCoreUnconvertedCardUseQualificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsUnconvertedActualCardUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.ProgramCardContinuation? continuation);
}

internal interface ICoreCardActionsCoreCompletedUndamagedTargetRevealOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferCompletedUndamagedTargetReveal(global::CardGame.Core.ProgramTriggerLegalityContext gate);
}

internal interface ICoreCardActionsCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
    void RecordFactionRewardOffers(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> candidates);
    int? FactionActionRewardChooser(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CardActionContext action);
    bool WasFactionRewardOffered(int owner, string skillId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuZglReturnedCopyCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.CardMovementRecord? finish = default);
}

internal interface ICoreCardActionsCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksRoundGainedSourceUses { get; }
}

internal interface ICoreCardActionsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreCardActionsCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionCapability { get; }
}

internal interface ICoreCardActionsCoreChainedStateBasicReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChainedStateQinglongCompletedWindowRide(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCharacterZhaoYunOlGod2016Operations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureHpScaledCardUseAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCoreOverflowTargetCancellationObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? OverflowTargetCancellationObserverRoot();
}

internal interface ICoreCardActionsCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCuiYanCapability { get; }

    bool IsCuiYanNextUseForbidden(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> materialIds, global::CardGame.Core.Suit? nativeEffectiveSuit);
    bool TryReturnCuiYanLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame w);
}

internal interface ICoreCardActionsCharacterHuLieMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHuLieUse(long id);
}

internal interface ICoreCardActionsCoreForeignPublicPileSlashesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignPublicPileSlashUse(long resolutionId);
}

internal interface ICoreCardActionsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginJizhiOrNullificationWindow(long resolutionId, global::CardGame.Core.Card effectCard, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.LegalActionKind actionKind, int? targetCardId = default, global::CardGame.Core.CardKind? requiredCardKind = default, global::CardGame.Core.CardKind? playedCardKind = default);
    void ContinueJizhiOrNullificationAfterTargetTriggers(global::CardGame.Core.NativeOwners.JizhiResolution pending);
}

internal interface ICoreCardActionsCharacterHuangZhongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryHzIneffectiveResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreCardActionsCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameTypeActualUseAid { get; }
}

internal interface ICoreCardActionsCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasChenglieCompletedUseObserver(long useId);
}

internal interface ICoreCardActionsCharacterZhangYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShiZySuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreCardActionsCoreDragonBoatGeneralsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureDragonBoatFinalizedUse(long useId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCharacterHuaManOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHuaManCapability { get; }

    void CaptureHuaManFinalizedUse(long id, global::CardGame.Core.CardActionContext action);
    void CaptureHuaManCompletedUse(long id, global::CardGame.Core.CardActionContext action);
    global::CardGame.Core.ProgramSkillWindowContext CreateHuaManCardUseContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
    bool HasHuaManCardObserver(long useId);
    bool IsHuaManSouyingEmptyTargetWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool IsHuaManClaimedEntity(long useId, int cardId);
}

internal interface ICoreCardActionsCoreCurrentTurnShownCardBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShownCardTurnCapability { get; }
}

internal interface ICoreCardActionsCoreRoundGainedEquipmentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRoundGainedEquipmentDrawCardObserver(long useId);
}

internal interface ICoreCardActionsCharacterMaMiDiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaMiDiCapability { get; }

    global::CardGame.Core.ProgramSkillWindowContext CreateMaMiDiIssuedContext(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c);
}

internal interface ICoreCardActionsCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRankSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind);
    int? ZhangbaSpecificSlashRank(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CharacterState? provider = default);
}

internal interface ICoreCardActionsCorePaidColorConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksPaidColorUseAppearance(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> chain);
}

internal interface ICoreCardActionsCharacterLiFengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiFengCapability { get; }

    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectJiyunCompletedUseCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation? continuation);
    global::CardGame.Core.ProgramSkillWindowContext CreateJiyunCompletedUseContext(global::CardGame.Core.ProgramCardTriggerWindowFrame mother, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary);
    bool IsJiyunClaimedUseEntity(long nativeUseId, int cardId);
    bool IsJiyunClaimedResponseEntity(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind, int cardId);
}

internal interface ICoreCardActionsCoreFulueQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFulueCompletedCounterChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCharacterZhangXiuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouZhangXiuAnniversaryUse(long id);
}

internal interface ICoreCardActionsCoreProgramConversionPolarityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillPolarity GetProgramConversionPolarity(int ownerSeat, string skillId);
}

internal interface ICoreCardActionsCoreShanheLeizhenOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordShanheLeizhenCommittedUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuYanCapability { get; }

    bool TryReturnLiuYanLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame w);
}

internal interface ICoreCardActionsCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMoLuBu { get; }
}

internal interface ICoreCardActionsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuyouCapability { get; }

    bool HasWuGuanYuJuewuCapability { get; }

    int? CaptureJuewuActionRank(int providerSeat, global::System.Collections.Generic.IReadOnlyList<int> ids);
    int? CaptureJuewuResponseRank(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs);
}

internal interface ICoreCardActionsCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnMouBingcaiLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool IsMouLixianRemovedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardActionsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreCardActionsCoreTieredRoundZeroReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesTieredRoundZeroUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasYuanchouCardPolicy { get; }
}

internal interface ICoreCardActionsCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExchangedCardClaim(long actionId, int cardId);
}

internal interface ICoreCardActionsCoreJoinedTrickDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJoinedTrickDamageRewardCardObserver(long useId);
}

internal interface ICoreCardActionsCharacterZhouYuOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouZuifengCapability { get; }
}

internal interface ICoreCardActionsCoreWushuangShopCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangShopDestroyedNullificationPayment(global::CardGame.Core.CardActionContext action, long windowId, global::CardGame.Core.CardActionCost material);
}

internal interface ICoreCardActionsCoreSourceCursesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSourceCurseActionColors { get; }

    global::CardGame.Core.SourceCurseDeposit? EligibleSourceCurseUse(int owner, string skill, string instance, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramEffect effect);
}

internal interface ICoreCardActionsCoreModeCardRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanModeHandEquipmentRecast(global::CardGame.Core.Card card);
}

internal interface ICoreCardActionsCoreCommittedResponseUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    void ContinueCommittedResponseUse(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreCardActionsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCardActionsCoreOriginalUseLightningOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesOriginalUseLightningAction(long id, global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreCardActionsCoreHellDizangProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanHellDitingMountRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreCardActionsCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateOwnPublicPileConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse, global::CardGame.Core.CardKind? effectiveKind = default);
    global::CardGame.Core.Card? OwnPublicPileResponseMaterial(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreCardActionsCoreQuotaTopProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RegisterFirstTurnTargetActions(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardActionsCoreOffTurnUsedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
    global::CardGame.Core.GameEngine.AttackCompletionReceipt CaptureAttackCompletion(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardActionsCharacterMengDaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGoudeCompletedSlashChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCharacterZhangYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhangYanSujiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCharacterWeiYanHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyWeiYanIneffectiveResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreCardActionsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreCardActionsCharacterTaoQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZongluanUse(long id);
}

internal interface ICoreCardActionsCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSameNameHandCardObserver(long parentFrameId);
}

internal interface ICoreCardActionsCoreComparedBlackSlashPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasComparedBlackSlashPolicies { get; }
}

internal interface ICoreCardActionsCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateBiguoTargetContext(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary);
}

internal interface ICoreCardActionsCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
    bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId);
}

internal interface ICoreCardActionsCoreUniqueLeaderTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksUniqueLeaderTrickTargets { get; }

    void CaptureUniqueLeaderTrickQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardActionsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource? GetSelectedUseConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreCardActionsCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnRedSlashCapability { get; }
}

internal interface ICoreCardActionsCharacterLiuZhangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZonghuUse(long id);
}

internal interface ICoreCardActionsCoreDrawFundedDistinctBasicUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedDrawFundedDistinctBasicUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreCardActionsCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreCardActionsCharacterWuAnGuoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuAnGuoCapability { get; }
}

internal interface ICoreCardActionsCoreEquipmentRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginEquipmentRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreCardActionsCharacterZhaoXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanOfferMeiYingConversionDraw(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreCardActionsCorePairedColorResponseObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPairedColorDispositionCardObserver(long parentId);
}

internal interface ICoreCardActionsModeRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.MatchModeRuntime _modeRuntime { get; }
}

internal interface ICoreCardActionsCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation CapturedDeclarationOrigin(int cardId, global::CardGame.Core.CardLocation actual);
}

internal interface ICoreCardActionsCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuZhangFeiUseCapability { get; }

    global::CardGame.Core.CardActionContext CaptureWuZhangFeiUseRankAction(global::CardGame.Core.CardActionContext action);
    int? CaptureWuZhangFeiResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source);
    void BindWuZhangFeiResponseRanks(global::CardGame.Core.CardActionContext action);
    void CaptureWuZhangFeiUseQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardActionsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FangtianHalberdHandle? ActiveFangtianHalberd { get; set; }

    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }
}

internal interface ICoreCardActionsCoreCurrentUsePhysicalClaimsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentUsePhysicalCardClaim(long frameId, int cardId);
}

internal interface ICoreCardActionsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardActionsCharacterWangYunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWangYunPlanCapability { get; }

    bool HasWangYunTrickCapability { get; }

    void CompleteWangYunNativePayment(long useId);
}

internal interface ICoreCardActionsCharacterYuanShaoMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureMouYuanShaoDesignation(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouYuanShaoTargets(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreCardActionsCoreGuanduChariotResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureGuanduChariotResponsePayments(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.ProgramCardContinuation continuation);
    global::CardGame.Core.CardMovementRecord? GuanduChariotResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}

internal interface ICoreCardActionsCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHellDestroyedSoulMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDiouSuspendedCompletedAttack(global::CardGame.Core.CardUseFrame original, long? requiredWindowId = default);
}

internal interface ICoreCardActionsCharacterZouShiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLeZouShiCapability { get; }
}

internal interface ICoreCardActionsCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesTreatedWounded { get; }

    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardActionsCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseTargetIndex(long frameId, int targetIndex);
}

internal interface ICoreCardActionsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreCardActionsCoreLastZoneConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateLastZoneConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse);
}

internal interface ICoreCardActionsCharacterLuKangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuLuKangDixianCapability { get; }

    int? CaptureWuLuKangDixianDeclarationRank(int provider, global::System.Collections.Generic.IReadOnlyList<int> ids);
    bool IsWuLuKangKegouUse(long id);
}

internal interface ICoreCardActionsCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginMouSmPrivateWindow(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.MouSmPrivateCall call);
    bool IsMouSmPlayedWindow(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    global::CardGame.Core.MouSmPlayedReturn? MouSmPlayedWindowReturn(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
    bool TryReturnMouSmPrivateWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame p);
    bool TryReturnMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool ValidMouSmPlayedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool IsMouSmPlayedCostFinished(global::CardGame.Core.ProgramCardTriggerWindowFrame w, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreCardActionsCoreFinalizedCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFinalizedTargetProgram(global::CardGame.Core.CardKind kind);
}

internal interface ICoreCardActionsCharacterXinPiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasXinPiCardObserver(long useId);
    bool IsXinPiPhysicalClaim(long useId, int cardId);
}

internal interface ICoreCardActionsCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesQinLangXianyingUseAction(long id, global::CardGame.Core.CardActionContext a, int cardId);
}

internal interface ICoreCardActionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreCardActionsCoreEphemeralCardMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEphemeralCardDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCardActionsCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesMouZhijueUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAcceptedShouliMaterial(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool response);
    bool HasHengwuCapability { get; }
}

internal interface ICoreCardActionsCharacterLuXunWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuLuXunZhangcaiCapability { get; }

    int? CaptureWuLuXunResponseRank(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.CardConversionSource? source);
    void CaptureWuLuXunZhangcai(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardActionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCardActionsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsYouqueUse(long id);
    bool IsXieweiRemovedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    void ContinueSlashAfterDodgePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardActionsCharacterSunJianMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouSunJianOlSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCharacterZhouQunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhouQunCapability { get; }
}

internal interface ICoreCardActionsKernelCoreMainOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreCardActionsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesSelectedActorDuelAction(long frameId, global::CardGame.Core.CardActionContext action, int cardId);
    global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false);
}

internal interface ICoreCardActionsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardActionsCoreDamageTargetBudgetOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksDamageTargetBudgetTargets { get; }

    void SettleDamageMarkerBudgetTargets(long id);
}

internal interface ICoreCardActionsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttackAfterCardResolution(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion);
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardActionsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeProgramViewAsUsage(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> sources);
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    bool TracksPlayCardHistory { get; }

    global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card);
}

internal interface ICoreCardActionsCharacterBaoSanNiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesChosenAssaultUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsCharacterCaoPiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteKemingConverted(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied);
}

internal interface ICoreCardActionsModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreCardActionsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreCardActionsCharacterQuHuangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasQuHuangCapability { get; }
}

internal interface ICoreCardActionsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryMarkFinalizedTrickProgramsStarted(long frameId);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void ContinueSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
    void ContinueCommittedTrickUse(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
    bool TryMarkProgramUseAccepted(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreCardActionsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode);
    void ClearPendingDecision();
}

internal interface ICoreCardActionsCoreCompletedUndamagedTargetRevealObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? CompletedUndamagedTargetRevealObserverRoot();
}

internal interface ICoreCardActionsCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}

internal interface ICoreCardActionsCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasResponseUseCompletionObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    void ContinueCompletedResponseUse(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
    void ContinueNullificationAfterResponseUse(global::CardGame.Core.CardActionContext action);
    bool IsCompletedResponseUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCompletedResponseKind kind);
    bool IsCompletedResponseParentConsistent(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreCardActionsCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuanpeiUse(long id);
}

internal interface ICoreCardActionsCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWeijieUse(long id);
}

internal interface ICoreCardActionsCharacterWoLongFengChuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWoLongFengChuUseObserver(long id);
}

internal interface ICoreCardActionsCharacterLiuZanMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksAlternateActualUseColor { get; }

    void RecordAlternateActualUseColor(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardActionsCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICoreCardActionsCharacterTianChouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesTianChouShandaoUseAction(long id, global::CardGame.Core.CardActionContext action, int cardId);
}

internal interface ICoreCardActionsCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingRemovedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreCardUseDebitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseDebitIdentity? GetCardUseDebit(long actionId);
}

internal interface ICoreCardActionsCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteQigongNativePayment(long id);
    bool HasLvKuangLvXiangCardObserver(long originalUseId);
}

internal interface ICoreCardActionsCoreSoleTargetProtectionObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionCardObserver(long useId);
}

internal interface ICoreCardActionsCharacterLiuPanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuPanCapability { get; }

    global::CardGame.Core.ProgramSkillWindowContext CreatePijingIssuedContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    bool TryCompletePijingZeroTargetUse(global::CardGame.Core.ProgramCardTriggerWindowFrame window);
}

internal interface ICoreCardActionsCoreNativeCardUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopFinishedCardUse(long frameId);
    void CompleteFinishedAttackCardUse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardActionsCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CaoChunHandUsePayment? CaoChunPaidUseQualification(global::CardGame.Core.CardActionContext action);
    global::CardGame.Core.ProgramTriggerCandidate? CaoChunDebtCandidate(global::CardGame.Core.CaoChunHandUsePayment p);
    global::CardGame.Core.ProgramSkillWindowContext CaoChunDebtContext(global::CardGame.Core.CaoChunHandUsePayment p, long windowId);
}

internal interface ICoreCardActionsCharacterFeiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFeiYiHezhong { get; }
}

internal interface ICoreCardActionsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreCardActionsCharacterZhuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnZhuJunLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame frame);
}

internal interface ICoreCardActionsCoreAdjacentRedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAdjacentRedTargetCapability { get; }
}

internal interface ICoreCardActionsCharacterLuZhiMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouLzSuspendedCompletedParent(global::CardGame.Core.ProgramCardTriggerWindowFrame w);
}

internal interface ICoreCardActionsCoreKenshangPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKenshangRemovedMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCompleteSoleTargetQuarantinedUse(long id);
    bool IsSoleTargetQuarantinedMaterial(long id, int cardId);
}

internal interface ICoreCardActionsCharacterHuangfuSongWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWuHuangFuSongCapability { get; }

    global::CardGame.Core.CardActionContext CaptureWuHfUseAction(global::CardGame.Core.CardActionContext a);
}

internal interface ICoreCardActionsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureLeCaoZhiWenlanQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreCardActionsCharacterGuanYuHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyGuanYuHeartSlashCapability { get; }
}

internal interface ICoreCardActionsCharacterHuCheErAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryHuCheErClaimedUseMaterial(long useId, int cardId);
}

internal interface ICoreCardActionsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreCardActionsCharacterQiaoRuiAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasQiaoRuiCompletedUseObserver(long parentUseId);
}

internal interface ICoreCardActionsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId);
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreCardActionsCoreNativeRecastInvocationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeRecastInvocationDraft? CaptureNativeRecastInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> originalPrimaryIds, long? parentFrameId = default, int? instructionIndex = default, int? participantCursor = default, global::CardGame.Core.CardMoveReason? reason = default);
    void CompleteNativeRecastInvocation(global::CardGame.Core.NativeRecastInvocationDraft? draft, int actualDrawCount);
}
