#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeAiDecisionsCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveExtendedViewAsResponse(global::CardGame.Core.PromptChoice choice);
    bool TryResolveExtendedAiResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreNativeAiDecisionsCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveRoundOnceCardlessForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance);
}

internal interface ICoreNativeAiDecisionsCoreNativeActionDispatchOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default);
}

internal interface ICoreNativeAiDecisionsCorePublicDamagePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PublicDamagePrediction PredictPublicActionDamage(global::CardGame.Core.GameSnapshot view, int viewerSeat, global::CardGame.Core.LegalAction action, int targetSeat);
    global::CardGame.Core.PublicDamagePrediction PredictPublicIncomingDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int viewerSeat, int? sourceSeat = default, int? targetSeat = default, int? damageAmount = default);
}

internal interface ICoreNativeAiDecisionsCoreActualDiscardPhaseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSkipDiscardPolicyChoice(bool useSkill);
}

internal interface ICoreNativeAiDecisionsCharacterSimaYiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HoldMouSmAiAttack(int source, int target);
}

internal interface ICoreNativeAiDecisionsCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreNativeAiDecisionsCoreNativeFactionDefenseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFactionDefenseCandidateResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId);
    void BeginFactionDefenseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAiDecisionsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureAiCardResponseChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetFactionSlashSlashCards(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider);
    global::CardGame.Core.CardKind GetFactionSlashEffectiveSlashKind(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card);
    void ResolveDuelProgramMultiCardResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    void ResolveGroupProgramMultiCardResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeAiDecisionsCoreColorFireAttacksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveAiColorFireAttack(global::CardGame.Core.CardUseFrame pending, global::CardGame.Core.PendingDecision decision);
}

internal interface ICoreNativeAiDecisionsCoreNativeFireAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFireAttackReveal(global::CardGame.Core.CardUseFrame pending, int cardId);
    global::CardGame.Core.Card GetFireAttackRevealedCard(global::CardGame.Core.CardUseFrame pending);
    void ResolveFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId);
}

internal interface ICoreNativeAiDecisionsCoreMultiplayerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHumanControlledSeat(int seat);
}

internal interface ICoreNativeAiDecisionsCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveTieredRoundZeroForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance);
}

internal interface ICoreNativeAiDecisionsCoreNativeGroupResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse);
    void ResolveGroupZhangbaResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
}

internal interface ICoreNativeAiDecisionsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreNativeAiDecisionsCoreFactionRequestPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFactionRequestCostChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
}

internal interface ICoreNativeAiDecisionsCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash);
    void ResolveDuelZhangbaResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
}

internal interface ICoreNativeAiDecisionsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    bool IsAiSkipDiscardPolicyPending();
}

internal interface ICoreNativeAiDecisionsCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind PhysicalSlashUseBaseKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeAiDecisionsCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreNativeAiDecisionsCoreNativeHarvestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState picker, int cardId);
}

internal interface ICoreNativeAiDecisionsCoreNativeIceSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveIceSwordChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeAiDecisionsCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }
}

internal interface ICoreNativeAiDecisionsCoreNativeWeaponResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveCixiongDoubleSwordsChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeAiDecisionsCoreNativeNullificationFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetNullificationEffectCard(global::CardGame.Core.NullificationWindowFrame frame);
}

internal interface ICoreNativeAiDecisionsCoreFriendDuelResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FriendDuelResponseChoices(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder);
    void ResolveFriendDuelDiscard(global::CardGame.Core.PromptChoice supplied);
}

internal interface ICoreNativeAiDecisionsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreNativeAiDecisionsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.QilinBowHandle? ActiveQilinBow { get; set; }

    global::CardGame.Core.NativeOwners.IceSwordHandle? ActiveIceSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }

    global::CardGame.Core.NativeOwners.CixiongDoubleSwordsHandle? ActiveCixiongDoubleSwords { get; set; }

    global::CardGame.Core.NativeOwners.StoneAxeHandle? ActiveStoneAxe { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get; }

    global::CardGame.Core.CardUseFrame? ActiveFireAttack { get; }
}

