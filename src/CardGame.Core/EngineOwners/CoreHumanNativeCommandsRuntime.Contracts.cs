#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreHumanNativeCommandsCoreNativeActionDispatchOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default);
}

internal interface ICoreHumanNativeCommandsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreHumanNativeCommandsCoreMultiplayerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int HumanDecisionSeat { get; }
}

internal interface ICoreHumanNativeCommandsCoreNativeStoneAxeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveStoneAxeChoice(global::System.Collections.Generic.IReadOnlyList<int> discardedCardIds);
}

internal interface ICoreHumanNativeCommandsCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? FindZhangbaSlashPair(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds);
}

internal interface ICoreHumanNativeCommandsCoreNativeDamageApplicationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ApplyAttackDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreHumanNativeCommandsCoreNativeWeaponResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveCixiongDoubleSwordsChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreHumanNativeCommandsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? ResolveBaguaJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender);
    void CompleteBaguaResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool succeeded);
}

internal interface ICoreHumanNativeCommandsCoreNativeFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginFactionSlashResponseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ResolveFactionSlashCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
    void ResolveFactionSlashZhuqueFanChoice(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, bool convertToFireSlash);
    void ResolveFactionSlashZhangbaCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
    void ResolveFactionSlashProgramMultiCardCandidateResponse(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    void BeginBorrowedSwordFactionSlashRequest(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowedSword);
}

internal interface ICoreHumanNativeCommandsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreHumanNativeCommandsCoreNativeFireAttackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveFireAttackReveal(global::CardGame.Core.CardUseFrame pending, int cardId);
    void ResolveFireAttackDiscard(global::CardGame.Core.CardUseFrame pending, int? selectedCardId);
}

internal interface ICoreHumanNativeCommandsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequireHumanDecision(global::CardGame.Core.DecisionKind expected);
    void EnsureStarted();
    void ClearPendingDecision();
    void RefreshNationalResponseDecision();
}

internal interface ICoreHumanNativeCommandsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }

    global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get; }

    global::CardGame.Core.TargetCardSelectionFrame? ActiveTargetCardSelection { get; }

    global::CardGame.Core.CardUseFrame? ActiveFireAttack { get; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreHumanNativeCommandsCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDuelZhangbaResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
    void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash);
}

internal interface ICoreHumanNativeCommandsCoreNativeIceSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveIceSwordChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreHumanNativeCommandsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveBorrowedSwordZhangbaSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
    void ResolveBorrowedSwordProgramMultiCardSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBorrowedSwordSlashCards(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState slashTarget);
    void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default);
    void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon);
}

internal interface ICoreHumanNativeCommandsCoreNativeNullificationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetNullificationCards(global::CardGame.Core.CharacterState responder);
    void ValidateNullificationConversion(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? selectedSource);
    void ResolveNullificationChoice(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedCard, global::CardGame.Core.CardConversionSource? selectedConversionSource = default);
}

internal interface ICoreHumanNativeCommandsCoreNativeWoodenOxOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveWoodenOx(global::CardGame.Core.CharacterState actor, int storedCardId, int targetSeat);
}

internal interface ICoreHumanNativeCommandsCoreProgramAlcoholConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateAlcoholConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool forResponse);
}

internal interface ICoreHumanNativeCommandsCoreNativeCardUseFramesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreHumanNativeCommandsCoreActualDiscardPhaseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSkipDiscardPolicyChoice(bool useSkill);
}

internal interface ICoreHumanNativeCommandsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreHumanNativeCommandsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
}

internal interface ICoreHumanNativeCommandsCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveQinglongCrescentBladeChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreHumanNativeCommandsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveZhangbaSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards);
}

internal interface ICoreHumanNativeCommandsCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card[] GetDyingPeaches(global::CardGame.Core.CharacterState responder);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetDyingPeachConversionSources(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card);
    global::CardGame.Core.Card[] GetDyingAlcohols(global::CardGame.Core.CharacterState responder, int victimSeat);
    void ApplyDyingResponse(global::CardGame.Core.CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default);
}

internal interface ICoreHumanNativeCommandsCoreNativeTrickEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveTargetCardSelection(global::CardGame.Core.TargetCardSelectionFrame pending, int slot);
}

internal interface ICoreHumanNativeCommandsCoreNativeFactionDefenseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginFactionDefenseRequest(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ResolveFactionDefenseCandidateResponse(global::CardGame.Core.NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId);
}

internal interface ICoreHumanNativeCommandsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreHumanNativeCommandsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreHumanNativeCommandsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNationalWarMode { get; }
}

internal interface ICoreHumanNativeCommandsCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
    void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreHumanNativeCommandsKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreHumanNativeCommandsCoreNativeHarvestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveHarvestSelection(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState picker, int cardId);
}

internal interface ICoreHumanNativeCommandsCoreNativeGroupResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveGroupZhangbaResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair);
    void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse);
}

internal interface ICoreHumanNativeCommandsCoreNativeQilinBowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveQilinBowChoice(global::CardGame.Core.PromptChoice selected);
}

internal interface ICoreHumanNativeCommandsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteCurrentPlayPhase();
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> GetDyingProgramCandidates(global::CardGame.Core.CharacterState responder, global::CardGame.Core.DyingFrame dying);
    void BeginDyingProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.DyingFrame dying);
}

internal interface ICoreHumanNativeCommandsCoreMatchParticipantSetupOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ApplyGeneralSelection(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general);
    void ApplyGodFactionSelection(global::CardGame.Core.CharacterState player, string factionId);
}

internal interface ICoreHumanNativeCommandsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreHumanNativeCommandsCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBagua(global::CardGame.Core.CharacterState player);
}

internal interface ICoreHumanNativeCommandsCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CharacterState defender, global::CardGame.Core.Card responseCard);
}

internal interface ICoreHumanNativeCommandsCoreNationalGeneralRevealOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildNationalRevealActions(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreHumanNativeCommandsCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source);
    void ResolveGroupProgramMultiCardResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    void ResolveDuelProgramMultiCardResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreHumanNativeCommandsCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveNextActualUseZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}
