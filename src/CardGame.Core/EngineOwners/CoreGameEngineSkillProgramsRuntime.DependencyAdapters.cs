#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramSelectionModulesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramSelectionModulesAdapter(Func<CoreProgramSelectionModulesRuntime> targetFactory) : ICoreSkillProgramsCoreProgramSelectionModulesOperations
{
    private readonly Func<CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public ProgramSelectionRegistry ProgramSelections { get => this._targetFactory().ProgramSelections; }
}

[Core.ProgramCapability(typeof(CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreSkillProgramsCoreProgramRuntimeHooksAdapter(Func<CoreProgramRuntimeHooksRuntime> targetFactory) : ICoreSkillProgramsCoreProgramRuntimeHooksOperations
{
    private readonly Func<CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CoreSkillProgramsCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICoreSkillProgramsCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}

[Core.ProgramCapability(typeof(ProgramCompletionRuntime))]
internal sealed class CoreSkillProgramsProgramCompletionAdapter(Func<ProgramCompletionRuntime> targetFactory) : ICoreSkillProgramsProgramCompletionOperations
{
    private readonly Func<ProgramCompletionRuntime> _targetFactory = targetFactory;
    public ProgramCompletionRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreProgramActionModulesRuntime))]
internal sealed class CoreSkillProgramsCoreProgramActionModulesAdapter(Func<CoreProgramActionModulesRuntime> targetFactory) : ICoreSkillProgramsCoreProgramActionModulesOperations
{
    private readonly Func<CoreProgramActionModulesRuntime> _targetFactory = targetFactory;
    public ProgramActionRouter ProgramActions { get => this._targetFactory().ProgramActions; }
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreSkillProgramsCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreSkillProgramsCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[Core.ProgramCapability(typeof(CoreGameEngineProgramSelectionValidationRuntime))]
internal sealed class CoreSkillProgramsCoreProgramSelectionValidationAdapter(Func<CoreGameEngineProgramSelectionValidationRuntime> targetFactory) : ICoreSkillProgramsCoreProgramSelectionValidationOperations
{
    private readonly Func<CoreGameEngineProgramSelectionValidationRuntime> _targetFactory = targetFactory;
    public ProgramSelectionValidationRegistry ProgramSelectionValidators { get => this._targetFactory().ProgramSelectionValidators; }
}

[Core.ProgramCapability(typeof(CoreWushuangModeProjectionRuntime))]
internal sealed class CoreSkillProgramsCoreWushuangModeProjectionAdapter(Func<CoreWushuangModeProjectionRuntime> targetFactory) : ICoreSkillProgramsCoreWushuangModeProjectionOperations
{
    private readonly Func<CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}
