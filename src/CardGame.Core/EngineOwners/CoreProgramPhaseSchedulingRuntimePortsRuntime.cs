#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramPhaseSchedulingRuntimePortsRuntime : global::CardGame.Core.IProgramPhaseNativeEntries, global::CardGame.Core.IProgramPhaseNativeReturns, global::CardGame.Core.IProgramPhasePipelineHooks, global::CardGame.Core.IProgramPhaseQueries, global::CardGame.Core.IProgramPhaseScheduleState, global::CardGame.Core.IProgramPhaseSchedulingFacts
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries _characterChengGongYingOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries _characterZhangShiPingOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYiOlEntries _characterZhangYiOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPhaseScheduleAccess _programPhaseScheduleOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnSkillGrantsEntries _coreActualTurnSkillGrantsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries _coreAfterTurnEndedProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries _coreGrantedEntityPhasesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries _corePlayPhaseSkillGrantsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess _coreActualTurnProgressPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramPhaseSchedulingRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries characterChengGongYingOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries characterZhangShiPingOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYiOlEntries characterZhangYiOlOperations, global::CardGame.Core.EngineOwners.IKernelProgramPhaseScheduleAccess programPhaseScheduleOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualTurnSkillGrantsEntries coreActualTurnSkillGrantsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries coreAfterTurnEndedProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries coreGrantedEntityPhasesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries corePlayPhaseSkillGrantsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess coreActualTurnProgressPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoXiOlOperations = characterCaoXiOlOperations;
        this._characterChengGongYingOlOperations = characterChengGongYingOlOperations;
        this._characterDuFuRenOlOperations = characterDuFuRenOlOperations;
        this._characterZhangShiPingOlOperations = characterZhangShiPingOlOperations;
        this._characterZhangYiOlOperations = characterZhangYiOlOperations;
        this._programPhaseScheduleOperations = programPhaseScheduleOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreActualTurnSkillGrantsOperations = coreActualTurnSkillGrantsOperations;
        this._coreAfterTurnEndedProgramsOperations = coreAfterTurnEndedProgramsOperations;
        this._coreGrantedEntityPhasesOperations = coreGrantedEntityPhasesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._corePlayPhaseSkillGrantsOperations = corePlayPhaseSkillGrantsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreActualTurnProgressPhaseAccess = coreActualTurnProgressPhaseAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
