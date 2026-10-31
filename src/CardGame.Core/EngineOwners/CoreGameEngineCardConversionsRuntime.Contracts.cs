#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardConversionsCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreCardConversionsTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreCardConversionsCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreCardConversionsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreCardConversionsCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment? UnclaimedDeclarationPayment(int provider, int cardId);
    bool CanDeclareCard(global::CardGame.Core.CharacterState owner, string skillId);
}

internal interface ICoreCardConversionsCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
    bool HasZhuqueFan(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreCardConversionsCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreCardConversionsCharacterPangTongAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? ShenPangTongLuansuoIdentity(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCoreOffTurnUsedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardConversionsCharacterXunXuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryXunXuBoqiaSource(global::CardGame.Core.CardConversionSource? s);
    bool TryFinishAnniversaryXunXuResponseMaterials(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreCardConversionsCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCuiYanNativeResponseForbidden(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind required, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
    bool HasCuiYanCapability { get; }
}

internal interface ICoreCardConversionsCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRedSlashProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false);
}

internal interface ICoreCardConversionsCorePhysicalMovementFacadesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0);
}

internal interface ICoreCardConversionsCharacterPangTongFriendOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendYangmingOriginalUseChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreCardConversionsCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreCardConversionsCharacterHuJinDingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesZhongshenRoundGain(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.SkillProgramViewAs rule);
}

internal interface ICoreCardConversionsCharacterPangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBiguoCategoryForbidden(int seat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreCardConversionsCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MouHuCheErEquipmentDisabled(int target);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuUsableWoodenOxGrain(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardConversionsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WuyouIdentity(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card);
    bool HasWuGuanYuJuewuCapability { get; }

    bool JuewuSlashAliasAllowed(global::CardGame.Core.CharacterState owner, bool forResponse);
    bool IsJuewuTrueSlashUse(global::CardGame.Core.CharacterState owner);
    bool JuewuNativeConversionMatchesOutput(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind);
    bool IsJuewuSource(global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreCardConversionsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false);
}

internal interface ICoreCardConversionsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasUnlimitedTurnRuleModifier(int actorSeat, global::CardGame.Core.SkillRuleQuery query, global::CardGame.Core.CardKind? effectiveCardKind = default);
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreCardConversionsCharacterCaoZhiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FuleSlashAliasAllowed(global::CardGame.Core.CharacterState owner, bool forResponse);
    void PrepareLeCaoZhiWenlanResponse(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
    bool FuleNativeConversionMatchesOutput(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind);
    bool FuleAllowsUnspecifiedForcedSlash(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreCardConversionsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardConversionsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCardConversionsCoreAlcoholIdentityPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAlcoholIdentityOwnedLocation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCharacterWangGuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? ShiluIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCoreWushuangOriginalSkillRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WushuangChoufaIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreCardConversionsCoreNativeAttackCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardConversionsCoreActionDiscardColorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card);
    bool? CaptureActionColor(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICoreCardConversionsCoreProgramCardLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false);
    global::CardGame.Core.ProgramCardLegality CardLegality { get; }

    bool IsProgramViewAsAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default);
    bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId);
}

internal interface ICoreCardConversionsCorePaidColorConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetPaidColorTurnDuelConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool forResponse, global::CardGame.Core.CardZoneKind zone);
}

internal interface ICoreCardConversionsCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? FreezeCompletedResponseUseSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind);
}

internal interface ICoreCardConversionsCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner);
    bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    void ValidateOwnPublicPileConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreCardConversionsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreCardConversionsCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> LeiluanSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool forResponse, bool ignoreSuitUseProhibition = false);
    void PrepareLeiluanResponsePayment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, long nativeUseId);
    void BindLeiluanResponsePayment(long nativeUseId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardConversionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCardConversionsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreCardConversionsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreCardConversionsCharacterGuanYuRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WeilinIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiLsHandAccess(int actor, int id);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendShiLsPlayableCards(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> own);
    bool IsShiLsAutonomousResponse(global::CardGame.Core.CardActionContext a);
}

internal interface ICoreCardConversionsCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuZglCopyBlocked(int actor, int id);
}

internal interface ICoreCardConversionsCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
    bool HasProgramUsedBasicCardThisTurn(int actorSeat, global::CardGame.Core.CardKind kind);
}

internal interface ICoreCardConversionsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId);
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreCardConversionsCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default);
}

internal interface ICoreCardConversionsCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind);
    global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind);
}

internal interface ICoreCardConversionsCharacterHouChengAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryHouChengWineCard(int cardId);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> AnniversaryHouChengWineSources(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool forResponse, bool dyingUse);
}

internal interface ICoreCardConversionsCoreAssistedFactionSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.AssistedFactionSlashCard> GetAssistedFactionSlashCards(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider);
}

