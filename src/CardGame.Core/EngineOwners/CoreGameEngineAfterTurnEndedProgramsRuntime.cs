#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAfterTurnEndedProgramsRuntime
{
    private readonly RuntimeEntries.CharacterAHuiNanOlEntries _characterAHuiNanOlOperations;
    private readonly RuntimeEntries.CharacterCaiFuRenOlEntries _characterCaiFuRenOlOperations;
    private readonly RuntimeEntries.CharacterCaoYuOlEntries _characterCaoYuOlOperations;
    private readonly RuntimeEntries.CharacterCaoZhenMobileShiEntries _characterCaoZhenMobileShiOperations;
    private readonly RuntimeEntries.CharacterChengJiChengCuiOlEntries _characterChengJiChengCuiOlOperations;
    private readonly RuntimeEntries.CharacterHeQiMobileShiEntries _characterHeQiMobileShiOperations;
    private readonly RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly RuntimeEntries.CharacterWenYangOlEntries _characterWenYangOlOperations;
    private readonly RuntimeEntries.CharacterXunYuMouEntries _characterXunYuMouOperations;
    private readonly RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineActualEndedTurnEquipmentEntries _coreActualEndedTurnEquipmentOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries _coreDeferredHandAlignmentsOperations;
    private readonly RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAfterTurnEndedProgramsRuntime(RuntimeEntries.CharacterAHuiNanOlEntries characterAHuiNanOlOperations,RuntimeEntries.CharacterCaiFuRenOlEntries characterCaiFuRenOlOperations,RuntimeEntries.CharacterCaoYuOlEntries characterCaoYuOlOperations,RuntimeEntries.CharacterCaoZhenMobileShiEntries characterCaoZhenMobileShiOperations,RuntimeEntries.CharacterChengJiChengCuiOlEntries characterChengJiChengCuiOlOperations,RuntimeEntries.CharacterHeQiMobileShiEntries characterHeQiMobileShiOperations,RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations,RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations,RuntimeEntries.CharacterWenYangOlEntries characterWenYangOlOperations,RuntimeEntries.CharacterXunYuMouEntries characterXunYuMouOperations,RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations,IKernelContentRegistryAccess contentRegistryOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreGameEngineActualEndedTurnEquipmentEntries coreActualEndedTurnEquipmentOperations,RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries coreDeferredHandAlignmentsOperations,RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations,RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations,RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations,RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterAHuiNanOlOperations = characterAHuiNanOlOperations;
        this._characterCaiFuRenOlOperations = characterCaiFuRenOlOperations;
        this._characterCaoYuOlOperations = characterCaoYuOlOperations;
        this._characterCaoZhenMobileShiOperations = characterCaoZhenMobileShiOperations;
        this._characterChengJiChengCuiOlOperations = characterChengJiChengCuiOlOperations;
        this._characterHeQiMobileShiOperations = characterHeQiMobileShiOperations;
        this._characterHuangfuSongWumiaoOperations = characterHuangfuSongWumiaoOperations;
        this._characterQinLangOlOperations = characterQinLangOlOperations;
        this._characterWenYangOlOperations = characterWenYangOlOperations;
        this._characterXunYuMouOperations = characterXunYuMouOperations;
        this._characterZhangFeiAnniversaryGodOperations = characterZhangFeiAnniversaryGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualEndedTurnEquipmentOperations = coreActualEndedTurnEquipmentOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreDeferredHandAlignmentsOperations = coreDeferredHandAlignmentsOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
