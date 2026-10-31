#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries _characterBianFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries _characterCaoChunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChenLinMouEntries _characterChenLinMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterFanChouOlEntries _characterFanChouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoSiOlEntries _characterGuoSiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries _characterHuangQuanMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries _characterLiuYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaShiMouEntries _characterSimaShiMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSuFeiOlEntries _characterSuFeiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXuHuangMouMobileEntries _characterXuHuangMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries _characterYuanTanYuanShangOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries _coreDamageColorDuelProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineJianjiDeclarationsEntries _coreJianjiDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries _coreNamedAmbushInvalidationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries _coreOriginalUseLightningOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _coreProgramCardTargetEffectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries _coreProgramCardUseNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries _coreSoleTargetQuarantineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries _coreNativeCardUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries _coreNativeNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDelayedPlacementTargetProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries characterBianFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoChunOlEntries characterCaoChunOlOperations, global::CardGame.Core.RuntimeEntries.CharacterChenLinMouEntries characterChenLinMouOperations, global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterFanChouOlEntries characterFanChouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoSiOlEntries characterGuoSiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangQuanMouEntries characterHuangQuanMouOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries characterLiuYanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaShiMouEntries characterSimaShiMouOperations, global::CardGame.Core.RuntimeEntries.CharacterSuFeiOlEntries characterSuFeiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXuHuangMouMobileEntries characterXuHuangMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries characterYuanTanYuanShangOlOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageColorDuelProgramsEntries coreDamageColorDuelProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineJianjiDeclarationsEntries coreJianjiDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNamedAmbushInvalidationEntries coreNamedAmbushInvalidationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries coreOriginalUseLightningOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries coreProgramCardTargetEffectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries coreProgramCardUseNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries coreSoleTargetQuarantineOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseCompletionEntries coreNativeCardUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries coreNativeNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
