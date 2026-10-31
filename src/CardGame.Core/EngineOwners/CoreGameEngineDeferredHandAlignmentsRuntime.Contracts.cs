#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDeferredHandAlignmentsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceAfterDeferredTurnEnd(global::CardGame.Core.CharacterState previous, bool skipped);
}

internal interface ICoreDeferredHandAlignmentsCoreDeferredTurnEndPreludesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueDeferredTurnEndPrelude(global::CardGame.Core.DeferredTurnEndFrame parent);
    bool HasDeferredTurnEndPreludeBoundary(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICoreDeferredHandAlignmentsCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ContinueAfterTurnEndedPrograms(global::CardGame.Core.DeferredTurnEndFrame parent);
    bool CompleteAfterTurnEndedBinding(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context);
    bool HasAfterTurnEndedBoundaryFrame(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICoreDeferredHandAlignmentsCorePrivateTurnHoldsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasActualTurnEndMovementPrelude { get; }
}

internal interface ICoreDeferredHandAlignmentsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreDeferredHandAlignmentsCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveLogicalDiscardCards(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreDeferredHandAlignmentsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreDeferredHandAlignmentsCharacterAHuiNanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireAHuiNanJuemanTurnEndWine();
}

internal interface ICoreDeferredHandAlignmentsCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExpireQinLangXianyingTurnEndWine();
}

internal interface ICoreDeferredHandAlignmentsCoreProgramOwnedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect GetOwnedSelectionEffect(global::CardGame.Core.ProgramSkillFrame frame);
    global::CardGame.Core.SkillProgramStepOutcome SelectProgramOwnedCards(global::CardGame.Core.ProgramSkillFrame frame, int cardOwnerSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? expression, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, string resultBind, int minimumCards, int maximumCards, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> cardKinds, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> suits);
}

internal interface ICoreDeferredHandAlignmentsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDeferredHandAlignmentsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDeferredHandAlignmentsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICoreDeferredHandAlignmentsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreDeferredHandAlignmentsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICoreDeferredHandAlignmentsCoreSoleTargetQuarantineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SoleTargetQuarantineReceipt[] OutstandingSoleTargetQuarantines(int? turn = default, int? owner = default);
    bool ContinueSoleTargetQuarantineReturns(global::CardGame.Core.DeferredTurnEndFrame parent);
    bool HasSoleTargetQuarantineEndBoundary(global::CardGame.Core.DeferredTurnEndFrame parent);
}

internal interface ICoreDeferredHandAlignmentsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDeferredHandAlignmentsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreDeferredHandAlignmentsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreDeferredHandAlignmentsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDeferredHandAlignmentsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}
