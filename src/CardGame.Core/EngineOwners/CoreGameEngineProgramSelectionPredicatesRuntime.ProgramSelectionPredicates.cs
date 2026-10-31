#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectionPredicatesRuntime
{
    CardLocation global::CardGame.Core.IProgramSelectionCardState.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    Suit global::CardGame.Core.IProgramSelectionCardState.EffectiveSuit(int ownerSeat, Card card) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    bool global::CardGame.Core.IProgramSelectionCardState.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IProgramSelectionCardState.IsSelfDiscardForbidden(int ownerSeat, Card card, CardLocation location) => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(ownerSeat, card, location, OwnedCardMoveIntent.Discard);
    bool global::CardGame.Core.IProgramSelectionCardState.IsForeignEquipmentDiscardPrevented(int ownerSeat, Card card, CardLocation location) => _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(ownerSeat, card, location, OwnedCardMoveIntent.Discard);
    bool global::CardGame.Core.IProgramSelectionCardState.IsActiveSourceEquipment(int ownerSeat, string skillId, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId), card);
    int? global::CardGame.Core.IProgramSelectionCardState.UniqueMaximumHandCardId(int ownerSeat) => _coreFriendEventsOperations.UniqueMaximumHandCard(ownerSeat)?.Id;
    int global::CardGame.Core.IProgramSelectionDiamondState.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool global::CardGame.Core.IProgramSelectionDiamondState.HasDelayedDiamondJudgment() => _coreDiamondDelayedProgramsOperations.DiamondJudgments().Any();
    bool global::CardGame.Core.IProgramSelectionDiamondState.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.IProgramSelectionDiamondState.JudgmentAreaAbolished(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].JudgmentAreaAbolished;
    bool global::CardGame.Core.IProgramSelectionDiamondState.HasIndulgenceJudgment(int seat) => _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Indulgence);
    bool global::CardGame.Core.IProgramSelectionDiamondState.IsTurnHandCardRestricted(int ownerSeat, Card card) => _coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    bool global::CardGame.Core.IProgramSelectionDiamondState.IsPlayPhaseCardRestricted(int ownerSeat, Card card) => _coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    bool global::CardGame.Core.IProgramSelectionDiamondState.IsCardUseForbidden(int ownerSeat, CardKind kind, CardActionType actionType) => _coreCardUseModulesOperations.IsCardUseForbidden(ownerSeat, kind, actionType);
    bool global::CardGame.Core.IProgramSelectionDiamondState.IsDirectedTargetProhibited(int ownerSeat, int targetSeat, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, targetSeat, kind);
    bool global::CardGame.Core.IProgramSelectionDiamondState.IsTargetProhibited(int targetSeat, CardKind kind, Suit suit, bool? isRed) => _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat], kind, suit, isRed);
    bool global::CardGame.Core.IProgramSelectionDiamondState.HasBeneficiarySuitShield(int ownerSeat, int targetSeat, Suit suit) => _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(ownerSeat, targetSeat, suit);
    bool global::CardGame.Core.IProgramSelectionTargetState.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.IProgramSelectionTargetState.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    int global::CardGame.Core.IProgramSelectionTargetState.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int global::CardGame.Core.IProgramSelectionTargetState.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    int global::CardGame.Core.IProgramSelectionTargetState.MarkerCount(int seat, PlayerMarkerKind marker) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(marker);
    bool global::CardGame.Core.IProgramSelectionTargetState.IsFarthestInRange(int ownerSeat, int targetSeat) => _coreAdvancedLifecycleOperations.IsFarthestInRange(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat]);
    bool global::CardGame.Core.IProgramSelectionTargetState.IsWithinAttackRange(int ownerSeat, int targetSeat) => _coreProgramDodgeResponsesOperations.IsWithinAttackRange(ownerSeat, targetSeat);
    int global::CardGame.Core.IProgramSelectionTargetState.TurnUsage(int ownerSeat, string skillId, string usageId) => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(ownerSeat, skillId, usageId, SkillUsageScope.Turn);
    IEnumerable<GameTargetHandHpChoiceIssuedEvent> global::CardGame.Core.IProgramSelectionTargetState.HandHpIssuedTargets => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameTargetHandHpChoiceIssuedEvent>();

    IEnumerable<DrawDiscardCategoryTargetBannedEvent> global::CardGame.Core.IProgramSelectionTargetState.CategoryRefundBans => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawDiscardCategoryTargetBannedEvent>();

    int global::CardGame.Core.IProgramSelectionTargetState.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IProgramSelectionTargetState.TurnOwnerSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int global::CardGame.Core.IProgramSelectionTargetState.DynamicDamagePaymentCount(int ownerSeat, string skillId) => _coreDynamicDiscardDamagePaymentsOperations.DynamicDiscardDamageCards(ownerSeat, skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId)).Length;
    bool global::CardGame.Core.IProgramSelectionTargetState.HasEmptySlotForOwnerHandEquipment(int ownerSeat, int targetSeat) => _coreNativeEquipmentAndFactionQueriesOperations.HasEmptyEquipmentSlotForOwnerHandEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat]);
    bool global::CardGame.Core.IProgramSelectionTargetState.CanUseProvidedSlashOn(int ownerSeat, int targetSeat) => _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat]);
    int global::CardGame.Core.IProgramSelectionPhaseLimitState.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int global::CardGame.Core.IProgramSelectionPhaseLimitState.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    string global::CardGame.Core.IProgramSelectionPhaseLimitState.SkillInstanceId(int ownerSeat, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    IEnumerable<ProgramHandComparisonResolvedEvent> global::CardGame.Core.IProgramSelectionPhaseLimitState.HandComparisonResults => _coreHandComparisonOperations.HandComparisonHistory().OfType<ProgramHandComparisonResolvedEvent>();

    int global::CardGame.Core.IProgramSelectionPhaseLimitState.CountLiving(Role role) => _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive && player.Role == role);
}
