#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineMouLuMengRuntime
{
    private readonly RuntimeEntries.CharacterGuanYuRuntimeEntries _characterGuanYuRuntimeOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly RuntimeEntries.CoreGameEngineCharacterHostEntries _coreCharacterHostOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundEndingEntries _coreRoundEndingOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheSourceDifficultyEntries _coreShanheSourceDifficultyOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    internal CoreGameEngineMouLuMengRuntime(RuntimeEntries.CharacterGuanYuRuntimeEntries characterGuanYuRuntimeOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelMatchModeRulesAccess modeRulesOperations, IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, RuntimeEntries.CoreGameEngineCharacterHostEntries coreCharacterHostOperations, RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, RuntimeEntries.CoreGameEngineRoundEndingEntries coreRoundEndingOperations, RuntimeEntries.CoreGameEngineShanheSourceDifficultyEntries coreShanheSourceDifficultyOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreActualRoundProgressRoundNumberRoundTurnSeatsAccess coreActualRoundProgressRoundNumberRoundTurnSeatsAccess, EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess)
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
