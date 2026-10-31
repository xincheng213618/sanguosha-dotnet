#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramWindowRuntimeServicesRuntime : global::CardGame.Core.IProgramWindowNativeReturns, global::CardGame.Core.IProgramWindowReturnQueries
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuHappyGodEntries _characterGuanYuHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterRuanHuiOlEntries _characterRuanHuiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualUseTargetProgramsEntries _coreActualUseTargetProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardEffectProgramsEntries _coreCardEffectProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUseChildReturnsEntries _coreCompletedUseChildReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries _coreForeignTurnContestsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgmentReplacementProgramsEntries _coreJudgmentReplacementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries _corePhaseHandSeizureOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries _corePindianRevealWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheYongzhanEntries _coreShanheYongzhanOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetBenefitWindowsEntries _coreSlashTargetBenefitWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetPenaltyWindowsEntries _coreSlashTargetPenaltyWindowsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangDyingExitEntries _coreWushuangDyingExitOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageWindowsEntries _coreNativeDamageWindowsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess _coreActualTurnProgressPhaseAccess;
    internal CoreProgramWindowRuntimeServicesRuntime(global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanYuHappyGodEntries characterGuanYuHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterRuanHuiOlEntries characterRuanHuiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualUseTargetProgramsEntries coreActualUseTargetProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardEffectProgramsEntries coreCardEffectProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUseChildReturnsEntries coreCompletedUseChildReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineForeignTurnContestsEntries coreForeignTurnContestsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineJudgmentReplacementProgramsEntries coreJudgmentReplacementProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries corePhaseHandSeizureOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries corePindianRevealWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheYongzhanEntries coreShanheYongzhanOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetBenefitWindowsEntries coreSlashTargetBenefitWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSlashTargetPenaltyWindowsEntries coreSlashTargetPenaltyWindowsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangDyingExitEntries coreWushuangDyingExitOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageWindowsEntries coreNativeDamageWindowsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess coreActualTurnProgressPhaseAccess)
    {
        this._characterBeiMiHuOlOperations = characterBeiMiHuOlOperations;
        this._characterGuanYuHappyGodOperations = characterGuanYuHappyGodOperations;
        this._characterRuanHuiOlOperations = characterRuanHuiOlOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreActualUseTargetProgramsOperations = coreActualUseTargetProgramsOperations;
        this._coreCardEffectProgramsOperations = coreCardEffectProgramsOperations;
        this._coreCompletedUseChildReturnsOperations = coreCompletedUseChildReturnsOperations;
        this._coreForeignTurnContestsOperations = coreForeignTurnContestsOperations;
        this._coreJudgmentReplacementProgramsOperations = coreJudgmentReplacementProgramsOperations;
        this._corePhaseHandSeizureOperations = corePhaseHandSeizureOperations;
        this._corePindianRevealWindowsOperations = corePindianRevealWindowsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreShanheYongzhanOperations = coreShanheYongzhanOperations;
        this._coreSlashTargetBenefitWindowsOperations = coreSlashTargetBenefitWindowsOperations;
        this._coreSlashTargetPenaltyWindowsOperations = coreSlashTargetPenaltyWindowsOperations;
        this._coreWushuangDyingExitOperations = coreWushuangDyingExitOperations;
        this._coreNativeDamageWindowsOperations = coreNativeDamageWindowsOperations;
        this._coreActualTurnProgressPhaseAccess = coreActualTurnProgressPhaseAccess;
    }
}
