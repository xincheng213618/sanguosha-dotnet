#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime))]
internal sealed class CoreCardConversionsCoreNativeDodgeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeDodgeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _targetFactory = targetFactory;
    public void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().ContinueAcceptedCardResponse(attack, action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CoreCardConversionsCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => this._targetFactory().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreCardConversionsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
    public global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ApplyProgramUseAppearance(owner, card, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreCardConversionsCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeclaredCardPayment? UnclaimedDeclarationPayment(int provider, int cardId) => this._targetFactory().UnclaimedDeclarationPayment(provider, cardId);
    public bool CanDeclareCard(global::CardGame.Core.CharacterState owner, string skillId) => this._targetFactory().CanDeclareCard(owner, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreCardConversionsCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => this._targetFactory().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
    public bool HasZhuqueFan(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasZhuqueFan(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime))]
internal sealed class CoreCardConversionsCoreNativeSkillContextQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeSkillContextQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSkillContextQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => this._targetFactory().CreateSkillContext(player, includeHandLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime))]
internal sealed class CoreCardConversionsCharacterPangTongAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterPangTongAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? ShenPangTongLuansuoIdentity(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card) => this._targetFactory().ShenPangTongLuansuoIdentity(holder, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime))]
internal sealed class CoreCardConversionsCoreOffTurnUsedCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreOffTurnUsedCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOffTurnUsedCardsRuntime> _targetFactory = targetFactory;
    public void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureOffTurnResponseUseOrigin(owningUseId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime))]
internal sealed class CoreCardConversionsCharacterXunXuAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterXunXuAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunXuAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryXunXuBoqiaSource(global::CardGame.Core.CardConversionSource? s) => this._targetFactory().IsAnniversaryXunXuBoqiaSource(s);
    public bool TryFinishAnniversaryXunXuResponseMaterials(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().TryFinishAnniversaryXunXuResponseMaterials(selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime))]
internal sealed class CoreCardConversionsCharacterCuiYanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterCuiYanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCuiYanOlRuntime> _targetFactory = targetFactory;
    public bool IsCuiYanNativeResponseForbidden(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind required, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().IsCuiYanNativeResponseForbidden(owner, required, cards);
    public bool HasCuiYanCapability { get => this._targetFactory().HasCuiYanCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime))]
internal sealed class CoreCardConversionsCoreTurnRedSlashBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreTurnRedSlashBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnRedSlashBenefitsRuntime> _targetFactory = targetFactory;
    public bool IsRedSlashProviderPaymentLegal(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, bool isTrueZhangba = false) => this._targetFactory().IsRedSlashProviderPaymentLegal(pending, kind, cards, isTrueZhangba);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreCardConversionsCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime))]
internal sealed class CoreCardConversionsCharacterPangTongFriendAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterPangTongFriendOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> _targetFactory = targetFactory;
    public bool IsFriendYangmingOriginalUseChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().IsFriendYangmingOriginalUseChoice(owner, card, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreCardConversionsCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get => this._targetFactory().ActiveDuel; set => this._targetFactory().ActiveDuel = value; }
    public global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get => this._targetFactory().ActiveGroupCard; set => this._targetFactory().ActiveGroupCard = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuJinDingOlRuntime))]
internal sealed class CoreCardConversionsCharacterHuJinDingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuJinDingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterHuJinDingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuJinDingOlRuntime> _targetFactory = targetFactory;
    public bool MatchesZhongshenRoundGain(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.SkillProgramViewAs rule) => this._targetFactory().MatchesZhongshenRoundGain(owner, card, rule);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime))]
internal sealed class CoreCardConversionsCharacterPangJiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterPangJiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangJiOlRuntime> _targetFactory = targetFactory;
    public bool IsBiguoCategoryForbidden(int seat, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().IsBiguoCategoryForbidden(seat, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime))]