internal interface ICoreCardConversionsCoreWenyiPeachSuppliesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWenyiPeachSupplyQuery(global::CardGame.Core.CharacterState actor, int target);
}

internal interface ICoreCardConversionsCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreCardConversionsCoreFriendTrioOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidWholeHandSuitConversion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreCardConversionsCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> WushuangJinzhiSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool response);
    bool IsWushuangJinzhiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreCardConversionsCoreRoundDistinctBasicUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseRoundDistinctBasicConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool forResponse, bool dyingUse = false);
    bool IsRoundDistinctBasicMaterial(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool MatchesRoundDistinctBasicMaterials(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramViewAs rule, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreCardConversionsCoreNativeMultiTargetActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0);
    void AddFangtianHalberdSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreCardConversionsCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
    bool IsTurnPhysicalUseForbiddenByExistingPolicies(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
    bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit);
}

internal interface ICoreCardConversionsCharacterChengJiChengCuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTousuiVariableSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.SkillProgramViewAs rule, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from);
}

internal interface ICoreCardConversionsCharacterMaChaoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ShouliIgnoresSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> ShouliFieldConversions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind output, bool response, bool dyingUse);
    bool HasHengwuCapability { get; }
}

internal interface ICoreCardConversionsCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreCardConversionsCoreGuanHaiBridgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreCardConversionsCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card WuJingLiubingUseCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion = default, global::System.Collections.Generic.IReadOnlyList<int>? ids = default, long? existingUseId = default);
}

internal interface ICoreCardConversionsCoreLastZoneConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetLastZoneJudgmentConversionCards(global::CardGame.Core.CharacterState owner, bool forResponse);
    bool IsLastZoneJudgmentCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueDuelAfterSuccessfulSlash(global::CardGame.Core.NativeOwners.DuelHandle duel, int responderSeat);
}

internal interface ICoreCardConversionsCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SiruiIgnoresSlashLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source);
    bool SiruiSlashAliasAllowed(global::CardGame.Core.CharacterState actor, bool forResponse);
    bool IsSiruiTrueResponseUse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool dyingUse = false, int? requestedUseTarget = default);
    bool SiruiNativeConversionMatchesOutput(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind);
    bool SiruiAllowsUnspecifiedForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
    bool IsSiruiSource(global::CardGame.Core.CardConversionSource? source);
    bool SiruiExplicitForcedSlashFrontier(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreCardConversionsCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileSlashUseCards(global::CardGame.Core.CharacterState provider);
    bool IsNativePublicPileOriginalChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool response);
}

internal interface ICoreCardConversionsCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}

internal interface ICoreCardConversionsCoreGiftedSlashUseRewardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? GetGiftedSlashIdentityMatch(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardConversionsCoreRecordedSuitSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRecordedSuitResponseAllowed(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind);
}

internal interface ICoreCardConversionsCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardConversionsCharacterZhangFeiWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PrepareWuZhangFeiResponseRanks(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? source, int? actorSeat = default);
    void BindWuZhangFeiResponseRanks(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardConversionsCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind EffectiveConversionInputKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCoreCuiYanNativeQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCuiYanAllowedFactionSlashVariant(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCharacterGongsunZanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> MouGszHuCards(global::CardGame.Core.CharacterState owner);
    bool MouGszHuHandSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones);
    bool PrepareMouGszHuResponse(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion, int? actor = default);
}

internal interface ICoreCardConversionsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardResponseCompletedObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreCardConversionsCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool XieweiNativeDirection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool forResponse);
    bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    void PrepareXieweiResponsePayment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, long nativeUseId, int? actorSeat = default);
    bool TryPayXieweiMaterials(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int providerSeat, global::CardGame.Core.CardLocation? sourceLocation, global::CardGame.Core.CardMoveReason reason);
    void BindXieweiResponsePayment(long nativeUseId, global::CardGame.Core.CardActionContext action);
    bool IsXieweiResponseRemovedMaterial(global::CardGame.Core.ProgramMultiCardViewAsSelection selection, int cardId);
}

internal interface ICoreCardConversionsCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreCardConversionsCharacterSunQuanAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryChouxiSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreCardConversionsCoreWushuangSkillsBOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WushuangBianyuIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreCardConversionsCharacterHanDangMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouHanDangPublishedResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool forResponse, bool dyingUse = false);
    bool IsMouHanDangNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, bool dyingUse = false);
}

internal interface ICoreCardConversionsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreCardConversionsCoreResponseEntityExchangeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsResponseEntityRestricted(int seat, int id);
    bool HasResponseEntityExchangeObservers();
}

internal interface ICoreCardConversionsCoreProgramDistanceRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramDistanceRules DistanceRules { get; }
}
