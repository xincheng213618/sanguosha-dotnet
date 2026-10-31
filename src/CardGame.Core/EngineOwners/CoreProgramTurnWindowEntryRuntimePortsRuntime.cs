#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramTurnWindowEntryRuntimePortsRuntime : IProgramTurnWindowEntryFacts, IProgramTurnWindowIssuedCandidates, IProgramTurnWindowPhaseReceipts
{
    private readonly RuntimeEntries.CharacterCaoHongMouEntries _characterCaoHongMouOperations;
    private readonly RuntimeEntries.CharacterLiuBaOlEntries _characterLiuBaOlOperations;
    private readonly RuntimeEntries.CharacterLuZhiWeiOlEntries _characterLuZhiWeiOlOperations;
    private readonly RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly RuntimeEntries.CharacterWangKuangOlEntries _characterWangKuangOlOperations;
    private readonly RuntimeEntries.CharacterZhangShiPingOlEntries _characterZhangShiPingOlOperations;
    private readonly RuntimeEntries.CharacterZhangXunAnniversaryEntries _characterZhangXunAnniversaryOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries _corePhaseHandSeizureOperations;
    private readonly RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries _corePhaseNamePredictionOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreGameEngineZhuijiProgramsEntries _coreZhuijiProgramsOperations;
    private readonly EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _coreActualRoundProgressRoundNumberAccess;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    private readonly EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess;
    internal CoreProgramTurnWindowEntryRuntimePortsRuntime(RuntimeEntries.CharacterCaoHongMouEntries characterCaoHongMouOperations, RuntimeEntries.CharacterLiuBaOlEntries characterLiuBaOlOperations, RuntimeEntries.CharacterLuZhiWeiOlEntries characterLuZhiWeiOlOperations, RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, RuntimeEntries.CharacterWangKuangOlEntries characterWangKuangOlOperations, RuntimeEntries.CharacterZhangShiPingOlEntries characterZhangShiPingOlOperations, RuntimeEntries.CharacterZhangXunAnniversaryEntries characterZhangXunAnniversaryOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries corePhaseHandSeizureOperations, RuntimeEntries.CoreGameEnginePhaseNamePredictionEntries corePhaseNamePredictionOperations, RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, RuntimeEntries.CoreGameEngineZhuijiProgramsEntries coreZhuijiProgramsOperations, EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess coreActualRoundProgressRoundNumberAccess, EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess, EngineStates.Contracts.ICoreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess)
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
