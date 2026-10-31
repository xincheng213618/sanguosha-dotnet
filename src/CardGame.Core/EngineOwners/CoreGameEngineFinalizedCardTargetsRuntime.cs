#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineFinalizedCardTargetsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries _characterCaoZhenBoundaryMobileOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengYuMouEntries _characterChengYuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries _characterHuangZhongAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityOrdinaryTricksEntries _coreActivityOrdinaryTricksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries _coreProgramCardUseNullificationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries _coreNativeEquipmentUseOperations;
    private readonly ICoreFinalizedCardTargetsCoreProgramTargetWindowProtocolsOperations _coreProgramTargetWindowProtocolsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineFinalizedCardTargetsRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoZhenBoundaryMobileEntries characterCaoZhenBoundaryMobileOperations, global::CardGame.Core.RuntimeEntries.CharacterChengYuMouEntries characterChengYuMouOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangZhongAnniversaryGodEntries characterHuangZhongAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityOrdinaryTricksEntries coreActivityOrdinaryTricksOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries coreProgramCardUseNullificationOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentUseEntries coreNativeEquipmentUseOperations, ICoreFinalizedCardTargetsCoreProgramTargetWindowProtocolsOperations coreProgramTargetWindowProtocolsOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCaoZhenBoundaryMobileOperations = characterCaoZhenBoundaryMobileOperations;
        this._characterChengYuMouOperations = characterChengYuMouOperations;
        this._characterHuangZhongAnniversaryGodOperations = characterHuangZhongAnniversaryGodOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActivityOrdinaryTricksOperations = coreActivityOrdinaryTricksOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramCardUseNullificationOperations = coreProgramCardUseNullificationOperations;
        this._coreNativeEquipmentUseOperations = coreNativeEquipmentUseOperations;
        this._coreProgramTargetWindowProtocolsOperations = coreProgramTargetWindowProtocolsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
