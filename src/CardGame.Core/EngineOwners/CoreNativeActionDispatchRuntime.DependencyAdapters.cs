#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed class CoreNativeActionDispatchCoreDrawFundedDistinctBasicOffersAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreDrawFundedDistinctBasicOffersOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _targetFactory = targetFactory;
    public bool TryExecuteDrawFundedDistinctBasicPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteDrawFundedDistinctBasicPlay(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime))]
internal sealed class CoreNativeActionDispatchCoreNativeWoodenOxAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNativeWoodenOxOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime> _targetFactory = targetFactory;
    public void ResolveWoodenOx(global::CardGame.Core.CharacterState actor, int storedCardId, int targetSeat) => this._targetFactory().ResolveWoodenOx(actor, storedCardId, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime))]
internal sealed class CoreNativeActionDispatchCoreShenwuCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreShenwuCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShenwuCardsRuntime> _targetFactory = targetFactory;
    public void ResolveShenwuSowBeans(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card) => this._targetFactory().ResolveShenwuSowBeans(source, card);
    public void ResolveShenwuTransplantWood(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card) => this._targetFactory().ResolveShenwuTransplantWood(source, target, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreNativeActionDispatchCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void ExecuteProgramSkill(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().ExecuteProgramSkill(owner, action, cards, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime))]
internal sealed class CoreNativeActionDispatchCoreWenheCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreWenheCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheCardsRuntime> _targetFactory = targetFactory;
    public void ResolveWenheCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int? targetSeat = default) => this._targetFactory().ResolveWenheCard(actor, card, kind, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreNativeActionDispatchCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public void ResolveHellReturnSoul(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => this._targetFactory().ResolveHellReturnSoul(actor, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime))]
internal sealed class CoreNativeActionDispatchCoreNativeTrickUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNativeTrickUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTrickUseRuntime> _targetFactory = targetFactory;
    public void ResolveDrawTwo(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card drawTwo) => this._targetFactory().ResolveDrawTwo(source, drawTwo);
    public void ResolveAlcohol(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card alcohol, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().ResolveAlcohol(source, alcohol, conversionSource);
    public void ResolveGroupCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card groupCard, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.CardKind requiredCardKind) => this._targetFactory().ResolveGroupCard(source, groupCard, actionKind, requiredCardKind);
    public void ResolvePeachGarden(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card peachGarden) => this._targetFactory().ResolvePeachGarden(source, peachGarden);
    public void ResolveFiveGrains(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card fiveGrains) => this._targetFactory().ResolveFiveGrains(source, fiveGrains);
    public void ResolveIronChain(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card ironChain, global::System.Collections.Generic.IReadOnlyList<int> targetSeats, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().ResolveIronChain(source, ironChain, targetSeats, playedCardKind);
    public void ResolveDelayedCard(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card delayedCard, global::CardGame.Core.LegalActionKind actionKind, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().ResolveDelayedCard(source, target, delayedCard, actionKind, playedCardKind);
    public void ResolveDismantlement(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card dismantlement, int? targetCardId, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.TargetHandCardPreselection? preselection = default) => this._targetFactory().ResolveDismantlement(source, target, dismantlement, targetCardId, playedCardKind, preselection);
    public void ResolveSnatch(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card snatch, int? targetCardId, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.TargetHandCardPreselection? preselection = default) => this._targetFactory().ResolveSnatch(source, target, snatch, targetCardId, playedCardKind, preselection);
    public void ResolveFireAttack(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card fireAttack, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().ResolveFireAttack(source, target, fireAttack, playedCardKind);
    public void ResolveBorrowedSword(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState slashTarget, global::CardGame.Core.Card borrowedSword) => this._targetFactory().ResolveBorrowedSword(source, weaponOwner, slashTarget, borrowedSword);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime))]
internal sealed class CoreNativeActionDispatchCoreHuxinMirrorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreHuxinMirrorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHuxinMirrorRuntime> _targetFactory = targetFactory;
    public bool TryPlaceHandHuxinMirror(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryPlaceHandHuxinMirror(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime))]
