#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineInitialHandModifiersRuntime
{
    private readonly ICoreInitialHandModifiersCharacterZhouYuLeOperations _characterZhouYuLeOperations;
    private readonly ICoreInitialHandModifiersModeRulesOperations _modeRulesOperations;
    private readonly ICoreInitialHandModifiersModeRuntimeOperations _modeRuntimeOperations;
    private readonly ICoreInitialHandModifiersCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess _coreMatchDefinitionInitialHandSizeModeDefinitionAccess;
    internal CoreGameEngineInitialHandModifiersRuntime(ICoreInitialHandModifiersCharacterZhouYuLeOperations characterZhouYuLeOperations, ICoreInitialHandModifiersModeRulesOperations modeRulesOperations, ICoreInitialHandModifiersModeRuntimeOperations modeRuntimeOperations, ICoreInitialHandModifiersCoreRuleQueriesOperations coreRuleQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionInitialHandSizeModeDefinitionAccess coreMatchDefinitionInitialHandSizeModeDefinitionAccess)
    {
        this._characterZhouYuLeOperations = characterZhouYuLeOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._modeRuntimeOperations = modeRuntimeOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._coreMatchDefinitionInitialHandSizeModeDefinitionAccess = coreMatchDefinitionInitialHandSizeModeDefinitionAccess;
    }
}
