#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativePlayerDeathRuntime
{
    private readonly ICoreNativePlayerDeathCharacterCaoYuOlOperations _characterCaoYuOlOperations;
    private readonly ICoreNativePlayerDeathCharacterDaQiaoXiaoQiaoHappyGodOperations _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly ICoreNativePlayerDeathCharacterDongZhaoOlOperations _characterDongZhaoOlOperations;
    private readonly ICoreNativePlayerDeathCharacterHeJinOlOperations _characterHeJinOlOperations;
    private readonly ICoreNativePlayerDeathCharacterYuanYinAnniversaryOperations _characterYuanYinAnniversaryOperations;
    private readonly ICoreNativePlayerDeathModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreNativePlayerDeathResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreNativePlayerDeathCoreCardAttackStateOperations _coreCardAttackStateOperations;
    private readonly ICoreNativePlayerDeathCoreCardDamageMaximumHpReplacementOperations _coreCardDamageMaximumHpReplacementOperations;
    private readonly ICoreNativePlayerDeathCoreCardUseModulesOperations _coreCardUseModulesOperations;
    private readonly ICoreNativePlayerDeathCoreDeathProgramsOperations _coreDeathProgramsOperations;
    private readonly ICoreNativePlayerDeathCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreNativePlayerDeathCoreNationalGeneralRevealOperations _coreNationalGeneralRevealOperations;
    private readonly ICoreNativePlayerDeathCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly ICoreNativePlayerDeathCoreNativeDeathFramesOperations _coreNativeDeathFramesOperations;
    private readonly ICoreNativePlayerDeathKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativePlayerDeathRuntime(ICoreNativePlayerDeathCharacterCaoYuOlOperations characterCaoYuOlOperations, ICoreNativePlayerDeathCharacterDaQiaoXiaoQiaoHappyGodOperations characterDaQiaoXiaoQiaoHappyGodOperations, ICoreNativePlayerDeathCharacterDongZhaoOlOperations characterDongZhaoOlOperations, ICoreNativePlayerDeathCharacterHeJinOlOperations characterHeJinOlOperations, ICoreNativePlayerDeathCharacterYuanYinAnniversaryOperations characterYuanYinAnniversaryOperations, ICoreNativePlayerDeathModeRuntimeOperations modeRuntimeOperations, ICoreNativePlayerDeathResolutionStackOperations resolutionStackOperations, ICoreNativePlayerDeathCoreCardAttackStateOperations coreCardAttackStateOperations, ICoreNativePlayerDeathCoreCardDamageMaximumHpReplacementOperations coreCardDamageMaximumHpReplacementOperations, ICoreNativePlayerDeathCoreCardUseModulesOperations coreCardUseModulesOperations, ICoreNativePlayerDeathCoreDeathProgramsOperations coreDeathProgramsOperations, ICoreNativePlayerDeathCoreRuntimeOperations coreRuntimeOperations, ICoreNativePlayerDeathCoreNationalGeneralRevealOperations coreNationalGeneralRevealOperations, ICoreNativePlayerDeathCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, ICoreNativePlayerDeathCoreNativeDeathFramesOperations coreNativeDeathFramesOperations, ICoreNativePlayerDeathKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
