#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreOwnedDrawPhaseRuntimePortsRuntime : IOwnedDrawPhaseFacts, IOwnedDrawPhaseNativeOperations
{
    private readonly RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly RuntimeEntries.CharacterChengGongYingOlEntries _characterChengGongYingOlOperations;
    private readonly RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly RuntimeEntries.CharacterZhuHuanBoundaryMobileEntries _characterZhuHuanBoundaryMobileOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries _coreNativeDrawAttemptsOperations;
    private readonly RuntimeEntries.CoreGameEngineNativeDrawThresholdEntries _coreNativeDrawThresholdOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheS5ShensuEntries _coreShanheS5ShensuOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheSourceChunlaoJudgment3161Entries _coreShanheSourceChunlaoJudgment3161Operations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries _coreLeastHandMarkerDrawProgramsOperations;
    private readonly RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    internal CoreOwnedDrawPhaseRuntimePortsRuntime(RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, RuntimeEntries.CharacterChengGongYingOlEntries characterChengGongYingOlOperations, RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, RuntimeEntries.CharacterZhuHuanBoundaryMobileEntries characterZhuHuanBoundaryMobileOperations, RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries coreNativeDrawAttemptsOperations, RuntimeEntries.CoreGameEngineNativeDrawThresholdEntries coreNativeDrawThresholdOperations, RuntimeEntries.CoreGameEngineShanheS5ShensuEntries coreShanheS5ShensuOperations, RuntimeEntries.CoreGameEngineShanheSourceChunlaoJudgment3161Entries coreShanheSourceChunlaoJudgment3161Operations, RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries coreLeastHandMarkerDrawProgramsOperations, RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess)
    {
        this._characterCaoXiOlOperations = characterCaoXiOlOperations;
        this._characterChengGongYingOlOperations = characterChengGongYingOlOperations;
        this._characterDuFuRenOlOperations = characterDuFuRenOlOperations;
        this._characterZhuHuanBoundaryMobileOperations = characterZhuHuanBoundaryMobileOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreNativeDrawAttemptsOperations = coreNativeDrawAttemptsOperations;
        this._coreNativeDrawThresholdOperations = coreNativeDrawThresholdOperations;
        this._coreShanheS5ShensuOperations = coreShanheS5ShensuOperations;
        this._coreShanheSourceChunlaoJudgment3161Operations = coreShanheSourceChunlaoJudgment3161Operations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreLeastHandMarkerDrawProgramsOperations = coreLeastHandMarkerDrawProgramsOperations;
        this._coreRecoveryReplacementOperations = coreRecoveryReplacementOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
    }
}