internal interface ICoreNativeAiDecisionsCoreNativeTrickEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveTargetCardSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot);
}

internal interface ICoreNativeAiDecisionsCoreSnapshotValueProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardSnapshot ToSnapshot(global::CardGame.Core.Card card);
}

internal interface ICoreNativeAiDecisionsCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFactionSlashZhuqueFanChoice(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool convertToFireSlash);
    void BeginBorrowedSwordFactionSlashRequest(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowedSword);
    void ResolveFactionSlashZhangbaCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
    void ResolveFactionSlashProgramMultiCardCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    void ResolveFactionSlashCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
    void BeginFactionSlashResponseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAiDecisionsCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    bool CanUseCardAsResponse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind requiredCardKind);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreNativeAiDecisionsCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginProgramDodgeResponse(global::CardGame.Core.PromptChoice choice, bool advanceToHumanBoundary);
}

internal interface ICoreNativeAiDecisionsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? ResolveBaguaJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender);
    void CompleteBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded);
}

internal interface ICoreNativeAiDecisionsCharacterXiaoQiaoMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BeginMouXqForcedSlash(global::CardGame.Core.PromptChoice choice);
    bool TryMouXqJueyanAiResponse(global::CardGame.Core.CardKind kind, bool dying = false);
}

internal interface ICoreNativeAiDecisionsCoreAssistedFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetFactionRequestZhangbaPairs(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetFactionRequestMultiCardSelections(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider);
    void ResolveAssistedFactionSlashChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary);
}

internal interface ICoreNativeAiDecisionsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind FuleForcedAiSlashKind(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.Card card, global::CardGame.Core.CardKind fallback);
}

internal interface ICoreNativeAiDecisionsCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasConditionalVirtualBaguaForAi(global::CardGame.Core.CharacterState player);
    bool HasBagua(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeAiDecisionsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBorrowedSwordSlashCards(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState slashTarget);
    void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
    void ResolveBorrowedSwordZhangbaSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
    void ResolveBorrowedSwordProgramMultiCardSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon);
}

internal interface ICoreNativeAiDecisionsCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICoreNativeAiDecisionsCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProvenanceCounterspellMaterials(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> validIds);
    bool HasProvenanceGrainCounterspellMaterials(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> validIds);
}

internal interface ICoreNativeAiDecisionsCoreNativeStoneAxeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveStoneAxeChoice(global::System.Collections.Generic.IReadOnlyList<int> discardedCardIds);
}

internal interface ICoreNativeAiDecisionsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeAiDecisionsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetNullificationCards(global::CardGame.Core.CharacterState responder);
    void ValidateNullificationConversion(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? selectedSource);
    void ResolveNullificationChoice(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedCard, global::CardGame.Core.CardConversionSource? selectedConversionSource = default);
}

internal interface ICoreNativeAiDecisionsCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard);
}

internal interface ICoreNativeAiDecisionsCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
    void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreNativeAiDecisionsCoreRoundOnceCardlessReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveRoundOnceCardlessAiDodge();
}

internal interface ICoreNativeAiDecisionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreNativeAiDecisionsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreNativeAiDecisionsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAiDecisionsCoreNativeQilinBowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveQilinBowChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeAiDecisionsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteCurrentPlayPhase();
}

internal interface ICoreNativeAiDecisionsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool TryCompleteXieweiFactionDefense(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreNativeAiDecisionsCharacterMaXiuMaTieOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KenshangBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
    bool TryBeginKenshangBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeAiDecisionsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreNativeAiDecisionsCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreNativeAiDecisionsCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveTieredRoundZeroAiCounterspell();
    bool TryResolveTieredRoundZeroAiDodge();
}

internal interface ICoreNativeAiDecisionsCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveQinglongCrescentBladeChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeAiDecisionsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreNativeAiDecisionsCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending);
    bool TryBeginTousuiBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreNativeAiDecisionsCoreConvertedSlashResponseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? ConvertedSlashSameColorResponseColor(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreNativeAiDecisionsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSurvivalMode { get; }

    bool IsNationalWarMode { get; }

    bool IsTeamMode { get; }
}