internal sealed class CoreCardConversionsCharacterHuCheErMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterHuCheErMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErMouRuntime> _targetFactory = targetFactory;
    public bool MouHuCheErEquipmentDisabled(int target) => this._targetFactory().MouHuCheErEquipmentDisabled(target);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuUsableWoodenOxGrain(global::CardGame.Core.CharacterState player) => this._targetFactory().KongwuUsableWoodenOxGrain(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreCardConversionsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WuyouIdentity(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card) => this._targetFactory().WuyouIdentity(holder, card);
    public bool HasWuGuanYuJuewuCapability { get => this._targetFactory().HasWuGuanYuJuewuCapability; }

    public bool JuewuSlashAliasAllowed(global::CardGame.Core.CharacterState owner, bool forResponse) => this._targetFactory().JuewuSlashAliasAllowed(owner, forResponse);
    public bool IsJuewuTrueSlashUse(global::CardGame.Core.CharacterState owner) => this._targetFactory().IsJuewuTrueSlashUse(owner);
    public bool JuewuNativeConversionMatchesOutput(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => this._targetFactory().JuewuNativeConversionMatchesOutput(owner, source, kind);
    public bool IsJuewuSource(global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsJuewuSource(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreCardConversionsCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => this._targetFactory().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().GetProgramCardIdentityMatches(owner, card);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false) => this._targetFactory().GetSlashUseCards(owner, ignoreSuitUseProhibition);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreCardConversionsCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool HasUnlimitedTurnRuleModifier(int actorSeat, global::CardGame.Core.SkillRuleQuery query, global::CardGame.Core.CardKind? effectiveCardKind = default) => this._targetFactory().HasUnlimitedTurnRuleModifier(actorSeat, query, effectiveCardKind);
    public bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCardRestricted(player, card);
    public bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => this._targetFactory().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime))]
internal sealed class CoreCardConversionsCharacterCaoZhiLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterCaoZhiLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhiLeRuntime> _targetFactory = targetFactory;
    public bool FuleSlashAliasAllowed(global::CardGame.Core.CharacterState owner, bool forResponse) => this._targetFactory().FuleSlashAliasAllowed(owner, forResponse);
    public void PrepareLeCaoZhiWenlanResponse(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().PrepareLeCaoZhiWenlanResponse(provider, kind, cards);
    public bool FuleNativeConversionMatchesOutput(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => this._targetFactory().FuleNativeConversionMatchesOutput(owner, source, kind);
    public bool FuleAllowsUnspecifiedForcedSlash(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().FuleAllowsUnspecifiedForcedSlash(owner, card, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreCardConversionsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPlayableCards(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime))]
internal sealed class CoreCardConversionsCoreAlcoholIdentityPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreAlcoholIdentityPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholIdentityPoliciesRuntime> _targetFactory = targetFactory;
    public bool IsAlcoholIdentityOwnedLocation(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsAlcoholIdentityOwnedLocation(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime))]
internal sealed class CoreCardConversionsCharacterWangGuanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterWangGuanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangGuanOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? ShiluIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().ShiluIdentity(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime))]
internal sealed class CoreCardConversionsCoreWushuangOriginalSkillRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreWushuangOriginalSkillRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WushuangChoufaIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().WushuangChoufaIdentity(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime))]
internal sealed class CoreCardConversionsCoreProgramCardUseRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramCardUseRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRulesRuntime> _targetFactory = targetFactory;
    public void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().AddProgramTargetCountSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreCardConversionsCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime))]
internal sealed class CoreCardConversionsCoreActionDiscardColorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreActionDiscardColorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionDiscardColorsRuntime> _targetFactory = targetFactory;
    public bool? CapturePhysicalCardColor(int seat, global::CardGame.Core.Card card) => this._targetFactory().CapturePhysicalCardColor(seat, card);
    public bool? CaptureActionColor(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionCost> costs, global::CardGame.Core.Suit? effectiveSuit = default) => this._targetFactory().CaptureActionColor(costs, effectiveSuit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime))]
internal sealed class CoreCardConversionsCoreProgramCardLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramCardLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramCardLegalityRuntime> _targetFactory = targetFactory;
    public bool IsProgramMaterialUseForbidden(int actorSeat, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IEnumerable<int> cardIds, bool response = false) => this._targetFactory().IsProgramMaterialUseForbidden(actorSeat, kind, cardIds, response);
    public global::CardGame.Core.ProgramCardLegality CardLegality { get => this._targetFactory().CardLegality; }

    public bool IsProgramViewAsAllowed(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardConversionSource source, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => this._targetFactory().IsProgramViewAsAllowed(owner, kind, cardIds, source, forResponse, dyingUse, requestedUseTarget);
    public bool IsProgramEntityPhysicalRestricted(int actorSeat, int cardId) => this._targetFactory().IsProgramEntityPhysicalRestricted(actorSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime))]
