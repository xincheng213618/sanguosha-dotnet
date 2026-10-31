#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreNativeDrawThresholdAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreNativeDrawThresholdOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawThresholdRuntime> _targetFactory = targetFactory;
    public bool TracksNativeDrawThresholds { get => this._targetFactory().TracksNativeDrawThresholds; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false) => this._targetFactory().FreezeAndCaptureCaoXiNativeDrawBase(player, replaced);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCharacterZhuHuanBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCharacterZhuHuanBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuHuanBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool TracksBoundaryZhuHuanPhaseSkips { get => this._targetFactory().TracksBoundaryZhuHuanPhaseSkips; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCharacterChengGongYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCharacterChengGongYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> _targetFactory = targetFactory;
    public void ConsumeChengGongYingDraw(int actorSeat) => this._targetFactory().ConsumeChengGongYingDraw(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceChunlaoJudgment3161Runtime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreShanheSourceChunlaoJudgment3161Adapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceChunlaoJudgment3161Runtime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreShanheSourceChunlaoJudgment3161Operations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceChunlaoJudgment3161Runtime> _targetFactory = targetFactory;
    public bool TryCompleteShanheSourceChunlaoJudgmentDraw(global::CardGame.Core.DrawPhaseObligationFrame frame) => this._targetFactory().TryCompleteShanheSourceChunlaoJudgmentDraw(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void EndTurn() => this._targetFactory().EndTurn();
    public void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false) => this._targetFactory().CompleteTurnStartAfterDraw(current, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCharacterDuFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCharacterDuFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> _targetFactory = targetFactory;
    public void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat) => this._targetFactory().ConsumeDuFuRenPhase(kind, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreShanheS5ShensuAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreShanheS5ShensuOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShensuRuntime> _targetFactory = targetFactory;
    public bool TracksShanheS5PhaseSkips { get => this._targetFactory().TracksShanheS5PhaseSkips; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreLeastHandMarkerDrawProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreLeastHandMarkerDrawProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreLeastHandMarkerDrawProgramsRuntime> _targetFactory = targetFactory;
    public bool HasIssuedExtraDrawMarker(int seat) => this._targetFactory().HasIssuedExtraDrawMarker(seat);
    public global::CardGame.Core.ProgramMarkerExtraDrawPolicy? ConsumeIssuedExtraDrawMarker(global::CardGame.Core.DrawPhaseObligationFrame frame) => this._targetFactory().ConsumeIssuedExtraDrawMarker(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime))]
internal sealed class CoreOwnedDrawPhaseRuntimePortsCoreNativeDrawAttemptsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnedDrawPhaseRuntimePortsCoreNativeDrawAttemptsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime> _targetFactory = targetFactory;
    public bool HasNativeDrawAttemptCapability { get => this._targetFactory().HasNativeDrawAttemptCapability; }
}
