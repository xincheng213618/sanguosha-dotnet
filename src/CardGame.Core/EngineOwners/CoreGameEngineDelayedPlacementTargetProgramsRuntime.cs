#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsRuntime
{
    private readonly RuntimeEntries.CharacterBianFuRenOlEntries _characterBianFuRenOlOperations;
    private readonly RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly RuntimeEntries.CharacterChenLinMouEntries _characterChenLinMouOperations;
    private readonly RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterFanChouOlEntries _characterFanChouOlOperations;
    private readonly RuntimeEntries.CharacterGuoSiOlEntries _characterGuoSiOlOperations;
    private readonly RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly RuntimeEntries.CharacterLiuYanOlEntries _characterLiuYanOlOperations;
    private readonly RuntimeEntries.CharacterSimaShiMouEntries _characterSimaShiMouOperations;
    private readonly RuntimeEntries.CharacterSuFeiOlEntries _characterSuFeiOlOperations;
    private readonly RuntimeEntries.CharacterXuHuangMouMobileEntries _characterXuHuangMouMobileOperations;
    private readonly RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly RuntimeEntries.CharacterYuanTanYuanShangOlEntries _characterYuanTanYuanShangOlOperations;
    private readonly IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly RuntimeEntries.CoreGameEngineJianjiDeclarationsEntries _coreJianjiDeclarationsOperations;
    private readonly RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries _coreNamedAmbushInvalidationOperations;
    private readonly RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries _coreProgramCardUseNullificationOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries _coreSoleTargetQuarantineOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDelayedPlacementTargetProgramsRuntime(RuntimeEntries.CharacterBianFuRenOlEntries characterBianFuRenOlOperations, RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations, RuntimeEntries.CharacterChenLinMouEntries characterChenLinMouOperations, RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations, RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, RuntimeEntries.CharacterFanChouOlEntries characterFanChouOlOperations, RuntimeEntries.CharacterGuoSiOlEntries characterGuoSiOlOperations, RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, RuntimeEntries.CharacterLiuYanOlEntries characterLiuYanOlOperations, RuntimeEntries.CharacterSimaShiMouEntries characterSimaShiMouOperations, RuntimeEntries.CharacterSuFeiOlEntries characterSuFeiOlOperations, RuntimeEntries.CharacterXuHuangMouMobileEntries characterXuHuangMouMobileOperations, RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, RuntimeEntries.CharacterYuanTanYuanShangOlEntries characterYuanTanYuanShangOlOperations, IKernelCardZoneStoreAccess cardZonesOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, RuntimeEntries.CoreGameEngineJianjiDeclarationsEntries coreJianjiDeclarationsOperations, RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries coreNamedAmbushInvalidationOperations, RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations, RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations, RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries coreProgramCardUseNullificationOperations, RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries coreSoleTargetQuarantineOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._characterCaoChunOlOperations = characterCaoChunOlOperations;
        this._characterChenLinMouOperations = characterChenLinMouOperations;
        this._characterCuiLieAnniversaryOperations = characterCuiLieAnniversaryOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterFanChouOlOperations = characterFanChouOlOperations;
        this._characterGuoSiOlOperations = characterGuoSiOlOperations;
        this._characterHuangQuanMouOperations = characterHuangQuanMouOperations;
        this._characterLiuYanOlOperations = characterLiuYanOlOperations;
        this._characterSimaShiMouOperations = characterSimaShiMouOperations;
        this._characterSuFeiOlOperations = characterSuFeiOlOperations;
        this._characterXuHuangMouMobileOperations = characterXuHuangMouMobileOperations;
        this._characterXunYuMobileGodOperations = characterXunYuMobileGodOperations;
        this._characterYuanTanYuanShangOlOperations = characterYuanTanYuanShangOlOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreDamageColorDuelProgramsOperations = coreDamageColorDuelProgramsOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreJianjiDeclarationsOperations = coreJianjiDeclarationsOperations;
        this._coreNamedAmbushInvalidationOperations = coreNamedAmbushInvalidationOperations;
        this._coreOriginalUseLightningOperations = coreOriginalUseLightningOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramCardTargetEffectsOperations = coreProgramCardTargetEffectsOperations;
        this._coreProgramCardUseNullificationOperations = coreProgramCardUseNullificationOperations;
        this._coreSoleTargetProtectionOperations = coreSoleTargetProtectionOperations;
        this._coreSoleTargetQuarantineOperations = coreSoleTargetQuarantineOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardUseCompletionOperations = coreNativeCardUseCompletionOperations;
        this._coreNativeNullificationOperations = coreNativeNullificationOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