internal sealed class CoreCardConversionsCorePaidColorConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCorePaidColorConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidColorConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetPaidColorTurnDuelConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool forResponse, global::CardGame.Core.CardZoneKind zone) => this._targetFactory().GetPaidColorTurnDuelConversions(owner, card, kind, forResponse, zone);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime))]
internal sealed class CoreCardConversionsCoreResponseUseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreResponseUseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseUseCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? FreezeCompletedResponseUseSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind) => this._targetFactory().FreezeCompletedResponseUseSuit(owner, cards, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime))]
internal sealed class CoreCardConversionsCoreOwnPublicPileConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreOwnPublicPileConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnPublicPileConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner) => this._targetFactory().GetOwnPublicPileConversionCards(owner);
    public bool IsOwnPublicPileCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsOwnPublicPileCard(owner, card);
    public void ValidateOwnPublicPileConversionCommit(global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, bool forResponse, global::CardGame.Core.CardKind? effectiveKind = default) => this._targetFactory().ValidateOwnPublicPileConversionCommit(source, provider, materials, forResponse, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreCardConversionsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime))]
internal sealed class CoreCardConversionsCharacterKongShuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterKongShuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> LeiluanSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool forResponse, bool ignoreSuitUseProhibition = false) => this._targetFactory().LeiluanSelections(owner, kind, forResponse, ignoreSuitUseProhibition);
    public void PrepareLeiluanResponsePayment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, long nativeUseId) => this._targetFactory().PrepareLeiluanResponsePayment(owner, selection, nativeUseId);
    public void BindLeiluanResponsePayment(long nativeUseId, global::CardGame.Core.CardActionContext action) => this._targetFactory().BindLeiluanResponsePayment(nativeUseId, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreCardConversionsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime))]
internal sealed class CoreCardConversionsCharacterGuanYuRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterGuanYuRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WeilinIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().WeilinIdentity(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed class CoreCardConversionsCharacterLuSuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterLuSuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiLsHandAccess(int actor, int id) => this._targetFactory().IsShiLsHandAccess(actor, id);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendShiLsPlayableCards(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> own) => this._targetFactory().AppendShiLsPlayableCards(actor, own);
    public bool IsShiLsAutonomousResponse(global::CardGame.Core.CardActionContext a) => this._targetFactory().IsShiLsAutonomousResponse(a);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed class CoreCardConversionsCharacterZhugeLiangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterZhugeLiangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _targetFactory = targetFactory;
    public bool IsWuZglCopyBlocked(int actor, int id) => this._targetFactory().IsWuZglCopyBlocked(actor, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreCardConversionsCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind) => this._targetFactory().IsProgramResponseCardUse(responder, effectiveKind);
    public bool HasProgramUsedBasicCardThisTurn(int actorSeat, global::CardGame.Core.CardKind kind) => this._targetFactory().HasProgramUsedBasicCardThisTurn(actorSeat, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreCardConversionsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().HasRuntimeSkill(player, skillId);
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreCardConversionsCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ResolveSlashCore(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, int physicalOwnerSeat, global::CardGame.Core.NativeOwners.FactionCardRequestHandle? factionRequest = default, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowedSword = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, bool countsTowardSlashLimit = true, bool usesZhuqueFan = false, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default, long? programSkillCardUseFrameId = default, global::CardGame.Core.CardLocation? physicalSourceLocation = default) => this._targetFactory().ResolveSlashCore(source, target, slash, playedCardKind, physicalOwnerSeat, factionRequest, borrowedSword, physicalCards, countsTowardSlashLimit, usesZhuqueFan, conversionSource, additionalConversionSources, programSkillCardUseFrameId, physicalSourceLocation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed class CoreCardConversionsCoreNativeResponseMaterialQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeResponseMaterialQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetResponseCards(responder, requiredCardKind);
    public global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime))]
