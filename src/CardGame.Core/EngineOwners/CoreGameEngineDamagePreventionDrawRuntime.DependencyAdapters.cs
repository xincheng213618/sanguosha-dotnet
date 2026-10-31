#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePreventionDrawObserversRuntime))]
internal sealed class CoreDamagePreventionDrawCorePreventionDrawObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionDrawCorePreventionDrawObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePreventionDrawObserversRuntime> _targetFactory = targetFactory;
    public bool IsValidPreventionDrawReceipt(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.BeforeDamageProgramWindowFrame parent) => this._targetFactory().IsValidPreventionDrawReceipt(root, parent);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreDamagePreventionDrawCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionDrawCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime))]
internal sealed class CoreDamagePreventionDrawCoreDamagePreventionProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionDrawCoreDamagePreventionProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> _targetFactory = targetFactory;
    public void PreventProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program) => this._targetFactory().PreventProgramCurrentDamage(program);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreDamagePreventionDrawCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionDrawCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime))]
internal sealed class CoreDamagePreventionDrawCoreBoundCardMovementContinuationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreDamagePreventionDrawCoreBoundCardMovementContinuationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat) => this._targetFactory().AwaitProgramBoundCardMovements(frameId, ownerSeat);
}
