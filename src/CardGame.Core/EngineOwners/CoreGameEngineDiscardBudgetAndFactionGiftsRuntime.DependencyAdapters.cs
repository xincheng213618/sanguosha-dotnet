#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public void MoveLogicalDiscard(int initiatorSeat, int executorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, long? parentFrameId = default) => this._targetFactory().MoveLogicalDiscard(initiatorSeat, executorSeat, card, from, reason, beforeFact, parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreBoundCardMovementContinuationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreBoundCardMovementContinuationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat) => this._targetFactory().AwaitProgramBoundCardMovements(frameId, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgram> EnabledSkillPrograms(global::CardGame.Core.CharacterState player) => this._targetFactory().EnabledSkillPrograms(player);
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
    public global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default) => this._targetFactory().BeginProgramSkillDamage(frame, targetSeat, amount, sourceReference, nature, sourceLess, explicitSourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreActualPlayPhaseCardUsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreActualPlayPhaseCardUsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPlayPhaseCardUsesRuntime> _targetFactory = targetFactory;
    public int GetActualPlayPhaseUseCount(int actorSeat) => this._targetFactory().GetActualPlayPhaseUseCount(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => this._targetFactory().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreForeignEquipmentDiscardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreForeignEquipmentDiscardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreForeignEquipmentDiscardPoliciesRuntime> _targetFactory = targetFactory;
    public bool HasForeignDiscardCapability { get => this._targetFactory().HasForeignDiscardCapability; }

    public bool IsForeignEquipmentDiscardPrevented(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => this._targetFactory().IsForeignEquipmentDiscardPrevented(actorSeat, card, from, intent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreMatchSetupDefinitionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreMatchSetupDefinitionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchSetupDefinitionsRuntime> _targetFactory = targetFactory;
    public string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEffectiveFactionId(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> EventsSinceLastBoundary(global::System.Func<global::CardGame.Core.IGameEvent, bool> boundary) => this._targetFactory().EventsSinceLastBoundary(boundary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreDamagePreventionProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreDamagePreventionProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> _targetFactory = targetFactory;
    public void PreventProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program) => this._targetFactory().PreventProgramCurrentDamage(program);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreLordSkillProjectionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreLordSkillProjectionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLordSkillProjectionsRuntime> _targetFactory = targetFactory;
    public bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role) => this._targetFactory().HasSkillRoleQualification(owner, skillId, instance, role);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreProgramDodgeResponsesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreProgramDodgeResponsesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _targetFactory = targetFactory;
    public bool IsWithinAttackRange(int actorSeat, int targetSeat) => this._targetFactory().IsWithinAttackRange(actorSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreProgramOtherOwnedCardDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreProgramOtherOwnedCardDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOtherOwnedCardDiscardsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOtherOwnedCardDiscardChoices(long frameId, int chooserSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().BuildOtherOwnedCardDiscardChoices(frameId, chooserSeat, zones, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
    public void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason) => this._targetFactory().CancelProgramBindingAndCleanup(frame, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed class CoreDiscardBudgetAndFactionGiftsCharacterBianFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDiscardBudgetAndFactionGiftsCharacterBianFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _targetFactory = targetFactory;
    public void RecordBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement, int actorSeat, long producerFrameId) => this._targetFactory().RecordBianFuRenForeignDiscardProducer(movement, actorSeat, producerFrameId);
}