internal sealed class CoreCardConversionsCharacterHouChengAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterHouChengAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHouChengAnniversaryRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryHouChengWineCard(int cardId) => this._targetFactory().IsAnniversaryHouChengWineCard(cardId);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> AnniversaryHouChengWineSources(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool forResponse, bool dyingUse) => this._targetFactory().AnniversaryHouChengWineSources(owner, card, kind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime))]
internal sealed class CoreCardConversionsCoreAssistedFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreAssistedFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAssistedFactionSlashRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.AssistedFactionSlashCard> GetAssistedFactionSlashCards(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending, global::CardGame.Core.CharacterState provider) => this._targetFactory().GetAssistedFactionSlashCards(pending, provider);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime))]
internal sealed class CoreCardConversionsCoreWenyiPeachSuppliesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreWenyiPeachSuppliesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenyiPeachSuppliesRuntime> _targetFactory = targetFactory;
    public bool IsWenyiPeachSupplyQuery(global::CardGame.Core.CharacterState actor, int target) => this._targetFactory().IsWenyiPeachSupplyQuery(actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreCardConversionsCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime))]
internal sealed class CoreCardConversionsCoreFriendTrioAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreFriendTrioOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> _targetFactory = targetFactory;
    public bool ValidWholeHandSuitConversion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource source) => this._targetFactory().ValidWholeHandSuitConversion(owner, cards, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime))]
internal sealed class CoreCardConversionsCoreWushuangJinzhiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreWushuangJinzhiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> WushuangJinzhiSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool response) => this._targetFactory().WushuangJinzhiSelections(owner, kind, response);
    public bool IsWushuangJinzhiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().IsWushuangJinzhiSelection(selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime))]
internal sealed class CoreCardConversionsCoreRoundDistinctBasicUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreRoundDistinctBasicUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> _targetFactory = targetFactory;
    public bool CanUseRoundDistinctBasicConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool forResponse, bool dyingUse = false) => this._targetFactory().CanUseRoundDistinctBasicConversion(owner, instance, rule, forResponse, dyingUse);
    public bool IsRoundDistinctBasicMaterial(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsRoundDistinctBasicMaterial(owner, card);
    public bool MatchesRoundDistinctBasicMaterials(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramViewAs rule, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().MatchesRoundDistinctBasicMaterials(owner, rule, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime))]
internal sealed class CoreCardConversionsCoreNativeMultiTargetActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeMultiTargetActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeMultiTargetActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0) => this._targetFactory().GetFangtianOrderedSlashTargets(actor, slash, conversionSource, ignoresSlashLimit, effectiveKind);
    public void AddFangtianHalberdSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().AddFangtianHalberdSlashActions(actions, actor, physicalCard, legalTargets, slashName, playedCardKind, conversionSource, additionalConversionSources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreCardConversionsCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().IsTurnPhysicalUseForbidden(seat, ids);
    public bool IsTurnPhysicalUseForbiddenByExistingPolicies(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().IsTurnPhysicalUseForbiddenByExistingPolicies(seat, ids);
    public bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit) => this._targetFactory().IsTurnSuitUseForbidden(seat, suit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime))]
internal sealed class CoreCardConversionsCharacterChengJiChengCuiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterChengJiChengCuiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengJiChengCuiOlRuntime> _targetFactory = targetFactory;
    public bool IsTousuiVariableSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.SkillProgramViewAs rule, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => this._targetFactory().IsTousuiVariableSource(actor, rule, card, from);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed class CoreCardConversionsCharacterMaChaoAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterMaChaoAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool ShouliIgnoresSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ShouliIgnoresSlashLimit(owner, source);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> ShouliFieldConversions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind output, bool response, bool dyingUse) => this._targetFactory().ShouliFieldConversions(actor, card, output, response, dyingUse);
    public bool HasHengwuCapability { get => this._targetFactory().HasHengwuCapability; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreCardConversionsCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation FindOwnedCardLocation(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().FindOwnedCardLocation(actor, card);
    public void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind) => this._targetFactory().MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actorSeat, effectiveCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime))]
