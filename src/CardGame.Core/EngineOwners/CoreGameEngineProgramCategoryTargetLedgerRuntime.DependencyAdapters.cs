#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime))]
internal sealed class CoreProgramCategoryTargetLedgerCoreProgramCardUseRolesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCategoryTargetLedgerCoreProgramCardUseRolesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardUseRolesRuntime> _targetFactory = targetFactory;
    public void ReplaceCurrentProgramCardUseActor(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat) => this._targetFactory().ReplaceCurrentProgramCardUseActor(frame, actorSeat);
    public global::CardGame.Core.SkillProgramStepOutcome AddCurrentProgramCardUseTarget(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat) => this._targetFactory().AddCurrentProgramCardUseTarget(frame, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramCategoryTargetLedgerCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCategoryTargetLedgerCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageReductionRuntime))]
internal sealed class CoreProgramCategoryTargetLedgerCoreProgramDamageReductionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageReductionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramCategoryTargetLedgerCoreProgramDamageReductionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDamageReductionRuntime> _targetFactory = targetFactory;
    public void ReduceProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program, int amount) => this._targetFactory().ReduceProgramCurrentDamage(program, amount);
}
