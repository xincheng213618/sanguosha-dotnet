#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime))]
internal sealed class CoreProgramSkillChangesCoreDyingOwnerSkillReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreDyingOwnerSkillReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingOwnerSkillReplacementRuntime> _targetFactory = targetFactory;
    public bool IsDyingOwnerReplacementBinding(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().IsDyingOwnerReplacementBinding(frame);
    public global::System.Collections.Generic.IReadOnlyList<string> AcquireDyingOwnerReplacementSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> skillIds) => this._targetFactory().AcquireDyingOwnerReplacementSkills(frame, skillIds);
    public global::CardGame.Core.ProgramReplacedPaidHpContinuation? CaptureDyingOwnerReplacementContinuation(global::CardGame.Core.ProgramSkillFrame replacement, global::CardGame.Core.SkillProgramEffect effect) => this._targetFactory().CaptureDyingOwnerReplacementContinuation(replacement, effect);
    public void IssueReplacedPaidHpContinuation(global::CardGame.Core.ProgramReplacedPaidHpContinuation continuation) => this._targetFactory().IssueReplacedPaidHpContinuation(continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreProgramSkillChangesCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => this._targetFactory().FinishProgramSkill(frame, completed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreProgramSkillChangesCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreProgramSkillChangesCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<string> EnabledContentSkillIds(global::CardGame.Core.CharacterState player) => this._targetFactory().EnabledContentSkillIds(player);
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramSkillChangeRuntime))]
internal sealed class CoreProgramSkillChangesProgramSkillChangeAdapter(global::System.Func<global::CardGame.Core.ProgramSkillChangeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesProgramSkillChangeOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramSkillChangeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillChangeRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreProgramSkillChangesCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreProgramSkillChangesCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime))]
internal sealed class CoreProgramSkillChangesCoreProgramPersistentStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreProgramPersistentStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPersistentStateRuntime> _targetFactory = targetFactory;
    public void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value) => this._targetFactory().SetProgramBooleanState(ownerSeat, skillId, instanceId, stateId, value);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime))]
internal sealed class CoreProgramSkillChangesCoreConversionStatesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramSkillChangesCoreConversionStatesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> _targetFactory = targetFactory;
    public void RegisterTaggedConversionSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().RegisterTaggedConversionSkill(player, skillId);
}
