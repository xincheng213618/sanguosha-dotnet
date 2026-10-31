#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativePlayerDeathRuntime
{
    private readonly RuntimeEntries.CharacterCaoYuOlEntries _characterCaoYuOlOperations;
    private readonly RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly RuntimeEntries.CharacterDongZhaoOlEntries _characterDongZhaoOlOperations;
    private readonly RuntimeEntries.CharacterHeJinOlEntries _characterHeJinOlOperations;
    private readonly RuntimeEntries.CharacterYuanYinAnniversaryEntries _characterYuanYinAnniversaryOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries _coreCardDamageMaximumHpReplacementOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly RuntimeEntries.CoreGameEngineDeathProgramsEntries _coreDeathProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreNationalGeneralRevealEntries _coreNationalGeneralRevealOperations;
    private readonly RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly RuntimeEntries.CoreNativeDeathFramesEntries _coreNativeDeathFramesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativePlayerDeathRuntime(RuntimeEntries.CharacterCaoYuOlEntries characterCaoYuOlOperations, RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, RuntimeEntries.CharacterDongZhaoOlEntries characterDongZhaoOlOperations, RuntimeEntries.CharacterHeJinOlEntries characterHeJinOlOperations, RuntimeEntries.CharacterYuanYinAnniversaryEntries characterYuanYinAnniversaryOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries coreCardDamageMaximumHpReplacementOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, RuntimeEntries.CoreGameEngineDeathProgramsEntries coreDeathProgramsOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreNationalGeneralRevealEntries coreNationalGeneralRevealOperations, RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, RuntimeEntries.CoreNativeDeathFramesEntries coreNativeDeathFramesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoYuOlOperations = characterCaoYuOlOperations;
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterDongZhaoOlOperations = characterDongZhaoOlOperations;
        this._characterHeJinOlOperations = characterHeJinOlOperations;
        this._characterYuanYinAnniversaryOperations = characterYuanYinAnniversaryOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardDamageMaximumHpReplacementOperations = coreCardDamageMaximumHpReplacementOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreDeathProgramsOperations = coreDeathProgramsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNationalGeneralRevealOperations = coreNationalGeneralRevealOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreNativeDeathFramesOperations = coreNativeDeathFramesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