internal sealed class CoreNativeActionDispatchCoreNativeSlashUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNativeSlashUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashUseRuntime> _targetFactory = targetFactory;
    public void ResolveZhangbaSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> physicalCards) => this._targetFactory().ResolveZhangbaSlash(source, target, physicalCards);
    public void ResolveFangtianHalberdSlash(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> targets, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().ResolveFangtianHalberdSlash(source, targets, slash, playedCardKind, conversionSource, additionalConversionSources);
    public void ResolveSlash(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default) => this._targetFactory().ResolveSlash(source, target, slash, playedCardKind, conversionSource, additionalConversionSources);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime))]
internal sealed class CoreNativeActionDispatchCoreChainedStateBasicConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreChainedStateBasicConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime> _targetFactory = targetFactory;
    public bool TryExecuteChainedStateBasicPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteChainedStateBasicPlay(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime))]
internal sealed class CoreNativeActionDispatchCharacterXunYuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCharacterXunYuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryExecuteQizhengNaturalUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteQizhengNaturalUse(actor, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime))]
internal sealed class CoreNativeActionDispatchCoreProgramMultiTargetDuelsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreProgramMultiTargetDuelsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMultiTargetDuelsRuntime> _targetFactory = targetFactory;
    public void ResolveProgramMultiTargetDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card duel, global::CardGame.Core.LegalAction action) => this._targetFactory().ResolveProgramMultiTargetDuel(source, duel, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime))]
internal sealed class CoreNativeActionDispatchCoreRedAdditionalTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreRedAdditionalTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRedAdditionalTargetsRuntime> _targetFactory = targetFactory;
    public bool IsTargetAdjustmentAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().IsTargetAdjustmentAction(actor, action);
    public int GetAdditionalTargetAdjustmentLimit(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().GetAdditionalTargetAdjustmentLimit(actor, action);
    public bool HasRedAdditionalTargets(global::CardGame.Core.CharacterState actor) => this._targetFactory().HasRedAdditionalTargets(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed class CoreNativeActionDispatchCoreNativeOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNativeOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? FindOwnedPlayableCard(global::CardGame.Core.CharacterState actor, int? cardId) => this._targetFactory().FindOwnedPlayableCard(actor, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreNativeActionDispatchCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardDeclaration(global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? source, global::CardGame.Core.CardDeclarationParentReturn parent, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().TryBeginCardDeclaration(provider, card, kind, source, parent, targets);
    public global::CardGame.Core.CardDeclarationParentReturn DeclarationReturn(global::CardGame.Core.CardDeclarationPurpose purpose, int actor, global::CardGame.Core.LegalAction? action = default, int? target = default, int recovery = 1, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource>? policies = default, global::CardGame.Core.DyingResponseEvent? dying = default, bool fan = false, global::CardGame.Core.CardKind? finalKind = default) => this._targetFactory().DeclarationReturn(purpose, actor, action, target, recovery, policies, dying, fan, finalKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime))]
internal sealed class CoreNativeActionDispatchCoreSingleCardTrickConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreSingleCardTrickConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSingleCardTrickConversionsRuntime> _targetFactory = targetFactory;
    public bool TryExecuteSingleCardTrickConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteSingleCardTrickConversion(actor, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime))]
internal sealed class CoreNativeActionDispatchCoreGuanduCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreGuanduCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _targetFactory = targetFactory;
    public void ResolveGuanduCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().ResolveGuanduCard(actor, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime))]
internal sealed class CoreNativeActionDispatchCoreNationalGeneralRevealAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNationalGeneralRevealOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNationalGeneralRevealRuntime> _targetFactory = targetFactory;
    public void RevealNationalGeneral(global::CardGame.Core.CharacterState actor, global::CardGame.Core.GeneralSelectionSlot slot) => this._targetFactory().RevealNationalGeneral(actor, slot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime))]
internal sealed class CoreNativeActionDispatchCoreNativeEquipmentUseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNativeEquipmentUseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentUseRuntime> _targetFactory = targetFactory;
    public void ResolveEquip(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card equipment) => this._targetFactory().ResolveEquip(source, equipment);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed class CoreNativeActionDispatchCoreDragonBoatCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreDragonBoatCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _targetFactory = targetFactory;
    public void ResolveDragonBoatCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().ResolveDragonBoatCard(actor, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreNativeActionDispatchCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public void ResolveNextActualUseZhangba(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => this._targetFactory().ResolveNextActualUseZhangba(owner, targets, cards);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime))]
