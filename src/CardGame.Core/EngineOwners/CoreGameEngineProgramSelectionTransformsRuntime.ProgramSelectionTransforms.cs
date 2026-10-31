#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectionTransformsRuntime
{
    IReadOnlyList<int> ISelectionParticipantQueries.Seats => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => player.Seat).ToArray());

    bool ISelectionParticipantQueries.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool ISelectionParticipantQueries.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    int ISelectionParticipantQueries.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    int ISelectionParticipantQueries.EquipmentCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat).Count;
    int ISelectionParticipantQueries.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    IReadOnlyList<MatchingRecastMaterial> IMatchingRecastSelectionQueries.Materials(int ownerSeat) => Array.AsReadOnly(_coreMatchingRecastsOperations.MatchingRecastMaterials(ownerSeat));
    bool ISourceCurseSelectionQueries.HasCurse(int targetSeat) => _coreSourceCursesOperations.SourceCurseAt(targetSeat)is not null;
    string IPairObtainSelectionQueries.CurrentSkillInstance(int ownerSeat, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    bool IPairObtainSelectionQueries.CanObtainFrom(int ownerSeat, string skillId, string instanceId, int targetSeat) => _corePairObtainShownGiftsOperations.HasPayablePairObtainTarget(ownerSeat, skillId, instanceId, targetSeat);
    bool IForeignSlashSelectionQueries.CanStart(int ownerSeat, int actorSeat) => _coreForeignSelectedCardSlashOperations.CanStartSelectedForeignCardSlash(_coreMatchParticipantsPlayersAccess._players[ownerSeat], actorSeat);
    bool IDuelSelectionQueries.CanIssue(int ownerSeat, int actorSeat) => _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(ownerSeat, actorSeat);
    bool IEquipmentPlacementSelectionQueries.CanPlace(int ownerSeat, int targetSeat, int cardId) => _coreEquipmentPlacementAndHpPairOperations.CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
    bool IEquipmentPlacementSelectionQueries.CanPlaceCaptured(int ownerSeat, int targetSeat, int cardId, string skillId) => _coreCapturedEquipmentDrawsOperations.CanPlaceCapturedEquipmentActivation(ownerSeat, targetSeat, cardId, skillId);
    bool ISelectionUsageQueries.CanUseDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => _coreAppliedDamageBenefitsOperations.CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
    bool ISelectionUsageQueries.CanUsePhaseTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => _coreProgramParticipantPhaseOperationsOperations.CanActivateTargetPhaseLedger(ownerSeat, skillId, usageId, targetSeat);
    bool ISelectionUsageQueries.CanUseCategoryTarget(int ownerSeat, string skillId, string usageId, int cardId, int targetSeat) => _coreProgramCategoryTargetLedgerOperations.CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, [cardId], [targetSeat]);
    bool IPindianSelectionQueries.HasNextActualUseStructure(SkillProgramActivation activation) => _coreNextActualUsePindianAiOperations.HasNextActualUsePindianStructure(activation);
    int IPindianSelectionQueries.EffectiveRank(int ownerSeat, int cardId) => _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Single(card => card.Id == cardId));
    int IPindianSelectionQueries.HandKeepValue(int ownerSeat, int cardId) => CardCatalog.Get(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Single(card => card.Id == cardId).Kind).HandKeepValue;
}
