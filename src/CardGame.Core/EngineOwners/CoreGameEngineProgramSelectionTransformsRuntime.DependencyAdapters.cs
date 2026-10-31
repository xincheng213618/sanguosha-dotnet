#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreForeignSelectedCardSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreForeignSelectedCardSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignSelectedCardSlashRuntime> _targetFactory = targetFactory;
    public bool CanStartSelectedForeignCardSlash(global::CardGame.Core.CharacterState owner, int actorSeat) => this._targetFactory().CanStartSelectedForeignCardSlash(owner, actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreProgramSelectionTransformsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(int seat) => this._targetFactory().GetEquipment(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetRuntimeSkillInstanceId(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public bool CanActivateTargetPhaseLedger(int owner, string skill, string usage, int target) => this._targetFactory().CanActivateTargetPhaseLedger(owner, skill, usage, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime))]
internal sealed class CoreProgramSelectionTransformsCorePairObtainShownGiftsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCorePairObtainShownGiftsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairObtainShownGiftsRuntime> _targetFactory = targetFactory;
    public bool HasPayablePairObtainTarget(int owner, string skill, string instance, int target) => this._targetFactory().HasPayablePairObtainTarget(owner, skill, instance, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreDamageColorDuelProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreDamageColorDuelProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _targetFactory = targetFactory;
    public bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat) => this._targetFactory().CanIssueSelectedActorDuel(ownerSeat, actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreProgramCategoryTargetLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreProgramCategoryTargetLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCategoryTargetLedgerRuntime> _targetFactory = targetFactory;
    public bool CanActivateCategoryTargetLedger(int ownerSeat, string skillId, string usageId, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::System.Collections.Generic.IReadOnlyList<int> targetSeats) => this._targetFactory().CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, cardIds, targetSeats);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreEquipmentPlacementAndHpPairAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreEquipmentPlacementAndHpPairOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentPlacementAndHpPairRuntime> _targetFactory = targetFactory;
    public bool CanPlaceActivationEquipment(int ownerSeat, int targetSeat, int cardId) => this._targetFactory().CanPlaceActivationEquipment(ownerSeat, targetSeat, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime))]
internal sealed class CoreProgramSelectionTransformsCorePindianPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCorePindianPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianPoliciesRuntime> _targetFactory = targetFactory;
    public int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectivePindianRank(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreSourceCursesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreSourceCursesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SourceCurseDeposit? SourceCurseAt(int seat) => this._targetFactory().SourceCurseAt(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreNextActualUsePindianAiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreNextActualUsePindianAiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNextActualUsePindianAiRuntime> _targetFactory = targetFactory;
    public bool HasNextActualUsePindianStructure(global::CardGame.Core.SkillProgramActivation activation) => this._targetFactory().HasNextActualUsePindianStructure(activation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreMatchingRecastsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreMatchingRecastsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.MatchingRecastMaterial[] MatchingRecastMaterials(int seat) => this._targetFactory().MatchingRecastMaterials(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreCapturedEquipmentDrawsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreCapturedEquipmentDrawsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> _targetFactory = targetFactory;
    public bool CanPlaceCapturedEquipmentActivation(int owner, int recipient, int id, string skillId) => this._targetFactory().CanPlaceCapturedEquipmentActivation(owner, recipient, id, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime))]
internal sealed class CoreProgramSelectionTransformsCoreAppliedDamageBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSelectionTransformsCoreAppliedDamageBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> _targetFactory = targetFactory;
    public bool CanActivateDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => this._targetFactory().CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
}
