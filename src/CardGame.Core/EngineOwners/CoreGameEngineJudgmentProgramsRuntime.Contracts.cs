#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreJudgmentProgramsCoreJudgmentCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame? ActiveJudgment { get; }
}

internal interface ICoreJudgmentProgramsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreJudgmentProgramsCharacterZhangQiYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int EffectiveZhenyiJudgmentRank(global::CardGame.Core.JudgmentFrame frame, int fallback);
}

internal interface ICoreJudgmentProgramsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreJudgmentProgramsCoreJudgmentOutcomeDamageEdgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactJudgmentOutcomeFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index);
}

internal interface ICoreJudgmentProgramsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreJudgmentProgramsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? CompleteFinalizedJudgment(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard, bool succeeded);
    void ResumeCompletedJudgment(global::CardGame.Core.GameEngine.JudgmentCompletionReceipt receipt);
}

internal interface ICoreJudgmentProgramsProgramWindowCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramWindowCursorRuntime Get();
}

internal interface ICoreJudgmentProgramsCoreOwnedDamagePointJudgmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactOwnedDamagePointJudgmentFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int windowIndex);
}

internal interface ICoreJudgmentProgramsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreJudgmentProgramsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreJudgmentProgramsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, global::CardGame.Core.SkillProgramTargetKind targetKind, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default, global::CardGame.Core.PlayerMarkerKind? marker = default, global::CardGame.Core.ProgramParticipantReference? actorReference = default);
    bool CanRunProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreJudgmentProgramsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreJudgmentProgramsCoreYaodaoChitongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactYaodaoChitongJudgmentFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index);
}

internal interface ICoreJudgmentProgramsCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramJudgmentTriggerCandidate[] PrependShenZfJudgmentGain(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramJudgmentTriggerCandidate[] candidates);
    bool IsExactShenZfJudgmentGainSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index);
}

internal interface ICoreJudgmentProgramsCharacterXiZhiCaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactChouceFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame? judgmentFrame, int frameIndex);
}

internal interface ICoreJudgmentProgramsKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreJudgmentProgramsCoreDamageJudgmentSuitPaymentObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactDamageJudgmentSuitPaymentFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index);
}

internal interface ICoreJudgmentProgramsCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreJudgmentProgramsCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    global::CardGame.Core.CommandError? ValidateHumanPrompt(int actorSeat, global::CardGame.Core.DecisionKind expectedKind, global::CardGame.Core.PromptId? promptId, global::CardGame.Core.CommandErrorCode inactiveCode);
    void ClearPendingDecision();
}

internal interface ICoreJudgmentProgramsCharacterMaYuanYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactBinghuoFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame? judgment, int frameIndex);
}

internal interface ICoreJudgmentProgramsKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreJudgmentProgramsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
}

internal interface ICoreJudgmentProgramsCoreExactRepeatedJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactRepeatedJudgmentFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int windowIndex);
}

internal interface ICoreJudgmentProgramsCoreZhenyiJudgmentWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerWindow CurrentZhenyiJudgmentWindowKind(long judgmentId);
    global::CardGame.Core.ProgramJudgmentTriggerWindowFrame RefreshZhenyiBeforeResultWindow(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame pending);
    void CompleteZhenyiBeforeResultWindow(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame pending);
}

internal interface ICoreJudgmentProgramsCharacterZhouQunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactNingwuFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame? judgment, int index);
}

internal interface ICoreJudgmentProgramsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreJudgmentProgramsCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreJudgmentProgramsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}
