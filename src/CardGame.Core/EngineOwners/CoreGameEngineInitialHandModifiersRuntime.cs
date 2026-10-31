#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineInitialHandModifiersRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouYuLeEntries _characterZhouYuLeOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess _modeRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess _coreMatchDefinitionInitialHandSizeModeDefinitionAccess;
    internal CoreGameEngineInitialHandModifiersRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhouYuLeEntries characterZhouYuLeOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRuntimeAccess modeRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries coreRuleQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess coreMatchDefinitionInitialHandSizeModeDefinitionAccess)
    {
        this._characterZhouYuLeOperations = characterZhouYuLeOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreMatchDefinitionInitialHandSizeModeDefinitionAccess = coreMatchDefinitionInitialHandSizeModeDefinitionAccess;
    }
}
