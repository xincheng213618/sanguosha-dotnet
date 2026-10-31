#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramSelectionTransformsRuntime
{
    IReadOnlyList<int> global::CardGame.Core.ISelectionParticipantQueries.Seats => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => player.Seat).ToArray());

    bool global::CardGame.Core.ISelectionParticipantQueries.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.ISelectionParticipantQueries.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    int global::CardGame.Core.ISelectionParticipantQueries.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    int global::CardGame.Core.ISelectionParticipantQueries.EquipmentCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(seat).Count;
    int global::CardGame.Core.ISelectionParticipantQueries.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    IReadOnlyList<MatchingRecastMaterial> global::CardGame.Core.IMatchingRecastSelectionQueries.Materials(int ownerSeat) => Array.AsReadOnly(_coreMatchingRecastsOperations.MatchingRecastMaterials(ownerSeat));
    bool global::CardGame.Core.ISourceCurseSelectionQueries.HasCurse(int targetSeat) => _coreSourceCursesOperations.SourceCurseAt(targetSeat)is not null;
    string global::CardGame.Core.IPairObtainSelectionQueries.CurrentSkillInstance(int ownerSeat, string skillId) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[ownerSeat], skillId);
    bool global::CardGame.Core.IPairObtainSelectionQueries.CanObtainFrom(int ownerSeat, string skillId, string instanceId, int targetSeat) => _corePairObtainShownGiftsOperations.HasPayablePairObtainTarget(ownerSeat, skillId, instanceId, targetSeat);
    bool global::CardGame.Core.IForeignSlashSelectionQueries.CanStart(int ownerSeat, int actorSeat) => _coreForeignSelectedCardSlashOperations.CanStartSelectedForeignCardSlash(_coreMatchParticipantsPlayersAccess._players[ownerSeat], actorSeat);
    bool global::CardGame.Core.IDuelSelectionQueries.CanIssue(int ownerSeat, int actorSeat) => _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(ownerSeat, actorSeat);
    bool global::CardGame.Core.IEquipmentPlacementSelectionQueries.CanPlace(int ownerSeat, int targetSeat, int cardId) => _coreEquipmentPlacementAndHpPairOperations.CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
    bool global::CardGame.Core.IEquipmentPlacementSelectionQueries.CanPlaceCaptured(int ownerSeat, int targetSeat, int cardId, string skillId) => _coreCapturedEquipmentDrawsOperations.CanPlaceCapturedEquipmentActivation(ownerSeat, targetSeat, cardId, skillId);
    bool global::CardGame.Core.ISelectionUsageQueries.CanUseDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => _coreAppliedDamageBenefitsOperations.CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
    bool global::CardGame.Core.ISelectionUsageQueries.CanUsePhaseTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => _coreProgramParticipantPhaseOperationsOperations.CanActivateTargetPhaseLedger(ownerSeat, skillId, usageId, targetSeat);
    bool global::CardGame.Core.ISelectionUsageQueries.CanUseCategoryTarget(int ownerSeat, string skillId, string usageId, int cardId, int targetSeat) => _coreProgramCategoryTargetLedgerOperations.CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, [cardId], [targetSeat]);
    bool global::CardGame.Core.IPindianSelectionQueries.HasNextActualUseStructure(SkillProgramActivation activation) => _coreNextActualUsePindianAiOperations.HasNextActualUsePindianStructure(activation);
    int global::CardGame.Core.IPindianSelectionQueries.EffectiveRank(int ownerSeat, int cardId) => _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Single(card => card.Id == cardId));
    int global::CardGame.Core.IPindianSelectionQueries.HandKeepValue(int ownerSeat, int cardId) => CardCatalog.Get(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Single(card => card.Id == cardId).Kind).HandKeepValue;
}
