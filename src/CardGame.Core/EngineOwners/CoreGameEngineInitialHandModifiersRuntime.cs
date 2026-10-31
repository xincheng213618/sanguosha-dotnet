#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineInitialHandModifiersRuntime
{
    private readonly RuntimeEntries.CharacterZhouYuLeEntries _characterZhouYuLeOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess _coreMatchDefinitionInitialHandSizeModeDefinitionAccess;
    internal CoreGameEngineInitialHandModifiersRuntime(RuntimeEntries.CharacterZhouYuLeEntries characterZhouYuLeOperations, IKernelMatchModeRulesAccess modeRulesOperations, IKernelMatchModeRuntimeAccess modeRuntimeOperations, RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess coreMatchDefinitionInitialHandSizeModeDefinitionAccess)
    {
        this._characterZhouYuLeOperations = characterZhouYuLeOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreMatchDefinitionInitialHandSizeModeDefinitionAccess = coreMatchDefinitionInitialHandSizeModeDefinitionAccess;
    }
}
