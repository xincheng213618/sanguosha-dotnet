#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActualTurnSkillGrantsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreActualTurnSkillGrantsCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CommitProgramParticipantSkillGrants(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, global::System.Collections.Generic.IReadOnlyList<string> skillIds, string sourceId, global::CardGame.Core.SkillGrantTurnExpiry? turnExpiry, global::CardGame.Core.SkillGrantNextActualTurnStartExpiry? nextStartExpiry);
}

internal interface ICoreActualTurnSkillGrantsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreActualTurnSkillGrantsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreActualTurnSkillGrantsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreActualTurnSkillGrantsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreActualTurnSkillGrantsCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateProgramTurnEffectGrant(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreActualTurnSkillGrantsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreActualTurnSkillGrantsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    bool IsEndOwnedExtraPlayPhase();
}

internal interface ICoreActualTurnSkillGrantsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}