internal sealed class CoreCardConversionsCoreGuanHaiBridgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreGuanHaiBridgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardSuit(owner, selection);
    public bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardColor(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreCardConversionsCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card WuJingLiubingUseCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion = default, global::System.Collections.Generic.IReadOnlyList<int>? ids = default, long? existingUseId = default) => this._targetFactory().WuJingLiubingUseCard(actor, card, kind, conversion, ids, existingUseId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime))]
internal sealed class CoreCardConversionsCoreLastZoneConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreLastZoneConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetLastZoneJudgmentConversionCards(global::CardGame.Core.CharacterState owner, bool forResponse) => this._targetFactory().GetLastZoneJudgmentConversionCards(owner, forResponse);
    public bool IsLastZoneJudgmentCard(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsLastZoneJudgmentCard(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime))]
internal sealed class CoreCardConversionsCoreNativeDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> _targetFactory = targetFactory;
    public void ContinueDuelAfterSuccessfulSlash(global::CardGame.Core.NativeOwners.DuelHandle duel, int responderSeat) => this._targetFactory().ContinueDuelAfterSuccessfulSlash(duel, responderSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime))]
internal sealed class CoreCardConversionsCharacterMiHengLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterMiHengLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMiHengLeRuntime> _targetFactory = targetFactory;
    public bool SiruiIgnoresSlashLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().SiruiIgnoresSlashLimit(actor, source);
    public bool SiruiSlashAliasAllowed(global::CardGame.Core.CharacterState actor, bool forResponse) => this._targetFactory().SiruiSlashAliasAllowed(actor, forResponse);
    public bool IsSiruiTrueResponseUse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool dyingUse = false, int? requestedUseTarget = default) => this._targetFactory().IsSiruiTrueResponseUse(owner, kind, dyingUse, requestedUseTarget);
    public bool SiruiNativeConversionMatchesOutput(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => this._targetFactory().SiruiNativeConversionMatchesOutput(actor, source, kind);
    public bool SiruiAllowsUnspecifiedForcedSlash(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().SiruiAllowsUnspecifiedForcedSlash(actor, card, kind);
    public bool IsSiruiSource(global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsSiruiSource(source);
    public bool SiruiExplicitForcedSlashFrontier(global::CardGame.Core.CharacterState actor) => this._targetFactory().SiruiExplicitForcedSlashFrontier(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreCardConversionsCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileSlashUseCards(global::CardGame.Core.CharacterState provider) => this._targetFactory().NativePublicPileSlashUseCards(provider);
    public bool IsNativePublicPileOriginalChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool response) => this._targetFactory().IsNativePublicPileOriginalChoice(owner, card, kind, response);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreCardConversionsCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime))]
internal sealed class CoreCardConversionsCoreGiftedSlashUseRewardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreGiftedSlashUseRewardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGiftedSlashUseRewardsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? GetGiftedSlashIdentityMatch(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().GetGiftedSlashIdentityMatch(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime))]
internal sealed class CoreCardConversionsCoreRecordedSuitSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreRecordedSuitSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecordedSuitSlashRuntime> _targetFactory = targetFactory;
    public bool IsRecordedSuitResponseAllowed(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind) => this._targetFactory().IsRecordedSuitResponseAllowed(responder, cards, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed class CoreCardConversionsCoreActualPlayPhaseCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreActualPlayPhaseCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _targetFactory = targetFactory;
    public void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action) => this._targetFactory().RecordActualPlayPhaseUse(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime))]
internal sealed class CoreCardConversionsCharacterZhangFeiWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterZhangFeiWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiWumiaoRuntime> _targetFactory = targetFactory;
    public void PrepareWuZhangFeiResponseRanks(global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource? source, int? actorSeat = default) => this._targetFactory().PrepareWuZhangFeiResponseRanks(provider, kind, cards, source, actorSeat);
    public void BindWuZhangFeiResponseRanks(global::CardGame.Core.CardActionContext action) => this._targetFactory().BindWuZhangFeiResponseRanks(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime))]
internal sealed class CoreCardConversionsCoreWenhePoisonAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreWenhePoisonOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenhePoisonRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardKind EffectiveConversionInputKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveConversionInputKind(owner, card);
    public bool IsWenhePhysicalPoison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().IsWenhePhysicalPoison(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime))]
