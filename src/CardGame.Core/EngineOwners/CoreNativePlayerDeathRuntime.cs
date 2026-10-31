#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativePlayerDeathRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCaoYuOlEntries _characterCaoYuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDongZhaoOlEntries _characterDongZhaoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHeJinOlEntries _characterHeJinOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanYinAnniversaryEntries _characterYuanYinAnniversaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries _coreCardDamageMaximumHpReplacementOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries _coreDeathProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNationalGeneralRevealEntries _coreNationalGeneralRevealOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDeathFramesEntries _coreNativeDeathFramesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativePlayerDeathRuntime(global::CardGame.Core.RuntimeEntries.CharacterCaoYuOlEntries characterCaoYuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterDongZhaoOlEntries characterDongZhaoOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHeJinOlEntries characterHeJinOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYuanYinAnniversaryEntries characterYuanYinAnniversaryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDamageMaximumHpReplacementEntries coreCardDamageMaximumHpReplacementOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries coreDeathProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreNationalGeneralRevealEntries coreNationalGeneralRevealOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDeathFramesEntries coreNativeDeathFramesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
