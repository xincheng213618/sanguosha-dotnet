#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramTurnWindowEntryRuntimePortsRuntime : global::CardGame.Core.IProgramTurnWindowEntryFacts, global::CardGame.Core.IProgramTurnWindowIssuedCandidates, global::CardGame.Core.IProgramTurnWindowPhaseReceipts
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoHongMouEntries _characterCaoHongMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuBaOlEntries _characterLiuBaOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries _characterLuZhiWeiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWangKuangOlEntries _characterWangKuangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries _characterZhangShiPingOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangXunAnniversaryEntries _characterZhangXunAnniversaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries _corePhaseHandSeizureOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries _corePhaseNamePredictionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineZhuijiProgramsEntries _coreZhuijiProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess;
    internal CoreProgramTurnWindowEntryRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoHongMouEntries characterCaoHongMouOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuBaOlEntries characterLiuBaOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries characterLuZhiWeiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWangKuangOlEntries characterWangKuangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries characterZhangShiPingOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangXunAnniversaryEntries characterZhangXunAnniversaryOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries corePhaseHandSeizureOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries corePhaseNamePredictionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineZhuijiProgramsEntries coreZhuijiProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess)
    {
        this._characterCaoHongMouOperations = characterCaoHongMouOperations;
        this._characterLiuBaOlOperations = characterLiuBaOlOperations;
        this._characterLuZhiWeiOlOperations = characterLuZhiWeiOlOperations;
        this._characterLvQianOlOperations = characterLvQianOlOperations;
        this._characterWangKuangOlOperations = characterWangKuangOlOperations;
        this._characterZhangShiPingOlOperations = characterZhangShiPingOlOperations;
        this._characterZhangXunAnniversaryOperations = characterZhangXunAnniversaryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._corePhaseHandSeizureOperations = corePhaseHandSeizureOperations;
        this._corePhaseNamePredictionOperations = corePhaseNamePredictionOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreZhuijiProgramsOperations = coreZhuijiProgramsOperations;
        this._coreActualRoundProgressRoundNumberAccess = coreActualRoundProgressRoundNumberAccess;
        this._coreActualTurnProgressTurnProgressionAccess = coreActualTurnProgressTurnProgressionAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
        this._coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess = coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess;
    }
}
