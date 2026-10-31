#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreDrawPhaseObligationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreDrawPhaseObligationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDrawPhaseObligationsRuntime> _targetFactory = targetFactory;
    public void ReturnDrawPhaseObligationWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame completed) => this._targetFactory().ReturnDrawPhaseObligationWindow(completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreRoundEndingAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreRoundEndingOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundEndingRuntime> _targetFactory = targetFactory;
    public void ReturnRoundEndingLifecycle(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame child) => this._targetFactory().ReturnRoundEndingLifecycle(child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreAttributedMarkerEventsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreAttributedMarkerEventsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerEventsRuntime> _targetFactory = targetFactory;
    public bool HasAttributedEventOperations() => this._targetFactory().HasAttributedEventOperations();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterYanPuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterYanPuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanPuOlRuntime> _targetFactory = targetFactory;
    public void ReturnYanPuDyingExit(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().ReturnYanPuDyingExit(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public void ResumeCharacterStateChange(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().ResumeCharacterStateChange(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreHellActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreHellActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> _targetFactory = targetFactory;
    public bool IsExactHellAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().IsExactHellAppearanceWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCorePhaseNamePredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCorePhaseNamePredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext AttachPhaseNamePredictionContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().AttachPhaseNamePredictionContext(c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreZhuijiProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreZhuijiProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateZhuijiEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateZhuijiEndingContext(f, c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterLvQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterLvQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateWeiluEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateWeiluEndingContext(frame, candidate, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterXuRongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterXuRongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuRongOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateXuRongDyingContext(global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateXuRongDyingContext(context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreDyingEntryProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreDyingEntryProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> _targetFactory = targetFactory;
    public void CompleteDyingEntryProgramWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().CompleteDyingEntryProgramWindow(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreAlternativePhaseCostProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreAlternativePhaseCostProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreAlternativePhaseCostProgramsRuntime> _targetFactory = targetFactory;
    public void CompleteActualJudgmentStarting(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().CompleteActualJudgmentStarting(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterBeiMiHuOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterBeiMiHuOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBeiMiHuOlRuntime> _targetFactory = targetFactory;
    public void ReturnBeiMiHuTurnPreludeWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().ReturnBeiMiHuTurnPreludeWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterZhouQunOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterZhouQunOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouQunOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnZhouQunPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().TryReturnZhouQunPaidPreparation(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterDuFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterDuFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnDuFuRenPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().TryReturnDuFuRenPaidPreparation(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreActualDiscardPhaseAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreActualDiscardPhaseOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseRuntime> _targetFactory = targetFactory;
    public void CompleteDiscardPhaseAfterProgramWindow(global::CardGame.Core.CharacterState current) => this._targetFactory().CompleteDiscardPhaseAfterProgramWindow(current);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterZhaoZhongOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterZhaoZhongOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhaoZhongOlRuntime> _targetFactory = targetFactory;
    public bool IsIssuedDiciChainTrigger(global::CardGame.Core.SkillProgramTrigger t) => this._targetFactory().IsIssuedDiciChainTrigger(t);
    public global::CardGame.Core.ProgramSkillWindowContext CreateIssuedDiciChainContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext ordinary) => this._targetFactory().CreateIssuedDiciChainContext(w, c, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void ContinueTurnAfterForeignActualContests(global::CardGame.Core.CharacterState current) => this._targetFactory().ContinueTurnAfterForeignActualContests(current);
    public void ContinueTurnAfterHengye(global::CardGame.Core.CharacterState current) => this._targetFactory().ContinueTurnAfterHengye(current);
    public void ContinueTurnAfterRoundPrograms(global::CardGame.Core.CharacterState current) => this._targetFactory().ContinueTurnAfterRoundPrograms(current);
    public void CompleteDrawPhaseAfterProgramWindow(global::CardGame.Core.CharacterState current, bool skipPlayPhaseAfterDraw, bool normalDrawReplaced, int normalDrawAdjustment, int? frozenBaseDrawCount = default, bool confirmedActualDrawSubstitution = false) => this._targetFactory().CompleteDrawPhaseAfterProgramWindow(current, skipPlayPhaseAfterDraw, normalDrawReplaced, normalDrawAdjustment, frozenBaseDrawCount, confirmedActualDrawSubstitution);
    public void EndTurn() => this._targetFactory().EndTurn();
    public void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false) => this._targetFactory().CompleteTurnStartAfterDraw(current, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterChengGongYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterChengGongYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> _targetFactory = targetFactory;
    public bool TryReturnChengGongYingPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => this._targetFactory().TryReturnChengGongYingPaidPreparation(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateIdentityAppearanceContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate) => this._targetFactory().CreateIdentityAppearanceContext(window, candidate);
    public void ResumeIdentityAppearanceWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().ResumeIdentityAppearanceWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreNativeDrawAttemptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreNativeDrawAttemptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext AttachNativeDrawContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().AttachNativeDrawContext(window, context);
    public void ReturnNativeDrawAttemptWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => this._targetFactory().ReturnNativeDrawAttemptWindow(window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCharacterLuZhiWeiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCharacterLuZhiWeiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillWindowContext CreateQingzhongEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame f, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().CreateQingzhongEndingContext(f, c, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramLifecycleWindowRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramLifecycleWindowRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void BeginNormalTurnStartAfterProgramBindings(global::CardGame.Core.CharacterState current) => this._targetFactory().BeginNormalTurnStartAfterProgramBindings(current);
    public void CompletePlayPhaseAfterProgramWindow() => this._targetFactory().CompletePlayPhaseAfterProgramWindow();
}
