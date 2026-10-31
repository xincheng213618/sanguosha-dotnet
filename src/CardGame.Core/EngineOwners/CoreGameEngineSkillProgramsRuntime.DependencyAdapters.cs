#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramSelectionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramSelectionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get => this._targetFactory().ProgramSelections; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreSkillProgramsCoreProgramRuntimeHooksAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramRuntimeHooksOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreSkillProgramsCoreEquipmentContentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreEquipmentContentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.ProgramCompletionRuntime))]
internal sealed class CoreSkillProgramsProgramCompletionAdapter(global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsProgramCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.ProgramCompletionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramCompletionRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramActionModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramActionModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreNativeRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreNativeRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectionValidationRuntime))]
internal sealed class CoreSkillProgramsCoreProgramSelectionValidationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectionValidationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreProgramSelectionValidationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectionValidationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSelectionValidationRegistry ProgramSelectionValidators { get => this._targetFactory().ProgramSelectionValidators; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime))]
internal sealed class CoreSkillProgramsCoreWushuangModeProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreSkillProgramsCoreWushuangModeProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}
