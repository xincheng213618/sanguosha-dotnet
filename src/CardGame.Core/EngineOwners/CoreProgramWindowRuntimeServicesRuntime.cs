#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramWindowRuntimeServicesRuntime : IProgramWindowNativeReturns, IProgramWindowReturnQueries
{
    private readonly RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly RuntimeEntries.CharacterGuanYuHappyGodEntries _characterGuanYuHappyGodOperations;
    private readonly RuntimeEntries.CharacterRuanHuiOlEntries _characterRuanHuiOlOperations;
    private readonly RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineActualUseTargetProgramsEntries _coreActualUseTargetProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardEffectProgramsEntries _coreCardEffectProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineCompletedUseChildReturnsEntries _coreCompletedUseChildReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineForeignTurnContestsEntries _coreForeignTurnContestsOperations;
    private readonly RuntimeEntries.CoreGameEngineJudgmentReplacementProgramsEntries _coreJudgmentReplacementProgramsOperations;
    private readonly RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries _corePhaseHandSeizureOperations;
    private readonly RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries _corePindianRevealWindowsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheYongzhanEntries _coreShanheYongzhanOperations;
    private readonly RuntimeEntries.CoreGameEngineSlashTargetBenefitWindowsEntries _coreSlashTargetBenefitWindowsOperations;
    private readonly RuntimeEntries.CoreGameEngineSlashTargetPenaltyWindowsEntries _coreSlashTargetPenaltyWindowsOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangDyingExitEntries _coreWushuangDyingExitOperations;
    private readonly RuntimeEntries.CoreNativeDamageWindowsEntries _coreNativeDamageWindowsOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess _coreActualTurnProgressPhaseAccess;
    internal CoreProgramWindowRuntimeServicesRuntime(RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, RuntimeEntries.CharacterGuanYuHappyGodEntries characterGuanYuHappyGodOperations, RuntimeEntries.CharacterRuanHuiOlEntries characterRuanHuiOlOperations, RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, RuntimeEntries.CoreGameEngineActualUseTargetProgramsEntries coreActualUseTargetProgramsOperations, RuntimeEntries.CoreGameEngineCardEffectProgramsEntries coreCardEffectProgramsOperations, RuntimeEntries.CoreGameEngineCompletedUseChildReturnsEntries coreCompletedUseChildReturnsOperations, RuntimeEntries.CoreGameEngineForeignTurnContestsEntries coreForeignTurnContestsOperations, RuntimeEntries.CoreGameEngineJudgmentReplacementProgramsEntries coreJudgmentReplacementProgramsOperations, RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries corePhaseHandSeizureOperations, RuntimeEntries.CoreGameEnginePindianRevealWindowsEntries corePindianRevealWindowsOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineShanheYongzhanEntries coreShanheYongzhanOperations, RuntimeEntries.CoreGameEngineSlashTargetBenefitWindowsEntries coreSlashTargetBenefitWindowsOperations, RuntimeEntries.CoreGameEngineSlashTargetPenaltyWindowsEntries coreSlashTargetPenaltyWindowsOperations, RuntimeEntries.CoreGameEngineWushuangDyingExitEntries coreWushuangDyingExitOperations, RuntimeEntries.CoreNativeDamageWindowsEntries coreNativeDamageWindowsOperations, EngineStates.Contracts.ICoreActualTurnProgressPhaseAccess coreActualTurnProgressPhaseAccess)
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
