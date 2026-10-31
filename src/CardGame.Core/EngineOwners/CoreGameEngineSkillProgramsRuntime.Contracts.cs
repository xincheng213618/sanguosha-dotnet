#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreSkillProgramsCoreProgramSelectionModulesOperations : IProgramRuntimeOperations
{
    ProgramSelectionRegistry ProgramSelections { get; }
}

internal interface ICoreSkillProgramsCoreProgramRuntimeHooksOperations : IProgramRuntimeOperations
{
    ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreSkillProgramsCoreEquipmentContentOperations : IProgramRuntimeOperations
{
    EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreSkillProgramsProgramCompletionOperations : IProgramRuntimeOperations
{
    ProgramCompletionRuntime Get();
}

internal interface ICoreSkillProgramsCoreProgramActionModulesOperations : IProgramRuntimeOperations
{
    ProgramActionRouter ProgramActions { get; }
}

internal interface ICoreSkillProgramsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreSkillProgramsCoreProgramSelectionValidationOperations : IProgramRuntimeOperations
{
    ProgramSelectionValidationRegistry ProgramSelectionValidators { get; }
}

internal interface ICoreSkillProgramsCoreWushuangModeProjectionOperations : IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}
