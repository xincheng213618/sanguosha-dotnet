#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PredicateSelectionQueries(Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreNativeCardAppearanceEntries> getCoreNativeCardAppearanceRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreForeignEquipmentDiscardPoliciesEntries> getCoreForeignEquipmentDiscardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineEquipmentSkillsEntries> getCoreGameEngineEquipmentSkillsRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CoreGameEngineFriendEventsEntries> getCoreGameEngineFriendEventsRuntime, Func<RuntimeEntries.CoreGameEngineDiamondDelayedProgramsEntries> getCoreGameEngineDiamondDelayedProgramsRuntime, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<RuntimeEntries.CoreGameEngineProgramPhaseColorRestrictionsEntries> getCoreGameEngineProgramPhaseColorRestrictionsRuntime, Func<RuntimeEntries.CoreDirectedTurnCardPoliciesEntries> getCoreDirectedTurnCardPoliciesRuntime, Func<RuntimeEntries.CoreNativeDamagePoliciesEntries> getCoreNativeDamagePoliciesRuntime, Func<RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries> getCoreGameEngineDeckEquipmentProgramsRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries> getCoreGameEngineProgramDodgeResponsesRuntime, Func<SkillRuntimeStateStore> getSkillRuntimeStateStore, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, Func<RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries> getCoreGameEngineDynamicDiscardDamagePaymentsRuntime, Func<RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries> getCoreNativeEquipmentAndFactionQueriesRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreActualRoundProgressState actualRoundProgressState, Func<RuntimeEntries.CoreGameEngineHandComparisonEntries> getCoreGameEngineHandComparisonRuntime) : IProgramSelectionCardState, IProgramSelectionDiamondState, IProgramSelectionTargetState, IProgramSelectionPhaseLimitState
{
    public CardLocation Location(int cardId) => getCardZoneStore().GetLocation(cardId);
    public Suit EffectiveSuit(int ownerSeat, Card card) => getCoreNativeCardAppearanceRuntime().EffectiveSuit(matchParticipantsState.ReferenceToPlayers[ownerSeat], card);
    public bool IsAlive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool IsSelfDiscardForbidden(int ownerSeat, Card card, CardLocation location) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(ownerSeat, card, location, OwnedCardMoveIntent.Discard);
    public bool IsForeignEquipmentDiscardPrevented(int ownerSeat, Card card, CardLocation location) => getCoreForeignEquipmentDiscardPoliciesRuntime().IsForeignEquipmentDiscardPrevented(ownerSeat, card, location, OwnedCardMoveIntent.Discard);
    public bool IsActiveSourceEquipment(int ownerSeat, string skillId, Card card) => getCoreGameEngineEquipmentSkillsRuntime().IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId), card);
    public int? UniqueMaximumHandCardId(int ownerSeat) => getCoreGameEngineFriendEventsRuntime().UniqueMaximumHandCard(ownerSeat)?.Id;
    public int PlayerCount => matchParticipantsState.ReferenceToPlayers.Count;

    public bool HasDelayedDiamondJudgment() => getCoreGameEngineDiamondDelayedProgramsRuntime().DiamondJudgments().Any();
    public bool JudgmentAreaAbolished(int seat) => matchParticipantsState.ReferenceToPlayers[seat].JudgmentAreaAbolished;
    public bool HasIndulgenceJudgment(int seat) => getCoreActualTurnsRuntime().HasJudgmentEffectiveCard(matchParticipantsState.ReferenceToPlayers[seat], CardKind.Indulgence);
    public bool IsTurnHandCardRestricted(int ownerSeat, Card card) => getCoreGameEngineCardUseModulesRuntime().IsTurnHandCardRestricted(matchParticipantsState.ReferenceToPlayers[ownerSeat], card);
    public bool IsPlayPhaseCardRestricted(int ownerSeat, Card card) => getCoreGameEngineProgramPhaseColorRestrictionsRuntime().IsPlayPhasePhysicalCardRestricted(matchParticipantsState.ReferenceToPlayers[ownerSeat], card);
    public bool IsCardUseForbidden(int ownerSeat, CardKind kind, CardActionType actionType) => getCoreGameEngineCardUseModulesRuntime().IsCardUseForbidden(ownerSeat, kind, actionType);
    public bool IsDirectedTargetProhibited(int ownerSeat, int targetSeat, CardKind kind) => getCoreDirectedTurnCardPoliciesRuntime().IsDirectedCardTargetProhibited(ownerSeat, targetSeat, kind);
    public bool IsTargetProhibited(int targetSeat, CardKind kind, Suit suit, bool? isRed) => getCoreNativeDamagePoliciesRuntime().IsCardTargetProhibited(matchParticipantsState.ReferenceToPlayers[targetSeat], kind, suit, isRed);
    public bool HasBeneficiarySuitShield(int ownerSeat, int targetSeat, Suit suit) => getCoreGameEngineDeckEquipmentProgramsRuntime().HasBeneficiarySuitShield(ownerSeat, targetSeat, suit);
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public int MarkerCount(int seat, PlayerMarkerKind marker) => matchParticipantsState.ReferenceToPlayers[seat].Markers.GetValueOrDefault(marker);
    public bool IsFarthestInRange(int ownerSeat, int targetSeat) => getCoreGameEngineAdvancedLifecycleRuntime().IsFarthestInRange(matchParticipantsState.ReferenceToPlayers[ownerSeat], matchParticipantsState.ReferenceToPlayers[targetSeat]);
    public bool IsWithinAttackRange(int ownerSeat, int targetSeat) => getCoreGameEngineProgramDodgeResponsesRuntime().IsWithinAttackRange(ownerSeat, targetSeat);
    public int TurnUsage(int ownerSeat, string skillId, string usageId) => getSkillRuntimeStateStore().GetUsage(ownerSeat, skillId, usageId, SkillUsageScope.Turn);
    public IEnumerable<GameTargetHandHpChoiceIssuedEvent> HandHpIssuedTargets => getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().OfType<GameTargetHandHpChoiceIssuedEvent>();
    public IEnumerable<DrawDiscardCategoryTargetBannedEvent> CategoryRefundBans => getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().OfType<DrawDiscardCategoryTargetBannedEvent>();

    public int DynamicDamagePaymentCount(int ownerSeat, string skillId) => getCoreGameEngineDynamicDiscardDamagePaymentsRuntime().DynamicDiscardDamageCards(ownerSeat, skillId, getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId)).Length;
    public bool HasEmptySlotForOwnerHandEquipment(int ownerSeat, int targetSeat) => getCoreNativeEquipmentAndFactionQueriesRuntime().HasEmptyEquipmentSlotForOwnerHandEquipment(matchParticipantsState.ReferenceToPlayers[ownerSeat], matchParticipantsState.ReferenceToPlayers[targetSeat]);
    public bool CanUseProvidedSlashOn(int ownerSeat, int targetSeat) => getCoreNativeEquipmentAndFactionQueriesRuntime().CanUseProvidedSlashTarget(matchParticipantsState.ReferenceToPlayers[ownerSeat], matchParticipantsState.ReferenceToPlayers[targetSeat]);
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public int TurnOwnerSeat => actualTurnProgressState.ReferenceToCurrentSeat;

    public string SkillInstanceId(int ownerSeat, string skillId) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public IEnumerable<ProgramHandComparisonResolvedEvent> HandComparisonResults => getCoreGameEngineHandComparisonRuntime().HandComparisonHistory().OfType<ProgramHandComparisonResolvedEvent>();

    public int CountLiving(Role role) => matchParticipantsState.ReferenceToPlayers.Count(player => player.IsAlive && player.Role == role);
}
