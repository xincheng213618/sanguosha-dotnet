#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreRoundOnceCardlessNativeResponsesCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreRoundOnceCardlessNativeResponsesKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreResponseUseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasResponseUseCompletionObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreRoundOnceCardlessNativeResponsesKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreCommittedResponseUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCommittedResponseUsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreRoundOnceCardlessNativeUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardConversionSource Source, global::CardGame.Core.SkillProgramViewAs Rule)> RoundOnceCardlessRules(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind? output = default, bool forResponse = false);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreNativeResponseWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PopResponseWindow(long parentFrameId);
    void SetResponseParentStep(long frameId, global::CardGame.Core.ResolutionFrameStep step);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedPlayPhaseUseBan(int seat);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreRoundOnceCardlessBasicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RoundOnceCardlessBasicPaymentReceipt ConsumeRoundOnceCardlessBasic(global::CardGame.Core.CardConversionSource source, bool forResponse);
    global::CardGame.Core.RoundOnceCardlessBasicUseReceipt IssueRoundOnceCardlessBasicUse(global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource source);
    bool IsIssuedRoundOnceCardlessBasicUse(global::CardGame.Core.RoundOnceCardlessBasicUseReceipt? receipt, global::CardGame.Core.ResolutionFrame owner, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreResponseCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardResponseCompletedObserver(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreOffTurnUsedCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreFriendEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureFriendMinimumHandUse(global::CardGame.Core.CardActionContext? action);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreNativeDodgeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueAcceptedCardResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation);
    void CompleteSuccessfulDodgeResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.GroupCardHandle? ActiveGroupCard { get; set; }
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreRoundOnceCardlessNativeResponsesResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreRoundOnceCardlessNativeResponsesCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
}
