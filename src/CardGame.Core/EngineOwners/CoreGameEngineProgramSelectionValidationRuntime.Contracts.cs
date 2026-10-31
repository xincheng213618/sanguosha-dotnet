#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramSelectionValidationCoreMatchingRecastsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidMatchingRecastSelection(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids);
}

internal interface ICoreProgramSelectionValidationCoreGuanHaiBridgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
    bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreProgramSelectionValidationCoreHpLossSlashMaterialsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHpLossSlashTargets(global::CardGame.Core.CharacterState owner);
    int HpLossSlashMaximum(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, int original = 1);
}

internal interface ICoreProgramSelectionValidationCoreEquipmentDonationProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPayableAllEquipmentDonation(int ownerSeat, string skillId, string usageId);
}

internal interface ICoreProgramSelectionValidationCoreDistinctFactionDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPayableDistinctFactionOwnerHe(int seat, string skillId, string instanceId);
}

internal interface ICoreProgramSelectionValidationCoreOwnedHandRankSumProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanStartOwnedHandRankSum(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan);
}

internal interface ICoreProgramSelectionValidationCoreDamageColorDuelProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat);
}

internal interface ICoreProgramSelectionValidationCoreFriendTrioOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidWholeHandSuitConversion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreProgramSelectionValidationCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
}

internal interface ICoreProgramSelectionValidationCoreDiscardedProvenanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksDiscardedEntityProvenance { get; }
}

internal interface ICoreProgramSelectionValidationCorePairObtainShownGiftsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanActivatePairObtain(global::CardGame.Core.CharacterState owner, string skill, string instance, global::CardGame.Core.SkillProgramActivation activation, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreProgramSelectionValidationCoreHalfHandPhaseDebtProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPayableDeferredHandPair(int owner, int first, int second);
}

internal interface ICoreProgramSelectionValidationCoreDrawDiscardCategoryRefundOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DrawDiscardCategoryTargetAvailable(int owner, string skill, string stateId, int target);
}

internal interface ICoreProgramSelectionValidationCoreRecipientConsequencesReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BlackGiftGameIssued(int owner, string skill);
}

internal interface ICoreProgramSelectionValidationCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreProgramSelectionValidationCoreGameTargetHandHpChoicesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSelectGameHandHpTarget(int owner, string skill, string state, int target);
}

internal interface ICoreProgramSelectionValidationCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent? CurrentNextActualUseAdjustment(int ownerSeat);
    bool CanUseNextActualUseSelectedSlash(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreProgramSelectionValidationCoreDynamicDiscardDamagePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanStartDynamicDiscardDamage(global::CardGame.Core.CharacterState owner, string skill);
    bool CanSelectDynamicDiscardDamageTarget(global::CardGame.Core.CharacterState owner, string skill, int target);
}

internal interface ICoreProgramSelectionValidationProgramSelectionValidatorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.ProgramSelectionValidationRegistry? _programSelectionValidators { get; }
}

internal interface ICoreProgramSelectionValidationCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreProgramSelectionValidationCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreProgramSelectionValidationCoreEquipmentPlacementAndHpPairOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanPlaceActivationEquipment(int ownerSeat, int targetSeat, int cardId);
}

internal interface ICoreProgramSelectionValidationCoreCapturedEquipmentDrawsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanPlaceCapturedEquipmentActivation(int owner, int recipient, int id, string skillId);
}

internal interface ICoreProgramSelectionValidationCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramSelectionValidationCoreForeignSelectedCardSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanStartSelectedForeignCardSlash(global::CardGame.Core.CharacterState owner, int actorSeat);
}

internal interface ICoreProgramSelectionValidationCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IProgramSelectionQueries ProgramSelectionQueries { get; }
}

internal interface ICoreProgramSelectionValidationCoreOwnedExactCountAndNamedTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanStartExactOwnedCardCount(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan);
}

internal interface ICoreProgramSelectionValidationCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source);
}
