#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreOwnedDrawPhaseRuntimePortsRuntime : global::CardGame.Core.IOwnedDrawPhaseFacts, global::CardGame.Core.IOwnedDrawPhaseNativeOperations
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries _characterCaoXiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries _characterChengGongYingOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhuHuanBoundaryMobileEntries _characterZhuHuanBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries _coreHpChangeProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries _coreNativeDrawAttemptsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawThresholdEntries _coreNativeDrawThresholdOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShensuEntries _coreShanheS5ShensuOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceChunlaoJudgment3161Entries _coreShanheSourceChunlaoJudgment3161Operations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries _coreLeastHandMarkerDrawProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries _coreRecoveryReplacementOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    internal CoreOwnedDrawPhaseRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries characterCaoXiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries characterChengGongYingOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhuHuanBoundaryMobileEntries characterZhuHuanBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries coreHpChangeProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries coreNativeDrawAttemptsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawThresholdEntries coreNativeDrawThresholdOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5ShensuEntries coreShanheS5ShensuOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceChunlaoJudgment3161Entries coreShanheSourceChunlaoJudgment3161Operations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreLeastHandMarkerDrawProgramsEntries coreLeastHandMarkerDrawProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreRecoveryReplacementEntries coreRecoveryReplacementOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess)
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
