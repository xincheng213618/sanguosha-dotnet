#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramLifecycleWindowRuntimePortsRuntime : global::CardGame.Core.IProgramLifecycleContexts, global::CardGame.Core.IProgramLifecycleFacts, global::CardGame.Core.IProgramLifecycleReturns
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries _characterChengGongYingOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries _characterLuZhiWeiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXuRongOlEntries _characterXuRongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries _characterZhaoZhongOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouQunOlEntries _characterZhouQunOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseEntries _coreActualDiscardPhaseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreAlternativePhaseCostProgramsEntries _coreAlternativePhaseCostProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDrawPhaseObligationsEntries _coreDrawPhaseObligationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries _coreAttributedMarkerEventsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries _coreCharacterStateProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries _coreDyingEntryProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries _coreIdentityConcealmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries _coreNativeDrawAttemptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries _corePhaseNamePredictionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundEndingEntries _coreRoundEndingOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineZhuijiProgramsEntries _coreZhuijiProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramLifecycleWindowRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries characterChengGongYingOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries characterLuZhiWeiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXuRongOlEntries characterXuRongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries characterZhaoZhongOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhouQunOlEntries characterZhouQunOlOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseEntries coreActualDiscardPhaseOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreAlternativePhaseCostProgramsEntries coreAlternativePhaseCostProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreDrawPhaseObligationsEntries coreDrawPhaseObligationsOperations, global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAttributedMarkerEventsEntries coreAttributedMarkerEventsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries coreCharacterStateProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingEntryProgramsEntries coreDyingEntryProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIdentityConcealmentEntries coreIdentityConcealmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries coreNativeDrawAttemptsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries corePhaseNamePredictionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundEndingEntries coreRoundEndingOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineZhuijiProgramsEntries coreZhuijiProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreDelayedTurnEffectsPendingTurnDelayedEffectsAccess coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
