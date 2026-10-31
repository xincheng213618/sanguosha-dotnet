#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void AdvanceAfterDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped) => this._targetFactory().AdvanceAfterDeferredTurnEnd(previous, skipped);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreDeferredTurnEndPreludesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreDeferredTurnEndPreludesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredTurnEndPreludesRuntime> _targetFactory = targetFactory;
    public bool ContinueDeferredTurnEndPrelude(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().ContinueDeferredTurnEndPrelude(parent);
    public bool HasDeferredTurnEndPreludeBoundary(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().HasDeferredTurnEndPreludeBoundary(parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreAfterTurnEndedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreAfterTurnEndedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _targetFactory = targetFactory;
    public bool ContinueAfterTurnEndedPrograms(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().ContinueAfterTurnEndedPrograms(parent);
    public bool CompleteAfterTurnEndedBinding(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CompleteAfterTurnEndedBinding(parent, child, context);
    public bool HasAfterTurnEndedBoundaryFrame(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().HasAfterTurnEndedBoundaryFrame(parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCorePrivateTurnHoldsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCorePrivateTurnHoldsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> _targetFactory = targetFactory;
    public bool HasActualTurnEndMovementPrelude { get => this._targetFactory().HasActualTurnEndMovementPrelude; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().MoveLogicalDiscardCards(initiatorSeat, executorSeat, cards, from, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().EnabledUniqueProgramTriggers(player, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime))]
internal sealed class CoreDeferredHandAlignmentsCharacterAHuiNanOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCharacterAHuiNanOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterAHuiNanOlRuntime> _targetFactory = targetFactory;
    public void ExpireAHuiNanJuemanTurnEndWine() => this._targetFactory().ExpireAHuiNanJuemanTurnEndWine();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CoreDeferredHandAlignmentsCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public void ExpireQinLangXianyingTurnEndWine() => this._targetFactory().ExpireQinLangXianyingTurnEndWine();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreProgramOwnedCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreProgramOwnedCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect GetOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetOwnedSelectionEffect(frame);
    public global::CardGame.Core.SkillProgramStepOutcome SelectProgramOwnedCards(global::CardGame.Core.ProgramSkillFrame frame, int cardOwnerSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? expression, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, string resultBind, int minimumCards, int maximumCards, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> suits) => this._targetFactory().SelectProgramOwnedCards(frame, cardOwnerSeat, amount, expression, zones, resultBind, minimumCards, maximumCards, cardKinds, suits);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreDeferredHandAlignmentsCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreDeferredHandAlignmentsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => this._targetFactory().AdvanceRuntimeTop<TFrame>();
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
    public void ReturnRuntimeProgramMovement(long frameId) => this._targetFactory().ReturnRuntimeProgramMovement(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
    public void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreSoleTargetQuarantineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreSoleTargetQuarantineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SoleTargetQuarantineReceipt[] OutstandingSoleTargetQuarantines(int? turn = default, int? owner = default) => this._targetFactory().OutstandingSoleTargetQuarantines(turn, owner);
    public bool ContinueSoleTargetQuarantineReturns(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().ContinueSoleTargetQuarantineReturns(parent);
    public bool HasSoleTargetQuarantineEndBoundary(global::CardGame.Core.DeferredTurnEndFrame parent) => this._targetFactory().HasSoleTargetQuarantineEndBoundary(parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreDeferredHandAlignmentsCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDeferredHandAlignmentsCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}
