#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineAfterTurnEndedProgramsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterAHuiNanOlEntries _characterAHuiNanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaiFuRenOlEntries _characterCaiFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoYuOlEntries _characterCaoYuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhenMobileShiEntries _characterCaoZhenMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengJiChengCuiOlEntries _characterChengJiChengCuiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHeQiMobileShiEntries _characterHeQiMobileShiOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries _characterHuangfuSongWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWenYangOlEntries _characterWenYangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunYuMouEntries _characterXunYuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries _characterZhangFeiAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualEndedTurnEquipmentEntries _coreActualEndedTurnEquipmentOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries _coreDeferredHandAlignmentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineAfterTurnEndedProgramsRuntime(global::CardGame.Core.RuntimeEntries.CharacterAHuiNanOlEntries characterAHuiNanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCaiFuRenOlEntries characterCaiFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoYuOlEntries characterCaoYuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCaoZhenMobileShiEntries characterCaoZhenMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterChengJiChengCuiOlEntries characterChengJiChengCuiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHeQiMobileShiEntries characterHeQiMobileShiOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangfuSongWumiaoEntries characterHuangfuSongWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWenYangOlEntries characterWenYangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXunYuMouEntries characterXunYuMouOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries characterZhangFeiAnniversaryGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualEndedTurnEquipmentEntries coreActualEndedTurnEquipmentOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeferredHandAlignmentsEntries coreDeferredHandAlignmentsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
