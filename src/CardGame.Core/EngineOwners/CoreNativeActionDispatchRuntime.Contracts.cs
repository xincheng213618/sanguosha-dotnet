#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeActionDispatchCoreDrawFundedDistinctBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteDrawFundedDistinctBasicPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreNativeWoodenOxOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveWoodenOx(global::CardGame.Core.CharacterState actor, int storedCardId, int targetSeat);
}

internal interface ICoreNativeActionDispatchCoreShenwuCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveShenwuSowBeans(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card);
    void ResolveShenwuTransplantWood(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card);
}

internal interface ICoreNativeActionDispatchCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteProgramSkill(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreNativeActionDispatchCoreWenheCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveWenheCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int? targetSeat = default);
}

internal interface ICoreNativeActionDispatchCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveHellReturnSoul(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card);
}

internal interface ICoreNativeActionDispatchCoreNativeTrickUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDrawTwo(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card drawTwo);
    void ResolveAlcohol(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card alcohol, global::CardGame.Core.CardConversionSource? conversionSource = default);
    void ResolveGroupCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card groupCard, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.CardKind requiredCardKind);
    void ResolvePeachGarden(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card peachGarden);
    void ResolveFiveGrains(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card fiveGrains);
    void ResolveIronChain(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card ironChain, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default);
    void ResolveDelayedCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card delayedCard, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.CardKind? playedCardKind = default);
    void ResolveDismantlement(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card dismantlement, int? targetCardId, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.TargetHandCardPreselection? preselection = default);
    void ResolveSnatch(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card snatch, int? targetCardId, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.TargetHandCardPreselection? preselection = default);
    void ResolveFireAttack(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card fireAttack, global::CardGame.Core.CardKind? playedCardKind = default);
    void ResolveBorrowedSword(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState slashTarget, global::CardGame.Core.Card borrowedSword);
}

internal interface ICoreNativeActionDispatchCoreHuxinMirrorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryPlaceHandHuxinMirror(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveZhangbaSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards);
    void ResolveFangtianHalberdSlash(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> targets, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
    void ResolveSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreNativeActionDispatchCoreChainedStateBasicConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteChainedStateBasicPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCharacterXunYuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteQizhengNaturalUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreProgramMultiTargetDuelsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveProgramMultiTargetDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card duel, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreRedAdditionalTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTargetAdjustmentAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
    int GetAdditionalTargetAdjustmentLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
    bool HasRedAdditionalTargets(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeActionDispatchCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId);
}

internal interface ICoreNativeActionDispatchCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets);
    global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default);
}

internal interface ICoreNativeActionDispatchCoreSingleCardTrickConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteSingleCardTrickConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveGuanduCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreNationalGeneralRevealOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RevealNationalGeneral(global::CardGame.Core.CharacterState actor, global::CardGame.Core.GeneralSelectionSlot slot);
}

internal interface ICoreNativeActionDispatchCoreNativeEquipmentUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveEquip(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment);
}

internal interface ICoreNativeActionDispatchCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDragonBoatCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveNextActualUseZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreNativeActionDispatchCoreHulaoCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveAllianceFeast(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card);
}

internal interface ICoreNativeActionDispatchCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SelectUseConversion(global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCharacterLuBuMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResolveMobileWumouUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteRoundOnceCardlessPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteTieredRoundZeroPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreProgramActionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramActionRouter ProgramActions { get; }
}

internal interface ICoreNativeActionDispatchCoreMantianGuohaiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveMantianGuohai(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreNativeActionDispatchCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
}

internal interface ICoreNativeActionDispatchCoreProgramIronChainTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetProgramIronChainTargetLimit(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreNativeActionDispatchCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreNativeActionDispatchCoreNativeRecoveryCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolvePeach(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card peach, bool allowDying, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default);
}

internal interface ICoreNativeActionDispatchCoreActualTurnViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ConsumeActualTurnLimitedViewAs(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreNativeActionDispatchCoreNativeDuelOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card duel, global::CardGame.Core.CardKind? playedCardKind = default);
}

internal interface ICoreNativeActionDispatchCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryExecuteWumiaoFloodNaturalUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
    bool TryExecuteWuyouIdentityUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICoreNativeActionDispatchCoreUnexpectedAssaultOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveUnexpectedAssault(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default);
}