internal sealed class CoreNativeActionDispatchCoreHulaoCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreHulaoCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHulaoCardsRuntime> _targetFactory = targetFactory;
    public void ResolveAllianceFeast(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card) => this._targetFactory().ResolveAllianceFeast(source, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreNativeActionDispatchCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public void SelectUseConversion(global::CardGame.Core.LegalAction action) => this._targetFactory().SelectUseConversion(action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime))]
internal sealed class CoreNativeActionDispatchCharacterLuBuMobileGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCharacterLuBuMobileGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMobileGodRuntime> _targetFactory = targetFactory;
    public bool TryResolveMobileWumouUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryResolveMobileWumouUse(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime))]
internal sealed class CoreNativeActionDispatchCoreRoundOnceCardlessNativeUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreRoundOnceCardlessNativeUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundOnceCardlessNativeUsesRuntime> _targetFactory = targetFactory;
    public bool TryExecuteRoundOnceCardlessPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteRoundOnceCardlessPlay(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime))]
internal sealed class CoreNativeActionDispatchCoreTieredRoundZeroUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreTieredRoundZeroUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTieredRoundZeroUsesRuntime> _targetFactory = targetFactory;
    public bool TryExecuteTieredRoundZeroPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteTieredRoundZeroPlay(actor, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime))]
internal sealed class CoreNativeActionDispatchCoreProgramActionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreProgramActionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime))]
internal sealed class CoreNativeActionDispatchCoreMantianGuohaiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreMantianGuohaiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMantianGuohaiRuntime> _targetFactory = targetFactory;
    public void ResolveMantianGuohai(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().ResolveMantianGuohai(source, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreNativeActionDispatchCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ApplyProgramUseAppearance(owner, card, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreNativeActionDispatchCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player) => this._targetFactory().GetPlayableCards(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime))]
internal sealed class CoreNativeActionDispatchCoreProgramIronChainTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreProgramIronChainTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> _targetFactory = targetFactory;
    public int GetProgramIronChainTargetLimit(global::CardGame.Core.CharacterState actor) => this._targetFactory().GetProgramIronChainTargetLimit(actor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreNativeActionDispatchCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public void ResolveRecast(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().ResolveRecast(actor, card, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime))]
internal sealed class CoreNativeActionDispatchCoreNativeRecoveryCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNativeRecoveryCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRecoveryCardsRuntime> _targetFactory = targetFactory;
    public void ResolvePeach(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card peach, bool allowDying, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.DyingResponseEvent? dyingResponse = default) => this._targetFactory().ResolvePeach(source, target, peach, allowDying, conversionSource, dyingResponse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnViewAsRuntime))]
internal sealed class CoreNativeActionDispatchCoreActualTurnViewAsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnViewAsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreActualTurnViewAsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnViewAsRuntime> _targetFactory = targetFactory;
    public void ConsumeActualTurnLimitedViewAs(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ConsumeActualTurnLimitedViewAs(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime))]
internal sealed class CoreNativeActionDispatchCoreNativeDuelAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreNativeDuelOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDuelRuntime> _targetFactory = targetFactory;
    public void ResolveDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card duel, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().ResolveDuel(source, target, duel, playedCardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreNativeActionDispatchCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public bool TryExecuteWumiaoFloodNaturalUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteWumiaoFloodNaturalUse(actor, card, action);
    public bool TryExecuteWuyouIdentityUse(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action) => this._targetFactory().TryExecuteWuyouIdentityUse(actor, card, action);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime))]
internal sealed class CoreNativeActionDispatchCoreUnexpectedAssaultAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreNativeActionDispatchCoreUnexpectedAssaultOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUnexpectedAssaultRuntime> _targetFactory = targetFactory;
    public void ResolveUnexpectedAssault(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind? playedCardKind = default) => this._targetFactory().ResolveUnexpectedAssault(source, target, card, playedCardKind);
}
