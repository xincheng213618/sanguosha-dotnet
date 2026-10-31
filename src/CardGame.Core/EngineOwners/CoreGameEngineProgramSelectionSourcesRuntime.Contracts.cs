#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramSelectionSourcesCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreProgramSelectionSourcesCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseFactionSlashRequest(global::CardGame.Core.CharacterState owner, string providerFactionId);
}

internal interface ICoreProgramSelectionSourcesCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent? CurrentNextActualUseAdjustment(int ownerSeat);
}

internal interface ICoreProgramSelectionSourcesCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
}

internal interface ICoreProgramSelectionSourcesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreProgramSelectionSourcesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreProgramSelectionSourcesCoreProgramOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreProgramSelectionSourcesCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreProgramSelectionSourcesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProgramCardIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramMultiCardViewAsSelection> GetProgramMultiCardViewAsSelections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind outputKind, bool forResponse, bool ignoreSuitUseProhibition = false, int? requestedUseTarget = default);
}

internal interface ICoreProgramSelectionSourcesCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreProgramSelectionSourcesCoreWenhePoisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardKind EffectiveConversionInputKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramSelectionSourcesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramSelectionSourcesCoreHealthQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int? UnambiguousHp(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramSelectionSourcesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramSelectionSourcesCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreProgramSelectionSourcesCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseGlobalCard(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool excludeOwner = false);
}

internal interface ICoreProgramSelectionSourcesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramSelectionSourcesCoreHpLossSlashMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHpLossSlashTargets(global::CardGame.Core.CharacterState owner);
    int HpLossSlashMaximum(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, int original = 1);
}

internal interface ICoreProgramSelectionSourcesCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesTreatedWounded { get; }

    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}
