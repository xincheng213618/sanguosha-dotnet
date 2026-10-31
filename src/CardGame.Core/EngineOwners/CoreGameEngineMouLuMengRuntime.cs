#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineMouLuMengRuntime
{
    private readonly ICoreMouLuMengCharacterGuanYuRuntimeOperations _characterGuanYuRuntimeOperations;
    private readonly ICoreMouLuMengContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreMouLuMengModeRulesOperations _modeRulesOperations;
    private readonly ICoreMouLuMengSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICoreMouLuMengCoreCharacterHostOperations _coreCharacterHostOperations;
    private readonly ICoreMouLuMengCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreMouLuMengCoreRoundEndingOperations _coreRoundEndingOperations;
    private readonly ICoreMouLuMengCoreShanheSourceDifficultyOperations _coreShanheSourceDifficultyOperations;
    private readonly ICoreMouLuMengCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreMouLuMengKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    internal CoreGameEngineMouLuMengRuntime(ICoreMouLuMengCharacterGuanYuRuntimeOperations characterGuanYuRuntimeOperations, ICoreMouLuMengContentRegistryOperations contentRegistryOperations, ICoreMouLuMengModeRulesOperations modeRulesOperations, ICoreMouLuMengSkillRuntimeStateOperations skillRuntimeStateOperations, ICoreMouLuMengCoreCharacterHostOperations coreCharacterHostOperations, ICoreMouLuMengCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreMouLuMengCoreRoundEndingOperations coreRoundEndingOperations, ICoreMouLuMengCoreShanheSourceDifficultyOperations coreShanheSourceDifficultyOperations, ICoreMouLuMengCoreMatchLogOperations coreMatchLogOperations, ICoreMouLuMengKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess coreActualRoundProgressRoundNumberRoundTurnSeatsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess)
    {
        this._characterGuanYuRuntimeOperations = characterGuanYuRuntimeOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreCharacterHostOperations = coreCharacterHostOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreRoundEndingOperations = coreRoundEndingOperations;
        this._coreShanheSourceDifficultyOperations = coreShanheSourceDifficultyOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualRoundProgressRoundNumberRoundTurnSeatsAccess = coreActualRoundProgressRoundNumberRoundTurnSeatsAccess;
        this._coreActualTurnProgressTurnProgressionAccess = coreActualTurnProgressTurnProgressionAccess;
    }
}
