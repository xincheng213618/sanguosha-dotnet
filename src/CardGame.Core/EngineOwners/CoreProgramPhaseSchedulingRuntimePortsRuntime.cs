#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramPhaseSchedulingRuntimePortsRuntime : IProgramPhaseNativeEntries, IProgramPhaseNativeReturns, IProgramPhasePipelineHooks, IProgramPhaseQueries, IProgramPhaseScheduleState, IProgramPhaseSchedulingFacts
{
    private readonly RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly RuntimeEntries.CharacterChengGongYingOlEntries _characterChengGongYingOlOperations;
    private readonly RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly RuntimeEntries.CharacterZhangShiPingOlEntries _characterZhangShiPingOlOperations;
    private readonly RuntimeEntries.CharacterZhangYiOlEntries _characterZhangYiOlOperations;
    private readonly IKernelProgramPhaseScheduleAccess _programPhaseScheduleOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreGameEngineActualTurnSkillGrantsEntries _coreActualTurnSkillGrantsOperations;
    private readonly RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries _coreAfterTurnEndedProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries _coreGrantedEntityPhasesOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries _corePlayPhaseSkillGrantsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess _coreActualTurnProgressPhaseAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramPhaseSchedulingRuntimePortsRuntime(RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, RuntimeEntries.CharacterChengGongYingOlEntries characterChengGongYingOlOperations, RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, RuntimeEntries.CharacterZhangShiPingOlEntries characterZhangShiPingOlOperations, RuntimeEntries.CharacterZhangYiOlEntries characterZhangYiOlOperations, IKernelProgramPhaseScheduleAccess programPhaseScheduleOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, RuntimeEntries.CoreGameEngineActualTurnSkillGrantsEntries coreActualTurnSkillGrantsOperations, RuntimeEntries.CoreGameEngineAfterTurnEndedProgramsEntries coreAfterTurnEndedProgramsOperations, RuntimeEntries.CoreGameEngineGrantedEntityPhasesEntries coreGrantedEntityPhasesOperations, RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, RuntimeEntries.CoreGameEnginePlayPhaseSkillGrantsEntries corePlayPhaseSkillGrantsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations, EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess coreActualTurnProgressPhaseAccess, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