internal sealed class CoreCardConversionsCoreCuiYanNativeQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreCuiYanNativeQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCuiYanNativeQueriesRuntime> _targetFactory = targetFactory;
    public bool HasCuiYanAllowedFactionSlashVariant(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card) => this._targetFactory().HasCuiYanAllowedFactionSlashVariant(request, provider, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime))]
internal sealed class CoreCardConversionsCharacterGongsunZanMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterGongsunZanMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id) => this._targetFactory().IsMouGszHuCard(owner, id);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> MouGszHuCards(global::CardGame.Core.CharacterState owner) => this._targetFactory().MouGszHuCards(owner);
    public bool MouGszHuHandSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones) => this._targetFactory().MouGszHuHandSource(owner, card, zones);
    public bool PrepareMouGszHuResponse(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion, int? actor = default) => this._targetFactory().PrepareMouGszHuResponse(provider, cards, kind, conversion, actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreCardConversionsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime))]
internal sealed class CoreCardConversionsCoreResponseCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreResponseCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseCompletionRuntime> _targetFactory = targetFactory;
    public bool HasCardResponseCompletedObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => this._targetFactory().HasCardResponseCompletedObserver(action, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime))]
internal sealed class CoreCardConversionsCharacterGuanHaiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterGuanHaiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanHaiOlRuntime> _targetFactory = targetFactory;
    public bool XieweiNativeDirection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, bool forResponse) => this._targetFactory().XieweiNativeDirection(owner, kind, forResponse);
    public bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().IsXieweiSelection(selection);
    public void PrepareXieweiResponsePayment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection, long nativeUseId, int? actorSeat = default) => this._targetFactory().PrepareXieweiResponsePayment(owner, selection, nativeUseId, actorSeat);
    public bool TryPayXieweiMaterials(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int providerSeat, global::CardGame.Core.CardLocation? sourceLocation, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().TryPayXieweiMaterials(cards, providerSeat, sourceLocation, reason);
    public void BindXieweiResponsePayment(long nativeUseId, global::CardGame.Core.CardActionContext action) => this._targetFactory().BindXieweiResponsePayment(nativeUseId, action);
    public bool IsXieweiResponseRemovedMaterial(global::CardGame.Core.ProgramMultiCardViewAsSelection selection, int cardId) => this._targetFactory().IsXieweiResponseRemovedMaterial(selection, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime))]
internal sealed class CoreCardConversionsCoreProgramFactionActionRewardAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramFactionActionRewardOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramFactionActionRewardRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action) => this._targetFactory().CaptureFactionAction(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime))]
internal sealed class CoreCardConversionsCharacterSunQuanAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterSunQuanAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> _targetFactory = targetFactory;
    public bool IsAnniversaryChouxiSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().IsAnniversaryChouxiSource(owner, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime))]
internal sealed class CoreCardConversionsCoreWushuangSkillsBAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreWushuangSkillsBOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangSkillsBRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WushuangBianyuIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().WushuangBianyuIdentity(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime))]
internal sealed class CoreCardConversionsCharacterHanDangMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCharacterHanDangMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouHanDangPublishedResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source, bool forResponse, bool dyingUse = false) => this._targetFactory().IsMouHanDangPublishedResponseAllowed(provider, card, source, forResponse, dyingUse);
    public bool IsMouHanDangNativeResponseAllowed(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, bool dyingUse = false) => this._targetFactory().IsMouHanDangNativeResponseAllowed(provider, card, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreCardConversionsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false) => this._targetFactory().TryBeginProgramCardWindow(attack, action, window, opponents, continuation, cardUseCausedDamage, trickContinuation, simpleContinuation, completedResponseReturn, caoChunPayment, caoChunDebtOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed class CoreCardConversionsCoreResponseEntityExchangeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreResponseEntityExchangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _targetFactory = targetFactory;
    public bool IsResponseEntityRestricted(int seat, int id) => this._targetFactory().IsResponseEntityRestricted(seat, id);
    public bool HasResponseEntityExchangeObservers() => this._targetFactory().HasResponseEntityExchangeObservers();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime))]
internal sealed class CoreCardConversionsCoreProgramDistanceRulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardConversionsCoreProgramDistanceRulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}
