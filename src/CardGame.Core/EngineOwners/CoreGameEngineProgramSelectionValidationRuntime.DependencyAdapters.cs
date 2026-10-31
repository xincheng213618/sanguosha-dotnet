#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreMatchingRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreMatchingRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> _targetFactory = targetFactory;
    public bool ValidMatchingRecastSelection(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => this._targetFactory().ValidMatchingRecastSelection(seat, ids);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime))]
internal sealed class CoreProgramSelectionValidationCoreGuanHaiBridgesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreGuanHaiBridgesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanHaiBridgesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? ProgramMultiCardSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardSuit(owner, selection);
    public bool? ProgramMultiCardColor(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().ProgramMultiCardColor(owner, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreHpLossSlashMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreHpLossSlashMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime> _targetFactory = targetFactory;
    public bool HasHpLossSlashTargets(global::CardGame.Core.CharacterState owner) => this._targetFactory().HasHpLossSlashTargets(owner);
    public int HpLossSlashMaximum(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, int original = 1) => this._targetFactory().HpLossSlashMaximum(owner, kind, original);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreEquipmentDonationProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreEquipmentDonationProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime> _targetFactory = targetFactory;
    public bool HasPayableAllEquipmentDonation(int ownerSeat, string skillId, string usageId) => this._targetFactory().HasPayableAllEquipmentDonation(ownerSeat, skillId, usageId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreDistinctFactionDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreDistinctFactionDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> _targetFactory = targetFactory;
    public bool HasPayableDistinctFactionOwnerHe(int seat, string skillId, string instanceId) => this._targetFactory().HasPayableDistinctFactionOwnerHe(seat, skillId, instanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreOwnedHandRankSumProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreOwnedHandRankSumProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedHandRankSumProgramsRuntime> _targetFactory = targetFactory;
    public bool CanStartOwnedHandRankSum(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan) => this._targetFactory().CanStartOwnedHandRankSum(owner, plan);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat) => this._targetFactory().CanIssueSelectedActorDuel(ownerSeat, actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime))]
internal sealed class CoreProgramSelectionValidationCoreFriendTrioAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreFriendTrioOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> _targetFactory = targetFactory;
    public bool ValidWholeHandSuitConversion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardConversionSource source) => this._targetFactory().ValidWholeHandSuitConversion(owner, cards, source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime))]
internal sealed class CoreProgramSelectionValidationCoreNativeTargetLegalityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreNativeTargetLegalityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeTargetLegalityRuntime> _targetFactory = targetFactory;
    public bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true) => this._targetFactory().CanUseVirtualSlashTarget(source, target, effectiveKind, physicalSuit, effectiveColor, effectiveRank, physicalCardIds, ignoreDistance, conversionSource, actualUnconvertedPhysical, countsTowardLimit);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime))]
internal sealed class CoreProgramSelectionValidationCoreDiscardedProvenanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreDiscardedProvenanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _targetFactory = targetFactory;
    public bool TracksDiscardedEntityProvenance { get => this._targetFactory().TracksDiscardedEntityProvenance; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CoreProgramSelectionValidationCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public bool CanActivatePairObtain(global::CardGame.Core.CharacterState owner, string skill, string instance, global::CardGame.Core.SkillProgramActivation activation, global::System.Collections.Generic.IReadOnlyList<int> targets) => this._targetFactory().CanActivatePairObtain(owner, skill, instance, activation, targets);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreHalfHandPhaseDebtProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreHalfHandPhaseDebtProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHalfHandPhaseDebtProgramsRuntime> _targetFactory = targetFactory;
    public bool IsPayableDeferredHandPair(int owner, int first, int second) => this._targetFactory().IsPayableDeferredHandPair(owner, first, second);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime))]
internal sealed class CoreProgramSelectionValidationCoreDrawDiscardCategoryRefundAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreDrawDiscardCategoryRefundOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawDiscardCategoryRefundRuntime> _targetFactory = targetFactory;
    public bool DrawDiscardCategoryTargetAvailable(int owner, string skill, string stateId, int target) => this._targetFactory().DrawDiscardCategoryTargetAvailable(owner, skill, stateId, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreRecipientConsequencesReceiptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreRecipientConsequencesReceiptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientConsequencesReceiptsRuntime> _targetFactory = targetFactory;
    public bool BlackGiftGameIssued(int owner, string skill) => this._targetFactory().BlackGiftGameIssued(owner, skill);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetRuntimeSkillInstanceId(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime))]
internal sealed class CoreProgramSelectionValidationCoreGameTargetHandHpChoicesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreGameTargetHandHpChoicesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime> _targetFactory = targetFactory;
    public bool CanSelectGameHandHpTarget(int owner, string skill, string state, int target) => this._targetFactory().CanSelectGameHandHpTarget(owner, skill, state, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreNextActualUseTargetAdjustmentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreNextActualUseTargetAdjustmentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NextActualUseTargetAdjustmentGrantedEvent? CurrentNextActualUseAdjustment(int ownerSeat) => this._targetFactory().CurrentNextActualUseAdjustment(ownerSeat);
    public bool CanUseNextActualUseSelectedSlash(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState target, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => this._targetFactory().CanUseNextActualUseSelectedSlash(owner, target, selection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreDynamicDiscardDamagePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreDynamicDiscardDamagePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> _targetFactory = targetFactory;
    public bool CanStartDynamicDiscardDamage(global::CardGame.Core.CharacterState owner, string skill) => this._targetFactory().CanStartDynamicDiscardDamage(owner, skill);
    public bool CanSelectDynamicDiscardDamageTarget(global::CardGame.Core.CharacterState owner, string skill, int target) => this._targetFactory().CanSelectDynamicDiscardDamageTarget(owner, skill, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreProgramSelectionValidationCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreProgramSelectionValidationCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime))]
internal sealed class CoreProgramSelectionValidationCoreEquipmentPlacementAndHpPairAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreEquipmentPlacementAndHpPairOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> _targetFactory = targetFactory;
    public bool CanPlaceActivationEquipment(int ownerSeat, int targetSeat, int cardId) => this._targetFactory().CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreCapturedEquipmentDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreCapturedEquipmentDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> _targetFactory = targetFactory;
    public bool CanPlaceCapturedEquipmentActivation(int owner, int recipient, int id, string skillId) => this._targetFactory().CanPlaceCapturedEquipmentActivation(owner, recipient, id, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramSelectionValidationCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime))]
internal sealed class CoreProgramSelectionValidationCoreForeignSelectedCardSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreForeignSelectedCardSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> _targetFactory = targetFactory;
    public bool CanStartSelectedForeignCardSlash(global::CardGame.Core.CharacterState owner, int actorSeat) => this._targetFactory().CanStartSelectedForeignCardSlash(owner, actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime))]
internal sealed class CoreProgramSelectionValidationCoreProgramSelectionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreProgramSelectionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.IProgramSelectionQueries ProgramSelectionQueries { get => this._targetFactory().ProgramSelectionQueries; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreOwnedExactCountAndNamedTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreOwnedExactCountAndNamedTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedExactCountAndNamedTargetsRuntime> _targetFactory = targetFactory;
    public bool CanStartExactOwnedCardCount(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan) => this._targetFactory().CanStartExactOwnedCardCount(owner, plan);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreProgramSelectionValidationCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionValidationCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source) => this._targetFactory().FindProgramMultiCardViewAsSelection(owner, cardIds, outputKind, forResponse, source);
}
