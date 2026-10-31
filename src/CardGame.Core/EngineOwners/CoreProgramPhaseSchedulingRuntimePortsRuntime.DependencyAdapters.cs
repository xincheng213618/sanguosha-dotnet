#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCoreGrantedEntityPhasesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCoreGrantedEntityPhasesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGrantedEntityPhasesRuntime> _targetFactory = targetFactory;
    public bool IsReturningGrantedEntityPhase(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsReturningGrantedEntityPhase(f);
    public global::CardGame.Core.ProgramSkillFrame FreezeGrantedEntityPhaseEnd(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().FreezeGrantedEntityPhaseEnd(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCharacterDuFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCharacterDuFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> _targetFactory = targetFactory;
    public void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat) => this._targetFactory().ConsumeDuFuRenPhase(kind, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCoreAfterTurnEndedProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCoreAfterTurnEndedProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _targetFactory = targetFactory;
    public bool MatchesAfterTurnEndedChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramSkillFrame child, global::CardGame.Core.ProgramSkillWindowContext context) => this._targetFactory().MatchesAfterTurnEndedChild(parent, child, context);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYiOlRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCharacterZhangYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCharacterZhangYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYiOlRuntime> _targetFactory = targetFactory;
    public void ResumeZhangYiDianjunExtraPlay(long programFrameId) => this._targetFactory().ResumeZhangYiDianjunExtraPlay(programFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCoreActualTurnSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCoreActualTurnSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnSkillGrantsRuntime> _targetFactory = targetFactory;
    public void ExpireActualTurnSkillGrants(int turnNumber, int turnOwnerSeat) => this._targetFactory().ExpireActualTurnSkillGrants(turnNumber, turnOwnerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public bool TryBeginDrawPhaseProgramWindow(global::CardGame.Core.CharacterState owner, bool skipPlayPhaseAfterDraw) => this._targetFactory().TryBeginDrawPhaseProgramWindow(owner, skipPlayPhaseAfterDraw);
    public void CompleteDetachedProgramBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().CompleteDetachedProgramBinding(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCharacterZhangShiPingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCharacterZhangShiPingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> _targetFactory = targetFactory;
    public bool TryResumeZhangShiPingHongjiPhase() => this._targetFactory().TryResumeZhangShiPingHongjiPhase();
    public bool TryCancelZhangShiPingHongjiPhase() => this._targetFactory().TryCancelZhangShiPingHongjiPhase();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCoreActualTurnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCoreActualTurnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _targetFactory = targetFactory;
    public void EnterPlayPhase(global::CardGame.Core.CharacterState current) => this._targetFactory().EnterPlayPhase(current);
    public void CompleteTurnStartAfterDraw(global::CardGame.Core.CharacterState current, global::CardGame.Core.GameEngine.DelayedTurnEffects delayedEffects, bool afterNormalDrawProgramsCompleted = false, bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false) => this._targetFactory().CompleteTurnStartAfterDraw(current, delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    public void ExpireActualTurnEndEffects(global::CardGame.Core.CharacterState previous) => this._targetFactory().ExpireActualTurnEndEffects(previous);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCorePlayPhaseSkillGrantsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCorePlayPhaseSkillGrantsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayPhaseSkillGrantsRuntime> _targetFactory = targetFactory;
    public void ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber) => this._targetFactory().ExpirePlayPhaseSkillGrantsForEndedTurn(turnNumber);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCharacterChengGongYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCharacterChengGongYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengGongYingOlRuntime> _targetFactory = targetFactory;
    public void ConsumeChengGongYingDraw(int actorSeat) => this._targetFactory().ConsumeChengGongYingDraw(actorSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCharacterCaoXiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCharacterCaoXiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiOlRuntime> _targetFactory = targetFactory;
    public int FreezeAndCaptureCaoXiNativeDrawBase(global::CardGame.Core.CharacterState player, bool replaced = false) => this._targetFactory().FreezeAndCaptureCaoXiNativeDrawBase(player, replaced);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramPhaseSchedulingRuntimePortsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramPhaseSchedulingRuntimePortsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => this._targetFactory().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
}
