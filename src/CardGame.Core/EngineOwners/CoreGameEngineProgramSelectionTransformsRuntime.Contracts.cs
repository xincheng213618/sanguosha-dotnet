#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramSelectionTransformsCoreForeignSelectedCardSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanStartSelectedForeignCardSlash(global::CardGame.Core.CharacterState owner, int actorSeat);
}

internal interface ICoreProgramSelectionTransformsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramSelectionTransformsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat);
}

internal interface ICoreProgramSelectionTransformsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreProgramSelectionTransformsCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanActivateTargetPhaseLedger(int owner, string skill, string usage, int target);
}

internal interface ICoreProgramSelectionTransformsCorePairObtainShownGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPayablePairObtainTarget(int owner, string skill, string instance, int target);
}

internal interface ICoreProgramSelectionTransformsCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat);
}

internal interface ICoreProgramSelectionTransformsCoreProgramCategoryTargetLedgerOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanActivateCategoryTargetLedger(int ownerSeat, string skillId, string usageId, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreProgramSelectionTransformsCoreEquipmentPlacementAndHpPairOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanPlaceActivationEquipment(int ownerSeat, int targetSeat, int cardId);
}

internal interface ICoreProgramSelectionTransformsCorePindianPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreProgramSelectionTransformsCoreSourceCursesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SourceCurseDeposit? SourceCurseAt(int seat);
}

internal interface ICoreProgramSelectionTransformsCoreNextActualUsePindianAiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNextActualUsePindianStructure(global::CardGame.Core.SkillProgramActivation activation);
}

internal interface ICoreProgramSelectionTransformsCoreMatchingRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchingRecastMaterial[] MatchingRecastMaterials(int seat);
}

internal interface ICoreProgramSelectionTransformsCoreCapturedEquipmentDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanPlaceCapturedEquipmentActivation(int owner, int recipient, int id, string skillId);
}

internal interface ICoreProgramSelectionTransformsCoreAppliedDamageBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanActivateDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat);
}
