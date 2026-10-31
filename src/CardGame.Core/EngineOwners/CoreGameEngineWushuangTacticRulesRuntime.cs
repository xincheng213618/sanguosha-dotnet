#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineWushuangTacticRulesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangEntries _coreWushuangOperations;
    internal CoreGameEngineWushuangTacticRulesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangEntries coreWushuangOperations)
    {
        this._coreWushuangOperations = coreWushuangOperations;
    }
}
