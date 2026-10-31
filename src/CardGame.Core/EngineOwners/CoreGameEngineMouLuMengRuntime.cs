#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineMouLuMengRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanYuRuntimeEntries _characterGuanYuRuntimeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterHostEntries _coreCharacterHostOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundEndingEntries _coreRoundEndingOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceDifficultyEntries _coreShanheSourceDifficultyOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    internal CoreGameEngineMouLuMengRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuanYuRuntimeEntries characterGuanYuRuntimeOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCharacterHostEntries coreCharacterHostOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundEndingEntries coreRoundEndingOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSourceDifficultyEntries coreShanheSourceDifficultyOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess coreActualRoundProgressRoundNumberRoundTurnSeatsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess)
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
