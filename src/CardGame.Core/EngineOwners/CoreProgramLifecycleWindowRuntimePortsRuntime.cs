#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramLifecycleWindowRuntimePortsRuntime : IProgramLifecycleContexts, IProgramLifecycleFacts, IProgramLifecycleReturns
{
    private readonly RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly RuntimeEntries.CharacterChengGongYingOlEntries _characterChengGongYingOlOperations;
    private readonly RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly RuntimeEntries.CharacterLuZhiWeiOlEntries _characterLuZhiWeiOlOperations;
    private readonly RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly RuntimeEntries.CharacterXuRongOlEntries _characterXuRongOlOperations;
    private readonly RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly RuntimeEntries.CharacterZhaoZhongOlEntries _characterZhaoZhongOlOperations;
    private readonly RuntimeEntries.CharacterZhouQunOlEntries _characterZhouQunOlOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreActualDiscardPhaseEntries _coreActualDiscardPhaseOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreAlternativePhaseCostProgramsEntries _coreAlternativePhaseCostProgramsOperations;
    private readonly RuntimeEntries.CoreDrawPhaseObligationsEntries _coreDrawPhaseObligationsOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries _coreAttributedMarkerEventsOperations;
    private readonly RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries _coreDyingEntryProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries _coreNativeDrawAttemptsOperations;
    private readonly RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries _corePhaseNamePredictionOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundEndingEntries _coreRoundEndingOperations;
    private readonly RuntimeEntries.CoreGameEngineZhuijiProgramsEntries _coreZhuijiProgramsOperations;
    private readonly EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramLifecycleWindowRuntimePortsRuntime(RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, RuntimeEntries.CharacterChengGongYingOlEntries characterChengGongYingOlOperations, RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, RuntimeEntries.CharacterLuZhiWeiOlEntries characterLuZhiWeiOlOperations, RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, RuntimeEntries.CharacterXuRongOlEntries characterXuRongOlOperations, RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, RuntimeEntries.CharacterZhaoZhongOlEntries characterZhaoZhongOlOperations, RuntimeEntries.CharacterZhouQunOlEntries characterZhouQunOlOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreActualDiscardPhaseEntries coreActualDiscardPhaseOperations, RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, RuntimeEntries.CoreAlternativePhaseCostProgramsEntries coreAlternativePhaseCostProgramsOperations, RuntimeEntries.CoreDrawPhaseObligationsEntries coreDrawPhaseObligationsOperations, RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries coreAttributedMarkerEventsOperations, RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries coreDyingEntryProgramsOperations, RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations, RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries coreNativeDrawAttemptsOperations, RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries corePhaseNamePredictionOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRoundEndingEntries coreRoundEndingOperations, RuntimeEntries.CoreGameEngineZhuijiProgramsEntries coreZhuijiProgramsOperations, EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBeiMiHuOlOperations = characterBeiMiHuOlOperations;
        this._characterChengGongYingOlOperations = characterChengGongYingOlOperations;
        this._characterDuFuRenOlOperations = characterDuFuRenOlOperations;
        this._characterLuZhiWeiOlOperations = characterLuZhiWeiOlOperations;
        this._characterLvQianOlOperations = characterLvQianOlOperations;
        this._characterXuRongOlOperations = characterXuRongOlOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._characterZhaoZhongOlOperations = characterZhaoZhongOlOperations;
        this._characterZhouQunOlOperations = characterZhouQunOlOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualDiscardPhaseOperations = coreActualDiscardPhaseOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreAlternativePhaseCostProgramsOperations = coreAlternativePhaseCostProgramsOperations;
        this._coreDrawPhaseObligationsOperations = coreDrawPhaseObligationsOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAttributedMarkerEventsOperations = coreAttributedMarkerEventsOperations;
        this._coreCharacterStateProgramsOperations = coreCharacterStateProgramsOperations;
        this._coreDyingEntryProgramsOperations = coreDyingEntryProgramsOperations;
        this._coreHellActivitiesOperations = coreHellActivitiesOperations;
        this._coreIdentityConcealmentOperations = coreIdentityConcealmentOperations;
        this._coreNativeDrawAttemptsOperations = coreNativeDrawAttemptsOperations;
        this._corePhaseNamePredictionOperations = corePhaseNamePredictionOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRoundEndingOperations = coreRoundEndingOperations;
        this._coreZhuijiProgramsOperations = coreZhuijiProgramsOperations;
        this._coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess = coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
