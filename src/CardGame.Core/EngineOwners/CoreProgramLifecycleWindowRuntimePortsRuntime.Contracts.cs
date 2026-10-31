#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramLifecycleWindowRuntimePortsCoreDrawPhaseObligationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnDrawPhaseObligationWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame completed);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreRoundEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnRoundEndingLifecycle(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame child);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreAttributedMarkerEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAttributedEventOperations();
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterYanPuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnYanPuDyingExit(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResumeCharacterStateChange(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreHellActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactHellAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCorePhaseNamePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext AttachPhaseNamePredictionContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreZhuijiProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateZhuijiEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterLvQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateWeiluEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterXuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateXuRongDyingContext(global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreDyingEntryProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDyingEntryProgramWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreAlternativePhaseCostProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteActualJudgmentStarting(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReturnBeiMiHuTurnPreludeWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterZhouQunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnZhouQunPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterDuFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnDuFuRenPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreActualDiscardPhaseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDiscardPhaseAfterProgramWindow(global::CardGame.Core.CharacterState current);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterZhaoZhongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsIssuedDiciChainTrigger(global::CardGame.Core.SkillProgramTrigger t);
    global::CardGame.Core.ProgramSkillWindowContext CreateIssuedDiciChainContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext ordinary);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ContinueTurnAfterForeignActualContests(global::CardGame.Core.CharacterState current);
    void ContinueTurnAfterHengye(global::CardGame.Core.CharacterState current);
    void ContinueTurnAfterRoundPrograms(global::CardGame.Core.CharacterState current);
    void CompleteDrawPhaseAfterProgramWindow(global::CardGame.Core.CharacterState current, bool skipPlayPhaseAfterDraw, bool normalDrawReplaced, int normalDrawAdjustment, int? frozenBaseDrawCount = default, bool confirmedActualDrawSubstitution = false);
    void EndTurn();
    void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterChengGongYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryReturnChengGongYingPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateIdentityAppearanceContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate);
    void ResumeIdentityAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreNativeDrawAttemptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext AttachNativeDrawContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window, global::CardGame.Core.ProgramSkillWindowContext context);
    void ReturnNativeDrawAttemptWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCharacterLuZhiWeiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillWindowContext CreateQingzhongEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreProgramLifecycleWindowRuntimePortsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginNormalTurnStartAfterProgramBindings(global::CardGame.Core.CharacterState current);
    void CompletePlayPhaseAfterProgramWindow();
}
