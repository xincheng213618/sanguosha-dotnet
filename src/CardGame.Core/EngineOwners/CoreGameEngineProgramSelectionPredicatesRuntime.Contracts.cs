#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramSelectionPredicatesCoreForeignEquipmentDiscardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreProgramSelectionPredicatesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreProgramSelectionPredicatesCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? UniqueMaximumHandCard(int seat);
}

internal interface ICoreProgramSelectionPredicatesCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreProgramSelectionPredicatesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreProgramSelectionPredicatesCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasEmptyEquipmentSlotForOwnerHandEquipment(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target);
    bool CanUseProvidedSlashTarget(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreProgramSelectionPredicatesCoreDynamicDiscardDamagePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card[] DynamicDiscardDamageCards(int owner, string skill, string instance);
}

internal interface ICoreProgramSelectionPredicatesCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasJudgmentEffectiveCard(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICoreProgramSelectionPredicatesCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreProgramSelectionPredicatesCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramSelectionPredicatesCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreProgramSelectionPredicatesCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramSelectionPredicatesCoreTurnHandCategoryRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent);
}

internal interface ICoreProgramSelectionPredicatesCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> HandComparisonHistory();
}

internal interface ICoreProgramSelectionPredicatesCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreProgramSelectionPredicatesCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICoreProgramSelectionPredicatesCoreProgramPhaseColorRestrictionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPlayPhasePhysicalCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
}

internal interface ICoreProgramSelectionPredicatesCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreProgramSelectionPredicatesCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFarthestInRange(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramSelectionPredicatesCoreDiamondDelayedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(int Seat, global::CardGame.Core.Card Card)> DiamondJudgments();
}

internal interface ICoreProgramSelectionPredicatesCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramSelectionPredicatesSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreProgramSelectionPredicatesCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card);
}
