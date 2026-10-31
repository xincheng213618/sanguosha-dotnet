#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineIssuedPlayUsePoliciesRuntime : global::CardGame.Core.IIssuedPlayUseProgramHost
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChenDaoMobileShiEntries _characterChenDaoMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangChongMobileShiEntries _characterHuangChongMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries _characterHuangGaiMouMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries _characterHuangZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvKuangLvXiangOlEntries _characterLvKuangLvXiangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChaoBoundaryMobileEntries _characterMaChaoBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries _characterPanZhangMaZhongBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangDeBoundaryMobileEntries _characterPangDeBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries _characterWeiYanMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiBoundaryMobileEntries _characterZhangFeiBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries _coreActualPlayPhaseCardUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNonBasicSlashProfileEntries _coreNonBasicSlashProfileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries _coreResponseUseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess _coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess _coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess;
    internal CoreGameEngineIssuedPlayUsePoliciesRuntime(global::CardGame.Core.RuntimeEntries.CharacterChenDaoMobileShiEntries characterChenDaoMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangChongMobileShiEntries characterHuangChongMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries characterHuangGaiMouMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZhongBoundaryMobileEntries characterHuangZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLvKuangLvXiangOlEntries characterLvKuangLvXiangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChaoBoundaryMobileEntries characterMaChaoBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterPanZhangMaZhongBoundaryMobileEntries characterPanZhangMaZhongBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterPangDeBoundaryMobileEntries characterPangDeBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries characterWeiYanMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiBoundaryMobileEntries characterZhangFeiBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualPlayPhaseCardUsesEntries coreActualPlayPhaseCardUsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNonBasicSlashProfileEntries coreNonBasicSlashProfileOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseUseCompletionEntries coreResponseUseCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess)
    {
        this._characterChenDaoMobileShiOperations = characterChenDaoMobileShiOperations;
        this._characterHuangChongMobileShiOperations = characterHuangChongMobileShiOperations;
        this._characterHuangGaiMouMobileOperations = characterHuangGaiMouMobileOperations;
        this._characterHuangZhongBoundaryMobileOperations = characterHuangZhongBoundaryMobileOperations;
        this._characterLvKuangLvXiangOlOperations = characterLvKuangLvXiangOlOperations;
        this._characterMaChaoBoundaryMobileOperations = characterMaChaoBoundaryMobileOperations;
        this._characterPanZhangMaZhongBoundaryMobileOperations = characterPanZhangMaZhongBoundaryMobileOperations;
        this._characterPangDeBoundaryMobileOperations = characterPangDeBoundaryMobileOperations;
        this._characterWeiYanMobileShiOperations = characterWeiYanMobileShiOperations;
        this._characterZhangFeiBoundaryMobileOperations = characterZhangFeiBoundaryMobileOperations;
        this._characterZhangYanOlOperations = characterZhangYanOlOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualPlayPhaseCardUsesOperations = coreActualPlayPhaseCardUsesOperations;
        this._coreNonBasicSlashProfileOperations = coreNonBasicSlashProfileOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreResponseUseCompletionOperations = coreResponseUseCompletionOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess = coreGameEngineActualPlayPhaseCardUsesActualPlayPhaseUseActionsAccess;
        this._coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess = coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess;
        this._coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess = coreGameEngineIssuedPlayUsePoliciesIssuedPlayPhaseUseProhibitionsAccess;
    }
}
