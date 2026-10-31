#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAttackEffectsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries _characterDaQiaoLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoMobileEntries _characterDaQiaoMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaWanOlEntries _characterMaWanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries _coreCurrentEnhancementSlashRedirectsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries _coreCurrentSlashFireOffersOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries _coreDesignatedExtraTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries _coreDirectedCardPolicyQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries _coreShortRangeSlashTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries _coreSoleTargetRedirectOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramAttackEffectsRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoLeEntries characterDaQiaoLeOperations, global::CardGame.Core.RuntimeEntries.CharacterDaQiaoMobileEntries characterDaQiaoMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaWanOlEntries characterMaWanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDirectedTurnCardPoliciesEntries coreDirectedTurnCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentEnhancementSlashRedirectsEntries coreCurrentEnhancementSlashRedirectsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries coreCurrentSlashFireOffersOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries coreDesignatedExtraTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDirectedCardPolicyQueriesEntries coreDirectedCardPolicyQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries coreShortRangeSlashTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries coreSoleTargetRedirectOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamagePoliciesEntries coreNativeDamagePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterDaQiaoLeOperations = characterDaQiaoLeOperations;
        this._characterDaQiaoMobileOperations = characterDaQiaoMobileOperations;
        this._characterLiuPanOlOperations = characterLiuPanOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterMaWanOlOperations = characterMaWanOlOperations;
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._characterQianZhaoOlOperations = characterQianZhaoOlOperations;
        this._characterYangFuOlOperations = characterYangFuOlOperations;
        this._characterZhangYanOlOperations = characterZhangYanOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDirectedTurnCardPoliciesOperations = coreDirectedTurnCardPoliciesOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCurrentEnhancementSlashRedirectsOperations = coreCurrentEnhancementSlashRedirectsOperations;
        this._coreCurrentSlashFireOffersOperations = coreCurrentSlashFireOffersOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreDirectedCardPolicyQueriesOperations = coreDirectedCardPolicyQueriesOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShortRangeSlashTargetsOperations = coreShortRangeSlashTargetsOperations;
        this._coreSoleTargetRedirectOperations = coreSoleTargetRedirectOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDamagePoliciesOperations = coreNativeDamagePoliciesOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
