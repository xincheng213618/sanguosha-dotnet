#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime))]
internal sealed class CoreProgramBooleanStateExpirationCoreProgramPersistentStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBooleanStateExpirationCoreProgramPersistentStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramBooleanStateDefinition GetProgramBooleanStateDefinition(global::CardGame.Core.ProgramSkillFrame frame, string stateId) => this._targetFactory().GetProgramBooleanStateDefinition(frame, stateId);
    public void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value) => this._targetFactory().SetProgramBooleanState(ownerSeat, skillId, instanceId, stateId, value);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CoreProgramBooleanStateExpirationCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramBooleanStateExpirationCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().ValidateProgramTurnEffectGrant(frame);
}
