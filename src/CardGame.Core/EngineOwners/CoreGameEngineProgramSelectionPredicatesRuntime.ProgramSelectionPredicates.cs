#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectionPredicatesRuntime
{
    CardLocation IProgramSelectionCardState.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    Suit IProgramSelectionCardState.EffectiveSuit(int ownerSeat, Card card) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    bool IProgramSelectionCardState.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IProgramSelectionCardState.IsSelfDiscardForbidden(int ownerSeat, Card card, CardLocation location) => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(ownerSeat, card, location, OwnedCardMoveIntent.Discard);
    bool IProgramSelectionCardState.IsForeignEquipmentDiscardPrevented(int ownerSeat, Card card, CardLocation location) => _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(ownerSeat, card, location, OwnedCardMoveIntent.Discard);
    bool IProgramSelectionCardState.IsActiveSourceEquipment(int ownerSeat, string skillId, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId), card);
    int? IProgramSelectionCardState.UniqueMaximumHandCardId(int ownerSeat) => _coreFriendEventsOperations.UniqueMaximumHandCard(ownerSeat)?.Id;
    int IProgramSelectionDiamondState.PlayerCount => _coreMatchParticipantsPlayersAccess._players.Count;

    bool IProgramSelectionDiamondState.HasDelayedDiamondJudgment() => _coreDiamondDelayedProgramsOperations.Value.DiamondJudgments().Any();
    bool IProgramSelectionDiamondState.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IProgramSelectionDiamondState.JudgmentAreaAbolished(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].JudgmentAreaAbolished;
    bool IProgramSelectionDiamondState.HasIndulgenceJudgment(int seat) => _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], CardKind.Indulgence);
    bool IProgramSelectionDiamondState.IsTurnHandCardRestricted(int ownerSeat, Card card) => _coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    bool IProgramSelectionDiamondState.IsPlayPhaseCardRestricted(int ownerSeat, Card card) => _coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card);
    bool IProgramSelectionDiamondState.IsCardUseForbidden(int ownerSeat, CardKind kind, CardActionType actionType) => _coreCardUseModulesOperations.IsCardUseForbidden(ownerSeat, kind, actionType);
    bool IProgramSelectionDiamondState.IsDirectedTargetProhibited(int ownerSeat, int targetSeat, CardKind kind) => _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, targetSeat, kind);
    bool IProgramSelectionDiamondState.IsTargetProhibited(int targetSeat, CardKind kind, Suit suit, bool? isRed) => _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat], kind, suit, isRed);
    bool IProgramSelectionDiamondState.HasBeneficiarySuitShield(int ownerSeat, int targetSeat, Suit suit) => _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(ownerSeat, targetSeat, suit);
    bool IProgramSelectionTargetState.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IProgramSelectionTargetState.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    int IProgramSelectionTargetState.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    int IProgramSelectionTargetState.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    int IProgramSelectionTargetState.MarkerCount(int seat, PlayerMarkerKind marker) => _coreMatchParticipantsPlayersAccess._players[seat].Markers.GetValueOrDefault(marker);
    bool IProgramSelectionTargetState.IsFarthestInRange(int ownerSeat, int targetSeat) => _coreAdvancedLifecycleOperations.IsFarthestInRange(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat]);
    bool IProgramSelectionTargetState.IsWithinAttackRange(int ownerSeat, int targetSeat) => _coreProgramDodgeResponsesOperations.IsWithinAttackRange(ownerSeat, targetSeat);
    int IProgramSelectionTargetState.TurnUsage(int ownerSeat, string skillId, string usageId) => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(ownerSeat, skillId, usageId, SkillUsageScope.Turn);
    IEnumerable<GameTargetHandHpChoiceIssuedEvent> IProgramSelectionTargetState.HandHpIssuedTargets => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GameTargetHandHpChoiceIssuedEvent>();

    IEnumerable<DrawDiscardCategoryTargetBannedEvent> IProgramSelectionTargetState.CategoryRefundBans => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawDiscardCategoryTargetBannedEvent>();

    int IProgramSelectionTargetState.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int IProgramSelectionTargetState.TurnOwnerSeat => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int IProgramSelectionTargetState.DynamicDamagePaymentCount(int ownerSeat, string skillId) => _coreDynamicDiscardDamagePaymentsOperations.DynamicDiscardDamageCards(ownerSeat, skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId)).Length;
    bool IProgramSelectionTargetState.HasEmptySlotForOwnerHandEquipment(int ownerSeat, int targetSeat) => _coreNativeEquipmentAndFactionQueriesOperations.HasEmptyEquipmentSlotForOwnerHandEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat]);
    bool IProgramSelectionTargetState.CanUseProvidedSlashOn(int ownerSeat, int targetSeat) => _coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat]);
    int IProgramSelectionPhaseLimitState.RoundNumber => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int IProgramSelectionPhaseLimitState.TurnNumber => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    string IProgramSelectionPhaseLimitState.SkillInstanceId(int ownerSeat, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    IEnumerable<ProgramHandComparisonResolvedEvent> IProgramSelectionPhaseLimitState.HandComparisonResults => _coreHandComparisonOperations.HandComparisonHistory().OfType<ProgramHandComparisonResolvedEvent>();

    int IProgramSelectionPhaseLimitState.CountLiving(Role role) => _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive && player.Role == role);
}
