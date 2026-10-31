#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreSkillProgramsCoreProgramSelectionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSelectionRegistry ProgramSelections { get; }
}

internal interface ICoreSkillProgramsCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}

internal interface ICoreSkillProgramsCoreEquipmentContentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.EquipmentContentRegistry EquipmentRules { get; }
}

internal interface ICoreSkillProgramsProgramCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCompletionRuntime Get();
}

internal interface ICoreSkillProgramsCoreProgramActionModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramActionRouter ProgramActions { get; }
}

internal interface ICoreSkillProgramsCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreSkillProgramsCoreProgramSelectionValidationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSelectionValidationRegistry ProgramSelectionValidators { get; }
}

internal interface ICoreSkillProgramsCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}
