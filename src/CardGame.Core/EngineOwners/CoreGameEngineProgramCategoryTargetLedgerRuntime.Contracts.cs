#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramCategoryTargetLedgerCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceCurrentProgramCardUseActor(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat);
    global::CardGame.Core.SkillProgramStepOutcome AddCurrentProgramCardUseTarget(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat);
}

internal interface ICoreProgramCategoryTargetLedgerCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreProgramCategoryTargetLedgerCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreProgramCategoryTargetLedgerSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreProgramCategoryTargetLedgerCoreProgramDamageReductionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReduceProgramCurrentDamage(global::CardGame.Core.ProgramSkillFrame program, int amount);
}

internal interface ICoreProgramCategoryTargetLedgerKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
